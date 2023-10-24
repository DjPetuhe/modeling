
using lab1.Generators;

namespace lab1.Auxiliary
{
    public static class ConsoleWorker
    {
        public static Generator GetGenerator()
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("(0)Exponential\n(1)Normal\n(2)Uniform\nChooseDistribution:");
            } while (!int.TryParse(Console.ReadLine(), out choice));
            return choice switch
            {
                1 => new NormalGenerator(),
                2 => new UniformGenerator(),
                _ => new ExponentialGenerator()
            };
        }

        public static void PrintStatistics(List<double> numbers)
        {
            double avarage = numbers.Average();
            Console.WriteLine("Avarage: " + avarage);
            Console.WriteLine("Dispersion: " + numbers.Select(x => Math.Pow(x - avarage, 2)).Sum() / numbers.Count);
        }
    }
}
