using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotRealCoverWeb.Models;

namespace NotRealCoverWeb.Controllers
{
    public class DetFacturaVentumsController : Controller
    {
        private readonly NotRealCoverWebContext _context;

        public DetFacturaVentumsController(NotRealCoverWebContext context)
        {
            _context = context;
        }

        // GET: DetFacturaVentums
        public async Task<IActionResult> Index()
        {
            var notRealCoverWebContext = _context.DetFacturaVenta.Include(d => d.IdFacturaVentaNavigation);
            return View(await notRealCoverWebContext.ToListAsync());
        }

        // GET: DetFacturaVentums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetFacturaVenta == null)
            {
                return NotFound();
            }

            var detFacturaVentum = await _context.DetFacturaVenta
                .Include(d => d.IdFacturaVentaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detFacturaVentum == null)
            {
                return NotFound();
            }

            return View(detFacturaVentum);
        }

        // GET: DetFacturaVentums/Create
        public IActionResult Create()
        {
            ViewData["IdFacturaVenta"] = new SelectList(_context.FacturaVenta, "Id", "Id");
            return View();
        }

        // POST: DetFacturaVentums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdFacturaVenta,Album,Descripcion,Cantidad,PrecioUnitario")] DetFacturaVentum detFacturaVentum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detFacturaVentum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFacturaVenta"] = new SelectList(_context.FacturaVenta, "Id", "Id", detFacturaVentum.IdFacturaVenta);
            return View(detFacturaVentum);
        }

        // GET: DetFacturaVentums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetFacturaVenta == null)
            {
                return NotFound();
            }

            var detFacturaVentum = await _context.DetFacturaVenta.FindAsync(id);
            if (detFacturaVentum == null)
            {
                return NotFound();
            }
            ViewData["IdFacturaVenta"] = new SelectList(_context.FacturaVenta, "Id", "Id", detFacturaVentum.IdFacturaVenta);
            return View(detFacturaVentum);
        }

        // POST: DetFacturaVentums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdFacturaVenta,Album,Descripcion,Cantidad,PrecioUnitario")] DetFacturaVentum detFacturaVentum)
        {
            if (id != detFacturaVentum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detFacturaVentum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetFacturaVentumExists(detFacturaVentum.Id))
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
            ViewData["IdFacturaVenta"] = new SelectList(_context.FacturaVenta, "Id", "Id", detFacturaVentum.IdFacturaVenta);
            return View(detFacturaVentum);
        }

        // GET: DetFacturaVentums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetFacturaVenta == null)
            {
                return NotFound();
            }

            var detFacturaVentum = await _context.DetFacturaVenta
                .Include(d => d.IdFacturaVentaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detFacturaVentum == null)
            {
                return NotFound();
            }

            return View(detFacturaVentum);
        }

        // POST: DetFacturaVentums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetFacturaVenta == null)
            {
                return Problem("Entity set 'NotRealCoverWebContext.DetFacturaVenta'  is null.");
            }
            var detFacturaVentum = await _context.DetFacturaVenta.FindAsync(id);
            if (detFacturaVentum != null)
            {
                _context.DetFacturaVenta.Remove(detFacturaVentum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetFacturaVentumExists(int id)
        {
          return (_context.DetFacturaVenta?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
