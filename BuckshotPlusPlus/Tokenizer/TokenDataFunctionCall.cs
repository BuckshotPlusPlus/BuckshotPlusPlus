using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    class TokenDataFunctionCall : TokenData
    {
        public string FuncName { get; set; }
        public List<Token> FuncArgs { get; set; }

        public TokenDataFunctionCall(Token myToken)
        {
            myToken.Type = "function_call";

            this.FuncName = GetFunctionCallName(myToken.LineData, myToken);
            this.FuncArgs = GetFunctionArgs(myToken.LineData, myToken);
        }
        

        public static bool IsTokenDataFunctionCall(Token myToken)
        {
            int parenPos = myToken.LineData.IndexOf('(');
            Formater.DebugMessage(parenPos.ToString() + " -> " + StringHandler.IsInsideQuotes(myToken.LineData, parenPos).ToString());
            return parenPos != -1 && !StringHandler.IsInsideQuotes(myToken.LineData, parenPos);
        }

        public static string GetFunctionCallName(string value, Token myToken)
        {
            string funName = "";
            foreach (char c in value)
            {
                if (c != '(')
                {
                    funName += c;
                }
                else
                {
                    return funName;
                }
            }
            Formater.TokenCriticalError("Invalid function name", myToken);
            return "";
        }

        public static List<Token> GetFunctionArgs(string value, Token myToken)
        {
            List<Token> functionArgs = new List<Token>();
            string currentVar = "";
            bool isArgs = false;
            int subPar = 0;

            foreach (char c in value)
            {
                if (c == '(')
                {
                    isArgs = true;
                    subPar++;
                }
                else if (c == ')')
                {
                    subPar--;
                    if (subPar == 0)
                    {
                        Token myNewToken = new Token(
                            myToken.FileName,
                            currentVar,
                            myToken.LineNumber,
                            myToken.MyTokenizer
                        );
                        new TokenDataVariable(myNewToken);
                        functionArgs.Add(myNewToken);
                        return functionArgs;
                    }
                }
                else
                {
                    if (isArgs && c == ',')
                    {
                        Token myNewToken = new Token(
                            myToken.FileName,
                            currentVar,
                            myToken.LineNumber,
                            myToken.MyTokenizer
                        );
                        new TokenDataVariable(myNewToken);
                        functionArgs.Add(myNewToken);
                        currentVar = "";
                    }
                    else if (isArgs)
                    {
                        currentVar += c;
                    }
                }
            }
            Formater.TokenCriticalError("Invalid function args", myToken);
            return functionArgs;
        }
    }
}
