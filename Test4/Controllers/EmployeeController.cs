using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Test4.Models;


namespace Test4.Controllers
{
	public class EmployeeController : Controller
	{
		private Default db = new Default();

		// GET: Employee
		public async Task<ActionResult> Index(string sortOrder, string current_filter, string search, int? page)
		{
			// Setting initial conditions 
			ViewBag.Curr_sort = sortOrder;
			ViewBag.SortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DataSortParam = sortOrder == "dept" ? "dept_desc" : "dept";

			// searching according to page
			if (search != null)
			{
				page = 1;
			}
			else
			{
				search = current_filter;
			}

			ViewBag.Curr_filter = search;

			// fetching all data
			IQueryable<Employee> employees = from s in db.Employees
											 select s;

			// searching in string
			if (!string.IsNullOrEmpty(search))
			{
				employees = employees.Where(s => s.Name.Contains(search));
			}

			// sorting logic
			switch (sortOrder)
			{
				case "name_desc":
					employees = employees.OrderByDescending(x => x.Id);
					break;
				case "dept_desc":
					employees = employees.OrderByDescending(x => x.DepId);
					break;
				case "dept":
					employees = employees.OrderBy(x => x.DepId);
					break;
				default:
					employees = employees.OrderBy(x => x.Id);
					break;
			}

			// setting page size
			int pageSize = 100;
			int pageNumber = page ?? 1;

			// return the page!
			return View(employees.ToPagedList(pageNumber, pageSize));
		}

		// GET: Employee/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Employee employee = db.Employees.Find(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			return View(employee);
		}

		// GET: Employee/Create
		public ActionResult Create()
		{
			ViewBag.Department_Id = new SelectList(db.Departments, "Id", "DepartmentName");
			return View(new Employee());
		}

		// POST: Employee/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Name,email,UserName,password,DepId,Department_Id")] Employee employee)
		{
			if (ModelState.IsValid)
			{
				db.Employees.Add(employee);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.Department_Id = new SelectList(db.Departments, "Id", "DepartmentName", employee.Department_Id);
			return View(employee);
		}

		// GET: Employee/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Employee employee = db.Employees.Find(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			ViewBag.Department_Id = new SelectList(db.Departments, "Id", "DepartmentName", employee.Department_Id);
			return View(employee);
		}

		// POST: Employee/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Name,email,UserName,password,DepId,Department_Id")] Employee employee)
		{
			if (ModelState.IsValid)
			{
				db.Entry(employee).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.Department_Id = new SelectList(db.Departments, "Id", "DepartmentName", employee.Department_Id);
			return View(employee);
		}

		// GET: Employee/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Employee employee = db.Employees.Find(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			return View(employee);
		}

		// POST: Employee/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Employee employee = db.Employees.Find(id);
			db.Employees.Remove(employee);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}


		public void AddEmployees()
		{

			for (int i = 100; i < 1000; i++)
			{
				db.Employees.Add(new Employee()
				{
					Name = $"Employee_{i + 1}",
					email = $"emp{i + 1}@hotmail.com",
					UserName = $"emp_{i + 1}",
					password = $"emp{i + 1}",
					DepId = ((i + 1) % 10) + 6,
					Department_Id = ((i + 1) % 10) + 6,
				});
			}

			db.SaveChanges();
		}


		public JsonResult GiveMeData(int pageNumber, int pageSize)
		{
			db.Configuration.ProxyCreationEnabled = false;
			var data = db.Employees.OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize);
			var test = data.ToList().Count;
			if (test == 0)
			{
				Console.WriteLine(test);
				return Json(new { }, JsonRequestBehavior.AllowGet);
			}
			return Json(data, JsonRequestBehavior.AllowGet);
		}


	}
}
