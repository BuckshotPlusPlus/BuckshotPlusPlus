using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    class TokenDataFunctionCall : TokenData
    {
        public string FuncName { get; set; }
        public List<Token> FuncArgs { get; set; }

        public TokenDataFunctionCall(Token MyToken)
        {
            MyToken.Type = "function_call";

            this.FuncName = GetFunctionCallName(MyToken.LineData, MyToken);
            this.FuncArgs = GetFunctionArgs(MyToken.LineData, MyToken);

            Console.WriteLine(
                "I found a function call of name : "
                    + this.FuncName
                    + " and "
                    + this.FuncArgs.Count
                    + " args"
            );
        }

        public static bool IsTokenDataFunctionCall(Token MyToken)
        {
            return Formater.SafeContains(MyToken.LineData, '(');
        }

        public static string GetFunctionCallName(string Value, Token MyToken)
        {
            string FunName = "";
            foreach (char c in Value)
            {
                if (c != '(')
                {
                    FunName += c;
                }
                else
                {
                    return FunName;
                }
            }
            Formater.TokenCriticalError("Invalid function name", MyToken);
            return "";
        }

        public static List<Token> GetFunctionArgs(string Value, Token MyToken)
        {
            List<Token> FunctionArgs = new List<Token>();
            string CurrentVar = "";
            bool isArgs = false;
            int SubPar = 0;

            foreach (char c in Value)
            {
                if (c == '(')
                {
                    isArgs = true;
                    SubPar++;
                }
                else if (c == ')')
                {
                    SubPar--;
                    if (SubPar == 0)
                    {
                        Token MyNewToken = new Token(
                            MyToken.FileName,
                            CurrentVar,
                            MyToken.LineNumber,
                            MyToken.MyTokenizer
                        );
                        new TokenDataVariable(MyNewToken);
                        FunctionArgs.Add(MyNewToken);
                        return FunctionArgs;
                    }
                }
                else
                {
                    if (isArgs && c == ',')
                    {
                        Token MyNewToken = new Token(
                            MyToken.FileName,
                            CurrentVar,
                            MyToken.LineNumber,
                            MyToken.MyTokenizer
                        );
                        new TokenDataVariable(MyNewToken);
                        FunctionArgs.Add(MyNewToken);
                        CurrentVar = "";
                    }
                    else if (isArgs)
                    {
                        CurrentVar += c;
                    }
                }
            }
            Formater.TokenCriticalError("Invalid function args", MyToken);
            return FunctionArgs;
        }
    }
}
