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

        public static void PrintStatistics(List<double> numbers, Generator generator, int testCounts, int numbersCount)
        {
            double avarage = numbers.Average();
            Console.WriteLine("\nAvarage: " + avarage);
            Console.WriteLine("Variance: " + numbers.Select(x => Math.Pow(x - avarage, 2)).Sum() / numbers.Count);
            double confidence = ChiSquaredTest.Test(numbers, generator, out double x2, out double tablex2);
            Console.WriteLine("X^2: " + x2);
            Console.WriteLine("Table X^2: " + tablex2);
            Console.WriteLine("Confidence: " + Math.Round(confidence, 2));
            double avarageConfidence = 0;
            for (int i = 0; i < 100; i++)
            {
                List<double> testNumbers = generator.GenerateNumbers(numbersCount);
                avarageConfidence += ChiSquaredTest.Test(testNumbers, generator, out double _, out double _);
            }
            Console.WriteLine($"\nAvarage confidence chance for {testCounts} tests: {Math.Round(avarageConfidence / testCounts, 2)}");
        }
    }
}
