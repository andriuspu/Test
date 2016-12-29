using Data;

namespace Business
{
    public interface IProductFactory
    {
        Product Create(int productId);
    }
}