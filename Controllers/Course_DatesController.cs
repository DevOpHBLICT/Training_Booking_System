using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cascadingdropdownlist.DAL;
using Cascadingdropdownlist.Models;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace Cascadingdropdownlist.Controllers
{




    public class Course_DatesController : Controller
    {
        
        private SchoolContext db = new SchoolContext();

            public ActionResult CourseTimeError()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

            public ActionResult SomeValuesNotFilledIn()
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }



            // GET: Course
            public ActionResult Live (string sortOrder, string searchString, string LocationString)
            {
                // SQL version of the above LINQ code.
                ViewBag.CurrentSort = sortOrder;

                ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                string query = "select * from Course_Dates where DF >=getdate()";

                var ss = "";
                if (!String.IsNullOrEmpty(searchString))
                {
                    ss = " and Course_Title like '%" + searchString + "%'";

                }

                if (ss == "")
                {
                    if (!String.IsNullOrEmpty(LocationString))
                    {
                        query = query + " and Dept like '%" + LocationString + "%'";

                    }
                }
                if (ss != "")
                {
                    query = query + ss;
                    if (!String.IsNullOrEmpty(LocationString))
                    {
                        query = query + " and Dept like '%" + LocationString + "%'";

                    }


                }




                switch (sortOrder)
                {
                    case "name_desc":
                        query = query + " order by Course_Title desc";
                        break;
                    case "Date":
                        query = query + " order by DF";
                        break;
                    case "date_desc":
                        query = query + " order by DF desc";
                        break;
                    default:
                        query = query + " order by Course_Title";
                        break;
                }

                IEnumerable<Course_Dates> courses = db.Database.SqlQuery<Course_Dates>(query);

                //    IQueryable<Course_Dates> courses = db.Course_Dates;

                var sql = courses.ToString();
                return View(courses.ToList());

            }



        // GET: Course
            public ActionResult Index(string sortOrder, string searchString,string LocationString)
        {
            // SQL version of the above LINQ code.
            ViewBag.CurrentSort = sortOrder;

            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            string query = "select * from Course_Dates";

            var ss="";
            if (!String.IsNullOrEmpty(searchString))
            {
               ss = " where Course_Title like '%" + searchString + "%'"; 
                                     
            }

            if (ss == "")
            {
                if (!String.IsNullOrEmpty(LocationString))
                {
                    query = query + " where Dept like '%" + LocationString + "%'";

                }
            }
           if (ss!="")
           {
               query = query + ss; 
               if (!String.IsNullOrEmpty(LocationString))
               {
                   query =query + " and Dept like '%" + LocationString + "%'";

               }


           }




            switch (sortOrder)
            {
                case "name_desc":
                    query = query + " order by Course_Title desc";
                    break;
                case "Date":
                    query = query + " order by DF";
                    break;
                case "date_desc":
                    query = query + " order by DF desc";
                    break;
                default:
                    query = query + " order by Course_Title";
                    break;
            }

            IEnumerable<Course_Dates> courses = db.Database.SqlQuery<Course_Dates>(query);

        //    IQueryable<Course_Dates> courses = db.Course_Dates;
                
          var sql = courses.ToString();
       return View(courses.ToList());
            
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        public ActionResult Browse()
        {

            IQueryable<Course_Dates> courses = db.Course_Dates
                   .OrderBy(d => d.CourseID)
                   .Include(d => d.Department);
            var sql = courses.ToString();
            return View(courses.ToList());

        }
        public ActionResult Archive(int? id)
        {
            var prod = db.Course_Dates.Find(id);
   
            db.Entry(prod).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            
        }
       
        public ActionResult Create()
        {
            List<SelectListItem> hours= new List<SelectListItem>();

              hours.Add(new SelectListItem() {Text="08", Value="8"});
        hours.Add(new SelectListItem() { Text="09", Value="9"});
        hours.Add(new SelectListItem() { Text="10", Value="10"});
        hours.Add(new SelectListItem() { Text="11", Value="11"});
        hours.Add(new SelectListItem() { Text="12", Value="12"});
        hours.Add(new SelectListItem() { Text="13", Value="13"});
        hours.Add(new SelectListItem() { Text="14", Value="14"});
       hours.Add( new SelectListItem() { Text="15", Value="15"});
        hours.Add(new SelectListItem() { Text="16", Value="16"});
        hours.Add(new SelectListItem() { Text="17", Value="17"});
        hours.Add(new SelectListItem() { Text = "18", Value = "18" });

        ViewBag.Hours =  new SelectList(hours, "Value", "Text");
 


            List<SelectListItem> minutes = new List<SelectListItem>();

             minutes.Add(new SelectListItem() {Text="00", Value="0"});
        minutes.Add(new SelectListItem() { Text="15", Value="15"});
        minutes.Add(new SelectListItem() { Text="30", Value="30"});
        minutes.Add(new SelectListItem() { Text="45", Value="45"});

        ViewBag.Minutes = new SelectList(minutes, "Value", "Text");
        



            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            var departmentsQuery = from d in db.Departments
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name");


            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "Fullname");

        


            //    PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,DF,Capacity,Archived,Start_Time,Course_Title,DepartmentID,Dept,FromHours,FromMinutes,ToHours,ToMinutes,ID")]Course_Dates course, int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool contactExists = db.Course_Dates.Any(o=> o.Course_DatesID.Equals(course.Course_DatesID));
                    course.Dept = (from p in db.Departments where p.DepartmentID == course.DepartmentID select p.Name).First();
                    course.Course_Title = (from p in db.Courses where p.CourseID == course.CourseID select p.Title).First();
              
                   
                    PopulateDepartmentsDropDownList(course.DepartmentID);


                    var FromTime = course.FromHours * 60 + course.FromMinutes;
                    var ToTime = course.ToHours * 60 + course.ToMinutes;

                    if (FromTime >= ToTime)
                    {

                        List<SelectListItem> hours = new List<SelectListItem>();

                        hours.Add(new SelectListItem() { Text = "08", Value = "8" });
                        hours.Add(new SelectListItem() { Text = "09", Value = "9" });
                        hours.Add(new SelectListItem() { Text = "10", Value = "10" });
                        hours.Add(new SelectListItem() { Text = "11", Value = "11" });
                        hours.Add(new SelectListItem() { Text = "12", Value = "12" });
                        hours.Add(new SelectListItem() { Text = "13", Value = "13" });
                        hours.Add(new SelectListItem() { Text = "14", Value = "14" });
                        hours.Add(new SelectListItem() { Text = "15", Value = "15" });
                        hours.Add(new SelectListItem() { Text = "16", Value = "16" });
                        hours.Add(new SelectListItem() { Text = "17", Value = "17" });
                        hours.Add(new SelectListItem() { Text = "18", Value = "18" });

                        ViewBag.Hours = new SelectList(hours, "Value", "Text");



                        List<SelectListItem> minutes = new List<SelectListItem>();

                        minutes.Add(new SelectListItem() { Text = "00", Value = "0" });
                        minutes.Add(new SelectListItem() { Text = "15", Value = "15" });
                        minutes.Add(new SelectListItem() { Text = "30", Value = "30" });
                        minutes.Add(new SelectListItem() { Text = "45", Value = "45" });

                        ViewBag.Minutes = new SelectList(minutes, "Value", "Text");




                    ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "Fullname");



                        ModelState.AddModelError("CustomError", "Start time cannot be greater than end time.");
                        var departmentsQuery = from d in db.Departments
                                               orderby d.Name
                                               select d;
                        ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name");

                        ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
                        return View("CourseTimeError");
                    }
                    else
                    {
                        course.Duration = ToTime - FromTime;
                         db.Course_Dates.Add(course);
                        db.SaveChanges();



                        var Course_DatesID = (from u in db.Course_Dates select u.Course_DatesID).Max();
                               
                     
                          SqlConnection conn = new SqlConnection("Data Source=HBL-Port01;Initial Catalog=Training_Courses5;Integrated Security=SSPI;");

                           string sql = "insert into CourseInstructor(CourseID,InstructorID) values (" +Course_DatesID + "," + id+ ")";

                           try
                           {
                               conn.Open();
                               SqlCommand cmd = new SqlCommand(sql, conn);
                               cmd.ExecuteNonQuery();
                           }
                           catch (System.Data.SqlClient.SqlException ex)
                           {
                               string msg = "Insert Error:";
                               msg += ex.Message;
                           }
                           finally
                           {
                               conn.Close();
                           }


            

                        return RedirectToAction("Index");
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            var departmentsQuery2 = from d in db.Departments
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery2, "DepartmentID", "Name");

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            return View("SomeValuesNotFilledIn");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_Dates course = db.Course_Dates.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);

            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseToUpdate = db.Course_Dates.Find(id);
            if (TryUpdateModel(courseToUpdate, "",
               new string[] { "Description","DF","Title", "Capacity", "DepartmentID" ,"Archived","Course_Title"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);

            //   PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            return View(courseToUpdate);
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.Departments
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
           // ViewBag.Department.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);

        }


        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_Dates course = db.Course_Dates.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course_Dates course = db.Course_Dates.Find(id);
            db.Course_Dates.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCourseCredits(int? multiplier)
        {
            if (multiplier != null)
            {
                ViewBag.RowsAffected = db.Database.ExecuteSqlCommand("UPDATE Course SET Capacity = Capacity * {0}", multiplier);
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
