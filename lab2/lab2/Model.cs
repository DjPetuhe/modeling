using lab2.Elements;

namespace lab2
{
    public class Model
    {
        private readonly List<Element> _elements;
        private int _step;
        private double _currTime;

        public Model(List<Element> elements) => _elements = elements;

        public void Simulate(double totalTime)
        {
            _step = 0;
            _currTime = 0;
            List<Element> nextElements = new();
            PrintSteps();
            double nextTime = _elements.Min(el => el.NextTime);
            while (nextTime < totalTime)
            {
                _currTime = nextTime;
                _elements.ForEach(el => el.CurrentTime = _currTime);
                nextElements = _elements.Where(el => el.NextTime == _currTime).ToList();
                nextElements.ForEach(el => el.NextStep());
                PrintSteps(nextElements);
                nextTime = _elements.Min(el => el.NextTime);
            }
            PrintResults();
        }

        public void PrintSteps() => PrintSteps(new());

        public void PrintSteps(List<Element> nextElements)
        {
            _step++;
            Console.Write($"\nStep #{_step}");
            Console.Write($"\nCurrent time: {_currTime}");
            nextElements.ForEach(el => el.PrintEvent());
            _elements.ForEach(el => el.PrintStatistic());
            Console.WriteLine();
        }

        public void PrintResults()
        {
            Console.Write("\n" + new string('=', 30) + "RESULT" + new string('=', 30));
            _elements.ForEach(el => el.PrintResults());
            Console.WriteLine();
        }
    }
}
