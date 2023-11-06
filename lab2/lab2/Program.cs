
using lab2;
using lab2.Elements;
using lab2.Generators;

IGenerator generator1 = new ConstantGenerator(1);
IGenerator generator2 = new UniformGenerator(3, 4);
IGenerator generator3 = new UniformGenerator(10, 20);

Create cr = new("Create", generator1);

ComplexProcess pr1 = new("Process1", generator2, 5, 3);
ComplexProcess pr2 = new("Process2", generator3, 5, 3);
ComplexProcess pr3 = new("Process3", generator3, 5, 3);

cr.AddNextElement(pr1, 1);

pr1.AddNextElement(pr2, 1);
pr1.AddNextElement(pr3, 1);

//pr2.AddNextElement(pr1, 1);
pr2.AddNextElement(null, 1);

//pr3.AddNextElement(pr1, 1);
pr3.AddNextElement(null, 1);

Model mod = new(new List<Element>() { cr, pr1, pr2, pr3 });
mod.Simulate(100);