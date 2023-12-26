using AocHelper;

internal class Program
{
    private static void Main(string[] args)
    {
        var lines = File.ReadAllLines("Data.aoc");

        var data = new List<Hailstone>();

        foreach (var line in lines)
        {
            line.ParseMany("", ", ", out decimal[] pos, " @ ").ParseMany("", ", ", out decimal[] v);
            var hailstone = new Hailstone(pos[0], pos[1], pos[2], v[0], v[1], v[2]);

            data.Add(hailstone);
        }

        long result = 0;
        for (int i  = 0; i < data.Count - 1; i++)
        {
            for (int j = i + 1; j < data.Count; j++)
            {
                var hailstone1 = data[i];
                var hailstone2 = data[j];

                var a1 = hailstone1.VY / hailstone1.VX;
                var a2 = hailstone2.VY / hailstone2.VX;

                Console.WriteLine(hailstone1);
                Console.WriteLine(hailstone2);

                if ((a2 - a1) == 0)
                {
                    Console.WriteLine("parallel");
                    Console.WriteLine();
                    continue;
                }

                //var x = ((-a1 * hailstone1.X) + (hailstone1.VY) + (a2 * hailstone2.X) - (hailstone2.VY)) / (a2 - a1);
                //var t = (x - hailstone1.X) / hailstone1.VX;
                var x = (a2 * hailstone2.X - a1 * hailstone1.X + hailstone1.Y - hailstone2.Y) / (a2 - a1);
                var t1 = (x - hailstone1.X) / hailstone1.VX;
                var t2 = (x - hailstone2.X) / hailstone2.VX;
                var y = a1 * x - a1 * hailstone1.X + hailstone1.Y;

                 Console.WriteLine(x);
                //Console.WriteLine(t);
                if (t1 < 0 || t2 < 0)
                {
                    Console.WriteLine("past");
                    Console.WriteLine();
                    continue;
                }

                //if (x >= 200000000000000 && x <= 400000000000000)
                if (x >= 200000000000000 && x <= 400000000000000 && y >= 200000000000000 && y <= 400000000000000)
                //if (x >= 7 && x <= 27 && y >= 7 && y <= 27)
                {
                    result++;
                    Console.WriteLine("inside");
                }
                else
                {
                    Console.WriteLine("outside");
                }


                Console.WriteLine();

                //var hailstoneA = data[i];
                //var hailstoneB = data[j];

                //Console.WriteLine(hailstoneA);
                //Console.WriteLine(hailstoneB);

                //if (hailstoneA.VX - hailstoneB.VX == 0)
                //{ 
                //    Console.WriteLine("parallel");
                //    continue;
                //}

                //var t = (hailstoneB.X - hailstoneA.X) / (hailstoneA.VX - hailstoneB.VX);
                //if (t < 0)
                //{
                //    Console.WriteLine("crossed in the past");
                //    continue;
                //}

                //var x = hailstoneA.X + hailstoneA.VX * t;
                //Console.WriteLine(x);
            }
        }

        Console.WriteLine(result);
    }

    public record Hailstone(decimal X, decimal Y, decimal Z, decimal VX, decimal VY, decimal VZ);
}