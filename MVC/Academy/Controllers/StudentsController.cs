using Academy;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class StudentsController : Controller
{
    private readonly AcademyContext _context;

    public StudentsController(AcademyContext context)
    {
        _context = context;
    }

	// GET: STUDENTS
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

		IQueryable<Student> students = _context.Students;

		if (!string.IsNullOrEmpty(searchString))
		{
			students = students.Where(s =>
				s.last_name.Contains(searchString) ||
				s.first_name.Contains(searchString));
		}

		students = sortOrder switch
		{
			"name_desc" => students.OrderByDescending(s => s.last_name),
			"date_desc" => students.OrderByDescending(s => s.birth_date),
			"Date" => students.OrderBy(s => s.birth_date),
			_ => students.OrderBy(s => s.last_name)
		};

		int pageSize = 5;
		return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
	}

	// GET: STUDENTS/Details/5
	public async Task<IActionResult> Details(int? stud_id)
    {
        if (stud_id == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.stud_id == stud_id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // GET: STUDENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: STUDENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("stud_id,group,Group,last_name,first_name,middle_name,birth_date,email,phone,photo,FullName")] Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    // GET: STUDENTS/Edit/5
    public async Task<IActionResult> Edit(int? stud_id)
    {
        if (stud_id == null)
        {
            return NotFound();
        }

        var student = await _context.Students.FindAsync(stud_id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    // POST: STUDENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? stud_id, [Bind("stud_id,group,Group,last_name,first_name,middle_name,birth_date,email,phone,photo,FullName")] Student student)
    {
        if (stud_id != student.stud_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.stud_id))
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
        return View(student);
    }

    // GET: STUDENTS/Delete/5
    public async Task<IActionResult> Delete(int? stud_id)
    {
        if (stud_id == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.stud_id == stud_id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // POST: STUDENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? stud_id)
    {
        var student = await _context.Students.FindAsync(stud_id);
        if (student != null)
        {
            _context.Students.Remove(student);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(int? stud_id)
    {
        return _context.Students.Any(e => e.stud_id == stud_id);
    }
}
