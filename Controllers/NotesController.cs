using NotesApplication.Filters;
using NotesApplication.Helpers;
using NotesApplication.Interfaces;
using NotesApplication.ViewModels;
using NotesApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace NotesApplication.Controllers
{
    [JwtAuthorize]
    public class NotesController : Controller
    {
        private readonly INoteRepository _noteRepository;

        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // GET: Notes
        public ActionResult Index(NoteFilterViewModel filter)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return RedirectToAction("Login", "Account");
                
            IEnumerable<Note> notes = _noteRepository.GetAllByUserId(userId);
            
            // Apply filtering
            if (!string.IsNullOrEmpty(filter.TitleSearch))
            {
                notes = notes.Where(n => n.Title.Contains(filter.TitleSearch, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrEmpty(filter.ContentSearch))
            {
                notes = notes.Where(n => n.Content != null && 
                              n.Content.Contains(filter.ContentSearch, StringComparison.OrdinalIgnoreCase));
            }
            
            // Apply sorting
            if (filter.CreatedAtSortOrder == SortOrder.Ascending)
            {
                notes = notes.OrderBy(n => n.CreatedAt);
            }
            else if (filter.CreatedAtSortOrder == SortOrder.Descending)
            {
                notes = notes.OrderByDescending(n => n.CreatedAt);
            }
            
            if (filter.UpdatedAtSortOrder == SortOrder.Ascending)
            {
                notes = notes.OrderBy(n => n.UpdatedAt);
            }
            else if (filter.UpdatedAtSortOrder == SortOrder.Descending)
            {
                notes = notes.OrderByDescending(n => n.UpdatedAt);
            }
            
            // Pass both notes and filter model to view
            ViewBag.Filter = filter;
            return View(notes.ToList());
        }

        // GET: Notes/Details/5
        public ActionResult Details(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return RedirectToAction("Login", "Account");
                
            var note = _noteRepository.GetById(id);
            
            if (note == null || note.UserId != userId)
                return HttpNotFound();
                
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            if (GetCurrentUserId() == Guid.Empty)
                return RedirectToAction("Login", "Account");
                
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return RedirectToAction("Login", "Account");
                
            if (ModelState.IsValid)
            {
                note.UserId = userId;
                note.CreatedAt = DateTime.Now;
                
                _noteRepository.Add(note);
                
                return RedirectToAction("Index");
            }
            
            return View(note);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return RedirectToAction("Login", "Account");
                
            var note = _noteRepository.GetById(id);
            
            if (note == null || note.UserId != userId)
                return HttpNotFound();
                
            return View(note);
        }

        // POST: Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return RedirectToAction("Login", "Account");
                
            if (ModelState.IsValid)
            {
                var existingNote = _noteRepository.GetById(note.Id);
                
                if (existingNote == null || existingNote.UserId != userId)
                    return HttpNotFound();
                    
                note.UserId = userId;
                note.UpdatedAt = DateTime.Now;
                
                _noteRepository.Update(note);
                
                return RedirectToAction("Index");
            }
            
            return View(note);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return RedirectToAction("Login", "Account");
                
            var note = _noteRepository.GetById(id);
            
            if (note == null || note.UserId != userId)
                return HttpNotFound();
                
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return RedirectToAction("Login", "Account");
                
            var note = _noteRepository.GetById(id);
            
            if (note == null || note.UserId != userId)
                return HttpNotFound();
                
            _noteRepository.Delete(id);
            
            return RedirectToAction("Index");
        }

        private Guid GetCurrentUserId()
        {
            var cookie = Request.Cookies["auth_token"];
            if (cookie == null)
                return Guid.Empty;

            var principal = JwtHelper.ValidateToken(cookie.Value);
            if (principal == null)
                return Guid.Empty;

            var identity = principal.Identity as ClaimsIdentity;
            var userIdClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim?.Value, out var guid) ? guid : Guid.Empty;
        }
    }
}