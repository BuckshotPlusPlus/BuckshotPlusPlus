using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus.Compiler.JS
{
    public class Variables
    {
        public static string GetVarString(List<Token> FunctionToken, int currentToken)
        {
            Token CurrentToken = FunctionToken[currentToken];
            string CurrentTokenName = TokenUtils.GetTokenName(CurrentToken);
            if(CurrentToken.Data.GetType() == typeof(TokenDataVariable))
            {
                TokenDataVariable MyVarData = (TokenDataVariable)CurrentToken.Data;
                string VarString = "let ";

                int TokenCounter = 0;
                int TokensFound = 0;
                foreach (Token ContainerChildToken in FunctionToken)
                {
                    if(CurrentTokenName == TokenUtils.GetTokenName(ContainerChildToken))
                    {
                        TokensFound++;
                    }
                    if (CurrentTokenName == TokenUtils.GetTokenName(ContainerChildToken) && TokenCounter < currentToken)
                    {
                        VarString = "";
                        break;
                    }
                    TokenCounter++;
                }

                if(TokensFound == 1)
                {
                    VarString = "const";
                }
                return VarString + MyVarData.VariableName + " = " + MyVarData.VariableData;
            }

            return "";
        }
    }
}
