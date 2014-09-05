using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSeguranca
{
    public class Crypt
    {
        /// <summary> 
        /// Retorna o hashcode MD5 para a palavra informada 
        /// </summary> 
        /// <param name="Palavra">palavra chave para criação do hashcode MD5</param> 
        /// <returns>hashcode MD5 para a palavra informada</returns> 
        public static string MD5HashCode(string Palavra)
        {
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(Palavra);
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            string password = "";
            foreach (byte b in encodedBytes)
                password += b.ToString("x2");
            return password;
        }
        public static int CheckSum(string Palavra)
        {
            int chksum = 0;
            try
            {
                foreach (char caracter in Palavra.ToCharArray())
                    chksum += (int)caracter;
            }
            catch { }
            return chksum;
        }
    }
}
