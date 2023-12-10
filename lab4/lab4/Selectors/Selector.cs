using lab4.Items;
using lab4.Elements;

namespace lab4.Selectors
{
    public abstract class Selector
    {
        public abstract Element? ChooseNextElement(Item item);
    }
}
