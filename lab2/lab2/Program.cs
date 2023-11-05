
using lab2;
using lab2.Elements;
using lab2.Generators;

IGenerator generator1 = new UniformGenerator(1, 5);
IGenerator generator2 = new UniformGenerator(4, 5);
Create cr = new("Create", generator1);
Process pr = new("Process", generator2, 5);

cr.AddNextElement(pr, 1);

Model mod = new(new List<Element>() { cr, pr });
mod.Simulate(100);