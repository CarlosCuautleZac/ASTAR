using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU1
{
    public static class JuegoHelper
    {
        public static bool[,]? Tablero { get; set; } = new bool[30, 30];
        public static int RenglonDestino { get; set; }
        public static int ColumnaDestino { get; set; }
    }
}
