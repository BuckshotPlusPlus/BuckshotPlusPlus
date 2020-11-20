using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    class TokenDataFunctionCall : TokenData
    {
        public string FuncName { get; set; }
        public List<Token> FuncArgs { get; set; }

        public TokenDataFunctionCall(Token MyToken, string LineData, int LineNumber)
        {
            MyToken.Type = "function_call";

            this.FuncName = GetFunctionCallName(LineData,LineNumber);
            this.FuncArgs = GetFunctionArgs(LineData, LineNumber);


            Console.WriteLine("I found a function call of name : " + this.FuncName + " and " + this.FuncArgs.Count + " args");
        }

        public static bool IsTokenDataFunctionCall(string LineData)
        {
            return Formater.SafeContains(LineData, '(');
        }

        public static string GetFunctionCallName(string LineData, int LineNumber)
        {
            string FunName = "";
            foreach(char c in LineData)
            {
                if(c != '(')
                {
                    FunName += c;
                }
                else
                {
                    return FunName;
                }
            }

            Formater.CriticalError("Invalid function name line : " + LineNumber + Environment.NewLine + "   => " + LineData);
            return "";
        }

        public static List<Token> GetFunctionArgs(string LineData, int LineNumber)
        {
            List<Token> FunctionArgs = new List<Token>();
            string CurrentVar = "";
            bool isArgs = false;
            int SubPar = 0;

            foreach(char c in LineData)
            {
                if(c == '(')
                {
                    isArgs = true;
                    SubPar++;
                }else if (c == ')')
                {
                    SubPar--;
                    if(SubPar == 0)
                    {
                        Token MyToken = new Token(CurrentVar, LineNumber);
                        new TokenDataVariable(MyToken, CurrentVar, LineNumber);
                        FunctionArgs.Add(MyToken);
                        return FunctionArgs;
                    }
                }
                else
                {
                    if (isArgs && c == ',')
                    {
                        Token MyToken = new Token(CurrentVar, LineNumber);
                        new TokenDataVariable(MyToken, CurrentVar, LineNumber);
                        FunctionArgs.Add(MyToken);
                        CurrentVar = "";
                    }
                    else if(isArgs)
                    {
                        CurrentVar += c;
                    }
                }
            }
            Formater.CriticalError("Invalid function args line : " + LineNumber + Environment.NewLine + "   => " + LineData);
            return FunctionArgs;
        }
    }
}
