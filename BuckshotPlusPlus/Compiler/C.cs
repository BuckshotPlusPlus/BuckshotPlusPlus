using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BuckshotPlusPlus.Compiler
{
    public static class C
    {
        public static string AbsolutePath(string FileName)
        {
            return AppDomain.CurrentDomain.BaseDirectory + "/" + FileName;
        }
        public static void CompileFile(string FileName,string FileData)
        {
            File.WriteAllText(FileName, FileData, Encoding.UTF8);
            string strCmdText;
            strCmdText = "/C cl " + FileName;
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }

        public static string TokenToC(List<Token> Tokens)
        {
            string CFileData = "#include <stdio.h>" + Environment.NewLine +
                "#include <stdbool.h>" + Environment.NewLine +
                "#include <process.h>" + Environment.NewLine +
                "int main()" + Environment.NewLine + 
                "{" + Environment.NewLine;
            foreach(Token MyToken in Tokens)
            {
                if(MyToken.Type == "variable")
                {
                    TokenDataVariable MyTokenVariable = (TokenDataVariable)MyToken.Data;
                    if(MyTokenVariable.VariableType == "string")
                    {
                        MyTokenVariable.VariableType = "char";
                        MyTokenVariable.VariableName = "*" + MyTokenVariable.VariableName;
                        MyTokenVariable.VariableData = '"' + MyTokenVariable.VariableData + '"';
                    }
                    CFileData += MyTokenVariable.VariableType + " " + MyTokenVariable.VariableName + " = " + MyTokenVariable.VariableData + ";" + Environment.NewLine;
                }
            }
            return CFileData + "printf(montest);\nsystem(\"pause\");\nreturn 0;}";
        }
    }
}
