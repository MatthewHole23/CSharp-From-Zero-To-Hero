using System;
using System.Collections.Generic;
using System.Text;

namespace BootCamp.Chapter.Computer
{
    public class Body : Component
    {
        public Body(string productName, string manufacturer) : base(productName, manufacturer)
        {
            _manufacturer = manufacturer;
            _productName = productName;
        }
    }
}
