using System.Security.Cryptography.X509Certificates;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await Problem1();
        await Problem2();

        static async Task Problem2()
        {
            var lines = await File.ReadAllLinesAsync("Data.aoc");
            int yLength = lines.Length + 2;
            int xLength = lines[0].Length + 2;
            var map = new char[xLength, yLength];

            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    if (x == 0 || y == 0 || x == (xLength - 1) || y == (yLength - 1))
                    {
                        map[x, y] = '#';
                    }
                    else
                    {
                        map[x, y] = lines[y - 1][x - 1];
                    }
                }
            }

            PrintMap(map, xLength, yLength);

            var dict = new Dictionary<string, long>();

            long result = 0;
            for (long i = 1; i <= 1000000000; i++)
            {
                SolveMap(map, yLength, xLength, null);
                //Console.WriteLine(key.ToString());
                //PrintMap(map, xLength, yLength);

                map = RotateMap(map, xLength, yLength);
                //PrintMap(map, xLength, yLength);

                SolveMap(map, yLength, xLength, null);
                map = RotateMap(map, xLength, yLength);
                //PrintMap(map, xLength, yLength);


                SolveMap(map, yLength, xLength, null);
                map = RotateMap(map, xLength, yLength);
                //PrintMap(map, xLength, yLength);

                var keyBuilder = new StringBuilder();
                SolveMap(map, yLength, xLength, keyBuilder);
                map = RotateMap(map, xLength, yLength);


                string key = keyBuilder.ToString();
                bool hasSkipped = false;
                if (dict.ContainsKey(key))
                {
                    //PrintMap(map, xLength, yLength);
                    result = ScoreMap(map, xLength, yLength);
                    long prev = dict[key];
                    Console.WriteLine($"Loop at {i}, previously seen at {prev}, result: {result}");

                    if (!hasSkipped)
                    {
                        long loop = i - prev;
                        long factor = (1000000000 - prev) / loop;

                        i = prev + ((factor) * loop);
                        hasSkipped = true;
                    }
                    
                }
                else
                {
                    result = ScoreMap(map, xLength, yLength);
                    Console.WriteLine($"{i}: {result}");
                    

                    dict.Add(key, i);
                }

                //PrintMap(map, xLength, yLength);
            }

            Console.WriteLine(result);


            //Console.WriteLine(result);
        }

        static char[,] RotateMap(char[,] map, int yLength, int xLength)
        {
            char[,] newArray = new char[xLength, yLength];

            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    newArray[xLength - j - 1, i] = map[i, j];
                }
            }

            return newArray;
        }

        static void PrintMap(char[,] map, int yLength, int xLength)
        {
            for (int y = 0; y < yLength; y++)
            {

                for (int x = 0; x < xLength; x++)
                {
                    Console.Write(map[x, y]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        static long ScoreMap(char[,] map, int yLength, int xLength)
        {
            long score = 0;
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    if (map[x, y] == 'O')
                    {
                        long local = yLength - y - 1;
                        score += local;
                    }
                }

            }

            return score;
        }

        static long SolveMap(char[,] map, int yLength, int xLength, StringBuilder? stateKey)
        {
            long result = 0;
            for (int x = 0; x < xLength; x++)
            {
                int rockCount = 0;
                for (int y = yLength - 1; y >= 0; y--)
                {
                    char c = map[x, y];
                    
                    if (c == 'O')
                    {
                        rockCount++;
                        map[x, y] = '.';
                        continue;
                    }

                    if (c == '#')
                    {
                        for (int i = 0; i < rockCount; i++)
                        {
                            long score = yLength - y - i - 1;
                            int yIndex = y + 1 + i;
                            map[x, yIndex] = 'O';

                            stateKey?.Append(x.ToString());
                            stateKey?.Append(",");
                            stateKey?.Append(y.ToString());
                            stateKey?.Append("|");

                            //Console.WriteLine(score);
                            result += score;
                        }

                        rockCount = 0;
                    }
                }

                //Console.WriteLine();
            }

            return result;
        }





        
    }

    public enum Direction
    {
        North,
        South,
        East,
        West,
    }

    static async Task Problem1()
    {
        var lines = await File.ReadAllLinesAsync("Data.aoc");

        long result = 0;
        for (int x = 0; x < lines[0].Length; x++)
        {
            int rockCount = 0;
            for (int y = lines.Length - 1; y >= 0; y--)
            {
                char c = lines[y][x];
                if (c == 'O')
                {
                    rockCount++;
                }

                bool isEnd = y == 0;
                if (c == '#' || isEnd)
                {
                    for (int i = 0; i < rockCount; i++)
                    {
                        long score = lines.Length - y - i - 1;
                        if (isEnd && c != '#')
                        {
                            score += 1;
                        }

                        //Console.WriteLine(score);
                        result += score;
                    }

                    rockCount = 0;
                }
            }

            //Console.WriteLine();
        }

        Console.WriteLine(result);
    }
}