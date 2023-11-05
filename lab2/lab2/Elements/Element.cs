using lab2.Generators;

namespace lab2.Elements
{
    public abstract class Element
    {
        public readonly string Name;
        private readonly IGenerator _delayGenerator;
        private readonly Random _rand = new();

        public double CurrentTime { get; set; } = 0;
        public double NextTime { get; protected set; }

        private readonly List<(Element? el, int weight)> _nextElements = new();
        private int _weightSum = 0;

        public Element(string name, IGenerator delayGenerator)
        {
            Name = name;
            _delayGenerator = delayGenerator;
            GenerateNextTime();
        }

        public virtual void GenerateNextTime() => NextTime = CurrentTime + _delayGenerator.NextDelay();

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
            foreach(var (el, weight) in _nextElements)
            {
                if (randVal <= currentWeight)
                    return el;
                currentWeight += weight;
            }
            return null;
        }

        public abstract void NextStep();
        public virtual void MoveTo() { }
        public virtual void PrintEvent() => Console.WriteLine($"Event happened in {Name}.");
        public abstract void PrintStatistic();
    }
}
