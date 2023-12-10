using lab4.Elements;
using lab4.Generators;
using lab4.Selectors;

namespace lab4
{
    public class ModelCreator
    {
        public static Model CreateModel(int smoCount)
        {
            if (smoCount <= 0)
                throw new ArgumentException("smoCount param must be more than 0");

            IGenerator createGenerator = new ExponentialGenerator(1);
            IGenerator processGenerator = new ExponentialGenerator(1);

            List<Element> elements = new();
            WeightSelector selectorCreate = new();
            Create create = new("create", createGenerator, selectorCreate);

            Element? nextElement = null;
            for (int i = 0; i < smoCount; i++)
            {
                WeightSelector selector = new();
                selector.AddNextElement(nextElement, 1);
                Process process = new((smoCount - i).ToString(), processGenerator, selector, 5);
                elements.Add(process);
                nextElement = process;
            }
            selectorCreate.AddNextElement(elements.Last(), 1);

            elements.Add(create);
            elements.Reverse();

            return new(elements);
        }

        public static Model CreateModelWithLines(int smoLineCount, int lines)
        {
            if (smoLineCount <= 0)
                throw new ArgumentException("smoLineCount param must be more than 0");
            if (lines <= 1)
                throw new ArgumentException("lines param must be more than 1");

            IGenerator createGenerator = new ExponentialGenerator(1.0 / lines);
            IGenerator processGenerator = new ExponentialGenerator(1);

            List<Element> elements = new();

            WeightSelector selectorCreate = new();
            Create create = new("create", createGenerator, selectorCreate);
            elements.Add(create);
            for (int i = 0; i < lines; i++)
            {
                List<Element> lineElements = new();
                Element? nextElement = null;
                for (int j = 0; j < smoLineCount; j++)
                {
                    WeightSelector selector = new();
                    selector.AddNextElement(nextElement, 1);
                    Process process = new($"{i + 1}.{smoLineCount - j}", processGenerator, selector, 5);
                    lineElements.Add(process);
                    nextElement = process;
                }
                selectorCreate.AddNextElement(lineElements.Last(), 1);
                lineElements.Reverse();
                elements.AddRange(lineElements);
            }

            return new(elements);
        }
    }
}
