
namespace lab3.Items
{
    public class Patient : Item
    {
        private static readonly Random _rand = new();

        public Patient()
        {
            Type = _rand.NextDouble() switch
            {
                <= 0.5 => 1,
                <= 0.6 => 2,
                _ => 3
            };
        }
    }
}
