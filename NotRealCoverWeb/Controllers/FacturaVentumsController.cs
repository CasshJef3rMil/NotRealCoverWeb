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
    public class FacturaVentumsController : Controller
    {
        private readonly NotRealCoverWebContext _context;

        public FacturaVentumsController(NotRealCoverWebContext context)
        {
            _context = context;
        }

        // GET: FacturaVentums
        public async Task<IActionResult> Index()
        {
              return _context.FacturaVenta != null ? 
                          View(await _context.FacturaVenta.ToListAsync()) :
                          Problem("Entity set 'NotRealCoverWebContext.FacturaVenta'  is null.");
        }

        // GET: FacturaVentums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FacturaVenta == null)
            {
                return NotFound();
            }

            var facturaVentum = await _context.FacturaVenta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaVentum == null)
            {
                return NotFound();
            }

            return View(facturaVentum);
        }

        // GET: FacturaVentums/Create
        public IActionResult Create()
        {
            var facturaVenta = new FacturaVentum();
            facturaVenta.FechaVenta = DateTime.Now;
            facturaVenta.TotalVenta = 0;
            facturaVenta.DetFacturaVenta = new List<DetFacturaVentum>();
            facturaVenta.DetFacturaVenta.Add(new DetFacturaVentum
            {
                Cantidad = 1
            });
            ViewBag.Accion = "Create";
            return View(facturaVenta);
        }

        // POST: FacturaVentums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta")] FacturaVentum facturaVentum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facturaVentum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facturaVentum);
        }

        //Metodo agregardetalles
        [HttpPost]
        public ActionResult AgregarDetalles([Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta,DetFacturaVenta")] FacturaVentum facturaVenta, string accion)
        {
            facturaVenta.DetFacturaVenta.Add(new DetFacturaVentum { Cantidad = 1 });
            ViewBag.Accion = accion;
            return View(accion, facturaVenta);
        }

        //Metodo EliminarDetalles
        public ActionResult EliminarDetalles([Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta,DetFacturaVenta")] FacturaVentum facturaVenta,
           int index, string accion)
        {
            var det = facturaVenta.DetFacturaVenta[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                facturaVenta.DetFacturaVenta.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, facturaVenta);
        }


        // GET: FacturaVentums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FacturaVenta == null)
            {
                return NotFound();
            }

            var facturaVentum = await _context.FacturaVenta.FindAsync(id);
            if (facturaVentum == null)
            {
                return NotFound();
            }
            return View(facturaVentum);
        }

        // POST: FacturaVentums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta")] FacturaVentum facturaVentum)
        {
            if (id != facturaVentum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facturaVentum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaVentumExists(facturaVentum.Id))
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
            return View(facturaVentum);
        }

        // GET: FacturaVentums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FacturaVenta == null)
            {
                return NotFound();
            }

            var facturaVentum = await _context.FacturaVenta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaVentum == null)
            {
                return NotFound();
            }

            return View(facturaVentum);
        }

        // POST: FacturaVentums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FacturaVenta == null)
            {
                return Problem("Entity set 'NotRealCoverWebContext.FacturaVenta'  is null.");
            }
            var facturaVentum = await _context.FacturaVenta.FindAsync(id);
            if (facturaVentum != null)
            {
                _context.FacturaVenta.Remove(facturaVentum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturaVentumExists(int id)
        {
          return (_context.FacturaVenta?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
