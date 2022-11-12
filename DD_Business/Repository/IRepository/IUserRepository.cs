using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD_DataAccess;
using DD_Models;

namespace DD_Business.Repository.IRepository
{
    public interface IUserRepository
    {
        public Task<UserAddressDTO> GetUserAddress(string userId);
        public Task<UserAddressDTO> CreateAddress(UserAddressDTO objDTO);  
        public Task<UserAddressDTO> UpdateAddress(UserAddressDTO objDTO);

    }
}
