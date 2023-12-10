using lab3.Items;
using lab3.Selectors;
using lab3.Generators;

namespace lab3.Elements
{
    public abstract class Element
    {
        public readonly string Name;
        private readonly IGenerator _delayGenerator;
        protected Selector Selector { get; }

        public virtual double CurrentTime { get; set; }
        public double NextTime { get; protected set; } = double.MaxValue;

        public string MovedTo { get; protected set; } = "";

        public Element(string name, IGenerator delayGenerator, Selector selector)
        {
            Name = name;
            _delayGenerator = delayGenerator;
            Selector = selector;
        }

        public virtual void UpdateNextTime() => NextTime = CurrentTime + _delayGenerator.NextDelay();
        public abstract void NextStep();
        public virtual void MoveTo(Item item) { }
        public virtual void PrintEvent() => Console.Write($"\nEvent happened in {Name}. Moved to {MovedTo}");
        public abstract void PrintStatistic();
        public abstract void PrintResults();
    }
}
