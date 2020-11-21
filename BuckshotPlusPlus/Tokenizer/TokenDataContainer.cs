using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    class TokenDataContainer : TokenData
    {
        public string ContainerName { get; set; }
        public List<Token> ContainerData { get; set; }
        public string ContainerType { get; set; }

        public TokenDataContainer(Token MyToken)
        {
            List<string> LinesData = Formater.SafeSplit(MyToken.LineData, '\n');
            this.ContainerData = new List<Token>();
            this.ContainerType = "";
            this.ContainerName = "";

            string[] SupportedContainerTypes = { "object", "function" };
            int OpenCount = 0;
            List<string> ChildContainerLines = new List<string>();

            foreach(string LineData in LinesData)
            {
                if (Formater.SafeContains(LineData, '{')) {
                    OpenCount++;
                    
                    if(OpenCount == 1)
                    {
                        Console.WriteLine("I found a new object");
                        // SPLIT FIRST LINE INTO AN ARRAY
                        List<string> MyArgs = Formater.SafeSplit(LineData, ' ');

                        // STORE CONTAINER NAME
                        if (MyArgs[1][MyArgs[1].Length - 1] == '{')
                        {
                            this.ContainerName = MyArgs[1].Substring(0, MyArgs[1].Length - 1);
                        } else
                        {
                            this.ContainerName = MyArgs[1];
                        }

                        // CHECK AND STORE CONTAINER TYPE (OBJECT, FUNCTION)
                        foreach (string ContainerType in SupportedContainerTypes)
                        {
                            if(MyArgs[0] == ContainerType)
                            {
                                this.ContainerType = ContainerType;
                            }
                        }
                        if(this.ContainerType == "")
                        {
                            Formater.TokenCriticalError("Invalid container type", MyToken);
                        }
                    }
                    
                }
                else if (OpenCount == 1 && !Formater.SafeContains(LineData, '}'))
                {
                    ContainerData.Add(new Token(MyToken.FileName, LineData, MyToken.LineNumber + LinesData.IndexOf(LineData) - 1, MyToken.MyTokenizer));
                }

                if (OpenCount == 2) {
                    ChildContainerLines.Add(LineData);
                }

                if (Formater.SafeContains(LineData, '}') && OpenCount == 2) {
                    OpenCount--;
                    ContainerData.Add(new Token(MyToken.FileName, String.Join('\n', ChildContainerLines), MyToken.LineNumber + LinesData.IndexOf(ChildContainerLines[0]) - 4, MyToken.MyTokenizer));
                    ChildContainerLines = new List<string>();
                }
                else if (Formater.SafeContains(LineData, '}') && OpenCount == 1)
                {
                    Formater.DebugMessage("Container found of name : " + ContainerName + " of type : " + ContainerType + " with " + ContainerData.Count + " Children");
                }
                else if (Formater.SafeContains(LineData, '}'))
                {
                    OpenCount--;
                }
                
            }
        }
        public static bool IsTokenDataContainer(Token MyToken)
        {
            if(Formater.SafeSplit(MyToken.LineData,' ')[0] == "object" || Formater.SafeSplit(MyToken.LineData, ' ')[0] == "function")
            {
                return true;
            }
            return false;
        }
    }
}
