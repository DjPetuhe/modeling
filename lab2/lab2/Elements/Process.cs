using lab2.Generators;

namespace lab2.Elements
{
    public class Process : Element
    {
        private int _queueMaxSize;
        private int _queueSize = 0;
        public bool Working { get; private set; } = false;
        public int FailureCount { get; private set; }
        public Process(string name, IGenerator delayGenerator, int queueMaxSize) : base(name, delayGenerator)
        {
            _queueMaxSize = queueMaxSize;
        }

        public override void MoveTo()
        {
            if (_queueSize >= _queueMaxSize)
            {
                FailureCount++;
                return;
            }
            if (Working)
                _queueSize++;
            else
                Working = true;
        }

        public override void NextStep()
        {
            Element? next = ChooseNextElement();
            GenerateNextTime();
            if (_queueSize == 0)
                Working = false;
            else
                _queueSize--;
            next?.MoveTo();
        }

        public override void PrintStatistic() => Console.WriteLine($"{Name}. Working: {Working}, Queue: {_queueSize}, Failure: {FailureCount}, Next time: {NextTime}");
    }
}
