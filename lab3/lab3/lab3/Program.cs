using lab3;
using lab3.Items;
using lab3.Elements;
using lab3.Selectors;
using lab3.Generators;

namespace lab2
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            CarBankModel();
        }

        public static void CarBankModel()
        {
            IGenerator generatorCreate = new ExponentialGenerator(0.5);
            IGenerator generatorCashier = new ExponentialGenerator(0.3);


            Process cashier1 = new("Cashier1", generatorCashier, 3);
            Process cashier2 = new("Cashier2", generatorCashier, 3);

            QueuePrioritySelector createSelector = new();
            createSelector.AddNextProcess(cashier1);
            createSelector.AddNextProcess(cashier2);
            Create<Item> cr = new("Create", generatorCreate, createSelector);

            static bool changeQueue(List<Element> elements)
            {
                bool swapped = false;
                List<Process> fullQueue = elements.OfType<Process>().Where(p => p.Queue.QueueSize >= 2).ToList();
                List<Process> emptyQueue = elements.OfType<Process>().Where(p => p.Queue.QueueSize <= 1).ToList();
                foreach (var full in fullQueue)
                {
                    foreach (var empty in emptyQueue)
                    {
                        if (full.Queue.QueueSize - empty.Queue.QueueSize >= 2)
                        {
                            swapped = true;
                            Console.Write($"\n\nCar moved from {full.Name} queue to {empty.Name} queue\n");
                            if (empty.Queue.QueueSize == 1)
                                emptyQueue.Remove(empty);
                            Item car = full.Queue.Dequeue();
                            empty.MoveTo(car);
                            break;
                        }
                    }
                }
                return swapped;
            }

            Model mod = new(new List<Element>() { cr, cashier1, cashier2 })
            {
                Addition = changeQueue
            };
            mod.Simulate(1000);
        }
    }
}