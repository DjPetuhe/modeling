
namespace lab1.Generators
{
    public class UniformGenerator : Generator
    {
        public double A { get; set; } = Math.Pow(5, 13);
        public double C { get; set; } = Math.Pow(2, 31);
        public double Z { get; set; } = 1;

        protected override double GenerateNumber(Random _)
        {
            Z = A * Z % C;
            return Z / C;
        }

        public override List<double> GenerateNumbers(double amount)
        {
            List<double> numbers = new();
            for (int i = 0; i < amount; i++)
            {
                numbers.Add(GenerateNumber());
            }
            return numbers;
        }

        public override double GetFunctionValue(double x)
        {
            return x switch
            {
                < 0 => 0,
                > 1 => 1,
                _ => x
            };
        }
    }
}
