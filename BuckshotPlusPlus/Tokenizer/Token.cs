using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    class Token
    {
        public string Type { get; set; }
        public TokenData Data { get; set; }

        public Token(string LineData, int LineNumber)
        {

            // If Line Contains "=" load data of a variable

            if (Formater.SafeContains(LineData,'='))
            {
                this.Type = "variable";
                string[] MyVariableParams = Formater.SafeSplit(LineData,' ').ToArray();
                Console.WriteLine(MyVariableParams.Length);
                TokenDataVariable MyTokenData = new TokenDataVariable();
                // check if all parameters of a vriables are present
                if (MyVariableParams.Length != 4)
                {
                    Formater.CriticalError("Invalid variable init line : " + LineNumber + Environment.NewLine + "=> " + LineData);
                }
                MyTokenData.VariableType = MyVariableParams[0];
                MyTokenData.VariableName = MyVariableParams[1];
                MyTokenData.VariableData = MyVariableParams[3];

                

                if (MyTokenData.VariableType == "string")
                {
                    if (MyTokenData.VariableData[0] != '"' || MyTokenData.VariableData[MyTokenData.VariableData.Length - 1] != '"')
                    {
                        Formater.CriticalError("Invalid string value line : " + LineNumber + Environment.NewLine + "=> " + LineData);
                    }
                    MyTokenData.VariableData = MyVariableParams[3].Substring(1, MyVariableParams[3].Length - 2);
                }


                this.Data = MyTokenData;

                Console.WriteLine("I found a variable of type " + MyTokenData.VariableType + " and name : " + MyTokenData.VariableName + " Value : " + MyTokenData.VariableData);
            }

        }
    }
}
