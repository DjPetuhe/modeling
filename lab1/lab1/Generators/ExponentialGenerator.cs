
namespace lab1.Generators
{
    public class ExponentialGenerator : Generator
    {
        public double Lambda { get; set; } = 2;

        protected override double GenerateNumber(Random random)
        {
            double number = random.NextDouble();
            number = number != 0 ? number : Double.Epsilon;
            return -1 / Lambda * Math.Log(number);
        }
    }
}
