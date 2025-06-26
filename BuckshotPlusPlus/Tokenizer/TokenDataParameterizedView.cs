using System;
using System.Collections.Generic;
using System.Linq;

namespace BuckshotPlusPlus
{
    public class TokenDataParameterizedView : TokenData
    {
        public string ViewName { get; set; }
        public string ParentViewName { get; set; }
        public List<string> Parameters { get; set; }
        public List<Token> ViewContent { get; set; }
        public Token ViewToken { get; set; }
        public TokenDataParameterizedView ParentView { get; private set; }

        public TokenDataParameterizedView(string viewName, string parentViewName, List<string> parameters, List<Token> content, Token token, List<Token> serverSideTokens = null)
        {
            if (string.IsNullOrWhiteSpace(viewName))
            {
                throw new ArgumentNullException(nameof(viewName), "View name cannot be null or empty");
            }
            
            ViewName = viewName.Trim();
            ParentViewName = parentViewName?.Trim();
            Parameters = parameters ?? new List<string>();
            ViewContent = content ?? new List<Token>();
            ViewToken = token ?? throw new ArgumentNullException(nameof(token));
            
            // Try to find the parent view in the server tokens
            if (!string.IsNullOrEmpty(parentViewName))
            {
                ParentView = TokenUtils.FindParameterizedView(serverSideTokens, parentViewName);
                if (ParentView == null)
                {
                    Formater.TokenCriticalError($"Parent view '{parentViewName}' not found for view '{ViewName}'", ViewToken);
                }
                else
                {
                    Formater.DebugMessage($"View '{ViewName}' inherits from '{ParentViewName}' with {Parameters.Count} parameters");
                    
                    // Merge parent parameters with child parameters
                    if (ParentView.Parameters.Count > 0)
                    {
                        var allParameters = new List<string>(ParentView.Parameters);
                        allParameters.AddRange(Parameters);
                        Parameters = allParameters;
                    }
                }
            }
            else
            {
                Formater.DebugMessage($"Created parameterized view '{ViewName}' with {Parameters.Count} parameters");
            }
        }

        public TokenDataContainer InstantiateView(List<string> arguments, List<Token> serverSideTokens)
        {
            try
            {
                if (arguments == null)
                {
                    Formater.RuntimeError("Arguments list is null", ViewToken);
                    return null;
                }

                if (arguments.Count != Parameters.Count)
                {
                    Formater.RuntimeError($"Parameter count mismatch for view '{ViewName}'. Expected {Parameters.Count}, got {arguments.Count}", ViewToken);
                    return null;
                }


                // Create a unique instance identifier
                string uniqueInstanceId = Guid.NewGuid().ToString("N")[..8]; // Short GUID
                string uniqueInstanceName = $"{ViewName}_instance_{uniqueInstanceId}";

                // Process each token in the view content
                var instantiatedContent = new List<Token>();

                // Create a dictionary to store the variables from parent and child views
                var viewVariables = new Dictionary<string, Token>();

                // First, process parent view content if this view inherits from another
                if (ParentView != null)
                {
                    Formater.DebugMessage($"Including content from parent view '{ParentView.ViewName}' in view '{ViewName}'");
                    
                    // Only pass arguments to the parent if it expects parameters
                    var parentArguments = ParentView.Parameters.Count > 0 ? arguments : new List<string>();
                    var parentContent = ParentView.InstantiateView(parentArguments, serverSideTokens)?.ContainerData;
                    
                    if (parentContent != null)
                    {
                        // Add parent's content first, but don't add to instantiatedContent yet
                        foreach (var token in parentContent)
                        {
                            if (token?.Data is TokenDataVariable varData)
                            {
                                viewVariables[varData.VariableName] = token;
                            }
                        }
                    }
                }


                // Process current view's content, which will override parent's content
                foreach (var token in ViewContent)
                {
                    try
                    {
                        var clonedToken = CloneTokenWithParameterSubstitution(token, arguments, serverSideTokens);
                        if (clonedToken != null && clonedToken.Data is TokenDataVariable varData)
                        {
                            // Store or update the variable in our dictionary
                            viewVariables[varData.VariableName] = clonedToken;
                        }
                    }
                    catch (Exception ex)
                    {
                        Formater.RuntimeError($"Error cloning token in view '{ViewName}': {ex.Message}", token);
                        Formater.DebugMessage($"Stack trace in token at line {token?.LineNumber}: {ex.StackTrace}");
                    }
                }


                // Instead of creating a container, we'll create a special token that represents the view instance
                var viewInstanceToken = new Token(
                    fileName: ViewToken?.FileName ?? "unknown",
                    lineData: $"view {uniqueInstanceName}",
                    lineNumber: ViewToken?.LineNumber ?? 0,
                    myTokenizer: ViewToken?.MyTokenizer,
                    parent: null,
                    previousToken: null
                );
                
                // Add all variables to the final content
                var finalContent = new List<Token>(viewVariables.Values);
                
                // Create a custom container that will handle the view content
                var viewContainer = new TokenDataContainer(viewInstanceToken)
                {
                    // Add all variables to the container
                    ContainerData = finalContent
                };
                
                // Set the container name to indicate it's a view instance
                viewContainer.ContainerName = uniqueInstanceName;
                viewContainer.ContainerType = "view";
                
                return viewContainer;
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error instantiating view '{ViewName}': {ex.Message}", ViewToken);
                Formater.DebugMessage($"Stack trace in view '{ViewName}': {ex.StackTrace}");
                return null;
            }
        }

        private Token CloneTokenWithParameterSubstitution(Token originalToken, List<string> arguments, List<Token> serverSideTokens)
        {
            var clonedToken = new Token(
                fileName: originalToken.FileName,
                lineData: originalToken.LineData,
                lineNumber: originalToken.LineNumber,
                myTokenizer: originalToken.MyTokenizer,
                parent: null,
                previousToken: null
            );

            if (originalToken.Data is TokenDataVariable variable)
            {
                // Handle parameter references in variable data
                string newVariableData = variable.VariableData;
                string trimmedValue = variable.VariableData.Trim();
                
                // If the variable data exactly matches a parameter name, replace it with the argument
                if (Parameters.Contains(trimmedValue))
                {
                    int paramIndex = Parameters.IndexOf(trimmedValue);
                    if (paramIndex >= 0 && paramIndex < arguments.Count)
                    {
                        newVariableData = arguments[paramIndex];
                        // If the original was a reference, keep it as a reference
                        if (variable.VariableType == "ref")
                        {
                            clonedToken.Data = new TokenDataVariable(clonedToken)
                            {
                                VariableName = variable.VariableName,
                                VariableData = newVariableData,
                                VariableType = "string"  // Change to string since we're substituting the value directly
                            };
                            return clonedToken;
                        }
                    }
                }
                
                clonedToken.Data = new TokenDataVariable(clonedToken)
                {
                    VariableName = variable.VariableName,
                    VariableData = newVariableData,
                    VariableType = variable.VariableType
                };
            }
            else if (originalToken.Data is TokenDataContainer container)
            {
                // For containers, we need to create a new container with the same type and content
                // but with parameters substituted in the content
                clonedToken.Data = new TokenDataContainer(clonedToken);
                var newContainer = (TokenDataContainer)clonedToken.Data;
                
                // Copy container properties
                newContainer.ContainerName = container.ContainerName;
                newContainer.ContainerType = container.ContainerType;
                
                // Clone and substitute parameters in container data
                foreach (var childToken in container.ContainerData)
                {
                    var clonedChild = CloneTokenWithParameterSubstitution(childToken, arguments, serverSideTokens);
                    if (clonedChild != null)
                    {
                        newContainer.ContainerData.Add(clonedChild);
                    }
                }
            }
            else
            {
                clonedToken.Data = originalToken.Data;
            }

            return clonedToken;
        }

        private string SubstituteParameters(string content, List<string> arguments)
        {
            if (content == null)
                return string.Empty;

            try
            {
                // If the content exactly matches a parameter, replace it
                for (int i = 0; i < Math.Min(Parameters.Count, arguments.Count); i++)
                {
                    if (Parameters[i] != null && arguments[i] != null && content.Trim() == Parameters[i])
                    {
                        return arguments[i];
                    }
                }
                
                // If we get here, no exact match was found
                return content;
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error substituting parameters in view '{ViewName}': {ex.Message}", ViewToken);
                Formater.DebugMessage($"Parameter substitution error in view '{ViewName}':\nContent: {content}\nParameters: {string.Join(", ", Parameters)}\nArguments: {string.Join(", ", arguments)}");
                return content; // Return original content on error
            }
        }
    }
}
