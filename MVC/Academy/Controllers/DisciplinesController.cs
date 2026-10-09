using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy;
using Academy.Models;

public class DisciplinesController : Controller
{
    private readonly AcademyContext _context;

    public DisciplinesController(AcademyContext context)
    {
        _context = context;
    }

	// GET: DISCIPLINES
	public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
	{
		ViewData["CurrentSort"] = sortOrder;
		ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
		ViewData["LessonsSortParam"] = sortOrder == "Lessons" ? "lessons_desc" : "Lessons";

		if (searchString != null)
			pageNumber = 1;
		else
			searchString = currentFilter;

		ViewData["CurrentFilter"] = searchString;

		IQueryable<Discipline> disciplines = _context.Disciplines;

		if (!string.IsNullOrEmpty(searchString))
			disciplines = disciplines.Where(d => d.discipline_name.Contains(searchString));

		disciplines = sortOrder switch
		{
			"name_desc" => disciplines.OrderByDescending(d => d.discipline_name),
			"lessons_desc" => disciplines.OrderByDescending(d => d.number_of_lessons),
			"Lessons" => disciplines.OrderBy(d => d.number_of_lessons),
			_ => disciplines.OrderBy(d => d.discipline_name)
		};

		int pageSize = 5;
		return View(await PaginatedList<Discipline>.CreateAsync(disciplines.AsNoTracking(), pageNumber ?? 1, pageSize));
	}

	// GET: DISCIPLINES/Details/5
	public async Task<IActionResult> Details(int? discipline_id)
    {
        if (discipline_id == null)
        {
            return NotFound();
        }

        var discipline = await _context.Disciplines
            .FirstOrDefaultAsync(m => m.discipline_id == discipline_id);
        if (discipline == null)
        {
            return NotFound();
        }

        return View(discipline);
    }

    // GET: DISCIPLINES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DISCIPLINES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("discipline_id,discipline_name,number_of_lessons,TeachersRelations")] Discipline discipline)
    {
        if (ModelState.IsValid)
        {
            _context.Add(discipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(discipline);
    }

    // GET: DISCIPLINES/Edit/5
    public async Task<IActionResult> Edit(int? discipline_id)
    {
        if (discipline_id == null)
        {
            return NotFound();
        }

        var discipline = await _context.Disciplines.FindAsync(discipline_id);
        if (discipline == null)
        {
            return NotFound();
        }
        return View(discipline);
    }

    // POST: DISCIPLINES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? discipline_id, [Bind("discipline_id,discipline_name,number_of_lessons,TeachersRelations")] Discipline discipline)
    {
        if (discipline_id != discipline.discipline_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(discipline);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisciplineExists(discipline.discipline_id))
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
        return View(discipline);
    }

    // GET: DISCIPLINES/Delete/5
    public async Task<IActionResult> Delete(int? discipline_id)
    {
        if (discipline_id == null)
        {
            return NotFound();
        }

        var discipline = await _context.Disciplines
            .FirstOrDefaultAsync(m => m.discipline_id == discipline_id);
        if (discipline == null)
        {
            return NotFound();
        }

        return View(discipline);
    }

    // POST: DISCIPLINES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? discipline_id)
    {
        var discipline = await _context.Disciplines.FindAsync(discipline_id);
        if (discipline != null)
        {
            _context.Disciplines.Remove(discipline);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DisciplineExists(int? discipline_id)
    {
        return _context.Disciplines.Any(e => e.discipline_id == discipline_id);
    }
}
