using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dBase3
{
    class ConsolaMejorada
    {
        public static string PedirString(string nombre)
        {
            Console.WriteLine(nombre);
            return Console.ReadLine();
        }
        public static int PedirEntero(string cantidad)
        {
            int resultado;
            while (true)
            {
                Console.WriteLine(cantidad);
                if (int.TryParse(Console.ReadLine(), out resultado))
                {
                    return resultado;
                }
                else
                {
                    Console.WriteLine("Valor inválido, inténtelo de nuevo.");
                }
            }
        }
    }
}
