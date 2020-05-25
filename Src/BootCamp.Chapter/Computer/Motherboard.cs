namespace BootCamp.Chapter.Computer
{
    public class Motherboard : Component
    {
        protected Motherboard(string productName, string manufacturer) : base(productName, manufacturer)
        {
            _manufacturer = manufacturer;
            _productName = productName;
        }
    }
}