using lab2.Generators;

namespace lab2.Elements
{
    public abstract class Element
    {
        public readonly string Name;
        private readonly IGenerator _delayGenerator;
        private readonly Random _rand = new();

        public virtual double CurrentTime { get; set; }
        public double NextTime { get; protected set; } = double.MaxValue;

        private readonly List<(Element? el, int weight)> _nextElements = new();
        protected string _movedTo = "";
        private int _weightSum;

        public Element(string name, IGenerator delayGenerator)
        {
            Name = name;
            _delayGenerator = delayGenerator;
        }

        public virtual void UpdateNextTime() => NextTime = CurrentTime + _delayGenerator.NextDelay();

        public virtual void AddNextElement(Element? element, int weight)
        {
            if (weight <= 0)
                throw new ArgumentException("Weight must be more than 0");
            _nextElements.Add((element, weight));
            _weightSum += weight;
        }

        public virtual Element? ChooseNextElement()
        {
            int randVal = _rand.Next(_weightSum);
            int currentWeight = 0;
            foreach (var (el, weight) in _nextElements)
            {
                if (randVal <= currentWeight)
                {
                    _movedTo = el is null ? "Dispose" : el.Name;
                    return el;
                }
                currentWeight += weight;
            }
            _movedTo = "Dispose";
            return null;
        }

        public abstract void NextStep();
        public virtual void MoveTo() { }
        public virtual void PrintEvent() => Console.Write($"\nEvent happened in {Name}. Moved to {_movedTo}");
        public abstract void PrintStatistic();
        public abstract void PrintResults();
    }
}
