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
using Cascadingdropdownlist.ViewModels;
using System.Data.Entity.Infrastructure;

namespace Cascadingdropdownlist.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();



        public ActionResult Students(int? id)
        {
            //      var l = db.CourseInstructor.Where(x => x.InstructorID == id.Value);
            //   var student = db.Students.Where(x => x.CourseID);
            ViewBag.CourseID = id;

            ViewBag.CourseTitle = (from p in db.Course_Dates where p.Course_DatesID == id select p.Course_Title).FirstOrDefault();
            ViewBag.CourseDF = (from p in db.Course_Dates where p.Course_DatesID == id select p.DF).FirstOrDefault();
            ViewBag.Location = (from p in db.Students where p.CourseID == id select p.Venue).FirstOrDefault();
            ViewBag.Capacity = (from p in db.Course_Dates where p.Course_DatesID == id select p.Capacity).FirstOrDefault();
            ViewBag.Venue = (from p in db.Students where p.CourseID == id select p.Venue).FirstOrDefault();




             return View();
        }

        // GET: Instructor
        public ActionResult Future(int? id, int? course_DatesID)
        {
            var viewModel = new InstructorIndexData();

            viewModel.Instructors = db.Instructors
                 .Include(i => i.Course_Dates.Select(c => c.Department))
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Course_Dates = viewModel.Instructors.Where(
                    i => i.ID == id.Value).Single().Course_Dates;



                foreach (Course_Dates c in viewModel.Course_Dates)
                {
                    var k = db.Students.Where(i => i.CourseID == c.Course_DatesID);


                }
            }

            if (course_DatesID != null)
            {
                ViewBag.Course_DatesID = course_DatesID.Value;
                // Lazy loading
                //viewModel.Enrollments = viewModel.Courses.Where(
                //    x => x.CourseID == courseID).Single().Enrollments;
                // Explicit loading
                //    var selectedCourse = viewModel.Course_Dates.Where(x => x.Course_DatesID == course_DatesID && x.DF > DateTime.Now).Single();

                var selectedCourse = viewModel.Course_Dates.Where(x => x.Course_DatesID == course_DatesID).Single();


            }

            return View(viewModel);
        }



        // GET: Instructor
        public ActionResult Index(int? id, int? course_DatesID)
        {
            var viewModel = new InstructorIndexData();

            viewModel.Instructors = db.Instructors
                 .Include(i => i.Course_Dates.Select(c => c.Department))
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Course_Dates = viewModel.Instructors.Where(
                    i => i.ID == id.Value).Single().Course_Dates;



                foreach (Course_Dates c in viewModel.Course_Dates)
                {
                    var k = db.Students.Where(i => i.CourseID == c.Course_DatesID);


                }
            }

            if (course_DatesID != null)
            {
                ViewBag.Course_DatesID = course_DatesID.Value;
                // Lazy loading
                //viewModel.Enrollments = viewModel.Courses.Where(
                //    x => x.CourseID == courseID).Single().Enrollments;
                // Explicit loading
                //    var selectedCourse = viewModel.Course_Dates.Where(x => x.Course_DatesID == course_DatesID && x.DF > DateTime.Now).Single();

                var selectedCourse = viewModel.Course_Dates.Where(x => x.Course_DatesID == course_DatesID).Single();


            }

            return View(viewModel);
        }


        // GET: Instructor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        public ActionResult Create()
        {
            var instructor = new Instructor();
            instructor.Course_Dates = new List<Course_Dates>();
            PopulateAssignedCourseData2(instructor);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,OfficeAssignment")]Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Course_Dates = new List<Course_Dates>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Course_Dates.Find(int.Parse(course));
                    instructor.Course_Dates.Add(courseToAdd);
                }
            }

            try
            {
                if (ModelState.IsValid)
                {


                    db.Instructors.Add(instructor);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

                PopulateAssignedCourseData(instructor);
                return View(instructor);
            }
            catch (Exception ex)
            {
                return View("Course_Taken", new HandleErrorInfo(ex, "EmployeeInfo", "Create"));
            }




        }


        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors
                 .Include(i => i.Course_Dates)
                .Where(i => i.ID == id)
                .Single();
            PopulateAssignedCourseData(instructor);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        private void PopulateAssignedCourseData2(Instructor instructor)
        {
            var allCourses = db.Course_Dates;
            var instructorCourses = new HashSet<int>(instructor.Course_Dates.Select(c => c.Course_DatesID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                if (course.Instructors.Count() == 0)
                {
                    viewModel.Add(new AssignedCourseData
                    {
                        Course_DatesID = course.Course_DatesID,
                        Title = course.Course_Title,
                        Assigned = instructorCourses.Contains(course.Course_DatesID),
                        DF = course.DF.ToString(),
                        FromTime = course.FromHours.ToString().PadRight(2, '0') + ":" + course.FromMinutes.ToString().PadRight(2, '0'),
                        ToTime = course.ToHours.ToString().PadRight(2, '0') + ":" + course.ToMinutes.ToString().PadRight(2, '0')
                    });
                };
            }
            ViewBag.Courses = viewModel;
        }




        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = db.Course_Dates;
            var instructorCourses = new HashSet<int>(instructor.Course_Dates.Select(c => c.Course_DatesID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                if ((course.Instructors.Count() == 0) || instructorCourses.Contains(course.Course_DatesID))
                {


                    viewModel.Add(new AssignedCourseData
                    {
                        Course_DatesID = course.Course_DatesID,
                        Title = course.Course_Title,
                        Assigned = instructorCourses.Contains(course.Course_DatesID),
                        DF = course.DF.ToString(),
                        FromTime = course.FromHours.ToString().PadRight(2, '0') + ":" + course.FromMinutes.ToString().PadRight(2, '0'),
                        ToTime = course.ToHours.ToString().PadRight(2, '0') + ":" + course.ToMinutes.ToString().PadRight(2, '0')
               
                    });
                };
            }
            ViewBag.Courses = viewModel;
        }
        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Instructors
               .Include(i => i.Course_Dates)
               .Where(i => i.ID == id)
               .Single();

            if (TryUpdateModel(instructorToUpdate, "",
               new string[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" }))
            {
                try
                {

                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View("Course_Taken", new HandleErrorInfo(ex, "EmployeeInfo", "Create"));
                }

            }
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }
        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.Course_Dates = new List<Course_Dates>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Course_Dates.Select(c => c.Course_DatesID));
            foreach (var course in db.Course_Dates)
            {
                if (selectedCoursesHS.Contains(course.Course_DatesID.ToString()))
                {
                    if (!instructorCourses.Contains(course.Course_DatesID))
                    {
                        instructorToUpdate.Course_Dates.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.Course_DatesID))
                    {
                        instructorToUpdate.Course_Dates.Remove(course);
                    }
                }
            }
        }



        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors
               .Where(i => i.ID == id)
              .Single();

            db.Instructors.Remove(instructor);

            var department = db.Departments
                .Where(d => d.InstructorID == id)
                .SingleOrDefault();
            if (department != null)
            {
                department.InstructorID = null;
            }

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
    }
}

