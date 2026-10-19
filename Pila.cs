
using System;
using System.Text;

namespace PilaLIFO
{
    public class Pila<T>
    {
        private const int CapacidadInicial = 4;

        private T[] elementos;
        private int tope;

        public Pila()
        {
            elementos = new T[CapacidadInicial];
            tope = -1;
        }

        public int Cantidad
        {
            get { return tope + 1; }
        }

        public void Push(T valor)
        {
            if (tope == elementos.Length - 1)
            {
                AmpliarCapacidad();
            }

            tope++;
            elementos[tope] = valor;
        }

        public T Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException(
                    "No se puede hacer Pop(): la pila esta vacia.");
            }

            T valor = elementos[tope];
            elementos[tope] = default(T);
            tope--;

            return valor;
        }
        public T Top()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException(
                    "No se puede hacer Top(): la pila esta vacia.");
            }

            return elementos[tope];
        }

        public bool IsEmpty()
        {
            return tope == -1;
        }

        public void Clear()
        {
            for (int i = 0; i <= tope; i++)
            {
                elementos[i] = default(T);
            }

            tope = -1;
        }

        private void AmpliarCapacidad()
        {
            T[] nuevoArreglo = new T[elementos.Length * 2];

            for (int i = 0; i < elementos.Length; i++)
            {
                nuevoArreglo[i] = elementos[i];
            }

            elementos = nuevoArreglo;
        }

        public override string ToString()
        {
            if (IsEmpty())
            {
                return "[ ]";
            }

            StringBuilder texto = new StringBuilder("[ ");

            for (int i = 0; i <= tope; i++)
            {
                texto.Append(elementos[i]);
                texto.Append(' ');
            }

            texto.Append("] <- Tope");

            return texto.ToString();
        }
    }
}
