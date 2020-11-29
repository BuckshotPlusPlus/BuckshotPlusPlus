using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class Atributes
    {
        public string href = "";
		public string id = "";

		public static string GetHTMLAttributes(Token MyToken)
		{
			string CompiledAtributes = "";
			FieldInfo[] HTMLAttributes = typeof(HTML.Atributes).GetFields();
			TokenDataContainer ViewContainer = (TokenDataContainer)MyToken.Data;
			foreach (FieldInfo HTMLAttribute in HTMLAttributes)
			{
				TokenDataVariable MyHTMLAttribute = TokenUtils.FindTokenDataVariableByName(ViewContainer.ContainerData, HTMLAttribute.Name);
				if (MyHTMLAttribute != null)
				{
					CompiledAtributes += HTMLAttribute.Name + "=\"" + MyHTMLAttribute.VariableData + "\"";
				}
			}
			return CompiledAtributes;
		}
	}
}
