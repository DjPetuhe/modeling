
namespace lab1.Generators
{
    public class ExponentialGenerator : Generator
    {
        public double Lambda { get; set; } = 100;

        protected override double GenerateNumber(Random random)
        {
            double number = random.NextDouble();
            number = number != 0 ? number : Double.Epsilon;
            return -Math.Log(number) / Lambda;
        }

        public override double GetFunctionValue(double x) => 1 - Math.Pow(Math.E, -Lambda * x);
    }
}
