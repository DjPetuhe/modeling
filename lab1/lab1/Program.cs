using lab1.Generators;
using lab1.Auxiliary;

namespace lab1
{
    class Program
    {
        private const int Count = 100000;
        public static void Main(string[] args)
        {
            Generator generator = ConsoleWorker.GetGenerator();
            List<double> numbers = generator.GenerateNumbers(Count);
            ConsoleWorker.PrintStatistics(numbers);
            BarChartBuilder chart = new();
            chart.BuildBarChart(numbers, generator.GetType().Name);
        }
    }
}
