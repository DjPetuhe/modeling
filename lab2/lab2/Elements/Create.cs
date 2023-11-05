using lab2.Generators;

namespace lab2.Elements
{
    public class Create : Element
    {
        private int _created = 0;
        public Create(string name, IGenerator delayGenerator) : base(name, delayGenerator) { }

        public override void NextStep()
        {
            Element? next = ChooseNextElement();
            next?.MoveTo();
            _created++;
            GenerateNextTime();
        }

        public override void PrintStatistic() => Console.WriteLine($"{Name}. Created: {_created}, Next time: {NextTime}");
    }
}