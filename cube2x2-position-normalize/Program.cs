using System;

[assembly: CLSCompliant(true)]
namespace Grayscale.Cube2X2PositionNormalize
{
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// プログラム。
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// サイズを小さくした定跡ファイルへのパス。
        /// </summary>
        private const string BookMinPath = "./book-min.txt";

        /// <summary>
        /// エントリーポイント。
        /// </summary>
        /// <param name="args">コマンドライン引数。</param>
        public static void Main(string[] args)
        {
            ReadBookMin();

            // Thread.Sleep(15);
            Console.ReadKey();
        }

        /// <summary>
        /// 定跡読込。
        /// </summary>
        public static void ReadBookMin()
        {
            if (File.Exists(Program.BookMinPath))
            {
                int row = 0;
                foreach (var line in File.ReadAllLines(Program.BookMinPath))
                {
                    var tokens = line.Split(' ');

                    // 次の一手。
                    var handle = int.Parse(tokens[1], CultureInfo.CurrentCulture);

                    // 現局面。
                    var currentPositionText = tokens[0];

                    Console.WriteLine(string.Format(
                        CultureInfo.CurrentCulture,
                        "CurrentPosition: {0}.",
                        currentPositionText));

                    // 正規化する。
                    var normalizer = new Normalizer();
                    (var normalizedPosition, var normalizedMove) = normalizer.Normalize(Position.Parse(currentPositionText), handle);

                    Console.WriteLine(string.Format(
                        CultureInfo.CurrentCulture,
                        "NormalizedPosition: {0}.",
                        normalizedPosition.BoardText));

                    for (int i = 0; i < 64; i++)
                    {
                        Console.WriteLine(string.Format(
                            CultureInfo.CurrentCulture,
                            "[{0}] {1} {2}.",
                            i,
                            normalizer.IsomorphicPosition[i].BoardText,
                            normalizer.RotateView(i, handle)));
                    }

                    row++;
                }
            }
        }
    }
}
