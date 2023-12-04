using lab3.Items;
using lab3.Elements;

namespace lab3.Selectors
{
    public abstract class Selector
    {
        public abstract Element? ChooseNextElement(Item item);
    }
}
