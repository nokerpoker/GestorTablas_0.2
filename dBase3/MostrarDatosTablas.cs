using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dBase3
{
    class MostrarDatosTablas
    {
        public void CargarTabla(string nombreTabla)
        {
            string dataFile = $"{nombreTabla}.data"; 
            string headerFile = $"{nombreTabla}.header";
            const int NUMERO = 3;

            if(!File.Exists(dataFile) && !File.Exists(headerFile))
            {
                Console.WriteLine("La tabla '{0}' no existe.", nombreTabla);
                return;
            }

            List<string> campos = new List<string>();
            List<int> longitudesDeCampos = new List<int>();
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(headerFile);
                Console.WriteLine("Leyendo campos de la tabla '{0}'...", nombreTabla);
                string primeraLinea = sr.ReadLine();

                if (!int.TryParse(primeraLinea, out int cantidadDeCampos))
                {
                    Console.WriteLine("El formato es incorrecto. Primera línea: " + primeraLinea);
                    return;
                }
                for(int i = 0; i < cantidadDeCampos; i++)
                {
                    string lineaDeCampo = sr.ReadLine();
                    string[] partes = lineaDeCampo.Split('-');

                    if(partes.Length == NUMERO)
                    {
                        Console.WriteLine("Añadiendo campo {0} de {1}: {2} (Tipo: {3}, Longitud: {4})", 
                                                    i + 1, cantidadDeCampos, partes[0], partes[1], partes[2]);
                        campos.Add(partes[0]);
                        longitudesDeCampos.Add(Convert.ToInt32(partes[2]));
                    }
                    else
                    {
                        Console.WriteLine("El formato es incorrecto en la línea: " + lineaDeCampo);
                        return;
                    }
                }
                MostrarRegistros(nombreTabla, campos, longitudesDeCampos);
                GestorBasesDeDatos.Ejecutar();
            }
            catch (FileLoadException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if(sr != null)
                {
                    sr.Close();
                }
            }
        }

        private void MostrarRegistros(string nombreTabla, List<string> campos, List<int> longitudesDeCampos)
        {
            string dataFile = $"{nombreTabla}.data";
            string[] lineas = File.ReadAllLines(dataFile);
            int cantidadDeRegistros = Convert.ToInt32(lineas[0]);
            const int NUMREGISTROS = 20;
            int registrosPorPagina = NUMREGISTROS;
            int paginaActual = 0;
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                int inicio = paginaActual * registrosPorPagina;
                int final = Math.Min(inicio + registrosPorPagina, cantidadDeRegistros);

                for(int registroIndexado = inicio; registroIndexado < final; registroIndexado++)
                {
                    string linea = "";
                    for(int campoIndexado = 0; campoIndexado < campos.Count; campoIndexado++)
                    {
                        linea += lineas[(registroIndexado * campos.Count) + campoIndexado + 1].PadRight(longitudesDeCampos[campoIndexado]) + " ";
                    }
                    Console.WriteLine(linea);
                }
                Console.WriteLine("Pagina {0} de {1}", 
                                    paginaActual + 1, (cantidadDeRegistros + registrosPorPagina - 1) / registrosPorPagina);

                ConsoleKeyInfo tecla = Console.ReadKey();
                if(tecla.Key == ConsoleKey.Escape)
                {
                    continuar = false;
                }
                else if(tecla.Key == ConsoleKey.UpArrow && paginaActual > 0)
                {
                    paginaActual--;
                }
                else if(tecla.Key == ConsoleKey.DownArrow && final < cantidadDeRegistros)
                {
                    paginaActual++;
                }
            }
        }
    }
}
