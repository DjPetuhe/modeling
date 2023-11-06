using lab2.Generators;

namespace lab2.Elements
{
    public class Process : Element
    {
        public readonly int QueueMaxSize;
        public int QueueSize { get; private set; }
        public int CountFinished { get; private set; }
        public bool Working { get; private set; }
        public int FailureCount { get; private set; }
        public double QueueSizeSum { get; private set; }

        private double _currentTime;
        public override double CurrentTime
        {
            get { return _currentTime; }
            set
            {
                QueueSizeSum += (value - _currentTime) * QueueSize;
                _currentTime = value;
            }
        }

        public Process(string name, IGenerator delayGenerator, int queueMaxSize) : base(name, delayGenerator)
        {
            QueueMaxSize = queueMaxSize;
        }

        public override void MoveTo()
        {
            if (QueueSize >= QueueMaxSize && Working)
            {
                FailureCount++;
                return;
            }
            if (Working)
            {
                QueueSize++;
                return;
            }
            Working = true;
            GenerateNextTime();
        }

        public override void NextStep()
        {
            CountFinished++;
            Element? next = ChooseNextElement();
            GenerateNextTime();
            if (QueueSize == 0)
            {
                Working = false;
                NextTime = double.MaxValue;
            }
            else
                QueueSize--;
            next?.MoveTo();
        }

        public override void PrintStatistic()
        {
            Console.Write($"\n{Name}");
            Console.Write($", Working: {Working}");
            Console.Write($", Queue: {QueueSize}");
            Console.Write($", Failure: {FailureCount}");
            Console.Write($", Next time: {(NextTime == double.MaxValue ? "-" : NextTime)}");
        }

        public override void PrintResults()
        {
            Console.Write($"\n{Name}");
            Console.Write($", total failures: {FailureCount}");
            Console.Write($", total proceed: {CountFinished}");
            Console.Write($", failure probability: {(CountFinished == 0 ? 0 : (double)FailureCount / CountFinished)}");
            Console.Write($", avarage queue size: {QueueSizeSum / CurrentTime}");
        }
    }
}
