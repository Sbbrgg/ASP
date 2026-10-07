using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

namespace Academy.Controllers
{
	public class DirectionsController : Controller
	{
		private readonly AcademyContext _context;

		public DirectionsController(AcademyContext context)
		{
			_context = context;
		}

		// GET: Directions
		public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
		{
			ViewData["CurrentSort"] = sortOrder;
			ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

			if (searchString != null)
				pageNumber = 1;
			else
				searchString = currentFilter;

			ViewData["CurrentFilter"] = searchString;

			IQueryable<Direction> directions = _context.Directions;

			if (!string.IsNullOrEmpty(searchString))
				directions = directions.Where(d => d.direction_name.Contains(searchString));

			directions = sortOrder switch
			{
				"name_desc" => directions.OrderByDescending(d => d.direction_name),
				_ => directions.OrderBy(d => d.direction_name)
			};

			int pageSize = 5;
			return View(await PaginatedList<Direction>.CreateAsync(directions.AsNoTracking(), pageNumber ?? 1, pageSize));
		}

		// GET: Directions/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var direction = await _context.Directions
				.FirstOrDefaultAsync(m => m.direction_id == id);
			if (direction == null)
			{
				return NotFound();
			}

			return View(direction);
		}

		// GET: Directions/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Directions/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("direction_id,direction_name")] Direction direction)
		{
			if (ModelState.IsValid)
			{
				_context.Add(direction);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(direction);
		}

		// GET: Directions/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var direction = await _context.Directions.FindAsync(id);
			if (direction == null)
			{
				return NotFound();
			}
			return View(direction);
		}

		// POST: Directions/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("direction_id,direction_name")] Direction direction)
		{
			if (id != direction.direction_id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(direction);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!DirectionExists(direction.direction_id))
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
			return View(direction);
		}

		// GET: Directions/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var direction = await _context.Directions
				.FirstOrDefaultAsync(m => m.direction_id == id);
			if (direction == null)
			{
				return NotFound();
			}

			return View(direction);
		}

		// POST: Directions/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var direction = await _context.Directions.FindAsync(id);
			if (direction != null)
			{
				_context.Directions.Remove(direction);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool DirectionExists(int id)
		{
			return _context.Directions.Any(e => e.direction_id == id);
		}
	}
}