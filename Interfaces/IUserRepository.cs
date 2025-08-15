using NotesApplication.Models;
using System;
using System.Collections.Generic;

namespace NotesApplication.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        User GetByEmail(string email);
        User GetByUsername(string username);
        void Add(User user);
        void Update(User user);
        void Delete(Guid id);
    }
}