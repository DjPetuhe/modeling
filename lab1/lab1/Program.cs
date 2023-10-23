using lab1.Generators;
using ScottPlot;
using ScottPlot.Statistics;

namespace lab1
{
    class Program
    {
        public static void Main(string[] args)
        {
            Generator generator = new NormalGenerator();
            List<double> numbers = generator.GenerateNumbers(1000);
            foreach (double number in numbers)
            {
                Console.WriteLine(number);
            }
        }
    }
}
