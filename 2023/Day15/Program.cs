using System.Collections;
using System.Collections.Specialized;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await Problem2();

        
    }

    public static int Hash(string code)
    {
        var currentValue = 0;
        foreach (char c in code)
        {
            currentValue += c;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    static async Task Problem2()
    {
        string text = await File.ReadAllTextAsync("Data.aoc");
        var codes = text.Split(',');


        var boxes = new OrderedDictionary[256];

        foreach (string code in codes)
        {
            string[] comps;
            char command;
            if (code.Contains("-"))
            {
                command = '-';
                comps = code.Split("-");

            }
            else
            {
                command = '=';
                comps = code.Split("=");
            }

            string label = comps[0];
            int box = Hash(label);

            var dict = boxes[box];
            if (dict == null)
            {
                dict = new OrderedDictionary();
            }

            if (command == '-')
            {
                dict.Remove(label);
            }
            else
            {
                try
                {
                    int value = int.Parse(comps[1]);
                    if (dict.Contains(label))
                    {
                        dict[label] = value;
                    }
                    else
                    {
                        dict.Add(label, value);
                    }
                }
                catch
                {
                    Console.WriteLine("error");
                }
                
            }

            boxes[box] = dict;
        }

        long result = 0;
        for (int i = 0; i < boxes.Length; i++) 
        {
            var boxValue = i + 1;
            var dict = boxes[i];
            if (dict == null)
            {
                continue;
            }

            long placeValue = 1;
            foreach (DictionaryEntry item in dict)
            {
                long power = (int)item.Value * placeValue++ * boxValue;
                //Console.WriteLine(power);
                result += power;
            }
        }

        Console.WriteLine(result);
    }

    static async Task Problem1()
    {
        string text = await File.ReadAllTextAsync("Data.aoc");
        var codes = text.Split(',');

        long result = 0;
        foreach (string code in codes)
        {
            var currentValue = 0;
            foreach (char c in code)
            {
                currentValue += c;
                currentValue *= 17;
                currentValue %= 256;
            }

            Console.WriteLine(currentValue);
            result += currentValue;
        }

        Console.WriteLine(result);
    }
}