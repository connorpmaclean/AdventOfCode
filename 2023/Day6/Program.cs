Problem2();


static async Task Problem1()
{
    var lines = await File.ReadAllLinesAsync("./Data2.aoc");

    var times = lines[0].Split(' ').Where(c => c != "").Where(c => char.IsDigit(c[0])).Select(int.Parse).ToArray();
    var dists = lines[1].Split(' ').Where(c => c != "").Where(c => char.IsDigit(c[0])).Select(int.Parse).ToArray();

    int result = 1;
    for (int i = 0; i < times.Length; i++)
    {
        var time = times[i];
        var dist = dists[i];

        int possibilities = 0;
        for (int j = 0; j <= time; j++)
        {
            var speed = j;
            var timeRunning = time - j;

            var distRan = timeRunning * speed;
            if (distRan > dist)
            {
                possibilities++;
            }
        }

        result *= possibilities;
    }

    Console.WriteLine(result);
}

static void Problem2()
{
    // 291117211762026 = (57726992 - t)*t - 1

    var a = 28863496 - Math.Sqrt(541984189579989);
    var b = 28863496 + Math.Sqrt(541984189579989);

    Console.WriteLine(b - a); // Round down

    ///https://www.wolframalpha.com/input?i=291117211762026+%3D+%2857726992+-+t%29*t+-+1
}