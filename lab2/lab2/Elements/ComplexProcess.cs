using lab2.Generators;

namespace lab2.Elements
{
    public class ComplexProcess : Process
    {
        List<Process> _subProcesses = new();
        List<Process> _eventProcesses = new();
        public bool PartlyWorking { get; private set; }

        public override double CurrentTime
        {
            get { return _currentTime; }
            set
            {
                QueueSizeSum += (value - _currentTime) * QueueSize;
                _currentTime = value;
                foreach (Process process in _subProcesses)
                    process.CurrentTime = _currentTime;
            }
        }

        public ComplexProcess(string name, IGenerator delayGenerator, int queueMaxSize, int subProcessesCount) : base(name, delayGenerator, queueMaxSize)
        {
            for (int i = 0; i < subProcessesCount; i++)
                _subProcesses.Add(new($"{i + 1}", delayGenerator, 0));
        }

        public ComplexProcess(string name, IGenerator delayGenerator, int queueMaxSize, List<Process> subProcess) : base(name, delayGenerator, queueMaxSize)
        {
            _subProcesses = subProcess;
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
            Process subProcess = _subProcesses.Where(p => !p.FullWorking).First();
            subProcess.MoveTo();
            CheckWorkingStatus();
            UpdateNextTime();
        }

        public override void NextStep()
        {
            CountFinished++;
            Element? next = ChooseNextElement();
            _eventProcesses = _subProcesses.Where(p => p.NextTime == NextTime).ToList();
            foreach (Process process in _eventProcesses)
            {
                process.NextStep();
                if (QueueSize != 0)
                {
                    process.MoveTo();
                    QueueSize--;
                }
            }
            CheckWorkingStatus();
            UpdateNextTime();
            next?.MoveTo();
        }

        public override void PrintEvent()
        {
            foreach (var process in _eventProcesses)
                Console.Write($"\nEvent happened in {Name}.{process.Name} Moved to {_movedTo}");
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
            Console.Write($", Queue: {QueueSize}");
            Console.Write($", Failure: {FailureCount}");
            Console.Write($", Next time: {(NextTime == double.MaxValue ? "-" : NextTime)}");
        }
    }
}
