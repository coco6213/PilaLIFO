
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PilaLIFO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            bool salir = false;

            while (!salir)
            {
                Console.WriteLine();
                Console.WriteLine("==============================");
                Console.WriteLine("          PILA LIFO");
                Console.WriteLine("==============================");
                Console.WriteLine("1. Operaciones de la pila");
                Console.WriteLine("2. Invertir texto o números");
                Console.WriteLine("3. Evaluar expresiones");
                Console.WriteLine("4. Salir");
                Console.Write("Opción: ");

                string opcion = Console.ReadLine() ?? "";

                switch (opcion.Trim())
                {
                    case "1":
                        MenuOperacionesBasicas();
                        break;

                    case "2":
                        MenuInversion();
                        break;

                    case "3":
                        MenuExpresiones();
                        break;

                    case "4":
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }

            Console.WriteLine("Programa finalizado.");
        }

        private static void MenuOperacionesBasicas()
        {
            Pila<int> pila = new Pila<int>();
            bool volver = false;

            while (!volver)
            {
                Console.WriteLine();
                Console.WriteLine("--- Operaciones de la pila ---");
                Console.WriteLine("Pila actual: " + pila);
                Console.WriteLine("a. Push - Agregar");
                Console.WriteLine("b. Pop - Extraer");
                Console.WriteLine("c. Top - Consultar el último");
                Console.WriteLine("d. IsEmpty - Verificar si está vacía");
                Console.WriteLine("e. Clear - Vaciar la pila");
                Console.WriteLine("f. Volver al menú principal");
                Console.Write("Opción: ");

                string opcion = Console.ReadLine() ?? "";

                try
                {
                    switch (opcion.Trim().ToLower())
                    {
                        case "a":
                            Console.Write("Número a agregar: ");
                            string entrada = Console.ReadLine() ?? "";

                            if (int.TryParse(entrada, out int numero))
                            {
                                pila.Push(numero);
                                Console.WriteLine("Agregado: " + numero);
                                Console.WriteLine("Pila actual: " + pila);
                            }
                            else
                            {
                                Console.WriteLine("Ingrese un número entero válido.");
                            }
                            break;

                        case "b":
                            int extraido = pila.Pop();
                            Console.WriteLine("Extraído: " + extraido);
                            Console.WriteLine("Pila actual: " + pila);
                            break;

                        case "c":
                            Console.WriteLine("Elemento superior: " + pila.Top());
                            break;

                        case "d":
                            if (pila.IsEmpty())
                                Console.WriteLine("La pila está vacía.");
                            else
                                Console.WriteLine("La pila contiene elementos.");
                            break;

                        case "e":
                            pila.Clear();
                            Console.WriteLine("Pila vaciada.");
                            Console.WriteLine("Pila actual: " + pila);
                            break;

                        case "f":
                            volver = true;
                            break;

                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Aviso: " + ex.Message);
                }
            }
        }

        private static void MenuInversion()
        {
            Console.WriteLine();
            Console.WriteLine("--- Invertir datos ---");
            Console.WriteLine("a. Invertir texto");
            Console.WriteLine("b. Invertir números");
            Console.WriteLine("c. Volver al menú principal");
            Console.Write("Opción: ");

            string opcion = Console.ReadLine() ?? "";

            if (opcion.Trim().ToLower() == "c")
                return;

            if (opcion.Trim().ToLower() == "a")
            {
                Console.Write("Texto: ");
                string texto = Console.ReadLine() ?? "";

                if (texto.Length == 0)
                {
                    Console.WriteLine("No ingresó texto.");
                    return;
                }

                string invertido = InvertirTexto(texto, true);

                Console.WriteLine("Original:  " + texto);
                Console.WriteLine("Invertido: " + invertido);
            }
            else if (opcion.Trim().ToLower() == "b")
            {
                Console.Write("Números separados por espacios: ");
                string linea = Console.ReadLine() ?? "";

                string[] numeros = linea.Split(
                    new[] { ' ', '\t' },
                    StringSplitOptions.RemoveEmptyEntries);

                if (numeros.Length == 0)
                {
                    Console.WriteLine("No ingresó números.");
                    return;
                }

                foreach (string n in numeros)
                {
                    if (!double.TryParse(
                        n,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out _))
                    {
                        Console.WriteLine("Número no válido: " + n);
                        return;
                    }
                }

                string[] invertidos = InvertirSerie(numeros, true);

                Console.WriteLine("Original:  " + string.Join(" ", numeros));
                Console.WriteLine("Invertido: " + string.Join(" ", invertidos));
            }
            else
            {
                Console.WriteLine("Opción no válida.");
            }
        }

        public static string InvertirTexto(string texto, bool mostrarProceso)
        {
            Pila<char> pila = new Pila<char>();

            if (mostrarProceso)
                Console.WriteLine("Agregando caracteres:");

            foreach (char caracter in texto)
            {
                pila.Push(caracter);

                if (mostrarProceso)
                    Console.WriteLine("Push('" + caracter + "') -> " + pila);
            }

            StringBuilder resultado = new StringBuilder();

            if (mostrarProceso)
                Console.WriteLine("Extrayendo caracteres:");

            while (!pila.IsEmpty())
            {
                char caracter = pila.Pop();
                resultado.Append(caracter);

                if (mostrarProceso)
                    Console.WriteLine(
                        "Pop(): '" + caracter +
                        "' | Resultado: " + resultado);
            }

            return resultado.ToString();
        }

        public static string[] InvertirSerie(
            string[] elementos, bool mostrarProceso)
        {
            Pila<string> pila = new Pila<string>();

            if (mostrarProceso)
                Console.WriteLine("Agregando números:");

            foreach (string elemento in elementos)
            {
                pila.Push(elemento);

                if (mostrarProceso)
                    Console.WriteLine("Push(" + elemento + ") -> " + pila);
            }

            string[] resultado = new string[elementos.Length];
            int posicion = 0;

            if (mostrarProceso)
                Console.WriteLine("Extrayendo números:");

            while (!pila.IsEmpty())
            {
                resultado[posicion] = pila.Pop();

                if (mostrarProceso)
                    Console.WriteLine("Pop(): " + resultado[posicion]);

                posicion++;
            }

            return resultado;
        }

        private static void MenuExpresiones()
        {
            Console.WriteLine();
            Console.WriteLine("--- Expresiones matemáticas ---");
            Console.WriteLine("Operadores: +, -, *, /, ^ y paréntesis.");
            Console.WriteLine("Ejemplo de expresión: (2 + 3) * 4");
            Console.Write("Escriba una expresión: ");

            string expresion = Console.ReadLine() ?? "";

            if (expresion.Trim().Length == 0)
            {
                Console.WriteLine("No ingresó ninguna expresión.");
                return;
            }

            ProcesarExpresion(expresion);
        }

        public static void ProcesarExpresion(string expresion)
        {
            Console.WriteLine();
            Console.WriteLine("Infija: " + expresion);

            try
            {
                List<string> tokens = Expresiones.Tokenizar(expresion);

                List<string> postfija =
                    Expresiones.InfijaAPostfija(tokens, true);

                Console.WriteLine(
                    "Postfija: " + string.Join(" ", postfija));

                double resultado =
                    Expresiones.EvaluarPostfija(postfija, true);

                Console.WriteLine(
                    "Resultado: " + Expresiones.Formatear(resultado));
            }
            catch (ErrorExpresion ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine("------------------------------");
        }
    }
}
