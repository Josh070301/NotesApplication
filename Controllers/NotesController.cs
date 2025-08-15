using NotesApplication.Helpers;
using NotesApplication.Interfaces;
using NotesApplication.Models;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace NotesApplication.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INoteRepository _noteRepository;

        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // GET: Notes
        public ActionResult Index()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");
                
            var notes = _noteRepository.GetAllByUserId(userId);
            return View(notes);
        }

        // GET: Notes/Details/5
        public ActionResult Details(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");
                
            var note = _noteRepository.GetById(id);
            
            if (note == null || note.UserId != userId)
                return HttpNotFound();
                
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            if (GetCurrentUserId() == 0)
                return RedirectToAction("Login", "Account");
                
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
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
        public ActionResult Edit(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
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
            if (userId == 0)
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
        public ActionResult Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");
                
            var note = _noteRepository.GetById(id);
            
            if (note == null || note.UserId != userId)
                return HttpNotFound();
                
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");
                
            var note = _noteRepository.GetById(id);
            
            if (note == null || note.UserId != userId)
                return HttpNotFound();
                
            _noteRepository.Delete(id);
            
            return RedirectToAction("Index");
        }

        private int GetCurrentUserId()
        {
            var cookie = Request.Cookies["auth_token"];
            if (cookie == null)
                return 0;
                
            var principal = JwtHelper.ValidateToken(cookie.Value);
            if (principal == null)
                return 0;
                
            var identity = principal.Identity as ClaimsIdentity;
            var userIdClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }
    }
}