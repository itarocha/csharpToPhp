using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSeguranca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(ASCIIEncoding.Default.CodePage); // 1252
            Console.WriteLine(ASCIIEncoding.ASCII.CodePage); // 20127
            Console.WriteLine(ASCIIEncoding.ASCII.CodePage); // 20127
            Console.WriteLine(Encoding.UTF8.CodePage); // 65001

            //TestarEncodeDecodeString();
            //TestarEncodeDecodeURL();
            string x = Crypt.MD5HashCode("cafe");

            string texto = "ÁÉFAé";

            Byte[] ascii = ASCIIEncoding.Default.GetBytes(texto);
            Byte[] utf = Encoding.UTF8.GetBytes(texto);


            Console.WriteLine("Pressione algo...");
            Console.ReadKey();
        }

        private static void TestarEncodeDecodeString()
        {
            string texto = "ALFA";
            string chave = "678";

            //texto = "123456789ABCDEFGH";
            //chave = "¨(§4+[¢£%4AÇz`";

            texto = "Viu Deus que era bom. Esse foi o resumo do 8° dia da criação";
            chave = "¨(§4+[¢£%4AÇz`";

            texto = "áéíóú (Art:§1° = ~$£25.47)";
            chave = "¨(§4+[¢£%4AÇz`123ª";


            string saida = QueryStringCrypt.Ita_StringEncode(texto, chave);
            string traduzido = QueryStringCrypt.Ita_StringDecode(saida, chave);

            //string saida = QueryStringCrypt.StringEncode(texto, saida);

            Console.WriteLine("ENTRADA");
            Console.WriteLine(string.Format("\"{0}\"\n", texto));
            Console.WriteLine("CHAVE");
            Console.WriteLine(string.Format("\"{0}\"\n", chave));
            Console.WriteLine("RESULT");
            Console.WriteLine(string.Format("{0}\n", saida));
            Console.WriteLine("REVERSO");
            Console.WriteLine(string.Format("\"{0}\"\n", traduzido));


            // Transforma essa função para os padrões LINX
            //texto = "testing";
            //chave = "56dsfkj3kj23asdf83kseegflkj43458afdl";
            //Console.WriteLine(QueryStringCrypt.ApiEncode(texto, chave));

        }

        private static void TestarEncodeDecodeURL() {
            string texto = "ALFA";
            string chave = "678";
            
            //texto = "DELTATRONICKÉFAÁé:zz";
            //texto = "DELTATRONICKÉFAÁé:zz";
            texto = "ÁÉFAÁé:ABCDEFG";
            chave = "5688782255";
            
            string saida = QueryStringCrypt.Ita_EncodeURL(texto, chave);
            string traduzido = QueryStringCrypt.Ita_DecodeURL(saida, chave);

            //string saida = QueryStringCrypt.EncodeURL(texto, chave);
            //string traduzido = QueryStringCrypt.DecodeURL(saida, chave);
            //string saida = QueryStringCrypt.StringEncode(texto, saida);

            Console.WriteLine(saida);
            Console.WriteLine("Significa...");
            Console.WriteLine(traduzido);
        }
    }
}
