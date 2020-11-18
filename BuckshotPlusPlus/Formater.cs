using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    public static class Formater
    {
        public static string FormatFileData(string FileData)
        {
            int i = 0;
            int spaceCount = 0;

            while (i < FileData.Length )
            {
                // Gérer les chaines de character
                if (FileData[i] == ' ')
                {
                    while (FileData[spaceCount + i] == ' ')
                    {
                        spaceCount++;
                    }

                    if (i == 0)
                    {
                        FileData = FileData.Remove(i, spaceCount);
                    }
                    else if(FileData[i - 1] == '\n') {
                        FileData = FileData.Remove(i, spaceCount);
                    } else
                    {
                        if (spaceCount > 1)
                        {
                            FileData = FileData.Remove(i, spaceCount - 1);
                        }
                        
                    }
                    spaceCount = 0;
                }
                i++;
            }
            return FileData;
        } 
    }
}
