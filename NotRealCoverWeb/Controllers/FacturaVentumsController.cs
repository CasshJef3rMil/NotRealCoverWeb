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

            var facturaVenta = await _context.FacturaVenta
                .Include(s => s.DetFacturaVenta)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (facturaVenta == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(facturaVenta);
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
        public async Task<IActionResult> Create([Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta,DetFacturaVenta")] FacturaVentum facturaVentum)
        {
            //Esta linea sirve para que calcule la cantidad al multiplicar PrecioUnitario * Cantidad
            facturaVentum.TotalVenta = facturaVentum.DetFacturaVenta.Sum(s => s.Cantidad * s.PrecioUnitario);

            _context.Add(facturaVentum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

           
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
           
            var facturaVentum = await _context.FacturaVenta
           .Include(s => s.DetFacturaVenta)
           .FirstAsync(s => s.Id == id);

            if (facturaVentum == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(facturaVentum);

        }

        // POST: FacturaVentums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta,DetFacturaVenta")] FacturaVentum facturaVentum)
         {
            try
            {
                // Obtener los datos de la base de datos que van a ser modificados
                var facturaUpdate = await _context.FacturaVenta
                        .Include(s => s.DetFacturaVenta)
                        .FirstAsync(s => s.Id == facturaVentum.Id);
                facturaUpdate.Correlativo = facturaVentum.Correlativo;
                facturaUpdate.TotalVenta = facturaVentum.DetFacturaVenta.Where(s => s.Id > -1).Sum(s => s.PrecioUnitario * s.Cantidad);
                facturaUpdate.Cliente = facturaVentum.Cliente;
                facturaUpdate.FechaVenta = facturaVentum.FechaVenta;
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = facturaVentum.DetFacturaVenta.Where(s => s.Id == 0);
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                foreach (var d in detNew)
                {
                    facturaUpdate.DetFacturaVenta.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = facturaVentum.DetFacturaVenta.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = facturaUpdate.DetFacturaVenta.FirstOrDefault(s => s.Id == d.Id);
                    det.Album = d.Album;
                    det.Descripcion = d.Descripcion;
                    det.Cantidad = d.Cantidad;
                    det.PrecioUnitario = d.PrecioUnitario;

                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDet = facturaVentum.DetFacturaVenta.Where(s => s.Id < 0).ToList();
                if (delDet != null && delDet.Count > 0)
                {
                    foreach (var d in delDet)
                    {
                        d.Id = d.Id * -1;
                        var det = facturaUpdate.DetFacturaVenta.FirstOrDefault(s => s.Id == d.Id);
                        _context.Remove(det);
                        // facturaUpdate.DetFacturaVenta.Remove(det);
                    }
                }
                // Aplicar esos cambios a la base de datos
                _context.Update(facturaUpdate);
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
        // GET: FacturaVentums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FacturaVenta == null)
            {
                return NotFound();
            }
             
            var facturaVentum = await _context.FacturaVenta       
                .Include(s => s.DetFacturaVenta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaVentum == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";
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

            var facturaVentum = await _context.FacturaVenta
                .FindAsync(id);

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
