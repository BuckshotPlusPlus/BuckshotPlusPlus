using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class TokenDataVariable : TokenData
    {
        public string VariableType { get; set; }
        public string VariableData { get; set; }
        public string VariableName { get; set; }
        public Token RefData { get; set; }
        public Token VariableToken { get; set; }

        public TokenDataVariable(Token myToken)
        {
            VariableToken = myToken;
            myToken.Type = "variable";
            string[] myVariableParams = Formater.SafeSplit(myToken.LineData, ' ').ToArray();
            //Console.WriteLine(MyVariableParams.Length);
            // check if all parameters of a vriables are present

            if (Formater.SafeContains(myToken.LineData, '+'))
            {
                this.VariableName = myVariableParams[0];
                this.VariableData = myVariableParams[2];
                this.VariableType = "multiple";
            }
            else if (myVariableParams.Length == 3)
            {
                this.VariableType = FindVariableType(myVariableParams[2], myToken);
                this.VariableName = myVariableParams[0];
                this.VariableData = myVariableParams[2];

                string[] variablePath = VariableName.Split('.');
                if(variablePath.Length > 1)
                {
                    myToken.Type = "edit";
                }
            }
            else if (myVariableParams.Length == 1)
            {
                this.VariableName = "";
                this.VariableData = myVariableParams[0];
                this.VariableType = FindVariableType(myVariableParams[0], myToken);
            }
            else if (myVariableParams.Length == 4)
            {
                this.VariableType = FindVariableType(myVariableParams[2], myToken);
                this.VariableName = myVariableParams[0];
                this.VariableData = myVariableParams[2];
            }

            if (this.VariableType == "")
            {
                return;
            }

            if (this.VariableType == "string")
            {
                this.VariableData = GetValueFromString(this.VariableData, myToken);
            }

            //Console.WriteLine("I found a variable of type " + this.VariableType + " and name : " + this.VariableName + " Value : " + this.VariableData);

            if (this.VariableType == "ref")
            {
                this.RefData = TokenUtils.FindTokenByName(
                    myToken.MyTokenizer.FileTokens,
                    this.VariableData
                );
                if (RefData == null)
                {
                    if (myToken.Parent != null)
                    {
                        this.RefData = TokenUtils.FindTokenByName(
                            myToken.Parent.ContainerData,
                            this.VariableData
                        );
                    }
                }
            }
        }

        public static string GetValueFromString(string initialValue, Token myToken)
        {
            // Trim any whitespace first
            initialValue = initialValue.Trim();

            // Check if the string starts and ends with quotes
            if (initialValue.Length < 2 ||
                (initialValue[0] != '"' && initialValue[0] != '\'') ||
                (initialValue[^1] != '"' && initialValue[^1] != '\''))
            {

                //Formater.TokenCriticalError("Invalid string value", myToken);
                return initialValue;
            }

            // Return the string without the quotes
            return initialValue.Substring(1, initialValue.Length - 2);
        }

        public static string FindVariableType(string value, Token myToken)
        {
            // Trim the value first
            value = value.Trim();

            // Check for array first
            if (value.StartsWith("[") && value.EndsWith("]"))
                return "array";

            // Check for string (both single and double quotes)
            if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                (value.StartsWith("'") && value.EndsWith("'")))
                return "string";

            // Try parsing as other types
            if (int.TryParse(value, out _))
                return "int";
            if (float.TryParse(value, out _))
                return "float";
            if (bool.TryParse(value, out _))
                return "bool";

            // If none of the above, treat as reference
            return "ref";
        }

        public static bool IsTokenDataVariable(Token myToken)
        {
            if (Formater.SafeContains(myToken.LineData, '='))
            {
                return true;
            }
            else if (FindVariableType(myToken.LineData, myToken) != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetCompiledVariableData(List<Token> fileTokens, bool compileRef = false)
        {
            if (this.VariableType == "multiple")
            {
                List<string> variables = Formater.SafeSplit(this.VariableData, '+');
                string result = "";

                foreach (string variable in variables)
                {
                    string trimmedVar = variable.Trim();
                    string safeVariableType = FindVariableType(trimmedVar, null);

                    if (safeVariableType == "string")
                    {
                        // Already a string literal, just remove the quotes and add to result
                        result += GetValueFromString(trimmedVar, VariableToken);
                    }
                    else if (safeVariableType == "ref")
                    {
                        TokenDataVariable foundToken = TokenUtils.FindTokenDataVariableByName(fileTokens, trimmedVar);
                        if (foundToken != null)
                        {
                            string value = foundToken.VariableData;

                            // If the value isn't already a quoted string and we're in a string context,
                            // we should use the raw value without quotes
                            if (foundToken.VariableType == "string")
                            {
                                value = GetValueFromString(value, foundToken.VariableToken);
                            }

                            result += value;
                        }
                        else
                        {
                            Formater.RuntimeError($"Token not found: {trimmedVar}", this.VariableToken);
                        }
                    }
                }
                return result;
            }
            else if (this.VariableType == "ref")
            {
                var sourceVar = TokenUtils.ResolveSourceReference(fileTokens, this.VariableData);
                if (sourceVar != null)
                {
                    return sourceVar.VariableData;
                }

                if (compileRef)
                {
                    TokenDataVariable foundToken = TokenUtils.FindTokenDataVariableByName(fileTokens, this.VariableData);
                    if (foundToken != null)
                    {
                        return foundToken.VariableData;
                    }
                    else
                    {
                        Formater.RuntimeError("Token not found!", this.VariableToken);
                    }
                }
            }

            return this.VariableData;
        }
    }
}
