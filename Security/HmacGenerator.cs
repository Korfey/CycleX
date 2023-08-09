using CycleX.Game;
using System.Security.Cryptography;
using System.Text;

namespace CycleX.Security
{
    internal class HmacGenerator
    {
        private readonly byte[] key;
        private readonly byte[] hmac;
        
        public HmacGenerator(Move move)
        {
            key = new byte[32];
            GenerateKey();
            hmac = CalculateHmac(move);
        }

        private void GenerateKey()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(key);
        }
        private byte[] CalculateHmac(Move move)
        {
            using HMACSHA256 hash = new(Encoding.UTF8.GetBytes(GetKey()));
              return hash.ComputeHash(Encoding.UTF8.GetBytes(move.PcMove)); 
        }

        public string GetHmac() => BitConverter.ToString(hmac).Replace("-", "");

        public string GetKey() => BitConverter.ToString(key).Replace("-", "");
    }
}
