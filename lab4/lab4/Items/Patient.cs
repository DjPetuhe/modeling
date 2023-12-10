
namespace lab4.Items
{
    public class Patient : Item
    {
        private static readonly Random _rand = new();

        public Patient()
        {
            InitialType = _rand.NextDouble() switch
            {
                <= 0.5 => 1,
                <= 0.6 => 2,
                _ => 3
            };
            Type = InitialType;
        }
    }
}
