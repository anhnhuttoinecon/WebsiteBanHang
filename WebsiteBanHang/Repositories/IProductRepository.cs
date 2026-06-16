namespace WebsiteBanHang.Repositories;
/*using global::WebsiteBanHang.Models;*/
using System.Collections.Generic;
using WebsiteBanHang.Models;

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }







/*using WebsiteBanHang.Models;

public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
*/
