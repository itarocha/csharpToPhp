using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteSeguranca
{
    public class TesteCriptografia
    {
        public static void Main()
        {
            string texto = "ALFA";
            string chave = "678";
            string saida = QueryStringCrypt.EncodeURL(texto, chave);
            //string saida = QueryStringCrypt.StringEncode(texto, saida);
        }

    }
}
