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
            string[] SubTokenNames = TokenName.Split('.');
            int Remain = SubTokenNames.Length;
            foreach (string LocalTokenName in SubTokenNames)
            {
                Remain--;
                foreach (Token MyToken in MyTokenList)
                {
                    if (MyToken.Data.GetType() == typeof(TokenDataVariable))
                    {
                        TokenDataVariable MyVar = (TokenDataVariable)MyToken.Data;
                        if (MyVar.VariableName == LocalTokenName)
                        {
                            if (Remain > 0)
                            {
                                Formater.TokenCriticalError("Not a container!", MyToken);
                            }
                            else
                            {
                                //Formater.Warn("Token found");
                                return MyToken;
                            }
                        }
                    }
                    else if (MyToken.Data.GetType() == typeof(TokenDataContainer))
                    {
                        TokenDataContainer MyContainer = (TokenDataContainer)MyToken.Data;
                        if (MyContainer.ContainerName == LocalTokenName)
                        {
                            if (Remain > 0)
                            {
                                MyTokenList = MyContainer.ContainerData;
                                break;
                            }
                            else
                            {
                                return MyToken;
                            }
                        }
                    }
                }
            }
            //Formater.CriticalError(TokenName + " does not exist");
            return null;
        }

        public static TokenDataVariable FindTokenDataVariableByName(
            List<Token> MyTokenList,
            string TokenName
        )
        {
            Token FoundToken = FindTokenByName(MyTokenList, TokenName);
            if (FoundToken != null)
            {
                if (FoundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable MyVar = (TokenDataVariable)FoundToken.Data;
                    return MyVar;
                }
            }
            else
            {
                //Formater.Warn("Token of name : " + TokenName + " not found");
            }
            return null;
        }

        public static TokenDataContainer FindTokenDataContainerByName(
            List<Token> MyTokenList,
            string TokenName
        )
        {
            Token FoundToken = FindTokenByName(MyTokenList, TokenName);
            if (FoundToken.Data.GetType() == typeof(TokenDataContainer))
            {
                TokenDataContainer MyVar = (TokenDataContainer)FoundToken.Data;
                return MyVar;
            }
            return null;
            //Formater.CriticalError(TokenName + " does not exist");
            return null;
        }
    }
}
