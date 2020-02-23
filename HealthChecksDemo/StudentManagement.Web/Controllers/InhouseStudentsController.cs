using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Web.Data;
using StudentManagement.Web.Models;

namespace StudentManagement.Web.Controllers
{
    public class InhouseStudentsController : Controller
    {
        private readonly InhouseContext _context;

        public InhouseStudentsController(InhouseContext context)
        {
            _context = context;
        }

        // GET: InhouseStudents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: InhouseStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inhouseStudent = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inhouseStudent == null)
            {
                return NotFound();
            }

            return View(inhouseStudent);
        }

        // GET: InhouseStudents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InhouseStudents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsInhouse")] InhouseStudent inhouseStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inhouseStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inhouseStudent);
        }

        // GET: InhouseStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inhouseStudent = await _context.Students.FindAsync(id);
            if (inhouseStudent == null)
            {
                return NotFound();
            }
            return View(inhouseStudent);
        }

        // POST: InhouseStudents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsInhouse")] InhouseStudent inhouseStudent)
        {
            if (id != inhouseStudent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inhouseStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InhouseStudentExists(inhouseStudent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inhouseStudent);
        }

        // GET: InhouseStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inhouseStudent = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inhouseStudent == null)
            {
                return NotFound();
            }

            return View(inhouseStudent);
        }

        // POST: InhouseStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inhouseStudent = await _context.Students.FindAsync(id);
            _context.Students.Remove(inhouseStudent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InhouseStudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
