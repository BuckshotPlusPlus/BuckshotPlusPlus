namespace BuckshotPlusPlus
{
    using System;
    using System.IO;

    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                string part_0 = "";
                string part_1 = "";
                bool bIsFirstPart = true;
                foreach (char c in line)
                {
                    if (c == '=' & bIsFirstPart) {
                        bIsFirstPart = false;
                        continue;
                    }

                    if (bIsFirstPart)
                    {
                        part_0 += c;
                        continue;
                    }

                    part_1 += c;
                }

                Console.WriteLine(part_0 + "=" + part_1);
                Environment.SetEnvironmentVariable(part_0, part_1);
            }
        }
    }
}
