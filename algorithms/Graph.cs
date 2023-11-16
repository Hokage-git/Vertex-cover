using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace algorithms
{
    internal class Graph
    {
        int[,] adjacency_matrix;//матрица смежности
        int n;

        //
        //Конструкторы
        //
        public Graph(int[,] adjacency_matrix)
        {
            this.adjacency_matrix = adjacency_matrix;
            n = adjacency_matrix.GetLength(0);
        }

        public Graph()
        {
            adjacency_matrix = new int[1,1];
            n = 1;
        }

        public Graph(int n)
        {
            adjacency_matrix = new int[n, n];
            this.n = n;
        }

        public Graph(Graph f)
        {
            adjacency_matrix = (int[,])f.adjacency_matrix.Clone();
            n = f.n;
        }
        //
        //Вывод изначального графа
        //
        public void Print()
        {
            for (int i = 0; i < adjacency_matrix.GetLength(0); i++)
                Console.Write($"  |v{i + 1}|");

            Console.WriteLine();
            for (int i = 0; i < adjacency_matrix.GetLength(0); i++)
            {
                Console.Write($"v{i + 1}");
                for (int j = 0; j < adjacency_matrix.GetLength(1); j++)
                    Console.Write($"|{adjacency_matrix[i, j]} |  ");

                Console.WriteLine();
            }
        }
        //
        //Создает граф размерностью n*n
        //
        public void Generator(int n)//Генерация рёбер
        {
            adjacency_matrix = new int[n, n];
            Random rand = new Random();

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    adjacency_matrix[i, j] = adjacency_matrix[j, i] = rand.Next(2);//1 - ребро есть, 0 - нету ребра
                }
            }

            this.n = adjacency_matrix.GetLength(0);
        }

        protected int[] Find_Vector_Power()//массив содержащий степени вершин
        {
            int[] vertex_power = new int[adjacency_matrix.GetLength(0)];

            for (int i = 0; i < adjacency_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacency_matrix.GetLength(1); j++)
                {
                    if (adjacency_matrix[i, j] == 1) vertex_power[i] += 1;//если из вершины выходит ребро, то степень += 1
                }
            }

            return vertex_power;
        }

        protected void Delete_Edges(int vertex)//удаление связанных с вершиной рёбер
        {
            for (int i = 0; i < adjacency_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacency_matrix.GetLength(1); j++)
                {
                    if (i == vertex || j == vertex)
                        adjacency_matrix[i, j] = 0; //По факту зануление ребра, что в условиях задачи одно и то же
                }
            }
        }

        protected int Find_Index(int[] vertex_power)//Поиск вершины с макс. степенью
        {
            int vertex = 0;
            int max = 0;

            for (int i = 0; i < vertex_power.Length; i++)
            {
                if (vertex_power[i] > max)
                {
                    max = vertex_power[i];
                    vertex = i;
                }
            }

            return vertex;
        }

        public List<int> Greedy_Algorithm()//жадный алгоритм
        {
            List<int> greedy_algorithm = new List<int>();
            int[] vertex_power = Find_Vector_Power();

            while (vertex_power.Max() != 0)
            {
                int index = Find_Index(vertex_power);//Поиск вершины с макс степенью

                greedy_algorithm.Add(index);//Добавление вершины в множество

                Delete_Edges(index);//Удаление инцедентных рёбер

                vertex_power = Find_Vector_Power();//Массив степеней вершин

            }

            return greedy_algorithm;
        }

        //
        // Алгоритм перебора
        //

        private List<int> bestCover = new List<int>();

        public List<int> FindMinimumVertexCover()
        {
            List<int> currentCover = new List<int>();
            RecursiveFindMinimumVertexCover(0, currentCover);

            return bestCover;
        }

        private void RecursiveFindMinimumVertexCover(int vertex, List<int> currentCover)
        {
            if (vertex == n)
            {
                if (IsVertexCover(currentCover) && (bestCover.Count == 0 || currentCover.Count < bestCover.Count))
                {
                    bestCover = new List<int>(currentCover);
                }
                return;
            }

            currentCover.Add(vertex);
            RecursiveFindMinimumVertexCover(vertex + 1, currentCover);
            currentCover.Remove(vertex);
            RecursiveFindMinimumVertexCover(vertex + 1, currentCover);
        }

        private bool IsVertexCover(List<int> cover)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (adjacency_matrix[i, j] == 1 && !cover.Contains(i) && !cover.Contains(j))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        //
        //Приближенный алгоритм
        //

        protected void Delete_Edges(List<int> vertex)//удаление связанных с вершиной рёбер
        {
            for (int i = 0; i < adjacency_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacency_matrix.GetLength(1); j++)
                {
                    if (i == vertex[0] || j == vertex[0] || i == vertex[1] || j == vertex[1])
                        adjacency_matrix[i, j] = 0; //По факту зануление ребра, что в условиях задачи одно и то же
                }
            }
        }

        protected List<int> Search_Edges()//Поиск произвольного ребра
        {
            List<int> vertex = new List<int>();
            for (int i = 0; i < adjacency_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacency_matrix.GetLength(1); j++)
                {
                    if (adjacency_matrix[i, j] == 1)
                    {
                        vertex.Add(i);
                        vertex.Add(j);
                        break;
                    }


                }
            }
            return vertex;
        }
        //Приближенный алгоритм
        public List<int> Approximate_algorithm()
        {
            List<int> approximate_algorithm = new List<int>();
            int[] vertex_power = Find_Vector_Power();
            while (vertex_power.Max() != 0)
            {
                List<int> index = Search_Edges();//

                approximate_algorithm.Add(index[0]);//Добавление вершин в множество
                approximate_algorithm.Add(index[1]);

                Delete_Edges(index);//Удаление инцедентных рёбер

                vertex_power = Find_Vector_Power();//

            }

            return approximate_algorithm;
        }

        public void Add(int[] mas, int k)//Добавление массива в определенную строку матрицы
        {
            for (int j = 0; j < n; j++)
            {
                adjacency_matrix[k, j] = mas[j];
            }
        }
    }
}
