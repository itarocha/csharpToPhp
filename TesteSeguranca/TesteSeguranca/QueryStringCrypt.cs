using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.ComponentModel;

namespace TesteSeguranca
{
    public class QueryStringCrypt
    {
        //Function to encode the string
        public static string StringEncode(string value, string key)
        {
            MACTripleDES mac3des = new MACTripleDES();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            mac3des.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            return Convert.ToBase64String(
              System.Text.Encoding.UTF8.GetBytes(value)) + '-' +
              Convert.ToBase64String(mac3des.ComputeHash(
              System.Text.Encoding.UTF8.GetBytes(value)));
        }

        //Function to decode the string
        //Throws an exception if the data is corrupt
        public static string StringDecode(string value, string key)
        {
            string dataValue = string.Empty;
            string calcHash = string.Empty;
            string storedHash = string.Empty;

            MACTripleDES mac3des = new MACTripleDES();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            mac3des.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));

            try
            {
                dataValue = System.Text.Encoding.UTF8.GetString(
                        Convert.FromBase64String(value.Split('-')[0]));
                storedHash = System.Text.Encoding.UTF8.GetString(
                        Convert.FromBase64String(value.Split('-')[1]));
                calcHash = System.Text.Encoding.UTF8.GetString(
                  mac3des.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dataValue)));

                if (storedHash != calcHash)
                {
                    //Data was corrupted
                    throw new ArgumentException("O valor passado não está correto!");
                }
            }
            catch
            {
                throw new ArgumentException("Chave inválida!");
            }

            return dataValue;
        }
        
        public static string EncodeURL(string psValue, string psKey)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(Crypt.MD5HashCode(psValue)));

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(psValue)) + '-' +
                Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(Crypt.MD5HashCode(psValue + psKey)));
        }

        public static string DecodeURL(string psValue, string psKey)
        {
            string sDataValue = string.Empty;
            string sCalculatedHash = string.Empty;
            string sStoredHash = string.Empty;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            try
            {
                sDataValue = Encoding.UTF8.GetString(Convert.FromBase64String(psValue.Split('-')[0]));

                sStoredHash = Encoding.UTF8.GetString(Convert.FromBase64String(psValue.Split('-')[1]));
                sCalculatedHash = Crypt.MD5HashCode(sDataValue + psKey);

                if (sStoredHash != sCalculatedHash)
                {
                    throw new ArgumentException("O valor passado não está correto!");
                }
            }
            catch
            {
                throw new ArgumentException("Chave inválida!");
            }

            return sDataValue;
        }

    }
}
