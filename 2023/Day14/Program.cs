using System.Security.Cryptography.X509Certificates;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await Problem1();
        await Problem2();

        static async Task Problem2()
        {
            var lines = await File.ReadAllLinesAsync("Data.aoc");
            int yLength = lines.Length;
            int xLength = lines[0].Length;
            var map = new char[lines[0].Length, lines.Length];

            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines.Length; y++)
                {
                    map[x, y] = lines[y][x];
                }
            }

            long result = SolveMap(map, yLength, xLength);

            Console.WriteLine(result);
        }

        static long SolveMap(char[,] map, int yLength, int xLength)
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
                    }

                    bool isEnd = y == 0;
                    if (c == '#' || isEnd)
                    {
                        for (int i = 0; i < rockCount; i++)
                        {
                            long score = yLength - y - i - 1;
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