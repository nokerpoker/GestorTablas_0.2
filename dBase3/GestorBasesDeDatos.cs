using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dBase3
{
    class GestorBasesDeDatos
    {
        private static string tablaActual = null;
        public static void Ejecutar()
        {
            Console.WriteLine("1 - Abrir tabla existente.");
            Console.WriteLine("2 - Crear una nueva tabla.");                   
            Console.WriteLine("3 - Añadir datos a la tabla actual.");                    
            Console.WriteLine("4 - Visualizar la tabla actual.");                    
            Console.WriteLine("0 - Salir.");                    

            ElegirOpcionMenu();
            Console.Clear();
        }

        public static void ElegirOpcionMenu()
        {
            string opcion;
            do 
            {
                opcion = ConsolaMejorada.PedirString("Elige una opción: ");
                switch (opcion)
                {
                    case "1":
                        AbrirTablaExistente();
                        break;

                    case "2":
                        CreadorDeTablas crearTabla = new CreadorDeTablas();
                        crearTabla.EjecutarCreadorTablas();
                        break; 

                    case "3":
                        InsertadorEnTablas insertarTabla = new InsertadorEnTablas();
                        insertarTabla.EjecutarInsertarTablas();
                        break;
                        
                    case "4":
                        if (tablaActual == null)
                        {
                            Console.WriteLine("Primero debe abrir una tabla existente.");
                        }
                        else
                        {
                            MostrarDatosTablas mostrar = new MostrarDatosTablas();
                            mostrar.CargarTabla(tablaActual);
                        }

                        break;

                    case "0":
                        Console.WriteLine("Saliendo del programa.");
                        Environment.Exit(0);
                        break;

                        default:
                        Console.WriteLine("Opción no válida, inténtelo de nuevo.");
                        break;
                }
            } while (opcion != "0");
        }

        private static void AbrirTablaExistente()
        {
            tablaActual = ConsolaMejorada.PedirString("Nombre de la tabla que vas a abrir: ");
            MostrarDatosTabla(tablaActual);
        }
        private static void MostrarDatosTabla(string nombreTabla)
        {
            string dataFile = $"{nombreTabla}.data";
            string headerFile = $"{nombreTabla}.header";

            if (!File.Exists(dataFile) || !File.Exists(headerFile))
            {
                Console.WriteLine("La tabla '{0}' no existe.", nombreTabla);
                return;
            }

            StreamReader sr = null;
            try
            {
                sr = new StreamReader(headerFile);               
                int cantidadDeCampos = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < cantidadDeCampos; i++)
                {
                    string[] partes = sr.ReadLine().Split('-');
                    Console.Write(partes[0].PadRight(Convert.ToInt32(partes[2]) + 1));
                }
                Console.WriteLine();

                sr = new StreamReader(dataFile);               
                string linea;
                int contador = 1;
                while ((linea = sr.ReadLine()) != null)
                {
                    Console.WriteLine("{0}: {1}", contador, linea);
                    contador++;
                }          
            }
            catch(FileLoadException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al mostrar la tabla: " + ex.Message);
            }
            finally
            {
                sr?.Close();
            }
        }
    }
}
