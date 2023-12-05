using lab3.Items;
using lab3.Queues;
using lab3.Selectors;
using lab3.Generators;

namespace lab3.Elements
{
    public class Process : Element
    {
        public int CountFinished { get; protected set; }
        public bool FullWorking { get; protected set; }
        public int FailureCount { get; protected set; }

        private double _currentTime;
        public override double CurrentTime
        {
            get { return _currentTime; }
            set
            {
                Queue.UpdateQueueSizeSum(_currentTime, value);
                _currentTime = value;
            }
        }

        public Queue Queue { get; }
        protected Item? WorkingOn { get; set; }
        public Action<Item>? Addition { get; set; } = null;

        public Process(string name, IGenerator delayGenerator, Selector selector, int queueMaxSize)
            : this(name, delayGenerator, selector, new Queue(queueMaxSize)) { }

        public Process(string name, IGenerator delayGenerator, Selector selector, Queue queue) 
            : base(name, delayGenerator, selector) => Queue = queue;

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
            next?.MoveTo(finishedItem);
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
            Console.Write($", failure probability: {(CountFinished == 0 ? 0 : (double)FailureCount / (FailureCount + CountFinished))}");
            Console.Write($", avarage queue size: {Queue.QueueSizeSum / CurrentTime}");
        }
    }
}
