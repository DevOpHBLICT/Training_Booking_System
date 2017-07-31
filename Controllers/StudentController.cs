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
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Data.Entity;
using Cascadingdropdownlist.ViewModels;


namespace Cascadingdropdownlist.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();



        // GET: Student
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }


        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Course_Choose()
        {
            //    var Query =SchoolContext.   
            //    ViewBag.CourseID = new SelectList(db.Course_Dates, "CourseID", "FullCourse");
            ViewBag.CourseID = new SelectList(db.Course_Dates, "CourseID", "Course_Title");

            ViewBag.Venues = new SelectList(db.Course_Dates, "CourseID", "Dept");
            ViewBag.DF = new SelectList(db.Course_Dates, "CourseID", "DF");


            return View();
        }

        public ActionResult LoadCourses()

        {
            /*
            ViewBag.Venues = new SelectList(db.Course_Dates, "CourseID", "Dept");
            */
            
            var a = db.Courses.Select(u => new SelectListItem
            {
                Text = u.Title,
                Value = u.CourseID.ToString()
            }).ToList();

            a.Insert(1, new SelectListItem()
            {
                Value = "-1",
                Text = "--------------------------------"
            });
            
            ViewData["Courses"] = a;

            return View();
        }
        public JsonResult GetDates(string id,string id2)

        {

            var st = from s in db.Students select s;

            foreach (var s in st)
            {
                Console.Write(s.CourseID);
            }
          
            var groups = st

           .GroupBy(n => n.CourseID)

            .Select(n => new

            {

                MetricName = n.Key,

                MetricCount = n.Count()

            }

           );


            var c =from gg in db.Course_Dates.Where(p => p.DepartmentID.ToString() == id && p.CourseID.ToString() == id2 && p.DF > DateTime.Now) select gg; 
            

             var q =

              from cc in c

              join p in groups on cc.Course_DatesID equals p.MetricName into ps

              from p in ps.DefaultIfEmpty()

              select new

              {

                  Course_Date =cc.DF,
                  Course_Date_ID = cc.Course_DatesID,

                  CourseName = cc.Course_Title,

                  Countt = p == null ? 0 : p.MetricCount,

                  Capacity = cc.Capacity

              };

             var qList = q.Where(a => a.Countt < a.Capacity);
        //     var a = new SelectListItem { Text = "Please Select Dates", Value = string.Empty };

          
    
       
         var b= qList.Select(u => new SelectListItem
          {
              Text = u.Course_Date.ToString(),
           Value = u.Course_Date_ID.ToString()
        }).Distinct().ToList();


         

            b.Insert(0, new SelectListItem { Text = "Please Select Dates", Value = string.Empty });




            return Json(new SelectList(b, "Value", "Text"));

        }

        public JsonResult GetVenues(string id)

        {



                
                 
                

             var a = db.Course_Dates.Where(p => p.CourseID.ToString() == id).Select(u => new SelectListItem
            {
                Text = u.Dept,
                Value = u.DepartmentID.ToString()
            }).Distinct().ToList();

          
            a.Insert(0, new SelectListItem { Text = "Please Select Venue" , Value = string.Empty});

            return Json( new SelectList(a, "Value", "Text"));

        }


        // GET: Student/Create
        public ActionResult Create()
        {
            //    var Query =SchoolContext.   
            //    ViewBag.CourseID = new SelectList(db.Course_Dates, "CourseID", "FullCourse");
        ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");

        ViewBag.Venues = new SelectList(db.Course_Dates, "CourseID", "Dept");

            var g = db.Course_Dates;
            var i=new SelectList(g, "CourseID", "DF");
            ViewBag.DF = i;


            var k = DateTime.Now;
            /*
         ViewBag.Venues = new SelectList(db.Course_Dates, "CourseID", "Dept");
         */

            var a = db.Courses.ToList().Select(u => new SelectListItem
            {
                Text = u.Title,
                Value = u.CourseID.ToString()
            });

            ViewData["Courses"] = a;
            
            var b = db.Customers.ToList().Select(t => new SelectListItem
            {
                Text = t.Title,
                Value = t.CustomerID.ToString()
            });

            ViewData["Customers"] = b;

            


            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify()
        {
            ViewBag.FormItems = TempData["FormValues"];
            return View();
        }





        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email_Address,Job_Title,Service_Team,Contact_Tel_No,Work_Base_Address,Mobile_No,Courier_Code_Internal_Post,Applicants_Name,Applicants_Date,Managers_Name,Managers_Date,Print_Managers_Name,Managers_Base,Organisation,Student_ID,LastName, FirstMidName, EnrollmentID,CourseID,StudentID,Attended,Capacity")]Student student,FormCollection FC)
        {
            try
            {

                if (ModelState.IsValid)
                {



                    string Customers = FC["Customers"].ToString();
                    string Courses = FC["Courses"].ToString();
                    string Venues = FC["Venues"].ToString();

                    student.CourseID = Int32.Parse(FC["Dates"]);




                    student.Venue = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.Dept).FirstOrDefault();

                    student.DF = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.DF).FirstOrDefault();
                    student.Course_Name = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.Course_Title).FirstOrDefault();

                    student.Organisation = (from p in db.Customers where p.CustomerID.ToString() == Customers select p.Title).FirstOrDefault();

                    var From_Hours = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.FromHours).FirstOrDefault().ToString();
                    var From_Minutes = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.FromMinutes).FirstOrDefault().ToString();
                    var To_Hours = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.ToHours).FirstOrDefault().ToString();
                    var To_Minutes = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.ToMinutes).FirstOrDefault().ToString();

                    
                    if (From_Minutes =="0") { From_Minutes="00";};
                    if (To_Minutes == "0") { To_Minutes = "00"; };

                    string query = "SELECT p.FirstName + ' ' + p.LastName as Name from person p,CourseInstructor i where i.InstructorID=p.ID and i.CourseID=" + student.CourseID + " and i.InstructorID=p.ID";
                   IEnumerable<TrainerForCourse> data = db.Database.SqlQuery<TrainerForCourse>(query);
                   var kkk=data.FirstOrDefault();
                    var ss="confirmed later.";
                    if (kkk != null)
                    {
                        ss = kkk.Name.ToString();
                    }
                     student.Attended = "-";
                         db.Students.Add(student);
                        db.SaveChanges();
                        if (ModelState.IsValid)
                        {
                            string from = "trainingfeedback@hblict.nhs.uk"; //Replace this with your own correct Gmail Address


                            string to = student.Email_Address; //Replace this with the Email Address to whom you want to send the mail


                            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                            mail.To.Add(to);

                            mail.From = new MailAddress(from, "HBL Training", System.Text.Encoding.UTF8);
                
                            mail.SubjectEncoding = System.Text.Encoding.UTF8;

                            string path = Server.MapPath(@"~/images/HBLICT.png");
                            LinkedResource logo = new LinkedResource(path);
                            logo.ContentId = "company logo";
                            var s =new DateTime ();
                            
                            if (student.DF.HasValue)
                            {
                                s = student.DF.Value;
                            }

                            mail.Subject = "HBL Joining Instructions for Course:" + s.ToShortDateString() + " " + student.Course_Name;
                          mail.Body = "<html><body lang=EN-GB link=blue vlink=purple style='' >";
                              mail.Body = mail.Body + "  <br class=MsoNormal align=right style='text-align:left'>HBL ICT Training Department";
                            mail.Body = mail.Body + " <br bclass=MsoNormal align=right style='text-align:left'>Charter House";
                            mail.Body = mail.Body + " <br class=MsoNormal align=right style='text-align:left'>Parkway";
                            mail.Body = mail.Body + " <br class=MsoNormal align=right style='text-align:left'>Welwyn Garden City";
                            mail.Body = mail.Body + " <br class=MsoNormalalign=right style='text-align:left'>Hertfordshire ";
                            mail.Body = mail.Body + " <br class=MsoNormal align=right style='text-align:left'>AL8 6JL";
                            mail.Body = mail.Body + "<br class=MsoNormal align=right style='text-align:left'>Date:" + DateTime.Now.ToShortDateString() + "";
                             mail.Body = mail.Body + " <p class=MsoNormal>Dear " + student.FirstMidName + " " + student.LastName + ",";
                              mail.Body = mail.Body + " <p class=MsoNormal>I am pleased to confirm you have a place booked on the <b>" + student.Course_Name + "</b> course.<o:p></o:p></span></p>";
                             mail.Body = mail.Body + "<table class=MsoTableGrid border=1 cellspacing=0 cellpadding=5 style='border-collapse:collapse;border:none;mso-border-alt:solid;'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'>";
                            mail.Body = mail.Body + "<td class=TBackground> <p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal'><b><span>Date<o:p></o:p></span></b></p> </td>";
                            mail.Body = mail.Body + "  <td class=TBackground><p class=MsoNormal><b><span>Start Time<o:p></o:p></span></b></p></td>";
                            mail.Body = mail.Body + "  <td class=TBackground><p class=MsoNormal><b><span>Finish Time<o:p></o:p></span></b></p> </td></tr>";
                            mail.Body = mail.Body + " <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'>";
                            mail.Body = mail.Body + " <td> <p class=MsoNormal><b><span>" + s.ToShortDateString() + "<o:p></o:p></span></p> </td> ";
                            mail.Body = mail.Body + "  <td> <p class=MsoNormal><b><span>" + From_Hours + ":" + From_Minutes + "<o:p></o:p></span></p> </td>";
                            mail.Body = mail.Body + " <td> <p class=MsoNormal><b><span>" + To_Hours + ":" + To_Minutes + "<o:p></o:p></span></p></td></tr></table>";
                            mail.Body = mail.Body + "<br class=MsoNormal>Venue:-</span></b><span >" + student.Venue +   "<o:p></o:p></span>";
                            mail.Body = mail.Body + " <br class=MsoNormal >Your trainer for this training course will be " + ss + "<span style='mso-spacerun:yes'>  </span>Please bring this letter of confirmation with you.<o:p></o:p></span></p>";
                            mail.Body = mail.Body + " <br class=MsoNormal><b><span>Breaks<o:p></o:p></span></b>";
                            mail.Body = mail.Body + " <br class=MsoNormal>The times shown include two short breaks of approximately 10 minutes. </span>";
                            mail.Body = mail.Body + "<br class=MsoNormal><b><span>Special Needs/Requirements</span></b> If you have any special needs/requirements, please can you email <a href:- mailto:training.support@nhs.net ><span>training.support@nhs.net</span></a> as soon as possible.";
                            mail.Body = mail.Body + " <br class=MsoNormal><b><span>Cancellation</span></b> If you need to cancel please email </span><a href:- mailto:training.support@nhs.net ><span>training.support@nhs.net</span></a><span> as soon as possible.";
                            mail.Body = mail.Body + "<br class=MsoNormal><b><span>Evaluation</b> On completion of the course you will be required to evaluate the training delivered the details of the how to access the evaluation form is shown below.";
                            mail.Body = mail.Body + "<br class=MsoNormal><span>The link is: https://nww.hblict.nhs.uk/trainingfeedback";
                 



                            mail.BodyEncoding = System.Text.Encoding.UTF8;
                            mail.IsBodyHtml = true;
                            mail.Priority = MailPriority.High;

                            SmtpClient client = new SmtpClient();

                            //Add the Creddentials- use your own email id and password
                            client.Credentials = new System.Net.NetworkCredential(from, "Ukraine1917!");

                            // client.Port = 587; // Gmail works on this port
                            client.Port = 25;
                            //  client.Host = "smtp.gmail.com";
                            client.Host = "char-ht-01";
                            //client.EnableSsl = true; //Gmail works on Server Secured Layer
                            client.EnableSsl = false;
                            try
                            {

                                client.Send(mail);
                            }

                            catch (Exception ex)

                            {

                                Exception ex2 = ex;

                                string errorMessage = string.Empty;

                                while (ex2 != null)

                                {

                                    errorMessage += ex2.ToString();

                                    ex2 = ex2.InnerException;

                                }

                                //    HttpContext.Current.Response.Write(errorMessage);
                            }
                            } // end try
                        
                            return View("Thankyou",student);

                    }
                    else
                    {
     

                    }
                    //   return RedirectToAction("Verify",Veri);
                }
            
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
             // return View();
            return RedirectToAction("Create");
         //   return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

        }


        public JsonResult GetDatesByID(int ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var s = db.Course_Dates.Where(p => p.CourseID == ID && p.DF < DateTime.Today);
            return Json(s, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDepartmentsByID(int ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Json(db.Course_Dates.Where(p =>p.CourseID== ID),JsonRequestBehavior.AllowGet);
        }


        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        public ActionResult CustomDropDownController()
        {

            return View();
        }



        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.Students.Find(id);
            if (TryUpdateModel(studentToUpdate, "",
               new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
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
            return View(studentToUpdate);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
