
using lab2;
using lab2.Elements;
using lab2.Generators;

IGenerator generator1 = new UniformGenerator(1,3);
IGenerator generator2 = new ConstantGenerator(2);
IGenerator generator3 = new ConstantGenerator(4);

Create cr = new("Create", generator1);
Process pr1 = new("Process1", generator2, 5);
Process pr2 = new("Process2", generator3, 5);
Process pr3 = new("Process3", generator3, 5);

cr.AddNextElement(pr1, 1);

pr1.AddNextElement(pr2, 1);
pr1.AddNextElement(pr3, 1);

pr2.AddNextElement(pr1, 1);
pr2.AddNextElement(null, 1);

pr3.AddNextElement(pr1, 1);
pr3.AddNextElement(null, 1);

Model mod = new(new List<Element>() { cr, pr1, pr2, pr3 });
mod.Simulate(100);