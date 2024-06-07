using dBase3;
using System;

class Ejecutar
{
    static void Main(string[] args)
    {
        try
        {
            GestorBasesDeDatos.Ejecutar();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
