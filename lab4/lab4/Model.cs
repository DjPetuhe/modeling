﻿using lab4.Elements;
using System.Diagnostics;

namespace lab4
{
    public class Model
    {
        public bool IsPrintingSteps { get; set; } = false;
        public bool IsPrintingResults { get; set; } = false;
        private readonly Stopwatch _stopwatch = new();
        public Func<List<Element>, bool>? Addition { get; set; } = null;
        private readonly List<Element> _elements;
        private int _additionalEventHappened;
        private double _avarageItemsInModelSum;
        private int _startingItems;
        private int _step;
        private double _currTime;
        private double dif;

        public Model(List<Element> elements) => _elements = elements;

        public void Simulate(double totalTime)
        {
            _stopwatch.Start();
            _step = 0;
            _currTime = 0;
            _startingItems = 0;
            foreach (var el in _elements.OfType<lab4.Elements.Process>())
            {
                _startingItems += el.Queue.QueueSize;
                _startingItems += el.WorkingProcesses;
            }
            Dispose.Clear();
            List<Element> nextElements = new();
            PrintSteps();
            double nextTime = _elements.Min(el => el.NextTime);
            while (nextTime < totalTime)
            {
                dif = nextTime - _currTime;
                _currTime = nextTime;
                _elements.ForEach(el => el.CurrentTime = _currTime);
                nextElements = _elements.Where(el => el.NextTime == _currTime).ToList();
                nextElements.ForEach(el => el.NextStep());
                PrintSteps(nextElements);
                EvaluateStatistics();
                if (Addition?.Invoke(_elements) == true)
                {
                    if (IsPrintingSteps)
                        _elements.ForEach(el => el.PrintStatistic());
                    _additionalEventHappened++;
                }
                nextTime = _elements.Min(el => el.NextTime);
            }
            _stopwatch.Stop();
            PrintResults();
        }

        private void EvaluateStatistics()
        {
            int itemsInModel = _elements.OfType<Create>().Sum(cr => cr.Created) + _startingItems - Dispose.Destroyed;
            _avarageItemsInModelSum += itemsInModel * dif;
        }

        private void PrintSteps() => PrintSteps(new());

        private void PrintSteps(List<Element> nextElements)
        {
            _step++;
            if (!IsPrintingSteps)
                return;
            Console.Write($"\n\nStep #{_step}");
            Console.Write($"\nCurrent time: {_currTime}");
            nextElements.ForEach(el => el.PrintEvent());
            _elements.ForEach(el => el.PrintStatistic());
        }

        private void PrintResults()
        {
            if (!IsPrintingResults)
                return;
            Console.Write("\n\n" + new string('=', 30) + "RESULT" + new string('=', 30));
            _elements.ForEach(el => el.PrintResults());
            int totalCreated = _elements.OfType<Create>().Sum(cr => cr.Created);
            Console.Write($"\nAvarage items in model: {_avarageItemsInModelSum / _currTime}");
            Console.Write($"\nAvarage time for item in model: {Dispose.AvarageLifeTime}");
            Console.Write($"\nAvarage time between dispose: {_currTime / Dispose.Destroyed}");
            Console.Write($"\nAdditional event happened: {_additionalEventHappened}");
            Console.WriteLine();
        }

        public void PrintWorkingTime() => Console.WriteLine($"\nWorking time: {_stopwatch.ElapsedMilliseconds / 1000.0} seconds"); 
    }
}
