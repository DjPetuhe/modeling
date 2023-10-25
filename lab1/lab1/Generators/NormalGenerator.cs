
namespace lab1.Generators
{
    public class NormalGenerator : Generator
    {
        public double Sigma { get; set; } = 0.5;
        public double A { get; set; } = 0;
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

        public override double GetFunctionValue(double x) => (1 + Erf((x - A) / (Math.Sqrt(2) * Sigma))) / 2;

        private static double Erf(double x)
        {
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x);

            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - ((((a5 * t + a4) * t + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }
    }
}
