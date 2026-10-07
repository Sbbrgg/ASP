using Academy;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TeachersController : Controller
{
    private readonly AcademyContext _context;

    public TeachersController(AcademyContext context)
    {
        _context = context;
    }

	// GET: TEACHERS
	public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
	{
		ViewData["CurrentSort"] = sortOrder;
		ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
		ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";

		if (searchString != null)
			pageNumber = 1;
		else
			searchString = currentFilter;

		ViewData["CurrentFilter"] = searchString;

		IQueryable<Teacher> teachers = _context.Teachers;

		if (!string.IsNullOrEmpty(searchString))
		{
			teachers = teachers.Where(t =>
				t.last_name.Contains(searchString) ||
				t.first_name.Contains(searchString));
		}

		teachers = sortOrder switch
		{
			"name_desc" => teachers.OrderByDescending(t => t.last_name),
			"date_desc" => teachers.OrderByDescending(t => t.work_since),
			"Date" => teachers.OrderBy(t => t.work_since),
			_ => teachers.OrderBy(t => t.last_name)
		};

		int pageSize = 5;
		return View(await PaginatedList<Teacher>.CreateAsync(teachers.AsNoTracking(), pageNumber ?? 1, pageSize));
	}

	// GET: TEACHERS/Details/5
	public async Task<IActionResult> Details(int? teacher_id)
    {
        if (teacher_id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.teacher_id == teacher_id);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    // GET: TEACHERS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TEACHERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("teacher_id,work_since,rate,DisciplinesRelations,last_name,first_name,middle_name,birth_date,email,phone,photo,FullName")] Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
    }

    // GET: TEACHERS/Edit/5
    public async Task<IActionResult> Edit(int? teacher_id)
    {
        if (teacher_id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers.FindAsync(teacher_id);
        if (teacher == null)
        {
            return NotFound();
        }
        return View(teacher);
    }

    // POST: TEACHERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? teacher_id, [Bind("teacher_id,work_since,rate,DisciplinesRelations,last_name,first_name,middle_name,birth_date,email,phone,photo,FullName")] Teacher teacher)
    {
        if (teacher_id != teacher.teacher_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacher.teacher_id))
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
        return View(teacher);
    }

    // GET: TEACHERS/Delete/5
    public async Task<IActionResult> Delete(int? teacher_id)
    {
        if (teacher_id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.teacher_id == teacher_id);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    // POST: TEACHERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? teacher_id)
    {
        var teacher = await _context.Teachers.FindAsync(teacher_id);
        if (teacher != null)
        {
            _context.Teachers.Remove(teacher);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(int? teacher_id)
    {
        return _context.Teachers.Any(e => e.teacher_id == teacher_id);
    }
}
