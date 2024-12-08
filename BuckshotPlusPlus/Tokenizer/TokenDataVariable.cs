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
            else
            {
                Formater.TokenCriticalError("Invalid variable init ", myToken);
            }

            if (this.VariableType == "")
            {
                Formater.TokenCriticalError("Unknown variable type ", myToken);
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
            if (
                    initialValue[0] != '"'
                )
            {
                Formater.TokenCriticalError("Invalid string value", myToken);
            }
            return initialValue.Substring(1, initialValue.Length - 2);
        }

        public static string FindVariableType(string value, Token myToken)
        {
            int variableIntData = 0;
            float variableFloatData = 0;
            bool variableBoolData = false;

            if (value[0] == '[' && value[^1] == ']')
            {
                return "array";
            }
            else if (value.Contains('"'))
            {
                return "string";
            }
            else if (int.TryParse(value, out variableIntData))
            {
                return "int";
            }
            else if (float.TryParse(value, out variableFloatData))
            {
                return "float";
            }
            else if (bool.TryParse(value, out variableBoolData))
            {
                return "bool";
            } //else if(TokenUtils.FindTokenByName(MyToken.MyTokenizer.FileTokens,Value) != null)
            else
            {
                return "ref";
            }
            //Formater.TokenCriticalError("Unknown variable type ", MyToken);
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
            if(this.VariableType == "multiple") {
                List<string> variables = Formater.SafeSplit(this.VariableData, '+');

                string result = "";

                foreach (string variable in variables)
                {
                    string safeVariableType = FindVariableType(variable, null);

                    if(safeVariableType == "string") {
                        result += GetValueFromString(variable, VariableToken);
                    }else if(safeVariableType == "ref") {
                        TokenDataVariable foundToken = TokenUtils.FindTokenDataVariableByName(fileTokens, variable);
                        if(foundToken != null)
                        {
                            result += foundToken.VariableData;
                        }
                        else
                        {
                            Formater.RuntimeError("Token not found!", this.VariableToken);
                        }
                        
                    }
                }
                return result;
            }else if(this.VariableType == "ref")
            {
                var sourceVar = TokenUtils.ResolveSourceReference(fileTokens, this.VariableData);
                if (sourceVar != null)
                {
                    return sourceVar.VariableData;
                }

                if (compileRef) {
                    Console.WriteLine("Editing ref value for var " + this.VariableName);
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
