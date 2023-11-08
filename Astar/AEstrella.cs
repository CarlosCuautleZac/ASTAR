using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar
{
    public class AEstrella
    {
        private class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int G { get; set; }
            public int H { get; set; }
            public Node Parent { get; set; }
        }

        public static List<(int, int)> FindPath(int[,] grid, (int, int) start, (int, int) goal)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            // Crear nodos de inicio y objetivo
            Node startNode = new Node { X = start.Item1, Y = start.Item2, G = 0, H = Heuristic(start, goal) };
            Node goalNode = new Node { X = goal.Item1, Y = goal.Item2 };

            // Lista abierta y lista cerrada
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                // Encontrar el nodo con el valor F más bajo en la lista abierta
                Node currentNode = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].G + openList[i].H < currentNode.G + currentNode.H)
                    {
                        currentNode = openList[i];
                    }
                }

                // Mover el nodo actual de la lista abierta a la lista cerrada
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // Si el nodo actual es el objetivo, construir el camino y devolverlo
                if (currentNode.X == goalNode.X && currentNode.Y == goalNode.Y)
                {
                    return BuildPath(currentNode);
                }

                // Generar sucesores del nodo actual
                List<Node> successors = GetSuccessors(currentNode, grid, rows, cols);

                foreach (Node successor in successors)
                {
                    if (closedList.Contains(successor))
                    {
                        continue;
                    }

                    int tentativeG = currentNode.G + 1; // Costo del movimiento

                    if (!openList.Contains(successor) || tentativeG < successor.G)
                    {
                        successor.G = tentativeG;
                        successor.H = Heuristic(successor, goal);
                        successor.Parent = currentNode;

                        if (!openList.Contains(successor))
                        {
                            openList.Add(successor);
                        }
                    }
                }
            }

            // No se encontró un camino
            return null;
        }

        private static List<(int, int)> BuildPath(Node node)
        {
            List<(int, int)> path = new List<(int, int)>();
            while (node != null)
            {
                path.Insert(0, (node.X, node.Y));
                node = node.Parent;
            }
            return path;
        }

        private static List<Node> GetSuccessors(Node node, int[,] grid, int rows, int cols)
        {
            List<Node> successors = new List<Node>();
            int x = node.X;
            int y = node.Y;

            // Movimientos arriba, abajo, izquierda y derecha
            int[,] directions = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            foreach (var dir in directions)
            {
                int newX = x + dir[0];
                int newY = y + dir[1];

                // Verificar límites y obstáculos
                if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && grid[newX, newY] == 0)
                {
                    successors.Add(new Node { X = newX, Y = newY });
                }
            }

            return successors;
        }

        private static int Heuristic((int, int) a, (int, int) b)
        {
            // Heurística de distancia Manhattan
            return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
        }
    }

}

