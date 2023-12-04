using lab3.Items;
using lab3.Generators;
using lab3.Selectors;

namespace lab3.Elements
{
    public class ComplexProcess : Process
    {
        private readonly List<Process> _subProcesses = new();
        private List<Process> _eventProcesses = new();
        public bool PartlyWorking { get; private set; }

        private double _currentTime;
        public override double CurrentTime
        {
            get { return _currentTime; }
            set
            {
                Queue.UpdateQueueSizeSum(_currentTime, value);
                _currentTime = value;
                foreach (Process process in _subProcesses)
                    process.CurrentTime = _currentTime;
            }
        }

        public ComplexProcess(string name, IGenerator delayGenerator, Selector selector, int queueMaxSize, int subProcessesCount) 
            : base(name, delayGenerator, selector, queueMaxSize)
        {
            for (int i = 0; i < subProcessesCount; i++)
                _subProcesses.Add(new($"{i + 1}", delayGenerator, Selector, 0));
        }

        public ComplexProcess(string name, IGenerator delayGenerator, Selector selector, int queueMaxSize, List<Process> subProcess) 
            : base(name, delayGenerator, selector, queueMaxSize) => _subProcesses = subProcess;

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
            Process subProcess = _subProcesses.Where(p => !p.FullWorking).First();
            subProcess.MoveTo(item);
            CheckWorkingStatus();
            UpdateNextTime();
        }

        public override void NextStep()
        {
            CountFinished++;
            _eventProcesses = _subProcesses.Where(p => p.NextTime == NextTime).ToList();
            foreach (Process process in _eventProcesses)
            {
                process.NextStep();
                if (!Queue.IsEmpty)
                    process.MoveTo(Queue.Dequeue());
            }
            CheckWorkingStatus();
            UpdateNextTime();
        }

        public override void PrintEvent()
        {
            foreach (var process in _eventProcesses)
                Console.Write($"\nEvent happened in {Name}.{process.Name} Moved to {process.MovedTo}");
        }

        private void CheckWorkingStatus()
        {
            if (_subProcesses.All(p => p.FullWorking))
            {
                FullWorking = true;
                PartlyWorking = true;
                return;
            }
            FullWorking = false;
            PartlyWorking = _subProcesses.Any(p => p.FullWorking);
        }

        public override void UpdateNextTime() => NextTime = _subProcesses.Min(p => p.NextTime);

        public override void PrintStatistic()
        {
            Console.Write($"\n{Name}");
            Console.Write($", Working processes: {_subProcesses.Where(p => p.FullWorking).Count()}");
            Console.Write($", Queue: {Queue.QueueSize}");
            Console.Write($", Failure: {FailureCount}");
            Console.Write($", Next time: {(NextTime == double.MaxValue ? "-" : NextTime)}");
        }
    }
}
