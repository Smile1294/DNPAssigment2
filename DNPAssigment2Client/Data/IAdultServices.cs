using System.Collections.Generic;
using System.Threading.Tasks;
using DNPAssigment1.Models;
using Models;

namespace DNPAssigment1.Persistance
{
    public interface IAdultServices
    {
        public Task<Adult> GetAdultAsync(int id);
        public Task UpdateAdultsAsync(Adult adult);
        public Task AddAdultAsync(Adult adult);
        public Task<IList<Adult>> GetAdultsAsync();

        public Task RemoveAdultAsync(int ID);



    }
}