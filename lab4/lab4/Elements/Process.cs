using lab4.Items;
using lab4.Queues;
using lab4.Selectors;
using lab4.Generators;

namespace lab4.Elements
{
    public class Process : Element
    {
        public int CountFinished { get; protected set; }
        public bool FullWorking { get; protected set; }
        public int FailureCount { get; protected set; }
        public double FailurePercent
        {
            get { return CountFinished == 0 ? 0 : Math.Round((double)FailureCount * 100 / (FailureCount + CountFinished), 3); }
        }

        public double WorkingTimePercent
        {
            get { return Math.Round(WorkingTime * 100 / CurrentTime, 3); }
        }

        private double _currentTime;
        public override double CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (FullWorking) WorkingTime += value - _currentTime;
                Queue.UpdateQueueSizeSum(_currentTime, value);
                _currentTime = value;
            }
        }

        public double WorkingTime { get; protected set; }

        public int WorkingProcesses
        {
            get { return FullWorking ? 1 : 0; }
        }

        public Queue Queue { get; }
        protected Item? WorkingOn { get; set; }
        public Action<Item>? Addition { get; set; } = null;

        public Process(string name, IGenerator delayGenerator, Selector selector, Queue queue)
            : base(name, delayGenerator, selector) => Queue = queue;

        public Process(string name, IGenerator delayGenerator, Selector selector, int queueMaxSize)
            : this(name, delayGenerator, selector, new Queue(queueMaxSize)) { }

        public Process(string name, IGenerator delayGenerator, Queue queue)
            : this(name, delayGenerator, new WeightSelector(), queue) { }

        public Process(string name, IGenerator delayGenerator, int queueMaxSize)
            : this(name, delayGenerator, new Queue(queueMaxSize)) { }

        public override void MoveTo(Item item)
        {
            if (Queue.IsFull && FullWorking)
            {
                FailureCount++;
                return;
            }
            if (FullWorking)
            {
                Queue.Enqueue(item);
                return;
            }
            FullWorking = true;
            WorkingOn = item;
            UpdateNextTime();
        }

        public override void NextStep()
        {
            CountFinished++;
            Item finishedItem = WorkingOn ?? throw new ArgumentException("Can't finish unexisting item.");
            Addition?.Invoke(finishedItem);
            if (Queue.IsEmpty)
            {
                FullWorking = false;
                NextTime = double.MaxValue;
                WorkingOn = null;
            }
            else
            {
                WorkingOn = Queue.Dequeue();
                UpdateNextTime();
            }
            Element? next = Selector.ChooseNextElement(finishedItem);
            MovedTo = next != null ? next.Name : "Dispose";
            if (next == null) Dispose.Destroy(finishedItem, CurrentTime);
            else next.MoveTo(finishedItem);
        }

        public override void PrintStatistic()
        {
            Console.Write($"\n{Name}");
            Console.Write($", Working: {FullWorking}");
            Console.Write($", Queue: {Queue.QueueSize}");
            Console.Write($", Failure: {FailureCount}");
            Console.Write($", Next time: {(NextTime == double.MaxValue ? "-" : NextTime)}");
        }

        public override void PrintResults()
        {
            Console.Write($"\n{Name}");
            Console.Write($", total failures: {FailureCount}");
            Console.Write($", total proceed: {CountFinished}");
            Console.Write($", failure percent: {FailurePercent}%");
            Console.Write($", Working time percent: {WorkingTimePercent}%");
            Console.Write($", avarage queue size: {Math.Round(Queue.QueueSizeSum / CurrentTime, 3)}");
        }

        public virtual void SetStartingWorkingOn(Item item, double finishTime)
        {
            WorkingOn = item;
            FullWorking = true;
            NextTime = finishTime;
        }
    }
}
