namespace MultiTask2
{
    public class Program
    {
        //ManualResetEvent, AutoResetEvent, ManualResetEvenSlim -> System.Threading
        private static readonly List<char> C = new();
        private static readonly ManualResetEvent Task2ContinueEvent = new(false);
        private static readonly ManualResetEvent Task3ContinueEvent = new(false);
        private static bool _isStopTask1;
        static void Main()
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

        static void Task1()
        {
            while (!_isStopTask1)
            {
                Console.Write("--> Task 1: Moi nhap ky tu: ");
                var input = Console.ReadKey();
                Console.WriteLine();

                C.Add(input.KeyChar);

                if (input.KeyChar == '!')
                {
                    _isStopTask1 = true;

                    Console.WriteLine("Task 1 da dung lai");
                }

                Task2ContinueEvent.Set(); // cho phép chạy
                Thread.Sleep(1);
            }

            Task3ContinueEvent.Set();

        }

        static void Task2()
        {
            while (!_isStopTask1)
            {
                Task2ContinueEvent.WaitOne();//đăng ký chờ cho phép thực hiện

                if (_isStopTask1) break;
                Console.WriteLine($"--> Task 2:Ky tu: {C[^1]}");

                Task2ContinueEvent.Reset();// reset trạng thái chờ -> quay trở về chờ tiếp
            }
            Console.WriteLine("Task 2 da dung lai");
            //ket thuc task2 -> ket thuc thread 2
        }

        static void Task3()
        {
            Task3ContinueEvent.WaitOne();

            Console.WriteLine($"So luong ky tu A da tao ra: {C.Count(x => x == 'A')}");

            Task3ContinueEvent.Reset();
        }
    }
}