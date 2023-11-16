using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace algorithms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //
            //заполнение графа
            //
            Console.WriteLine("Выберите способ заполнения графа:\n1.Рандомное заполнение;\n2.Чтение с клавиатуры;\n3.Чтение с файла.");

            int choice = 0;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Ошибка ввода! Введите целое число.");
            }

            Graph graph = new Graph();//объявление графа
            int n = 0;//количество вершин
            //
            //Рандомное заполнение
            //
            if (choice == 1)
            { Console.WriteLine("Введите количество вершин у графа");
                
                while (!int.TryParse(Console.ReadLine(), out n))
                {
                    Console.WriteLine("Ошибка ввода! Введите целое число.");
                }

                graph = new Graph(n);
                graph.Generator(n);
            }
            //
            //Чтение с клавиатуры 
            //
            if (choice == 2)
            {        
                Console.WriteLine("Введите количество вершин у графа");
                while (!int.TryParse(Console.ReadLine(), out n))
                {
                    Console.WriteLine("Ошибка ввода! Введите целое число.");
                }

                graph = new Graph(n);

                for (int i = 0; i < n; i++)
                {
                    string str = Console.ReadLine();

                    int[] a = str.Split(' ').Select(x => int.Parse(x)).ToArray();

                    graph.Add(a, i);
                }
            }
            //
            //Чтение из файла
            //
            if (choice == 3)
            {
                Console.WriteLine("Введите путь до файла");
                string path = Console.ReadLine();

                string line;
                int k = 0;
                try
                {
                    StreamReader sr = new StreamReader(path);
                    line = sr.ReadLine();
                    int[] a = line.Split(' ').Select(x => int.Parse(x)).ToArray();
                    graph = new Graph(a.Length);
                    graph.Add(a, k);
                    k++;
                    line = sr.ReadLine();

                    while (line != null)
                    {
                        a = line.Split(' ').Select(x => int.Parse(x)).ToArray();
                        graph.Add(a, k);
                        line = sr.ReadLine();
                        k++;
                    }
                    n = a.GetLength(0);
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }
            //
            //Логика программы, чтение графа закончилось
            //
            Stopwatch sw1 = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            Stopwatch sw3 = new Stopwatch();

            Console.WriteLine("Граф:\n");
            graph.Print();

            Graph graph1 = new Graph(graph);
            Graph graph2 = new Graph(graph);

            Console.WriteLine();
            Console.Write("Алгоритм перебора: ");

            sw1.Start();
            var minVerCover = graph1.FindMinimumVertexCover();
            sw1.Stop();

            minVerCover.Sort();

            foreach (var item in minVerCover)
            {
                Console.Write($"{item + 1} ");
            }

            Console.WriteLine();
            Console.Write("Жадный алгоритм: ");

            sw2.Start();
            var greed = graph.Greedy_Algorithm();
            sw2.Stop();
            greed.Sort();

            foreach (var item in greed)
            {
                Console.Write($"{item + 1} ");
            }

            Console.WriteLine();
            Console.Write("Приближенный алгоритм: ");

            sw3.Start();
            var approx = graph2.Approximate_algorithm();
            sw3.Stop();
            approx.Sort();

            foreach (var item in approx)
            {
                Console.Write($"{item + 1} ");
            }


            Console.WriteLine();
            Console.WriteLine($"\nКол-во вершин - {n}");

            Console.WriteLine($"Кол-во вершин в покрытии алгоритмом перебора - {minVerCover.Count}\n" +
                              $"Кол-во вершин в покрытии жадным алгоритмом - {greed.Count}\n" +
                              $"Кол-во вершин в покрытии приближенным алгоритмом - {approx.Count}\n");

            Console.WriteLine($"Время выполнения программы - {sw1.ElapsedMilliseconds}");
            Console.WriteLine($"Время выполнения программы - {sw2.ElapsedMilliseconds}");
            Console.WriteLine($"Время выполнения программы - {sw3.ElapsedMilliseconds}");

            Console.ReadKey();
        }
    }
}
