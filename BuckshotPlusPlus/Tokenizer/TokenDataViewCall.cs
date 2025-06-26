using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class TokenDataViewCall : TokenData
    {
        public string ViewName { get; set; }
        public List<string> Arguments { get; set; }
        public Token ViewCallToken { get; set; }

        public TokenDataViewCall(string viewName, List<string> arguments, Token token)
        {
            ViewName = viewName?.Trim() ?? throw new ArgumentNullException(nameof(viewName));
            Arguments = arguments ?? new List<string>();
            ViewCallToken = token ?? throw new ArgumentNullException(nameof(token));
        }

        public string CompileViewCall(List<Token> fileTokens)
        {
            try
            {
                if (fileTokens == null)
                {
                    Formater.RuntimeError("File tokens list is null", ViewCallToken);
                    return "";
                }
                
                Formater.DebugMessage($"Looking for parameterized view '{ViewName}' with {Arguments.Count} arguments");

                // Find the parameterized view definition
                var paramView = FindParameterizedView(fileTokens, ViewName);
                if (paramView == null)
                {
                    // The error is already logged in FindParameterizedView
                    return string.Empty;
                }

                // Instantiate the view with arguments
                var viewInstance = paramView.InstantiateView(Arguments, fileTokens);
                if (viewInstance == null)
                {
                    Formater.RuntimeError($"Failed to instantiate view '{ViewName}'", ViewCallToken);
                    return "";
                }

                // Compile the view content directly
                var result = new System.Text.StringBuilder();
                
                // Process the view instance to ensure all properties (like color) are properly handled
                if (viewInstance.ContainerData != null)
                {
                    // Create a token for the view instance and compile it
                    var viewToken = new Token(
                        fileName: ViewCallToken.FileName,
                        lineData: $"view {viewInstance.ContainerName} {{ ... }}",
                        lineNumber: ViewCallToken.LineNumber,
                        myTokenizer: ViewCallToken.MyTokenizer,
                        parent: null,
                        previousToken: null
                    );
                    viewToken.Data = viewInstance;
                    
                    // Compile the view which will handle all its children
                    string compiledView = Compiler.HTML.View.CompileView(fileTokens, viewToken);
                    
                    // Only append if we have content
                    if (!string.IsNullOrEmpty(compiledView))
                    {
                        result.Append(compiledView);
                    }
                }
                
                if (result.Length == 0)
                {
                    Formater.DebugMessage($"Empty result when compiling view '{ViewName}' at line {ViewCallToken?.LineNumber}");
                }
                
                return result.ToString().Trim();
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error compiling view call '{ViewName}': {ex.Message}", ViewCallToken);
                Formater.DebugMessage($"Stack trace: {ex.StackTrace}");
                return $"<!-- Error compiling view '{ViewName}': {ex.Message} -->";
            }
        }

        private TokenDataParameterizedView FindParameterizedView(List<Token> fileTokens, string viewName)
        {
            if (fileTokens == null || string.IsNullOrEmpty(viewName))
            {
                Formater.DebugMessage($"Invalid parameters - fileTokens: {fileTokens?.Count ?? 0} items, viewName: '{viewName}'");
                return null;
            }

            // Use the TokenUtils helper method to find the parameterized view
            var paramView = TokenUtils.FindParameterizedView(fileTokens, viewName);
            
            if (paramView != null)
            {
                Formater.DebugMessage($"Found parameterized view '{paramView.ViewName}'");
                return paramView;
            }
            
            string errorMessage = $"View '{viewName}' not found in current scope";
            Formater.DebugMessage(errorMessage);
            Formater.TokenCriticalError(errorMessage, ViewCallToken);
            return null;
        }
    }
}
