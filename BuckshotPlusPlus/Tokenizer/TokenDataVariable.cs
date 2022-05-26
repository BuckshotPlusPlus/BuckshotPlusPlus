using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    public class TokenDataVariable : TokenData
    {
        public string VariableType { get; set; }
        public string VariableData { get; set; }
        public string VariableName { get; set; }
        public Token RefData { get; set; }

        public TokenDataVariable(Token MyToken)
        {
            MyToken.Type = "variable";
            string[] MyVariableParams = Formater.SafeSplit(MyToken.LineData, ' ').ToArray();
            //Console.WriteLine(MyVariableParams.Length);
            // check if all parameters of a vriables are present
            if (MyVariableParams.Length == 3)
            {
                this.VariableType = FindVariableType(MyVariableParams[2], MyToken);
                this.VariableName = MyVariableParams[0];
                this.VariableData = MyVariableParams[2];
            }else if (MyVariableParams.Length == 1)
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
            
            if(this.VariableType == "")
            {
                Formater.TokenCriticalError("Unknown variable type ", MyToken);
            }

            

            if (this.VariableType == "string")
            {
                if (this.VariableData[0] != '"' || this.VariableData[this.VariableData.Length - 1] != '"')
                {
                    Formater.TokenCriticalError("Invalid string value", MyToken);
                }
                this.VariableData = this.VariableData.Substring(1, this.VariableData.Length - 2);
            }

            //Console.WriteLine("I found a variable of type " + this.VariableType + " and name : " + this.VariableName + " Value : " + this.VariableData);

            if (this.VariableType == "ref")
            {
                this.RefData = TokenUtils.FindTokenByName(MyToken.MyTokenizer.FileTokens, this.VariableData);
                if(RefData == null)
                {
                    if(MyToken.Parent != null)
                    {
                        this.RefData = TokenUtils.FindTokenByName(MyToken.Parent.ContainerData, this.VariableData);
                    }
                }
            }

        }

        public static string FindVariableType(string Value,Token MyToken)
        {

            int VariableIntData = 0;
            float VariableFloatData = 0;
            bool VariableBoolData = false;

            if(Value[0] == '[' && Value[Value.Length - 1] == ']')
            {
                return "array";
            }
            else if (Value.Contains('"'))
            {
                return "string";
            }else if (int.TryParse(Value, out VariableIntData))
            {
                return "int";
            }else if (float.TryParse(Value, out VariableFloatData))
            {
                return "float";
            }else if(bool.TryParse(Value, out VariableBoolData))
            {
                return "bool";
            }//else if(TokenUtils.FindTokenByName(MyToken.MyTokenizer.FileTokens,Value) != null)
            else
            {
                return "ref";
            }
            //Formater.TokenCriticalError("Unknown variable type ", MyToken);
            return "";
        }

        public static bool IsTokenDataVariable(Token MyToken)
        {
            if(Formater.SafeContains(MyToken.LineData, '='))
            {
                return true;
            }else if(FindVariableType(MyToken.LineData, MyToken) != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
