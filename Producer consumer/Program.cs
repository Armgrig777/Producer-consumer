namespace Producer_consumer
{
    public class Program
    {
        static Queue<int> buffer = new Queue<int>();
        private readonly static object o = new object();

        static void Produce()
        {
            Random rand = new Random(); 
         
            while (true)
            {
                int number = rand.Next(0, 100);

                lock (o)
                {
                    buffer.Enqueue(number);

                    if (buffer.Count > 100)
                    {
                        Monitor.Pulse(o);
                        Monitor.Wait(o);
                    }
                }
            }
            Thread.Sleep(rand.Next(0, 200));
        }

        static void Consume()
        {
            Random rand = new Random();
            while (true)
            {


                lock (o)
                {
                    if (buffer.Count < 90)
                    {
                        Monitor.Pulse(o);
                    }

                    if (buffer.Count == 0)
                    {
                        Monitor.Pulse(o);
                        Monitor.Wait(o);
                    }

                    int num = buffer.Dequeue();
                    Console.WriteLine(num);

                }

                Thread.Sleep(rand.Next(0, 200));
            }
        }

        static void Main(string[] args)
        {
            int n = 5;
            Thread[] producer = new Thread[n];
            Thread[] consumer = new Thread[n];
            for (int i = 0; i < n; i++)
            {
                producer[i] = new Thread(Produce);
                consumer[i] = new Thread(Consume);

                producer[i].Start();
                consumer[i].Start();
            }
            for (int i = 0; i < n; i++)
            {
                producer[i].Join();
                consumer[i].Join();
            }

        }
    }
}