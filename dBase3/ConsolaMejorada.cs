using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dBase3
{
    public static class ConsolaMejorada
    {
        public static string PedirString(string nombre)
        {
            Console.WriteLine(nombre);
            return Console.ReadLine();
        }

        public static int PedirEntero(string valor)
        {
            int resultado;
            while (true)
            {
                Console.WriteLine(valor);
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

        public static double PedirDouble(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (double.TryParse(Console.ReadLine(), out double resultado))
                {
                    return resultado;
                }
                else
                {
                    Console.WriteLine("Error. Debe introducir un número decimal.");
                }
            }
        }

        // Nuevos métodos.
        public static void Escribir(int x, int y, string texto, string color)
        {
            ConsoleColor colorFondoOriginal = Console.BackgroundColor;
            ConsoleColor colorTextoOriginal = Console.ForegroundColor;

            if(Enum.TryParse(color, true, out ConsoleColor colorDeFondo))
            {
                Console.BackgroundColor = colorDeFondo;
            }

            Console.SetCursorPosition(x, y);
            Console.WriteLine(texto);

            Console.BackgroundColor = colorFondoOriginal;
            Console.ForegroundColor = colorTextoOriginal;
        }

        public static string Pedir(int x, int y, int longitudMaxima)
        {
            Console.WriteLine("Introduce un texto que tenga como máximo {0} de caracteres.", longitudMaxima);
            Console.SetCursorPosition(x, y);
            string entrada = "";
            int posicionInicial = x;

            while (true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);

                if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace && entrada.Length > 0)
                {
                    entrada = entrada.Substring(0, entrada.Length - 1);
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
                else if (!char.IsControl(tecla.KeyChar) && entrada.Length < longitudMaxima)
                {
                    entrada += tecla.KeyChar;
                    Console.Write(tecla.KeyChar);
                }
            }

            Console.WriteLine();
            return entrada.PadRight(longitudMaxima);
        }

        public static void DibujarVentana(int x, int y, int ancho, int alto, string colorDeFondo)
        {
            ConsoleColor colorFondoOriginal = Console.BackgroundColor;
            ConsoleColor colorVentana;

            if (Enum.TryParse(colorDeFondo, true, out colorVentana))
            {
                Console.BackgroundColor = colorVentana;
            }

            Console.SetCursorPosition(x, y);

            for (int i = 0; i < alto; i++)
            {
                for (int j = 0; j < ancho; j++)
                {
                    if (i == 0 || i == alto - 1)
                    {
                        Console.Write("-");
                    }
                    else
                    {
                        if (j == 0 || j == ancho - 1)
                            Console.Write("|");
                        else
                            Console.Write(" ");
                    }
                }
                Console.WriteLine();
                Console.SetCursorPosition(x, Console.CursorTop);
            }

            Console.BackgroundColor = colorFondoOriginal;
        }

        public static void RellenarVentana(string texto, string colorDeTexto, string colorDeFondo)
        {
            int ancho = 40;
            int alto = 8;
            int x = (Console.WindowWidth - ancho) / 2;
            int y = (Console.WindowHeight - alto) / 2;

            DibujarVentana(x, y, ancho, alto, colorDeFondo);

            ConsoleColor colorTextoOriginal = Console.ForegroundColor;
            ConsoleColor colorTextoVentana;

            if (Enum.TryParse(colorDeTexto, true, out colorTextoVentana))
            {
                Console.ForegroundColor = colorTextoVentana;
            }

            Console.SetCursorPosition((Console.WindowWidth - texto.Length) / 2, y + alto / 2);
            Console.WriteLine(texto);

            Console.ForegroundColor = colorTextoOriginal;
        }

        public static void EscribirCentrado(int y, string texto, string color)
        {
            ConsoleColor colorTextoOriginal = Console.ForegroundColor;
            ConsoleColor colorDeTexto;

            if (Enum.TryParse(color, true, out colorDeTexto))
            {
                Console.ForegroundColor = colorDeTexto;
            }

            Console.SetCursorPosition((Console.WindowWidth - texto.Length) / 2, y);
            Console.WriteLine(texto);

            Console.ForegroundColor = colorTextoOriginal;
        }

        public static string PedirConfirmacion(int x, int y, int longitudMaxima, string caracteresPermitidos)
        {
            Console.WriteLine("Ingresa una opción que sea válida '{0}'", caracteresPermitidos);
            Console.SetCursorPosition(x, y);
            string entrada = string.Empty;
            int posicionX = x;

            while (true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);

                if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace)
                {
                    if (entrada.Length > 0)
                    {
                        entrada = entrada.Substring(0, entrada.Length - 1);
                        Console.SetCursorPosition(posicionX - 1, y);
                        Console.Write(' ');
                        Console.SetCursorPosition(posicionX - 1, y);
                        posicionX--;
                    }
                }
                else if (caracteresPermitidos.Contains(tecla.KeyChar))
                {
                    if (entrada.Length < longitudMaxima)
                    {
                        entrada += tecla.KeyChar;
                        Console.Write(tecla.KeyChar);
                        posicionX++;
                    }
                }
            }

            return entrada;
        }

        public static char EsperarTecla(string caracteresValidos)
        {
            Console.WriteLine("Tienes que presionar una tecla válida {0}.", caracteresValidos);
            while (true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);

                if (caracteresValidos.Contains(tecla.KeyChar))
                {
                    return tecla.KeyChar;
                }
                else
                {
                    Console.WriteLine("'{0}' no es una opción válida. Presione una tecla válida ({1}):", tecla.KeyChar, caracteresValidos);
                }
            }
        }

        public static string PedirPassword(int x, int y, int longitudMaxima)
        {
            Console.WriteLine("Ingresa una contraseña de hasta {} longitud máxima.", longitudMaxima);
            Console.SetCursorPosition(x, y);
            string password = "";
            int posicionInicial = x;

            while (true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);

                if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
                else if (!char.IsControl(tecla.KeyChar) && password.Length < longitudMaxima)
                {
                    password += "*";
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return password.PadRight(longitudMaxima);
        }

        public static int PedirEnteroMejorado(int x, int y, int min, int max)
        {
            int valor;
            string entrada;
            do
            {
                Escribir(x, y, $"Ingresa un número entre {min} y {max}: " , "Red");
                entrada = PedirConfirmacion(x + 30, y, 5, "0123456789");
            } while (!int.TryParse(entrada, out valor) || valor < min || valor > max);

            return valor;
        }

        public static double PedirReal(int x, int y, int longitudMaxima, string formato)
        {
            double valor;
            string entrada;
            do
            {
                Escribir(x, y, new string(' ', longitudMaxima), "Red");
                Escribir(x, y, "Ingrese un número real: ", "Gray");
                entrada = PedirConfirmacion(x + 22, y, longitudMaxima, "0123456789,.");
            } while (!double.TryParse(entrada.Replace(',', '.'), out valor));

            return valor;
        }

        public static void ProbarMetodos()
        {
            Console.Clear();

            // Ejemplo de uso de los métodos

            // 1. Escribir texto
            Escribir(10, 5, "helloooo!", "Yellow");

            // 2. Pedir texto limitado
            string movil = Pedir(10, 7, 12);
            Escribir(10, 8, movil, "Yellow");

            // 3. Dibujar ventana con texto
            DibujarVentana(5, 10, 40, 8, "White");
            Escribir(7, 12, "¿Desea continuar? (S/N)", "Yellow");

            // 4. Escribir centrado
            EscribirCentrado(15, "Mensaje centrado", "Cyan");

            // 5. Pedir texto confirmando los caracteres
            string confirmacion = PedirConfirmacion(10, 17, 1, "SN");
            Escribir(10, 18, confirmacion, "Yellow");

            // 6. Esperar una tecla
            char opcion = EsperarTecla("s");
            Escribir(10, 20, Convert.ToString(opcion).ToLower(), "Green");

            // 7. Pedir una contraseña
            string clave = PedirPassword(10, 22, 10);
            Escribir(10, 23, clave, "Yellow");

            // 8. Pedir un número entero (en un rango)
            int edad = PedirEnteroMejorado(10, 25, 5, 99);
            string edadTexto = Convert.ToString(edad);
            Escribir(10, 26, edadTexto, "Yellow");

            // 9. Pedir un número real (con un formato específico)
            double importe = PedirReal(10, 28, 10, "######.##");
            string importeTexto = Convert.ToString(importe);
            Escribir(10, 29, importeTexto, "Yellow");

            Console.WriteLine("\nPrueba finalizada. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
