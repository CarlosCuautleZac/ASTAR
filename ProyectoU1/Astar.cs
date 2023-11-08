using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU1
{
    public class Astar
    {
        List<Nodo> abiertos = new();
        List<Nodo> cerrados = new();

        public Nodo Buscar(Nodo origen, Nodo destino)
        {

            abiertos.Clear();
            cerrados.Clear();
            JuegoHelper.ColumnaDestino = destino.Columna;
            JuegoHelper.RenglonDestino = destino.Renglon;
            abiertos.Add(origen);
            bool existeRuta = true;
            bool solucionEncontrada = false;
            Nodo? mejorNodo = null;
            do
            {
                if (abiertos.Count == 0)
                {
                    existeRuta = false;
                }
                else
                {
                    mejorNodo = abiertos.OrderBy(n => n.F).First();
                    abiertos.Remove(mejorNodo);
                    cerrados.Add(mejorNodo);
                    Console.WriteLine($"{mejorNodo.Renglon},{mejorNodo.Columna}");
                    if (mejorNodo.Columna != destino.Columna
                        || mejorNodo.Renglon != destino.Renglon)
                    {
                        var sucesores = mejorNodo.GenerarSucesores();
                        foreach (var nodo in sucesores)
                        {
                            Nodo? viejo = abiertos
                                      .FirstOrDefault(n => n.Renglon == nodo.Renglon
                                                        && n.Columna == nodo.Columna);
                            if (viejo != null)
                            {
                                if (nodo.G < viejo.G)
                                {
                                    viejo.G = nodo.G;
                                    viejo.Padre = mejorNodo;
                                }
                            }
                            else
                            {
                                viejo = cerrados.FirstOrDefault(
                                        n => n.Renglon == nodo.Renglon &&
                                        n.Columna == nodo.Columna
                                    );
                                if (viejo != null)
                                {
                                    if (nodo.G < viejo.G)
                                    {
                                        viejo.Padre = mejorNodo;
                                        viejo.G = nodo.G;
                                        propagarG(viejo);
                                    }
                                }
                                else
                                {
                                    nodo.Padre = mejorNodo;
                                    abiertos.Add(nodo);
                                }
                            }
                        }
                    }
                    else
                    {
                        solucionEncontrada = true;
                    }
                }

            }
            while (existeRuta && !solucionEncontrada);
            if (solucionEncontrada)
            {
                List<Nodo> solucion = new List<Nodo>();
                if (mejorNodo != null)
                {
                    solucion.Add(mejorNodo);


                    while (mejorNodo.Padre != null)
                    {
                        mejorNodo = mejorNodo.Padre;
                        solucion.Add(mejorNodo);
                    }
                }
                solucion.Reverse();

                
                return solucion[1];
            }
            return new Nodo();
        }
        private void propagarG(Nodo nodo)
        {
            var hijos = abiertos.Where(n => n.Padre == nodo);
            foreach (var hijo in hijos)
            {
                hijo.G = nodo.G + 1;
            }
            hijos = cerrados.Where(n => n.Padre == nodo);
            foreach (var hijo in hijos)
            {
                hijo.G = nodo.G + 1;
                propagarG(hijo);
            }
        }
    }
}
