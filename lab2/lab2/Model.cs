using lab2.Elements;

namespace lab2
{
    public class Model
    {
        private List<Element> _elements;
        private int _step;
        private double _currTime;

        public Model(List<Element> elements)
        {
            _elements = elements;
        }

        public void Simulate(double totalTime)
        {
            _step = 0;
            _currTime = 0;
            while (_currTime < totalTime)
            {
                double nextTime = _elements.Min(el => el.NextTime);
                _currTime = nextTime;
                _elements.ForEach(el => el.CurrentTime = _currTime);
                List<Element> nextElements = _elements.Where(el => el.NextTime == nextTime).ToList();
                nextElements.ForEach(el => el.NextStep());
                PrintSteps(nextElements);
            }
        }

        public void PrintSteps(List<Element> nextElements)
        {
            _step++;
            Console.WriteLine($"Step #{_step}");
            Console.WriteLine($"Current time: {_currTime}");
            nextElements.ForEach(el => el.PrintEvent());
            _elements.ForEach(el => el.PrintStatistic());
            Console.WriteLine();
        }

        public void PrintResults()
        {

        }
    }
}
