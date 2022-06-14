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
        private IPhotoRepository _photoRepository;
        private IMessageRepository _messageRepository;

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

        public IPhotoRepository PhotoRepository
        {
            get
            {
                if (_photoRepository is null)
                {
                    _photoRepository = new PhotoRepository(_db, _mapper);
                }

                return _photoRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                if (_messageRepository is null)
                {
                    _messageRepository = new MessageRepository(_db, _mapper);
                }

                return _messageRepository;
            }
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
