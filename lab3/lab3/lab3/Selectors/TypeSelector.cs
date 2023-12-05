﻿using lab3.Elements;
using lab3.Items;

namespace lab3.Selectors
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
            foreach (var el in _nextElements)
            {
                if (el.type == item.Type)
                    return el.element;
            }    
        }
    }
}