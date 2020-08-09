using ProgressiveRate.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressiveRate.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            Task.Factory.StartNew(async () =>
            {
                var reader = new ExcelReader();
                reader.FileProcessed += (s, e) => Console.WriteLine(Math.Round(e * 100, 1) + " %");

                try
                {
                    await reader.ReadTableAsync(@"C:\Users\Константин\Desktop\Test.xlsx", "Груз", 3, token);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });

            Console.CursorVisible = false;

            Console.ReadKey();

            cancellationTokenSource.Cancel();

            Console.ReadKey();
        }
    }
}
