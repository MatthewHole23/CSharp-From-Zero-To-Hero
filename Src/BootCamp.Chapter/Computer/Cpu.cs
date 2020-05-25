using System;
using System.Security.Cryptography.X509Certificates;

namespace BootCamp.Chapter.Computer
{
    public class Cpu : Component
    {
        public Cpu(string productName, string manufacturer) : base(productName, manufacturer)
        {
            _manufacturer = manufacturer;
            _productName = productName;
        }
    }
}