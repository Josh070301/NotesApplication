using NotesApplication.Models;
using System;
using System.Collections.Generic;

namespace NotesApplication.Interfaces
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAll();
        IEnumerable<Note> GetAllByUserId(Guid userId);
        Note GetById(Guid id);
        void Add(Note note);
        void Update(Note note);
        void Delete(Guid id);
    }
}