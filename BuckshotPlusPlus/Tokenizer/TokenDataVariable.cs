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
            if (MyVariableParams.Length != 4)
            {
                Formater.CriticalError("Invalid variable init line : " + LineNumber + Environment.NewLine + "=> " + LineData);
            }
            this.VariableType = MyVariableParams[0];
            this.VariableName = MyVariableParams[1];
            this.VariableData = MyVariableParams[3];



            if (this.VariableType == "string")
            {
                if (this.VariableData[0] != '"' || this.VariableData[this.VariableData.Length - 1] != '"')
                {
                    Formater.CriticalError("Invalid string value line : " + LineNumber + Environment.NewLine + "=> " + LineData);
                }
                this.VariableData = MyVariableParams[3].Substring(1, MyVariableParams[3].Length - 2);
            }

            int VariableIntData = 0;

            if(this.VariableType == "int" && !int.TryParse(this.VariableData,out VariableIntData))
            {
                Formater.CriticalError("Invalid int value line : " + LineNumber + Environment.NewLine + "=> " + LineData);
            }

            float VariableFloatData = 0;
            if (this.VariableType == "float" && !float.TryParse(this.VariableData, out VariableFloatData))
            {
                Formater.CriticalError("Invalid float value line : " + LineNumber + Environment.NewLine + "=> " + LineData);
            }

            bool VariableBoolData = false;
            if (this.VariableType == "bool" && !bool.TryParse(this.VariableData, out VariableBoolData))
            {
                Formater.CriticalError("Invalid bool value line : " + LineNumber + Environment.NewLine + "=> " + LineData);
            }


            MyToken.Data = this;

            Console.WriteLine("I found a variable of type " + this.VariableType + " and name : " + this.VariableName + " Value : " + this.VariableData);

        }

        public static bool IsTokenDataVariable(string LineData)
        {
            return Formater.SafeContains(LineData, '=');
        }
    }
}
