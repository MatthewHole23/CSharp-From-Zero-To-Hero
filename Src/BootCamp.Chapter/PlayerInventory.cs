﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BootCamp.Chapter
{
    class PlayerInventory : Inventory
    {
        private const int _sizeOfInventory = 15;
        public PlayerInventory()
        {
            _items = new Item[_sizeOfInventory];
        }

        public Item[] GetItems(string name)
        {
            return _items;           
        }
    }

}
