﻿using lab4.Elements;
using lab4.Items;

namespace lab4.Selectors
{
    public class TypeSelector : Selector
    {
        List<(int type, Element? element)> _nextElements = new();

        public void AddElementForType(int type, Element? element)
        {
            if (_nextElements.Any(el => el.type == type))
                throw new ArgumentException("There is already transition for this type");
            _nextElements.Add((type, element));
        }

        public override Element? ChooseNextElement(Item item)
        {
            foreach (var (type, element) in _nextElements)
            {
                if (type == item.Type)
                    return element;
            }
            return null;
        }
    }
}
