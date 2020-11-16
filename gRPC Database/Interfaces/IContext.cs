using System.Threading.Tasks;
using System.Collections.Generic;
using gRPC_Database.Models;

namespace gRPC_Database.Interfaces
{
    public interface IContext<T>
    {
        Task<int> CreateAsync(T model);
        Task<List<T>> ReadAsync();
        Task<T> UpdateAsync(T model);
        Task<int> DeleteAsync(int id);
    }
}
