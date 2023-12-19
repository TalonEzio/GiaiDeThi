using Thread = System.Threading.Thread;

namespace ReadWriteFile
{
    class Program
    {
        private const string FilePath = @"D:\dulieu.txt";

        private static readonly ManualResetEvent Event = new(false);
        private static int _randomInput, _randomOutput;
        private const int OutputSleepTime = 500;
        private static bool _isStop;
        static void Main()
        {
            var thread1 = new Thread(Input);
            var thread2 = new Thread(Output);
            thread1.Start();
            thread2.Start();

        }

        static void Input()
        {
            _randomInput = GenerateRandom(3);
            while (!_isStop)
            {

                Console.Write("--> Task 1: Moi nhap ky tu: ");
                var input = Console.ReadKey();
                Console.WriteLine();

                var key = input.KeyChar;

                using FileStream fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write);
                using StreamWriter writer = new StreamWriter(fs);
                writer.Write(key);

                if (input.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("--> Task 1 da ket thuc");
                    _isStop = true;
                }

                _randomInput--;
                if (_randomInput == 0)
                {
                    Console.WriteLine("Tam dung qua trinh ghi, chuyen qua doc");

                    writer.Close();
                    fs.Close();

                    Event.Set();

                    _randomInput = GenerateRandom(3);
                    Thread.Sleep(OutputSleepTime * _randomOutput + 100);
                }


                if (_isStop)
                {
                    writer.Close();
                    fs.Close();

                    Event.Set();
                    _randomOutput = Int32.MaxValue;
                };
            }

        }


        static void Output()
        {
            int seek = 0;
            _randomOutput = GenerateRandom(3,10);
            while (true)
            {
                Event.WaitOne();

                using FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                using StreamReader reader = new StreamReader(fs);

                if (seek > 0)
                {
                    var bufferTemp = new char[seek];
                    reader.Read(bufferTemp, 0, seek);
                }
                if (reader.EndOfStream)
                {
                    Console.WriteLine("--> Task 2: File da duoc doc het,quay tro lai qua trinh ghi");
                    _randomOutput = GenerateRandom(3, 10);

                    reader.Close();
                    fs.Close();

                    Event.Reset();
                    continue;
                }
                seek++;

                var key = reader.Read();
                Console.WriteLine($"--> Task 2: Ky tu doc duoc tu file: {(char)key}");
                Thread.Sleep(OutputSleepTime);

                if (key == (char)ConsoleKey.Escape)
                {
                    Console.WriteLine($"--> Task 2: Da ket thuc");
                    return;
                }

                _randomOutput--;
                if (_randomOutput == 0)
                {
                    Console.WriteLine("Tam dung qua trinh doc, chuyen qua ghi");
                    _randomOutput = GenerateRandom(3,10);
                    Event.Reset();
                }
            }

        }

        static int GenerateRandom(int value, int min = 3) => new Random().Next(value) + min;
    }
}
