using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD_Business.Repository.IRepository;
using DD_Common;
using DD_DataAccess;
using DD_DataAccess.Data;
using DD_DataAccess.ViewModel;
using DD_Models;

namespace DD_Business.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UserAddressDTO> CreateAddress(UserAddressDTO objDTO)
        {
            try
            {
                var obj = _mapper.Map<UserAddressDTO, UserAddress>(objDTO);
                _db.UserAddress.Add(obj);
                await _db.SaveChangesAsync();
                var userAddress = new UserAddressDTO();
                userAddress = _mapper.Map<UserAddress, UserAddressDTO>(obj);
                return userAddress;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserAddressDTO> UpdateAddress(UserAddressDTO objDTO)
        {
            try
            {
                var objFromDb = await _db.UserAddress.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
                if (objFromDb != null)
                {
                    objFromDb.Name = objDTO.Name;
                    objFromDb.StreetAddress = objDTO.StreetAddress;
                    objFromDb.City = objDTO.City;
                    objFromDb.State = objDTO.State;
                    objFromDb.PostalCode = objDTO.PostalCode;
                    _db.UserAddress.Update(objFromDb);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<UserAddress, UserAddressDTO>(objFromDb);
                }
                return objDTO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<UserAddressDTO> GetUserAddress(string userId)
        {
            UserAddress userAddress = await _db.UserAddress.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userAddress != null)
            {
                return _mapper.Map<UserAddress, UserAddressDTO>(userAddress);
            }
            return new UserAddressDTO();
        }
    }
}
