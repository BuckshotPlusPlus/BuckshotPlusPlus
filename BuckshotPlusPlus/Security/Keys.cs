using System;
using System.Linq;

namespace BuckshotPlusPlus.Security
{
    public class Keys
    {
        public static string CreateRandomKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public static string CreateRandomUniqueKey()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }
    }
}
