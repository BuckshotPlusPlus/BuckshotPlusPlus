using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class Attributes
    {
        static List<(String, Boolean)> Props = new List<(string, bool)>
        {
            ("href", false),
            ("id", false),
            ("class", false),
            ("target", false),
            ("disabled", true)
        };

        public static string GetHTMLAttributes(List<Token> ServerSideTokens,Token MyToken)
        {
            string CompiledAtributes = "";
            TokenDataContainer ViewContainer = (TokenDataContainer)MyToken.Data;
            
            foreach ((String Name, bool WithoutValue) in Props)
            {
                TokenDataVariable MyHTMLAttribute = TokenUtils.TryFindTokenDataVariableValueByName(
                    ServerSideTokens,
                    ViewContainer.ContainerData,
                    Name
                );
                
                if (MyHTMLAttribute != null)
                {
                    if ( WithoutValue )
                    {
                        CompiledAtributes += Name;
                    }
                    else
                    {
                        CompiledAtributes +=
                            Name
                            + "=\""
                            + MyHTMLAttribute.GetCompiledVariableData(ServerSideTokens)
                            + "\"";
                    }
                }
            }
            
            return CompiledAtributes;
        }
    }
}
