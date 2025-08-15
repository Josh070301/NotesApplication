using NotesApplication.Interfaces;
using NotesApplication.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NotesApplication.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Note> GetAll()
        {
            return _context.Notes.Include(n => n.User).ToList();
        }

        public IEnumerable<Note> GetAllByUserId(int userId)
        {
            return _context.Notes.Where(n => n.UserId == userId).ToList();
        }

        public Note GetById(int id)
        {
            return _context.Notes.Find(id);
        }

        public void Add(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public void Update(Note note)
        {
            _context.Entry(note).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var note = _context.Notes.Find(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
            }
        }
    }
}