using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU1
{
    public class Nodo
    {
        #region Estado
        public int Renglon { get; set; }
        public int Columna { get; set; }
        #endregion

        #region Heuristica
        public int G { get; set; }
        public int H => Math.Abs(Renglon - JuegoHelper.RenglonDestino) + Math.Abs(Columna - JuegoHelper.ColumnaDestino);
        public int F => G + H;
        #endregion

        #region Padre y Sucesores
        public Nodo? Padre { get; set; }
        public IEnumerable<Nodo> GenerarSucesores()
        {
            int[] movimientosRenglon = { -1, 1, 0, 0 };
            int[] movimientosColumna = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int r = Renglon + movimientosRenglon[i];
                int c = Columna + movimientosColumna[i];
                if (JuegoHelper.Tablero != null)
                {
                    if (r >= 0 && r < JuegoHelper.Tablero.GetLength(0) &&
                        c >= 0 && c < JuegoHelper.Tablero.GetLength(1) &&
                        !JuegoHelper.Tablero[c, r])
                    {
                        yield return new Nodo
                        {
                            Renglon = r,
                            Columna = c,
                            G = G + 1
                        };
                    }
                }
            }

        }
        public override string ToString()
        {
            return $"{Renglon},{Columna}";
        }
        #endregion
    }
}
