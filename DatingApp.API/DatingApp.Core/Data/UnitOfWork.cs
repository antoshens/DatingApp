using AutoMapper;
using DatingApp.Core.Data.Repositories;
using DatingApp.Core.Model;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Core.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        // Repositories
        private IUserRepository _userRepository;

        public UnitOfWork(DataContext db, IMapper mapper, UserManager<User> userManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository is null)
                {
                    _userRepository = new UserRepository(_userManager, _db, _mapper);
                }

                return _userRepository;
            }
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
