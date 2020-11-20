using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    class TokenDataVariable : TokenData
    {
        public string VariableType { get; set; }
        public string VariableData { get; set; }
        public string VariableName { get; set; }

        public TokenDataVariable(Token MyToken, string LineData, int LineNumber)
        {
            MyToken.Type = "variable";
            string[] MyVariableParams = Formater.SafeSplit(LineData, ' ').ToArray();
            Console.WriteLine(MyVariableParams.Length);
            // check if all parameters of a vriables are present
            if (MyVariableParams.Length == 3)
            {
                this.VariableType = FindVariableType(MyVariableParams[2], LineNumber);
                this.VariableName = MyVariableParams[0];
                this.VariableData = MyVariableParams[2];
            }else if (MyVariableParams.Length == 1)
            {
                this.VariableName = "";
                this.VariableData = MyVariableParams[0];
                this.VariableType = FindVariableType(MyVariableParams[0], LineNumber);
            }
            else
            {
                Formater.CriticalError("Invalid variable init line : " + LineNumber + Environment.NewLine + "=> " + LineData);
            }
            



            if (this.VariableType == "string")
            {
                if (this.VariableData[0] != '"' || this.VariableData[this.VariableData.Length - 1] != '"')
                {
                    Formater.CriticalError("Invalid string value line : " + LineNumber + Environment.NewLine + "=> " + LineData);
                }
                this.VariableData = this.VariableData.Substring(1, this.VariableData.Length - 2);
            }


            MyToken.Data = this;

            Console.WriteLine("I found a variable of type " + this.VariableType + " and name : " + this.VariableName + " Value : " + this.VariableData);

        }

        public static string FindVariableType(string LineData, int LineNumber)
        {

            int VariableIntData = 0;
            float VariableFloatData = 0;
            bool VariableBoolData = false;

            if (LineData.Contains('"'))
            {
                return "string";
            }else if (int.TryParse(LineData, out VariableIntData))
            {
                return "int";
            }else if (float.TryParse(LineData, out VariableFloatData))
            {
                return "float";
            }else if(bool.TryParse(LineData, out VariableBoolData))
            {
                return "bool";
            }
            Formater.CriticalError("Unknown variable type at line : " + LineNumber + Environment.NewLine + "=> " + LineData);
            return "";
        }

        public static bool IsTokenDataVariable(string LineData)
        {
            return Formater.SafeContains(LineData, '=');
        }
    }
}
