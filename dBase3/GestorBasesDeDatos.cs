using System;
using System.Collections.Generic;
using System.IO;

namespace dBase3
{
    class GestorBasesDeDatos
    {
        private static string tablaActual = null;

        public static void Ejecutar()
        {
            while (true)
            {
                Console.WriteLine("1 - Abrir tabla existente.");
                Console.WriteLine("2 - Crear una nueva tabla.");
                Console.WriteLine("3 - Añadir datos a la tabla actual.");
                Console.WriteLine("4 - Visualizar la tabla actual.");
                Console.WriteLine("5 - Prueba de consola mejorada.");
                Console.WriteLine("0 - Salir.");

                ElegirOpcionMenu();
                Console.Clear();
            }
        }

        public static void ElegirOpcionMenu()
        {
            string opcion = ConsolaMejorada.PedirString("Elige una opción: ");
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
                    if (tablaActual == null)
                    {
                        Console.WriteLine("Primero debe abrir una tabla existente.");
                    }
                    else
                    {
                        InsertadorEnTablas insertarTabla = new InsertadorEnTablas();
                        insertarTabla.EjecutarInsertarTablas();
                    }
                    break;

                case "4":
                    if (tablaActual == null)
                    {
                        Console.WriteLine("Primero debe abrir una tabla existente.");
                    }
                    else
                    {
                        MostrarDatosTabla(tablaActual);
                    }
                    break;

                case "5":
                    ConsolaMejorada.ProbarMetodos();
                    break;

                case "0":
                    Console.WriteLine("Saliendo del programa.");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Opción no válida, inténtelo de nuevo.");
                    break;
            }
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

            List<string> campos = new List<string>();
            List<int> longitudes = new List<int>();
            CargarCamposYLongitudes(headerFile, campos, longitudes);

            string[] lineas = File.ReadAllLines(dataFile);
            int cantidadDeRegistros = Convert.ToInt32(lineas[0]);

            MostrarRegistrosConScroll(lineas, campos, longitudes, cantidadDeRegistros);
        }

        private static void CargarCamposYLongitudes(string headerFile, List<string> campos, List<int> longitudes)
        {
            using (StreamReader sr = new StreamReader(headerFile))
            {
                int cantidadDeCampos = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < cantidadDeCampos; i++)
                {
                    string[] partes = sr.ReadLine().Split('-');
                    campos.Add(partes[0]);
                    longitudes.Add(Convert.ToInt32(partes[2]));
                }
            }
        }

        private static void MostrarRegistrosConScroll(string[] lineas, List<string> campos, List<int> longitudes, int cantidadDeRegistros)
        {
            int posicionActual = 0;
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                MostrarRegistro(lineas, campos, longitudes, posicionActual);

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        continuar = false;
                        break;
                    case ConsoleKey.UpArrow:
                        if (posicionActual > 0) posicionActual--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (posicionActual < cantidadDeRegistros - 1) posicionActual++;
                        break;
                }
            }
        }

        private static void MostrarRegistro(string[] lineas, List<string> campos, List<int> longitudes, int posicionActual)
        {
            string linea = "";
            for (int campoActual = 0; campoActual < campos.Count; campoActual++)
            {
                linea += lineas[(posicionActual * campos.Count) + campoActual + 1].PadRight(longitudes[campoActual]) + " ";
            }

            Console.WriteLine(linea);
            Console.WriteLine();
            Console.WriteLine("Usa las flechas para navegar. Presione ESC para salir.");
        }
    }
}
