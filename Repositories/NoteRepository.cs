using NotesApplication.Interfaces;
using NotesApplication.Models;
using System;
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

        public IEnumerable<Note> GetAllByUserId(Guid userId)
        {
            return _context.Notes.Where(n => n.UserId == userId).ToList();
        }

        public Note GetById(Guid id)
        {
            return _context.Notes.Find(id);
        }

        public void Add(Note note)
        {
            if (note.Id == Guid.Empty)
                note.Id = Guid.NewGuid();
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public void Update(Note note)
        {
            var existingNote = _context.Notes.Find(note.Id);
            if (existingNote != null)
            {
                // Update only the properties you want to allow to change
                existingNote.Title = note.Title;
                existingNote.Content = note.Content;
                existingNote.UpdatedAt = note.UpdatedAt;

                _context.SaveChanges();
            }
        }

        public void Delete(Guid id)
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