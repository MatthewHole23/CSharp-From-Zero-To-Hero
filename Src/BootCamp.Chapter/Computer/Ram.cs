namespace BootCamp.Chapter.Computer
{
    public class Ram : Component
    {
        protected Ram(string productName, string manufacturer) : base(productName, manufacturer)
        {
            _manufacturer = manufacturer;
            _productName = productName;
        }
    }
}