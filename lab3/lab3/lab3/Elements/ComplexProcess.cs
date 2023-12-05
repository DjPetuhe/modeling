﻿using lab3.Items;
using lab3.Generators;
using lab3.Selectors;
using lab3.Queues;

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

        private Action<Item>? _addition = null;
        public new Action<Item>? Addition 
        { 
            get { return _addition; }
            set 
            {
                _addition = value;
                foreach (Process process in _subProcesses)
                    process.Addition = _addition;
            }
        }


        public ComplexProcess(string name, IGenerator delayGenerator, Selector selector, Queue queue, int subProcessesCount) 
            : base(name, delayGenerator, selector, queue)
        {
            for (int i = 0; i < subProcessesCount; i++)
                _subProcesses.Add(new($"{i + 1}", delayGenerator, Selector, 0));
        }

        public ComplexProcess(string name, IGenerator delayGenerator, Selector selector, Queue queue, List<Process> subProcess) 
            : base(name, delayGenerator, selector, queue) => _subProcesses = subProcess;

        public ComplexProcess(string name, IGenerator delayGenerator, Selector selector, int queueMaxSize, int subProcessesCount)
            : this(name, delayGenerator, selector, new Queue(queueMaxSize), subProcessesCount) { }

        public ComplexProcess(string name, IGenerator delayGenerator, Queue queue, int subProcessesCount)
            : this(name, delayGenerator, new WeightSelector(), queue, subProcessesCount) { }

        public ComplexProcess(string name, IGenerator delayGenerator, int queueMaxSize, int subProcessesCount)
            : this(name, delayGenerator, new WeightSelector(), new Queue(queueMaxSize), subProcessesCount) { }

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
