using Auth.Models;

namespace Auth.Repository
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        bool Update(int id,Product product);
        bool Delete(int id);
        List<Product> GetByName(string name);


	}
}