using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Token FindTokenByName(List<Token> MyTokenList, string TokenName, bool ReturnParent = false)
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
                            if (Remain > 0 && !ReturnParent)
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

        public static bool EditTokenData(List<Token> MyTokenList, Token MyToken)
        {
            TokenDataVariable Var = (TokenDataVariable)MyToken.Data;
            Token TokenToEdit = FindTokenByName(MyTokenList, Var.VariableName);
            if(TokenToEdit == null)
            {
                Token ParentToken = FindTokenByName(MyTokenList, Var.VariableName, true);
                if(ParentToken == null)
                {
                    Formater.TokenCriticalError("Can't find token with name: " + Var.VariableName, MyToken);
                    return false;
                }
                TokenDataContainer Container = (TokenDataContainer)ParentToken.Data;
                Var.VariableName = Var.VariableName.Split('.').Last();
                Container.ContainerData.Add(MyToken);
                return true;
                
            }
            TokenDataVariable MyVar = (TokenDataVariable)TokenToEdit.Data;
            MyVar.VariableData = Var.GetCompiledVariableData(MyTokenList);

            if(Var.VariableType == "multiple") {
                MyVar.VariableType = "string";
            }
            else
            {
                MyVar.VariableType = Var.VariableType;
            }
            return true;
        }

        public static bool SafeEditTokenData(string LineData,List<Token> MyTokenList, Token MyToken)
        {
            if(Formater.SafeSplit(LineData, '.').Count > 1)
            {
                return EditTokenData(MyTokenList, MyToken);
            }
            return false;
        }

        public static void EditAllTokensOfContainer(List<Token> FileTokens,Token MyContainer)
        {
            
            TokenDataContainer PageTokenDataContainer = (TokenDataContainer)MyContainer.Data;
            if (PageTokenDataContainer == null)
            {
                Formater.TokenCriticalError("The procided token is not a container!", MyContainer);
            }
            else
            {
                foreach(Token ChildToken in PageTokenDataContainer.ContainerData)
                {
                    TokenDataVariable VarToken = (TokenDataVariable)ChildToken.Data;
                    if(VarToken != null)
                    {
                        SafeEditTokenData(VarToken.VariableName, FileTokens, ChildToken);

                        if(VarToken.VariableType == "ref")
                        {
                            Token ReferencedToken = TokenUtils.FindTokenByName(FileTokens,VarToken.VariableData);

                            if (ReferencedToken == null)
                            {
                                Formater.TokenCriticalError("Token not found " + VarToken.VariableData, ChildToken);
                            }
                            else
                            {
                                if (ReferencedToken.Data.GetType() == typeof(TokenDataContainer))
                                {
                                    TokenDataContainer ContainerToken = (TokenDataContainer)ReferencedToken.Data;
                                    if (ContainerToken != null)
                                    {
                                        EditAllTokensOfContainer(FileTokens, ReferencedToken);
                                    }
                                }
                                
                            }                            
                        }
                    }
                }
            }
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

        public static TokenDataVariable TryFindTokenDataVariableValueByName(
            List<Token> FileTokens,
            List<Token> LocalTokenList,
            string TokenName
            )
        {
            Token FoundToken = TryFindTokenValueByName(FileTokens, LocalTokenList, TokenName);
            if (FoundToken != null)
            {
                if (FoundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable MyVar = (TokenDataVariable)FoundToken.Data;
                    return MyVar;
                    
                }
            }
            return null;
        }

        public static Token TryFindTokenValueByName(
            List<Token> FileTokens,
            List<Token> LocalTokenList,
            string TokenName
            )
        {
            Token FoundToken = FindTokenByName(LocalTokenList, TokenName);
            if (FoundToken != null)
            {
                if (FoundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable MyVar = (TokenDataVariable)FoundToken.Data;
                    if(MyVar.VariableType == "ref")
                    {
                        return TryFindTokenValueByName(FileTokens, FileTokens, MyVar.VariableData);
                    }
                    else
                    {
                        return FoundToken;
                    }

                }
                else
                {
                    return FoundToken;
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
            //Formater.CriticalError(TokenName + " does not exist");=
        }
    }
}
