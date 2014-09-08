using System;
using System.Collections.Generic;
using System.Text;
//using System.Security.Cryptography;
using System.Web;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace ConsoleSeguranca
{
    public class QueryStringCrypt
    {

        public static string ApiEncode(string data, string secret)
        {
            byte[] clear;

            var encoding = new UTF8Encoding();
            var md5 = new MD5CryptoServiceProvider();

            byte[] key = md5.ComputeHash(encoding.GetBytes(secret));

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = key;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;

            byte[] input = encoding.GetBytes(data);
            try { clear = des.CreateEncryptor().TransformFinalBlock(input, 0, input.Length); }
            finally
            {
                des.Clear();
                md5.Clear();
            }

            return Convert.ToBase64String(clear);
        }        


        //Function to encode the string
        public static string Ita_StringEncode(string value, string key)
        {
            MACTripleDES mac3des = new MACTripleDES();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] chaveMD5 = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            mac3des.Key = chaveMD5;

            //string chaveMD5StringBase64 = Convert.ToBase64String(chaveMD5);
            byte[] primeiraParteBytes = System.Text.Encoding.UTF8.GetBytes(value);
            string primeiraParteString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
            //P("Primeira parte",primeiraParteString);

            byte[] fraseBytes = System.Text.Encoding.UTF8.GetBytes(value);

            var tripleDesFrase = mac3des.ComputeHash(fraseBytes);

            string tripleDesFraseBase64 = Convert.ToBase64String(tripleDesFrase);
            //P("tripleDesFraseBase64", tripleDesFraseBase64);

            /*
            return 
             Convert.ToBase64String(
              System.Text.Encoding.UTF8.GetBytes(value)
             ) + '-' +
              Convert.ToBase64String(mac3des.ComputeHash(
              System.Text.Encoding.UTF8.GetBytes(value)));
            */

            string retorno = primeiraParteString +
                             '-' +
                             Convert.ToBase64String(
                                mac3des.ComputeHash(
                                    fraseBytes    
                                )
                             );

            //P("Retorno Encriptado", retorno);

            return retorno; 
        }

        //Function to decode the string
        //Throws an exception if the data is corrupt
        public static string Ita_StringDecode(string value, string key)
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

        public static void P(string titulo, string conteudo) {
            Console.WriteLine(titulo);
            Console.WriteLine(conteudo+"\n");
        }


        public static string Ita_EncodeURL(string palavra, string chave)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(Crypt.MD5HashCode(psValue)));
            
            // 1 - Base 64 de UTF 8 da palavra
            string primeiraParte = Convert.ToBase64String(Encoding.UTF8.GetBytes(palavra));
            P("Primeira Parte:", primeiraParte);

            // 2 - Concatena [palavra] com [chave]
            string palavraConcat = palavra + chave;

            // 3 - MD5 da [palavraConcat]
            string md5PalavraConcat = Crypt.MD5HashCode(palavraConcat);

            // 4 - Base64 de ASCII de [md5PalavraConcat]
            string segundaParte = Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(md5PalavraConcat));
            P("Segunda Parte:", segundaParte);

            // 5 - Concatena [primeiraParte] + ["-"] + [segundaParte]
            string retornoCalculado = primeiraParte + "-" + segundaParte;

            string retorno = Convert.ToBase64String(Encoding.UTF8.GetBytes(palavra)) + '-' +
                             Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(Crypt.MD5HashCode(palavraConcat)));

            if (retorno == retornoCalculado)
            {
                Console.WriteLine("TUDO CERTO\n");
            }
            else {
                Console.WriteLine("ERRO NO MEU ALGORITMO\n");
            }

            return retorno;

            //QUxGQQ==-YTYzYjZjY2Q0ODNkMWY2Y2ZhMTBmM2Q2MTQ2ZTgyZmM=
            //QUxGQQ==-YTYzYjZjY2Q0ODNkMWY2Y2ZhMTBmM2Q2MTQ2ZTgyZmM=
        }

        public static string Ita_DecodeURL(string palavra, string chave)
        {
            string sDataValue = string.Empty;
            string sCalculatedHash = string.Empty;
            string sStoredHash = string.Empty;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            try
            {
                string primeiraParte = palavra.Split('-')[0];
                string segundaParte = palavra.Split('-')[1];


                sDataValue = Encoding.UTF8.GetString(Convert.FromBase64String(palavra.Split('-')[0]));

                sStoredHash = Encoding.UTF8.GetString(Convert.FromBase64String(palavra.Split('-')[1]));
                sCalculatedHash = Crypt.MD5HashCode(sDataValue + chave);


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
          
        /*
        private byte[] Encrypt2(string plainText)
        {
            try
            {
                
                RijndaelManaged aes = new RijndaelManaged();
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.KeySize = 256;
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                MemoryStream msEncrypt = new MemoryStream();
                CryptoStream csEncrypt =
                  new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                StreamWriter swEncrypt = new StreamWriter(csEncrypt);

                swEncrypt.Write(plainText);

                swEncrypt.Close();
                csEncrypt.Close();
                aes.Clear();

                return msEncrypt.ToArray();
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Problem trying to encrypt.", ex);
            }
        }
        */

        private static void EncryptData(String inName, String outName, byte[] tdesKey, byte[] tdesIV)
        {
            //Create the file streams to handle the input and output files.
            FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            //Create variables to help with read and write. 
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption. 
            long rdlen = 0;              //This is the total number of bytes written. 
            long totlen = fin.Length;    //This is the total length of the input file. 
            int len;                     //This is the number of bytes to be written at a time.

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(fout, tdes.CreateEncryptor(tdesKey, tdesIV), CryptoStreamMode.Write);

            Console.WriteLine("Encrypting...");

            //Read from the input file, then encrypt and write to the output file. 
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
                Console.WriteLine("{0} bytes processed", rdlen);
            }

            encStream.Close();
        }
    }
}
