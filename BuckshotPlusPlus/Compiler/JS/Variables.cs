using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.JS
{
    public class Variables
    {
        public static string GetVarString(List<Token> ServerSideTokens,List<Token> FunctionToken, int currentToken)
        {
            Token CurrentToken = FunctionToken[currentToken];
            string CurrentTokenName = TokenUtils.GetTokenName(CurrentToken);
            if (CurrentToken.Data.GetType() == typeof(TokenDataVariable))
            {
                TokenDataVariable MyVarData = (TokenDataVariable)CurrentToken.Data;
                string VarString = "let ";

                int TokenCounter = 0;
                int TokensFound = 0;
                foreach (Token ContainerChildToken in FunctionToken)
                {
                    if (CurrentTokenName == TokenUtils.GetTokenName(ContainerChildToken))
                    {
                        TokensFound++;
                    }
                    if (
                        CurrentTokenName == TokenUtils.GetTokenName(ContainerChildToken)
                        && TokenCounter < currentToken
                    )
                    {
                        VarString = "";
                        break;
                    }
                    TokenCounter++;
                }

                if (TokensFound == 1)
                {
                    VarString = "const";
                }
                return VarString + MyVarData.VariableName + " = " + MyVarData.GetCompiledVariableData(ServerSideTokens);
            }

            return "";
        }
    }
}
