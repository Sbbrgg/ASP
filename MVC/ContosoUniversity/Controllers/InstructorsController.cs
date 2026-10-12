using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
	public class InstructorsController : Controller
	{
		private readonly ContosoUniversityContext _context;

		public InstructorsController(ContosoUniversityContext context)
		{
			_context = context;
		}

		// GET: Instructors
		public async Task<IActionResult> Index(int? id, int? courseID, int? pageNumber)
		{
			int pageSize = 2;
			IQueryable<Instructor> instructorsQuery = _context.Instructors
				.Include(i => i.OfficeAssignment)
				.Include(i => i.CourseAssignments)
					.ThenInclude(ca => ca.Course)
						.ThenInclude(c => c.Enrollments)
							.ThenInclude(e => e.Student)
				.Include(i => i.CourseAssignments)
					.ThenInclude(ca => ca.Course)
						.ThenInclude(c => c.Department)
				.AsNoTracking()
				.OrderBy(i => i.LastName);

			//ContosoUniversity.Models.ViewModels.InstructorIndexData viewModel = new Models.ViewModels.InstructorIndexData();
			//viewModel.Instructors = await _context.Instructors
			ContosoUniversity.Models.ViewModels.InstructorIndexData viewModel = new Models.ViewModels.InstructorIndexData();
			viewModel = new ContosoUniversity.Models.ViewModels.InstructorIndexData
			{
				Instructors = await PaginatedList<Instructor>.CreateAsync
				(
					instructorsQuery,
					pageNumber ?? 1,
					pageSize
				)
			};

			if (id != null)
			{
				ViewData["InstructorID"] = id.Value;

				Instructor instructor = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
				if(instructor==null)
				{
					instructor = await _context.Instructors
						.Include(i => i.CourseAssignments)
							.ThenInclude(ca => ca.Course)
								.ThenInclude(c => c.Enrollments)
									.ThenInclude(e => e.Student)
						.Include(i => i.CourseAssignments)
							.ThenInclude(ca => ca.Course)
								.ThenInclude(c => c.Department)
						.AsNoTracking()
						.FirstOrDefaultAsync(i => i.ID == id.Value);
				}

				if (instructor!=null)
				{
					viewModel.Courses = instructor.CourseAssignments.Select(ca => ca.Course); 
				}
			}

			if (courseID != null && viewModel.Courses != null)
			{
				ViewData["CourseID"] = courseID.Value;
				viewModel.Enrollments = viewModel.Courses.Where(c => c.CourseID == courseID)
					.Single().Enrollments;
			}

			ViewData["CurrentPage"] = pageNumber ?? 1;

			//return View(await _context.Instructors.ToListAsync());
			return View(viewModel);
		}

		// GET: Instructors/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var instructor = await _context.Instructors
				.FirstOrDefaultAsync(m => m.ID == id);
			if (instructor == null)
			{
				return NotFound();
			}

			return View(instructor);
		}

		// GET: Instructors/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Instructors/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,HireDate")] Instructor instructor)
		{
			if (ModelState.IsValid)
			{
				_context.Add(instructor);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(instructor);
		}

		// GET: Instructors/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var instructor = await _context.Instructors.FindAsync(id);
			if (instructor == null)
			{
				return NotFound();
			}
			return View(instructor);
		}

		// POST: Instructors/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,HireDate")] Instructor instructor)
		{
			if (id != instructor.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(instructor);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!InstructorExists(instructor.ID))
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
			return View(instructor);
		}

		// GET: Instructors/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var instructor = await _context.Instructors
				.FirstOrDefaultAsync(m => m.ID == id);
			if (instructor == null)
			{
				return NotFound();
			}

			return View(instructor);
		}

		// POST: Instructors/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var instructor = await _context.Instructors.FindAsync(id);
			if (instructor != null)
			{
				_context.Instructors.Remove(instructor);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool InstructorExists(int id)
		{
			return _context.Instructors.Any(e => e.ID == id);
		}
	}
}