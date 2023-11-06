using lab2.Generators;

namespace lab2.Elements
{
    public class Process : Element
    {
        public readonly int QueueMaxSize;
        public int QueueSize { get; protected set; }
        public int CountFinished { get; protected set; }
        public bool FullWorking { get; protected set; }
        public int FailureCount { get; protected set; }
        public double QueueSizeSum { get; protected set; }

        protected double _currentTime;
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
            if (QueueSize >= QueueMaxSize && FullWorking)
            {
                FailureCount++;
                return;
            }
            if (FullWorking)
            {
                QueueSize++;
                return;
            }
            FullWorking = true;
            UpdateNextTime();
        }

        public override void NextStep()
        {
            CountFinished++;
            Element? next = ChooseNextElement();
            if (QueueSize == 0)
            {
                FullWorking = false;
                NextTime = double.MaxValue;
            }
            else
            {
                QueueSize--;
                UpdateNextTime();
            }
            next?.MoveTo();
        }

        public override void PrintStatistic()
        {
            Console.Write($"\n{Name}");
            Console.Write($", Working: {FullWorking}");
            Console.Write($", Queue: {QueueSize}");
            Console.Write($", Failure: {FailureCount}");
            Console.Write($", Next time: {(NextTime == double.MaxValue ? "-" : NextTime)}");
        }

        public override void PrintResults()
        {
            Console.Write($"\n{Name}");
            Console.Write($", total failures: {FailureCount}");
            Console.Write($", total proceed: {CountFinished}");
            Console.Write($", failure probability: {(CountFinished == 0 ? 0 : (double)FailureCount / (FailureCount + CountFinished))}");
            Console.Write($", avarage queue size: {QueueSizeSum / CurrentTime}");
        }
    }
}
