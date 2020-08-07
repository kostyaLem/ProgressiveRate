using ProgressiveRate.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
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
                reader.FileProcessed += Reader_FileProcessed1;

                try
                {
                    var result = await reader.ReadTableAsync(@"C:\Users\Константин\Desktop\Test.xlsx", "Груз", 3, token);

                    Console.WriteLine(result.Rows.Count);
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

        private static void Reader_FileProcessed1(object sender, PositionEventArgs e)
        {
            Console.WriteLine(Math.Round((double)e.Position / e.Length * 100, 1) + " %");
        }
    }
}
