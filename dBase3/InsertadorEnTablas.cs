using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dBase3
{
    internal class InsertadorEnTablas
    {
        public void EjecutarInsertarTablas()
        {
            string nombreTabla = ConsolaMejorada.PedirString("Nombre de la tabla a la que desea añadir datos: ");
            List<string> campos = AnyadirCamposEnTablas(nombreTabla);
            if (campos != null)
            {
                AnyadirDatosEnTablas(nombreTabla, campos);  
            }
        }

        private List<string> AnyadirCamposEnTablas(string nombreTabla)
        {
            string headerFile = $"{nombreTabla}.header";
            const int LIMITE = 3;

            if (!File.Exists(headerFile))
            {
                Console.Clear();
                Console.WriteLine("La tabla '{0}' no existe.", nombreTabla);
                Console.WriteLine();
                GestorBasesDeDatos.Ejecutar();
                return null;
            }

            Console.WriteLine("Añadir campos a la tabla: ");
            List<string> campos = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(headerFile))
                {
                    string primeraLinea = sr.ReadLine();
                    if (!int.TryParse(primeraLinea, out int cantidadDeCampos))
                    {
                        Console.WriteLine("El formato es incorrecto. Primera línea: " + primeraLinea);
                        return null;
                    }

                    Console.WriteLine("Cantidad de campos encontrados: " + cantidadDeCampos);

                    for (int i = 0; i < cantidadDeCampos; i++)
                    {
                        string lineaCampo = sr.ReadLine();
                        if (lineaCampo == null)
                        {
                            Console.WriteLine("El archivo tiene menos líneas de las esperadas.");
                            return null;
                        }

                        string[] partes = lineaCampo.Split('-');
                        if (partes.Length != LIMITE)
                        {
                            Console.WriteLine("El formato del archivo es incorrecto. Línea: " + lineaCampo);
                            return null;
                        }
                        campos.Add(partes[0].Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo: " + ex.Message);
                return null;
            }

            Console.WriteLine("Campos añadidos correctamente.");
            return campos;
        }

        public void AnyadirDatosEnTablas(string nombreTabla, List<string> campos)
        {
            string dataFile = $"{nombreTabla}.data";

            List<string> datos = new List<string>();
            Console.Clear();
            Console.WriteLine("Añadir datos a la tabla: ");

            foreach (string campo in campos)
            {
                string valor = ConsolaMejorada.PedirString($"{campo}: ");
                datos.Add(valor);
            }

            // HASTA AQUI
            StreamWriter sw = null;
            try
            {
                if (File.Exists(dataFile))
                {
                    List<string> lineas = new List<string>(File.ReadAllLines(dataFile));
                    int cantidadDeRegistros = Convert.ToInt32(lineas[0]) + 1;
                    lineas[0] = cantidadDeRegistros.ToString();

                    sw = new StreamWriter(dataFile);
                    sw.WriteLine(lineas[0]);

                    for (int i = 1; i < lineas.Count; i++)
                    {
                        sw.WriteLine(lineas[i]);
                    }
                    foreach (string dato in datos)
                    {
                        sw.WriteLine(dato);
                    }
                }
                else
                {
                    sw = new StreamWriter(dataFile);
                    sw.WriteLine(1);
                    foreach (string dato in datos)
                    {
                        sw.WriteLine(dato);
                    }
                }
            }
            catch (FileLoadException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sw?.Close();
            }
            Console.Clear();
            Console.WriteLine("Datos añadidos correctamente.");
            GestorBasesDeDatos.Ejecutar();
        }
    }
}
