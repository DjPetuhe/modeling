using lab2;
using lab2.Elements;
using lab2.Generators;

namespace lab2
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            //ModelFromPicture();
            ComplexModel();
        }

        public static void ModelFromPicture()
        {
            IGenerator generatorCreate = new ExponentialGenerator(5);
            IGenerator generatorProcess1 = new ExponentialGenerator(5);
            IGenerator generatorProcess2 = new ExponentialGenerator(5);
            IGenerator generatorProcess3 = new ExponentialGenerator(5);

            Create cr = new("Create", generatorCreate);

            Process pr1 = new("Process1", generatorProcess1, 5);
            Process pr2 = new("Process2", generatorProcess2, 5);
            Process pr3 = new("Process3", generatorProcess3, 5);

            cr.AddNextElement(pr1, 1);

            pr1.AddNextElement(pr2, 1);
            pr2.AddNextElement(pr3, 1);

            Model mod = new(new List<Element>() { cr, pr1, pr2, pr3 });
            mod.Simulate(1000);
        }

        public static void ComplexModel()
        {
            IGenerator generatorCreate = new ExponentialGenerator(5);
            IGenerator generatorProcess1 = new ExponentialGenerator(5);
            IGenerator generatorProcess2 = new ExponentialGenerator(10);
            IGenerator generatorProcess3 = new ExponentialGenerator(10);

            Create cr = new("Create", generatorCreate);

            ComplexProcess pr1 = new("Process1", generatorProcess1, 5, 5);
            ComplexProcess pr2 = new("Process2", generatorProcess2, 5, 3);
            ComplexProcess pr3 = new("Process3", generatorProcess3, 5, 3);

            cr.AddNextElement(pr1, 1);

            pr1.AddNextElement(pr2, 1);
            pr1.AddNextElement(pr3, 1);

            pr2.AddNextElement(null, 3);
            pr2.AddNextElement(pr1, 1);

            pr3.AddNextElement(null, 3);
            pr3.AddNextElement(pr1, 1);

            Model mod = new(new List<Element>() { cr, pr1, pr2, pr3 });
            mod.Simulate(1000);
        }
    }
}