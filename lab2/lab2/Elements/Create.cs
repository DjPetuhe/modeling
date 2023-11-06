using lab2.Generators;

namespace lab2.Elements
{
    public class Create : Element
    {
        private int _created;
        public Create(string name, IGenerator delayGenerator) : base(name, delayGenerator) => UpdateNextTime();

        public override void NextStep()
        {
            Element? next = ChooseNextElement();
            next?.MoveTo();
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
    }
}