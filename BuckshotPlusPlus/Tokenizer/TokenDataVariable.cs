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

        public TokenDataVariable(Token MyToken)
        {
            VariableToken = MyToken;
            MyToken.Type = "variable";
            string[] MyVariableParams = Formater.SafeSplit(MyToken.LineData, ' ').ToArray();
            //Console.WriteLine(MyVariableParams.Length);
            // check if all parameters of a vriables are present

            if (Formater.SafeContains(MyToken.LineData, '+'))
            {
                this.VariableName = MyVariableParams[0];
                this.VariableData = MyVariableParams[2];
                this.VariableType = "multiple";
            }
            else if (MyVariableParams.Length == 3)
            {
                this.VariableType = FindVariableType(MyVariableParams[2], MyToken);
                this.VariableName = MyVariableParams[0];
                this.VariableData = MyVariableParams[2];

                string[] VariablePath = VariableName.Split('.');
                if(VariablePath.Length > 1)
                {
                    MyToken.Type = "edit";
                }
            }
            else if (MyVariableParams.Length == 1)
            {
                this.VariableName = "";
                this.VariableData = MyVariableParams[0];
                this.VariableType = FindVariableType(MyVariableParams[0], MyToken);
            }
            else if (MyVariableParams.Length == 4)
            {
                this.VariableType = FindVariableType(MyVariableParams[2], MyToken);
                this.VariableName = MyVariableParams[0];
                this.VariableData = MyVariableParams[2];
            }
            else
            {
                Formater.TokenCriticalError("Invalid variable init ", MyToken);
            }

            if (this.VariableType == "")
            {
                Formater.TokenCriticalError("Unknown variable type ", MyToken);
            }

            if (this.VariableType == "string")
            {
                this.VariableData = GetValueFromString(this.VariableData, MyToken);
            }

            //Console.WriteLine("I found a variable of type " + this.VariableType + " and name : " + this.VariableName + " Value : " + this.VariableData);

            if (this.VariableType == "ref")
            {
                this.RefData = TokenUtils.FindTokenByName(
                    MyToken.MyTokenizer.FileTokens,
                    this.VariableData
                );
                if (RefData == null)
                {
                    if (MyToken.Parent != null)
                    {
                        this.RefData = TokenUtils.FindTokenByName(
                            MyToken.Parent.ContainerData,
                            this.VariableData
                        );
                    }
                }
            }
        }

        public static string GetValueFromString(string InitialValue, Token MyToken)
        {
            if (
                    InitialValue[0] != '"'
                )
            {
                Formater.TokenCriticalError("Invalid string value", MyToken);
            }
            return InitialValue.Substring(1, InitialValue.Length - 2);
        }

        public static string FindVariableType(string Value, Token MyToken)
        {
            int VariableIntData = 0;
            float VariableFloatData = 0;
            bool VariableBoolData = false;

            if (Value[0] == '[' && Value[Value.Length - 1] == ']')
            {
                return "array";
            }
            else if (Value.Contains('"'))
            {
                return "string";
            }
            else if (int.TryParse(Value, out VariableIntData))
            {
                return "int";
            }
            else if (float.TryParse(Value, out VariableFloatData))
            {
                return "float";
            }
            else if (bool.TryParse(Value, out VariableBoolData))
            {
                return "bool";
            } //else if(TokenUtils.FindTokenByName(MyToken.MyTokenizer.FileTokens,Value) != null)
            else
            {
                return "ref";
            }
            //Formater.TokenCriticalError("Unknown variable type ", MyToken);
        }

        public static bool IsTokenDataVariable(Token MyToken)
        {
            if (Formater.SafeContains(MyToken.LineData, '='))
            {
                return true;
            }
            else if (FindVariableType(MyToken.LineData, MyToken) != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetCompiledVariableData(List<Token> FileTokens, bool compile_ref = false)
        {
            if(this.VariableType == "multiple") {
                List<string> Variables = Formater.SafeSplit(this.VariableData, '+');

                string Result = "";

                foreach (string Variable in Variables)
                {
                    string SafeVariableType = FindVariableType(Variable, null);

                    if(SafeVariableType == "string") {
                        Result += GetValueFromString(Variable, VariableToken);
                    }else if(SafeVariableType == "ref") {
                        TokenDataVariable FoundToken = TokenUtils.FindTokenDataVariableByName(FileTokens, Variable);
                        if(FoundToken != null)
                        {
                            Result += FoundToken.VariableData;
                        }
                        else
                        {
                            Formater.RuntimeError("Token not found!", this.VariableToken);
                        }
                        
                    }
                }
                return Result;
            }else if(this.VariableType == "ref" && compile_ref)
            {
                TokenDataVariable FoundToken = TokenUtils.FindTokenDataVariableByName(FileTokens, this.VariableData);
                if (FoundToken != null)
                {
                    return FoundToken.VariableData;
                }
                else
                {
                    Formater.RuntimeError("Token not found!", this.VariableToken);
                }
            }

            return this.VariableData;
        } 
    }
}
