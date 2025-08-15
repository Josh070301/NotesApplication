using NotesApplication.Models;
using System.Collections.Generic;

namespace NotesApplication.Interfaces
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAll();
        IEnumerable<Note> GetAllByUserId(int userId);
        Note GetById(int id);
        void Add(Note note);
        void Update(Note note);
        void Delete(int id);
    }
}