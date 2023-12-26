// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


string[] lines = File.ReadAllLines("./data1.txt");

long[][] data = new long[2][];
for (int i = 0; i < data.Length; i++) 
{
    data[i] = new long[lines.Length];
}

int c = 0;
foreach (string line in lines) 
{
    string[] comps = line.Split("   ");
    int y = 0;
    foreach (var comp in comps) 
    {
        data[y++][c] = long.Parse(comp);
    }
    
    c++;
}


Array.Sort(data[0]);
Array.Sort(data[1]);

long result = 0;
for (int i = 0; i < data[0].Length; i++)
{
    result += Math.Abs(data[0][i] - data[1][i]);
}

Console.WriteLine(result);

var dict = data[1].GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

long result2 = 0;
foreach (var item in data[0])
{
    if (dict.TryGetValue(item, out int value))
    {
        result2 += value * item;
    }
}

Console.WriteLine(result2);

