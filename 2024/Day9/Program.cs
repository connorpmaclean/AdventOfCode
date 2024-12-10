internal class Program
{

    // 6444783503027 too low
    private static void Main(string[] args)
    {
        P2();

    }

    private static void P2()
    {
        string line = File.ReadAllText("./data.aoc");
        int id = 0;
        Node end = null;
        Node start = null;
        Stack<Node> stack = new Stack<Node>();
        foreach (char c in line)
        {
            int size = c - '0';

            Node newNode;
            if (end == null || end.Id == -1)
            {
                newNode = new Node(id++, size);
                stack.Push(newNode);
            }
            else
            {
                newNode = new Node(-1, size);
            }

            if (start == null)
            {
                start = newNode;
            }

            newNode.Previous = end;
            if (end != null)
            {
                end.Next = newNode;
            }

            end = newNode;
        }

        DefragP2(end, start, stack);
        Dump(start);
        CalculateCheckSum(start);
    }

    private static void DefragP2(Node end, Node start, Stack<Node> stack)
    {
        while (stack.TryPop(out Node next))
        {
            Node front = start;
            while (front.Id != -1 || front.Size < next.Size)
            {
                if (front == next)
                {
                    break;
                }

                front = front.Next;

                if (front == null)
                {
                    break;
                }
            }

            if (front == null || front == next)
            {
                continue;
            }

            if (next.Size == front.Size)
            {
                front.Id = next.Id;
                next.Id = -1;

            }
            else // front > back
            {
                var newEmptyNode = new Node(-1, front.Size - next.Size);
                front.Id = next.Id;
                front.Size = next.Size;
                front.InsertAfter(newEmptyNode);
                next.Id = -1;
            }

            //Dump(start);
        }


    }

    private static void P1()
    {
        string line = File.ReadAllText("./data.aoc");
        int id = 0;
        Node end = null;
        Node start = null;
        foreach (char c in line)
        {
            int size = c - '0';

            Node newNode;
            if (end == null || end.Id == -1)
            {
                newNode = new Node(id++, size);
            }
            else
            {
                newNode = new Node(-1, size);
            }

            if (start == null)
            {
                start = newNode;
            }

            newNode.Previous = end;
            if (end != null)
            {
                end.Next = newNode;
            }

            end = newNode;
        }

        DefragP1(end, start);
        //Defrag(end, start);
        Dump(start);
        CalculateCheckSum(start);
    }

    private static void DefragP1(Node end, Node start)
    {
        Node front = start;
        Node back = end;
        while (true)
        {
            //front = start;
            while (front.Id != -1)
            {
                if (front == back)
                {
                    return;
                }

                if (front == null)
                {
                    return;
                }

                front = front.Next;
                
            }

            while (back.Id == -1)
            {
                if (front == back)
                {
                    return;
                }

                if (back == null)
                {
                    return;
                }

                back = back.Previous;
            }

            if (back.Size == front.Size)
            {
                front.Id = back.Id;
                back.Id = -1;

            }
            else if (back.Size > front.Size)
            {
                front.Id = back.Id;
                back.Size = back.Size - front.Size;
                back.InsertAfter(new Node(-1, front.Size));
                back = back.Next;
            }
            else // front > back
            {
                var newEmptyNode = new Node(-1, front.Size - back.Size);
                front.Id = back.Id;
                front.Size = back.Size;
                front.InsertAfter(newEmptyNode);
                back.Id = -1;
            }

            //front = front.Previous ?? front;
            //back = back.Next ?? back;

            //Dump(start);
        }

        
    }

    private static long CalculateCheckSum(Node node)
    {
        long sum = 0;
        int counter = 0;
        while (node != null)
        {
            //if (node.Id  == -1)
            //{
            //    break;
            //}
            int value = node.Id == -1 ? 0 : node.Id;
            for (int i = 0; i < node.Size; i++)
            {
                sum += value * counter++;
            }

            node = node.Next;
        }

        Console.WriteLine(sum);
        return sum;
    }

    private static void Dump(Node node)
    {
        while (node != null)
        {
            char c = node.Id == -1 ? '.' : (char)(node.Id  % 10 + '0');
            for (int i = 0; i < node.Size; i++)
            {
                Console.Write(c);
            }

            node = node.Next;
        }

        Console.WriteLine();
    }

    public class Node
    {
        public Node(int id, int size)
        {
            Id = id;
            Size = size;
        }

        public int Id;
        public int Size;

        public Node Next { get; set; }

        public Node Previous { get; set; }

        public void InsertAfter(Node node)
        {
            node.Next = Next;
            if (Next != null)
            {
                Next.Previous = node;
            }
            
            Next = node;
            node.Previous = this;
        }

        public void InsertBefore(Node node)
        {
            Previous.InsertAfter(node);
        }

        public override string ToString()
        {
            return $"{Id} ({Size})";
        }
    }
}