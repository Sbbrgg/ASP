using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy;
using Academy.Models;

public class GroupsController : Controller
{
    private readonly AcademyContext _context;

    public GroupsController(AcademyContext context)
    {
        _context = context;
    }

	// GET: GROUPS
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

		IQueryable<Group> groups = _context.Groups.Include(g=>g.Direction);

		if (!string.IsNullOrEmpty(searchString))
			groups = groups.Where(g => g.group_name.Contains(searchString));

		groups = sortOrder switch
		{
			"name_desc" => groups.OrderByDescending(g => g.group_name),
			"date_desc" => groups.OrderByDescending(g => g.start_date),
			"Date" => groups.OrderBy(g => g.start_date),
			_ => groups.OrderBy(g => g.group_name)
		};

		int pageSize = 5;
		return View(await PaginatedList<Group>.CreateAsync(groups.AsNoTracking(), pageNumber ?? 1, pageSize));
	}

	// GET: GROUPS/Details/5
	public async Task<IActionResult> Details(int? group_id)
    {
        if (group_id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups
            .FirstOrDefaultAsync(m => m.group_id == group_id);
        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    // GET: GROUPS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: GROUPS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("group_id,group_name,direction,learning_days,start_time,start_date,Direction,Students")] Group group)
    {
        if (ModelState.IsValid)
        {
            _context.Add(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(group);
    }

    // GET: GROUPS/Edit/5
    public async Task<IActionResult> Edit(int? group_id)
    {
        if (group_id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FindAsync(group_id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    // POST: GROUPS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? group_id, [Bind("group_id,group_name,direction,learning_days,start_time,start_date,Direction,Students")] Group group)
    {
        if (group_id != group.group_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(group);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(group.group_id))
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
        return View(group);
    }

    // GET: GROUPS/Delete/5
    public async Task<IActionResult> Delete(int? group_id)
    {
        if (group_id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups
            .FirstOrDefaultAsync(m => m.group_id == group_id);
        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    // POST: GROUPS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? group_id)
    {
        var group = await _context.Groups.FindAsync(group_id);
        if (group != null)
        {
            _context.Groups.Remove(group);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GroupExists(int? group_id)
    {
        return _context.Groups.Any(e => e.group_id == group_id);
    }
}
