using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                            return MyToken;
                        }
                    }
                }
            }

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
            MyVar.VariableType = Var.VariableType == "multiple" ? "string" : Var.VariableType;

            return true;
        }

        public static bool SafeEditTokenData(string LineData,List<Token> MyTokenList, Token MyToken)
        {
            if (Formater.SafeSplit(LineData, '.').Count > 1)
            {
                return EditTokenData(MyTokenList, MyToken);
            }

            return false;
        }

        public static void EditAllTokensOfContainer(List<Token> FileTokens,Token MyContainer)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            TokenDataContainer PageTokenDataContainer = (TokenDataContainer)MyContainer.Data;
            if (PageTokenDataContainer == null)
            {
                stopwatch.Stop();
                Formater.TokenCriticalError("The provided token is not a container!", MyContainer);
            }
            else
            {
                foreach(Token ChildToken in PageTokenDataContainer.ContainerData)
                {
                    if(ChildToken.Data.GetType() == typeof(TokenDataVariable))
                    {
                        TokenDataVariable VarToken = (TokenDataVariable)ChildToken.Data;
                        if (VarToken != null)
                        {
                            SafeEditTokenData(VarToken.VariableName, FileTokens, ChildToken);

                            if (VarToken.VariableType == "ref")
                            {
                                Token ReferencedToken = FindTokenByName(FileTokens, VarToken.VariableData);
                                if (ReferencedToken == null)
                                {
                                    Formater.TokenCriticalError("Token not super found " + VarToken.VariableData, ChildToken);
                                }
                            }
                        }
                    }
                    else if(ChildToken.Data.GetType() == typeof(TokenDataContainer))
                    {
                        TokenDataContainer NewContainerTokenData = (TokenDataContainer)ChildToken.Data;
                        if (NewContainerTokenData.ContainerType == "logic")
                        {
                            // RUN LOGIC TEST
                            TokenDataLogic MyLogic = (TokenDataLogic)NewContainerTokenData.ContainerMetaData;
                            MyLogic.RunLogicTest(FileTokens);

                        }
                    }
                    
                }
            }

            stopwatch.Stop();
            //Formater.SuccessMessage($"It took {stopwatch.ElapsedMilliseconds} ms to run EditAllTokensOfContainer of container {PageTokenDataContainer.ContainerName}");
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

            return null;
        }

        public static TokenDataVariable TryFindTokenDataVariableValueByName(
            List<Token> FileTokens,
            List<Token> LocalTokenList,
            string TokenName,
            bool replaceRef = true
            )
        {
            Token FoundToken = TryFindTokenValueByName(FileTokens, LocalTokenList, TokenName, replaceRef);
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


        public static TokenDataContainer TryFindTokenDataContainerValueByName(
            List<Token> FileTokens,
            List<Token> LocalTokenList,
            string TokenName,
            bool replaceRef = true
            )
        {
            Token FoundToken = TryFindTokenValueByName(FileTokens, LocalTokenList, TokenName, replaceRef);
            if (FoundToken != null)
            {
                if (FoundToken.Data.GetType() == typeof(TokenDataContainer))
                {
                    TokenDataContainer MyContainer = (TokenDataContainer)FoundToken.Data;
                    return MyContainer;
                }
            }
            return null;
        }

        public static Token TryFindTokenValueByName(
            List<Token> FileTokens,
            List<Token> LocalTokenList,
            string TokenName,
            bool replaceRef = true
            )
        {
            Token FoundToken = FindTokenByName(LocalTokenList, TokenName);
            if (FoundToken != null)
            {
                if (FoundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable MyVar = (TokenDataVariable)FoundToken.Data;
                    if(MyVar.VariableType == "ref" && replaceRef)
                    {
                        return TryFindTokenValueByName(FileTokens, FileTokens, MyVar.VariableData);
                    }

                    return FoundToken;
                }

                return FoundToken;
            }

            return null;
        }

        public static TokenDataContainer FindTokenDataContainerByName(
            List<Token> MyTokenList,
            string TokenName
        )
        {
            Token FoundToken = FindTokenByName(MyTokenList, TokenName);
            if (FoundToken == null) return null;
            if (FoundToken.Data.GetType() == typeof(TokenDataContainer))
            {
                TokenDataContainer MyVar = (TokenDataContainer)FoundToken.Data;
                return MyVar;
            }

            return null;
        }
    }
}
