
namespace lab1.Generators
{
    public class NormalGenerator : Generator
    {
        public double Sigma { get; set; } = 0.5;
        public double A { get; set; } = 2;
        private const int SumIterations = 12; 

        protected override double GenerateNumber(Random random)
        {
            double sum = 0;
            for (int i = 0; i < SumIterations; i++)
            {
                double number = random.NextDouble();
                sum += number != 0 ? number : Double.Epsilon;
            }
            return Sigma * (sum - 6) + A;
        }
    }
}
