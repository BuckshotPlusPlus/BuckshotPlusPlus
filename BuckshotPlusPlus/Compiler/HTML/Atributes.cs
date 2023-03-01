using System.Reflection;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class Atributes
    {
        public string _href = "";
        public string _id = "";
        public string _class = "";

        public static string GetHTMLAttributes(Token MyToken)
        {
            string CompiledAtributes = "";
            FieldInfo[] HTMLAttributes = typeof(HTML.Atributes).GetFields();
            TokenDataContainer ViewContainer = (TokenDataContainer)MyToken.Data;
            foreach (FieldInfo HTMLAttribute in HTMLAttributes)
            {
                TokenDataVariable MyHTMLAttribute = TokenUtils.FindTokenDataVariableByName(
                    ViewContainer.ContainerData,
                    HTMLAttribute.Name.Substring(1)
                );
                if (MyHTMLAttribute != null)
                {
                    CompiledAtributes +=
                        HTMLAttribute.Name.Substring(1)
                        + "=\""
                        + MyHTMLAttribute.VariableData
                        + "\"";
                }
            }
            return CompiledAtributes;
        }
    }
}
