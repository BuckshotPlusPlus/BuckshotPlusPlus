using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    public class TokenUtils
    {
        public static string GetTokenName(Token MyToken)
        {
            if (MyToken.Data.GetType() == typeof(TokenDataVariable))
            {
                TokenDataVariable MyVar = (TokenDataVariable)MyToken.Data;
                return MyVar.VariableName;
            }
            else if (MyToken.Data.GetType() == typeof(TokenDataContainer))
            {
                TokenDataContainer MyContainer = (TokenDataContainer)MyToken.Data;
                return MyContainer.ContainerName;
            }
            //Formater.CriticalError(TokenName + " does not exist");
            return null;
        }
        public static Token FindTokenByName(List<Token> MyTokenList, string TokenName)
        {
            foreach(Token MyToken in MyTokenList)
            {
                if(MyToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable MyVar = (TokenDataVariable)MyToken.Data;
                    if(MyVar.VariableName == TokenName)
                    {
                        return MyToken;
                    }
                }else if (MyToken.Data.GetType() == typeof(TokenDataContainer))
                {
                    TokenDataContainer MyContainer = (TokenDataContainer)MyToken.Data;
                    if (MyContainer.ContainerName == TokenName)
                    {
                        return MyToken;
                    }
                }
            }
            //Formater.CriticalError(TokenName + " does not exist");
            return null;
        }

        public static TokenDataVariable FindTokenDataVariableByName(List<Token> MyTokenList, string TokenName)
        {
            foreach (Token MyToken in MyTokenList)
            {
                if (MyToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable MyVar = (TokenDataVariable)MyToken.Data;
                    if (MyVar.VariableName == TokenName)
                    {
                        return MyVar;
                    }
                }
            }
            return null;
        }

        public static TokenDataContainer FindTokenDataContainerByName(List<Token> MyTokenList, string TokenName)
        {
            foreach (Token MyToken in MyTokenList)
            {
                if (MyToken.Data.GetType() == typeof(TokenDataContainer))
                {
                    TokenDataContainer MyContainer = (TokenDataContainer)MyToken.Data;
                    if (MyContainer.ContainerName == TokenName)
                    {
                        return MyContainer;
                    }
                }
            }
            //Formater.CriticalError(TokenName + " does not exist");
            return null;
        }
    }
}
