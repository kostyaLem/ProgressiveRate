using ProgressiveRate.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(async () =>
            {
                var reader = new ExcelReader();
                reader.FileProcessed += Reader_FileProcessed;

                await reader.ReadWorkSheet(@"C:\Users\Константин\Desktop\file.txt");
            });

            Console.CursorVisible = false;


            Console.ReadKey();
        }

        private static void Reader_FileProcessed(object sender, Tuple<long, long> e)
        {
            Console.WriteLine(Math.Round((double)e.Item1 / e.Item2 * 100, 1) + " %");
        }
    }
}
