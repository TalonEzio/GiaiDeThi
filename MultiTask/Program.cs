namespace MultiTask
{
    internal class Program
    {
        private static readonly CancellationTokenSource Cts = new();
        private static readonly List<char> C = new();
        private static readonly ManualResetEventSlim Task2StartEvent = new(false);
        private static readonly ManualResetEventSlim Task3StartEvent = new(false);

        private static void Main()
        {
            var thread1 = new Thread(Task1);
            var thread2 = new Thread(Task2);
            var thread3 = new Thread(Task3);
            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread3.Join();

            Console.WriteLine("Bam phim bat ki de ket thuc chuong trinh");
            Console.ReadLine();
        }

        private static void Task1()
        {
            while (!Cts.IsCancellationRequested)
            {
                Console.Write("Nhap ky tu: ");
                var key = Console.ReadKey();
                Console.WriteLine();

                C.Add(key.KeyChar);

                if (key.Key == ConsoleKey.X)
                {
                    Console.WriteLine("_Task1: ket thuc");
                    Cts.Cancel();
                }

                Task2StartEvent.Set();
                Thread.Sleep(1);

            }
            Task3StartEvent.Set();
        }

        private static void Task2()
        {

            while (true)
            {
                Task2StartEvent.Wait();

                if (Cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine("Task2 ket thuc theo Task1");
                    return;
                }
                Console.WriteLine($"Task 2: Ky tu : {C[^1]}");

                Task2StartEvent.Reset();

            }
        }
        static void Task3()
        {
            Task3StartEvent.Wait();
            Console.WriteLine($"So luong ky tu A: {C.Count(x => x.Equals('A'))}");
            Task3StartEvent.Reset();
        }
    }
}