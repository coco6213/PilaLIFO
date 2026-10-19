
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PilaLIFO
{
    public class ErrorExpresion : Exception
    {
        public ErrorExpresion(string mensaje) : base(mensaje)
        {
        }
    }

    public static class Expresiones
    {
        public static List<string> Tokenizar(string expresion)
        {
            List<string> tokens = new List<string>();
            int i = 0;

            while (i < expresion.Length)
            {
                char c = expresion[i];

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                if (char.IsDigit(c) || c == '.')
                {
                    int inicio = i;
                    bool punto = false;

                    while (i < expresion.Length &&
                           (char.IsDigit(expresion[i]) ||
                            expresion[i] == '.'))
                    {
                        if (expresion[i] == '.')
                        {
                            if (punto)
                                throw new ErrorExpresion(
                                    "Número decimal no válido.");

                            punto = true;
                        }

                        i++;
                    }

                    string numero = expresion.Substring(inicio, i - inicio);
                    double valor;

                    if (!double.TryParse(
                        numero,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out valor))
                    {
                        throw new ErrorExpresion(
                            "Número no válido: " + numero);
                    }

                    tokens.Add(numero);
                    continue;
                }

                if ("+-*/^()".IndexOf(c) >= 0)
                {
                    tokens.Add(c.ToString());
                    i++;
                    continue;
                }

                throw new ErrorExpresion(
                    "Carácter no válido: " + c);
            }

            if (tokens.Count == 0)
                throw new ErrorExpresion("La expresión está vacía.");

            return tokens;
        }

        public static List<string> InfijaAPostfija(
            List<string> tokens, bool mostrarProceso)
        {
            List<string> salida = new List<string>();
            Pila<string> operadores = new Pila<string>();

            foreach (string token in tokens)
            {
                double numero;

                if (double.TryParse(
                    token,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out numero))
                {
                    salida.Add(token);
                }
                else if (token == "(")
                {
                    operadores.Push(token);
                }
                else if (token == ")")
                {
                    bool encontrado = false;

                    while (!operadores.IsEmpty())
                    {
                        string op = operadores.Pop();

                        if (op == "(")
                        {
                            encontrado = true;
                            break;
                        }

                        salida.Add(op);
                    }

                    if (!encontrado)
                        throw new ErrorExpresion(
                            "Falta un paréntesis de apertura.");
                }
                else
                {
                    while (!operadores.IsEmpty() &&
                           operadores.Top() != "(" &&
                           (Prioridad(operadores.Top()) > Prioridad(token) ||
                           (Prioridad(operadores.Top()) == Prioridad(token) &&
                            token != "^")))
                    {
                        salida.Add(operadores.Pop());
                    }

                    operadores.Push(token);
                }

                if (mostrarProceso)
                {
                    Console.WriteLine(
                        "Procesado: " + token +
                        " | Salida: " + string.Join(" ", salida) +
                        " | Pila: " + operadores);
                }
            }

            while (!operadores.IsEmpty())
            {
                string op = operadores.Pop();

                if (op == "(")
                    throw new ErrorExpresion(
                        "Falta un paréntesis de cierre.");

                salida.Add(op);
            }

            return salida;
        }

        private static int Prioridad(string operador)
        {
            switch (operador)
            {
                case "+": 
                case "-": return 1;

                case "*": 
                case "/": return 2;

                case "^": return 3;

                default:
                    throw new ErrorExpresion(
                        "Operador no válido: " + operador);
            }
        }

        public static double EvaluarPostfija(
            List<string> tokens, bool mostrarProceso)
        {
            Pila<double> valores = new Pila<double>();

            foreach (string token in tokens)
            {
                double numero;

                if (double.TryParse(
                    token,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out numero))
                {
                    valores.Push(numero);
                }
                else
                {
                    if (valores.Cantidad < 2)
                        throw new ErrorExpresion(
                            "Faltan números para realizar la operación.");

                    double b = valores.Pop();
                    double a = valores.Pop();
                    double resultado;

                    switch (token)
                    {
                        case "+": resultado = a + b; break;
                        case "-": resultado = a - b; break;
                        case "*": resultado = a * b; break;

                        case "/":
                            if (b == 0)
                                throw new ErrorExpresion(
                                    "No se puede dividir entre cero.");

                            resultado = a / b;
                            break;

                        case "^": resultado = Math.Pow(a, b); break;

                        default:
                            throw new ErrorExpresion(
                                "Operador no válido: " + token);
                    }

                    valores.Push(resultado);
                }

                if (mostrarProceso)
                    Console.WriteLine(
                        "Procesado: " + token +
                        " | Pila: " + valores);
            }

            if (valores.Cantidad != 1)
                throw new ErrorExpresion(
                    "La expresión no está completa o es incorrecta.");

            return valores.Pop();
        }

        public static string Formatear(double resultado)
        {
            return resultado.ToString(
                "G", CultureInfo.InvariantCulture);
        }
    }
}
