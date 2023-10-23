
using System;

namespace lab1.Generators
{
    public abstract class Generator
    {
        public double GenerateNumber() => GenerateNumber(new Random());

        protected abstract double GenerateNumber(Random random);

        public virtual List<double> GenerateNumbers(double amount)
        {
            Random random = new();
            List<double> numbers = new();
            for (int i = 0; i < amount; i++)
            {
                numbers.Add(GenerateNumber(random));
            }
            return numbers;
        }
    }
}
