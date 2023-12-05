using lab3.Items;
using lab3.Selectors;
using lab3.Generators;

namespace lab3.Elements
{
    public class Create<T> : Element where T : Item, new()
    {
        private int _created;
        public Create(string name, IGenerator delayGenerator, Selector selector) 
            : base(name, delayGenerator, selector) => UpdateNextTime();

        public override void NextStep()
        {
            T item = new();
            Element? next = Selector.ChooseNextElement(item);
            MovedTo = next != null ? next.Name : "Dispose";
            next?.MoveTo(item);
            _created++;
            UpdateNextTime();
        }

        public override void PrintStatistic()
        {
            Console.Write($"\n{Name}");
            Console.Write($", Created: {_created}");
            Console.Write($", Next time: {NextTime}");
        }

        public override void PrintResults()
        {
            Console.Write($"\n{Name}");
            Console.Write($", Total created: {_created}.");
        }

        public void SetStartingTime(double time) => NextTime = time;
    }
}