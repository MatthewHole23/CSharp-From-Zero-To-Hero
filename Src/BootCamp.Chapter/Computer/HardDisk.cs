namespace BootCamp.Chapter.Computer
{
    public class HardDisk : Component
    {
        protected HardDisk(string productName, string manufacturer) : base(productName, manufacturer)
        {
            _manufacturer = manufacturer;
            _productName = productName;
        }
    }
}