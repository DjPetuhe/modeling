using lab1.Generators;
using lab1.Auxiliary;

namespace lab1
{
    class Program
    {
        private const int Count = 10000;
        private const int TestCount = 100;

        public static void Main(string[] args)
        {
            Generator generator = ConsoleWorker.GetGenerator();
            List<double> numbers = generator.GenerateNumbers(Count);
            BarChartBuilder chart = new();
            chart.BuildBarChart(numbers, generator.GetType().Name);
            ConsoleWorker.PrintStatistics(numbers, generator, TestCount, Count);
        }
    }
}
