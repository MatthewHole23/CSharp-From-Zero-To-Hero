namespace BootCamp.Chapter.Computer
{
    public class Gpu : Component
    {
        protected Gpu(string productName, string manufacturer) : base(productName, manufacturer)
        {
            _manufacturer = manufacturer;
            _productName = productName;
        }
    }
}