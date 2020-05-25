using System;
using System.Collections.Generic;
using System.Text;

namespace BootCamp.Chapter.Computer
{
    public class Component
    {
        protected string _productName;
        protected string _manufacturer;

        protected Component(string productName, string manufacturer)
        {
            _productName = productName;
            _manufacturer = manufacturer;
        }
    }
}
