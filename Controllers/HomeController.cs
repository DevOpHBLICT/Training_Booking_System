using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cascadingdropdownlist.DAL;
using Cascadingdropdownlist.ViewModels;
using Cascadingdropdownlist.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Data.Entity;
using Rotativa;
using System.IO;
using System.Data.Entity.Infrastructure;
using Rotativa.Options;
using PagedList;
using System.Globalization;
namespace Cascadingdropdownlist.Controllers
{
    public class HomeController : Controller
    {
        private SchoolContext db = new SchoolContext();

        public ActionResult Absent(int? id)
        {
            var prod = db.Students.Find(id);
            if (prod.Attended == "Y") { prod.Attended = "N"; }
            else if (prod.Attended == "N") { prod.Attended = "-"; }
            else if (prod.Attended == "-") { prod.Attended = "Y"; };

            db.Entry(prod).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

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
            Person student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
          return View(student);
       //     return RedirectToAction("Students_On_Course", new { id = ViewBag.CourseID });
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                ViewBag.CourseID = student.CourseID;
                db.Students.Remove(student);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Students_On_Course", new { id = ViewBag.CourseID});
        }





        public ActionResult Attended(int? id)
        {
            var prod = db.Students.Find(id);
            prod.Attended = "Y";
            db.Entry(prod).State = EntityState.Modified;
            db.SaveChanges();


            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        public ActionResult drop()
        {
            return View();
        }


        public ActionResult Sent()
        {
            return View();
        }


        public ActionResult Print(int? id)
        {
            var student = db.Students.Where(x => x.CourseID == id);
            ViewBag.CourseTitle = (from p in db.Course_Dates where p.Course_DatesID == id select p.Course_Title).First();
            ViewBag.CourseDF = (from p in db.Course_Dates where p.Course_DatesID == id select p.DF).First();
            ViewBag.Location = (from p in db.Students where p.CourseID == id select p.Venue).First();
            ViewBag.Capacity = (from p in db.Course_Dates where p.Course_DatesID == id select p.Capacity).First();
            ViewBag.Venue = (from p in db.Course_Dates where p.Course_DatesID == id select p.Department).First();

            MemoryStream htmlStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(htmlStream);

            string HTML = "<html>";
            HTML = HTML + "<body><table ><tr><td>Course Title: </td><td>" + ViewBag.CourseTitle + "</td></tr><tr><td>Course Date</td><td>" + ViewBag.CourseDF + "</td></tr><tr><td>Course Location</td><td>" + ViewBag.Location + "</td></tr><tr></tr></table><table style='width:100%' height='100%' 'border:1px solid black'><th>Name</th><th>Signature</th>";
            foreach (var s in student)
            {
                HTML = HTML + "<tr><td height='100' style='border:1px solid black'>" + s.FullName + "</td><td height='100' style='border:1px solid black'></td></tr>";
            }
            HTML = HTML + "</table></body></html>";
            writer.Write(HTML);
            writer.Flush();
            htmlStream.Position = 0;

            return File(htmlStream, "application/msword", Server.UrlEncode("filename5.doc"));
            //        return View(student);
        }



        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create(int? id)
        {
     //    ViewBag.CourseID = new SelectList(db.Course_Dates, "CourseID", "FullCourse");
            ViewData["CourseID"] = id;

            ViewBag.Venue = (from p in db.Course_Dates where p.CourseID == id select p.Dept).FirstOrDefault();
            ViewBag.CourseDF = (from p in db.Course_Dates where p.Course_DatesID == id select p.DF).FirstOrDefault().ToShortDateString();
            ViewBag.CourseName= (from p in db.Course_Dates where p.Course_DatesID == id select p.Course_Title).FirstOrDefault();

            var b = db.Customers.ToList().Select(t => new SelectListItem
            {
                Text = t.Title,
                Value = t.CustomerID.ToString()
            });

            ViewData["Customers"] = b;
            return View("Create");
        }


        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email_Address,Job_Title,Service_Team,Contact_Tel_No,Work_Base_Address,Mobile_No,Courier_Code_Internal_Post,Applicants_Name,Applicants_Date,Managers_Name,Managers_Date,Print_Managers_Name,Managers_Base,Organisation,Student_ID,LastName, FirstMidName, EnrollmentID,CourseID,StudentID,Attended,Capacity")]Student student, FormCollection FC, int id)
        {
            try
            {

                if (ModelState.IsValid)
                {



                    string Customers = FC["Customers"].ToString();

                   student.CourseID=id;



                   student.Venue = (from p in db.Course_Dates where p.Course_DatesID == id select p.Dept).FirstOrDefault();

               

                    student.DF = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.DF).FirstOrDefault();
                    student.Course_Name = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.Course_Title).FirstOrDefault();

                    student.Organisation = (from p in db.Customers where p.CustomerID.ToString() == Customers select p.Title).FirstOrDefault();

                    var From_Hours = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.FromHours).FirstOrDefault().ToString();
                    var From_Minutes = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.FromMinutes).FirstOrDefault().ToString();
                    var To_Hours = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.ToHours).FirstOrDefault().ToString();
                    var To_Minutes = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.ToMinutes).FirstOrDefault().ToString();


                    if (From_Minutes == "0") { From_Minutes = "00"; };
                    if (To_Minutes == "0") { To_Minutes = "00"; };

                    string query = "SELECT p.FirstName + ' ' + p.LastName as Name from person p,CourseInstructor i where i.InstructorID=p.ID and i.CourseID=" + student.CourseID + " and i.InstructorID=p.ID";
                    IEnumerable<TrainerForCourse> data = db.Database.SqlQuery<TrainerForCourse>(query);
                    var kkk = data.FirstOrDefault();
                    var ss = "confirmed later.";
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
                        var s = new DateTime();

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
                        mail.Body = mail.Body + "<br class=MsoNormal>Venue:-</span></b><span >" + student.Venue + "<o:p></o:p></span>";
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
                        client.EnableSsl =false; //Gmail works on Server Secured Layer

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

                    return View("Thankyou", student);

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


        public ActionResult Create_Admin(int? id)
        {
            //    ViewBag.CourseID = new SelectList(db.Course_Dates, "CourseID", "FullCourse");
            ViewData["CourseID"] = id;

            ViewBag.Venue = (from p in db.Course_Dates where p.Course_DatesID == id select p.Dept).FirstOrDefault();
            ViewBag.CourseDF = (from p in db.Course_Dates where p.Course_DatesID == id select p.DF).FirstOrDefault().ToShortDateString();
            ViewBag.CourseName = (from p in db.Course_Dates where p.Course_DatesID == id select p.Course_Title).FirstOrDefault();

            var b = db.Customers.ToList().Select(t => new SelectListItem
            {
                Text = t.Title,
                Value = t.CustomerID.ToString()
            });

            ViewData["Customers"] = b;
            return View("Create_Admin");
        }


        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Admin([Bind(Include = "Email_Address,Job_Title,Service_Team,Contact_Tel_No,Work_Base_Address,Mobile_No,Courier_Code_Internal_Post,Applicants_Name,Applicants_Date,Managers_Name,Managers_Date,Print_Managers_Name,Managers_Base,Organisation,Student_ID,LastName, FirstMidName, EnrollmentID,CourseID,StudentID,Attended,Capacity")]Student student, FormCollection FC, int id)
        {
            try
            {

                if (ModelState.IsValid)
                {


                    string Send = FC["Completed"];
                    string Customers = FC["Customers"].ToString();

                    student.CourseID = id;



                    student.Venue = (from p in db.Course_Dates where p.Course_DatesID == id select p.Dept).FirstOrDefault();



                    student.DF = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.DF).FirstOrDefault();
                    student.Course_Name = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.Course_Title).FirstOrDefault();

                    student.Organisation = (from p in db.Customers where p.CustomerID.ToString() == Customers select p.Title).FirstOrDefault();

                    var From_Hours = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.FromHours).FirstOrDefault().ToString();
                    var From_Minutes = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.FromMinutes).FirstOrDefault().ToString();
                    var To_Hours = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.ToHours).FirstOrDefault().ToString();
                    var To_Minutes = (from p in db.Course_Dates where p.Course_DatesID.ToString() == student.CourseID.ToString() select p.ToMinutes).FirstOrDefault().ToString();


                    if (From_Minutes == "0") { From_Minutes = "00"; };
                    if (To_Minutes == "0") { To_Minutes = "00"; };

                    string query = "SELECT p.FirstName + ' ' + p.LastName as Name from person p,CourseInstructor i where i.InstructorID=p.ID and i.CourseID=" + student.CourseID + " and i.InstructorID=p.ID";
                    IEnumerable<TrainerForCourse> data = db.Database.SqlQuery<TrainerForCourse>(query);
                    var kkk = data.FirstOrDefault();
                    var ss = "confirmed later.";
                    if (kkk != null)
                    {
                        ss = kkk.Name.ToString();
                    }
                    student.Attended = "-";
                    db.Students.Add(student);
                    db.SaveChanges();



                    if ((ModelState.IsValid) && (Send=="Send"))
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
                        var s = new DateTime();

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
                        mail.Body = mail.Body + "<br class=MsoNormal>Venue:-</span></b><span >" + student.Venue + "<o:p></o:p></span>";
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
                        client.EnableSsl = false; //Gmail works on Server Secured Layer

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


                    if (Send == "send") { return View("Thankyou", student); }
                    if (Send == "Don't Send") { return View("DontSend", student); }

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
            return RedirectToAction("About");
            //   return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

        }






        public ActionResult Students_On_Course(int? id)
        {

            var student = db.Students.Where(x => x.CourseID == id);
            ViewBag.CourseID = id;

            ViewBag.CourseTitle = (from p in db.Course_Dates where p.Course_DatesID == id select p.Course_Title).FirstOrDefault();
            ViewBag.CourseDF = (from p in db.Course_Dates where p.Course_DatesID == id select p.DF).FirstOrDefault().ToShortDateString();
          
            
            ViewBag.Location = (from p in db.Students where p.CourseID == id select p.Venue).FirstOrDefault();
            ViewBag.Capacity = (from p in db.Course_Dates where p.Course_DatesID == id select p.Capacity).FirstOrDefault();
            ViewBag.Venue = (from p in db.Course_Dates where p.Course_DatesID == id select p.Dept).FirstOrDefault();

            ViewBag.FromHours = (from p in db.Course_Dates where p.Course_DatesID == id select p.FromHours).FirstOrDefault();
          
            ViewBag.FromMinutes =  (from p in db.Course_Dates where p.Course_DatesID == id select p.FromMinutes).FirstOrDefault();
   
            ViewBag.ToHours =  (from p in db.Course_Dates where p.Course_DatesID == id select p.ToHours).FirstOrDefault();
            ViewBag.ToMinutes = (from p in db.Course_Dates where p.Course_DatesID == id select p.ToMinutes).FirstOrDefault();

            if (ViewBag.FromMinutes == 0) { ViewBag.FromMinutes = "00"; }
            if (ViewBag.ToMinutes == 0) { ViewBag.ToMinutes = "00"; }


            return View(student);
        }

        public ViewResult TrainingView(string q)
        {
            /*
            var persons = from p in db.People select p;
            if (!string.IsNullOrWhiteSpace(q))
            {
                persons = persons.Where(p => p.First.Contains(q));
            }*/
            return View();
        
            }


        public ActionResult TrainingInPeriod(string q,string q2)
        {
            // SQL version of the above LINQ code.
            string query = "SELECT p.Organisation as Organisation,c.Course_Title, COUNT(p.ID) AS SessionCount " +
"FROM Course_Dates c  left join Person p on c.Course_DatesID = p.CourseID GROUP BY p.Organisation,c.Course_Title";

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = "SELECT p.Organisation as Organisation,c.Course_Title, COUNT(p.ID) AS SessionCount " +
        "FROM Course_Dates c  left join Person p on c.Course_DatesID = p.CourseID WHERE c.DF >='" + q + "' and c.DF <='" + q2 + "' GROUP BY p.Organisation,c.Course_Title";
            }


            IEnumerable<TrainingInPeriod> data = db.Database.SqlQuery<TrainingInPeriod>(query);
            return View(data.ToList());
        }






        public ActionResult TrainerCourseDates()
        {
            // Commenting out LINQ to show how to do the same thing in SQL.
            //IQueryable<EnrollmentDateGroup> = from student in db.Students
            //           group student by student.EnrollmentDate into dateGroup
            //           select new EnrollmentDateGroup()
            //           {
            //               EnrollmentDate = dateGroup.Key,
            //               StudentCount = dateGroup.Count()
            //           };

         //   IList<TrainerCourseDates> tcd = new List<TrainerCourseDates>();
            /*
                var query= from Course_Dates in db.Course_Dates
                from CourseInstructor
                in db.Instructors
                    .Where(v => v.Course_Dates.== Course_Dates.Course_DatesID)
                    .DefaultIfEmpty()
                  select new { Course_Dates =Course_Dates.DF , CourseInstructor = CourseInstructor.LastName, Course_Title = Course_Dates.Course_Title};

                        var publishers = query.ToList();
                        foreach (var publisherData in publishers)
                        {
                            tcd.Add(new TrainerCourseDates()
                            {
                                Course_Title= publisherData.Course_Title,
                                DF= publisherData.Course_Dates,
                                Fullname = publisherData.CourseInstructor
                            });
                        }
            */

            // var l = data.ToList();



            //Vendor and Status properties will be null if the left join is null


            // SQL version of the above LINQ code.
            // string query = "SELECT p.FirstName + p.Lastname as Fullname,c.Course_Title as Course_Title,c.DF as DF from Course_Dates c left join CourseInstructor cc on c.Course_DatesID = cc.CourseID left join Person p on cc.InstructorID = p.ID";




            //var data = db.Database.SqlQuery<TrainerCourseDates>(query);


            return View();
        }

        public ActionResult HoursByCustomer()
        {
            var student = db.Students;

            return View();
        }
       // [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReportsByCustomer(string searchString,string searchString2 )
        {

       //    string itemId = formValue["Completed"];


        
            ViewBag.CurrentFilter = searchString;

        

  


            // Commenting out LINQ to show how to do the same thing in SQL.
            //IQueryable<EnrollmentDateGroup> = from student in db.Students
            //           group student by student.EnrollmentDate into dateGroup
            //           select new EnrollmentDateGroup()
            //           {
            //               EnrollmentDate = dateGroup.Key,
            //               StudentCount = dateGroup.Count()
            //           };

            // SQL version of the above LINQ code.
    //        string query = "SELECT c.Course_DatesID,c.DF,Course_Title,Dept, Duration,Organisation from Course_Dates c,Department d,Person p where c.DepartmentID=d.DepartmentID and p.CourseID=c.Course_DatesID";

            string query = "SELECT * FROM ( SELECT    Course_Name as Course_Title,  Organisation  FROM Person where Discriminator='student') as s PIVOT(  Count(Organisation) FOR [Organisation] IN (BCCG, LCCG, HVCG, ENHCCG))AS pvt";
        //    DateTime dt, dt2;

            if (!String.IsNullOrEmpty(searchString) ||!String.IsNullOrEmpty(searchString2)  )
           {
      //            dt = DateTime.ParseExact(searchString, "yyyy-mm-dd", CultureInfo.InvariantCulture);

        //        dt2 = DateTime.ParseExact(searchString2, "yyyy-mm-dd", CultureInfo.InvariantCulture);
          //        query = "SELECT * FROM ( SELECT    Course_Name as Course_Title,  Organisation  FROM Person where DF >'" +searchString + "') as s PIVOT(  Count(Organisation) FOR [Organisation] IN (BCCG, LCCG, HVCG, ENHCCG))AS pvt";

           query="SELECT * FROM ( SELECT    Course_Name as Course_Title,  Organisation  FROM Person where DF between CAST('" + searchString + "' AS DATETIME) and CAST('" + searchString2 + "' AS DATETIME) and Discriminator='Student') as s PIVOT(  Count(Organisation) FOR [Organisation] IN (BCCG, LCCG, HVCG, ENHCCG))AS pvt ";
           }
    //        if (!String.IsNullOrEmpty(searchString2))
    //        {/

  //            dt = DateTime.ParseExact(searchString, "yyyy-mm-dd", CultureInfo.InvariantCulture);

    //            dt2 = DateTime.ParseExact(searchString2, "yyyy-mm-dd", CultureInfo.InvariantCulture);
          
      //          query = "SELECT * FROM ( SELECT    Course_Name as Course_Title,  Organisation  FROM Person where DF >='" + dt + "' and DF <='" + dt2 + "') as s PIVOT(  Count(Organisation) FOR [Organisation] IN (BCCG, LCCG, HVCG, ENHCCG))AS pvt";


            //}    
            IEnumerable<ReportsByCustomer2> data = db.Database.SqlQuery<ReportsByCustomer2>(query);


            return View(data.ToList());
        }

        public ActionResult About(string sortOrder, string currentFilter, string searchString,string Course, int? page)
        {
            // Commenting out LINQ to show  to do the same thing in SQL.
            //IQueryable<EnrollmentDateGroup> = from student in db.Students
            //           group student by student.EnrollmentDate into dateGroup
            //           select new EnrollmentDateGroup()
            //           {
            //               EnrollmentDate = dateGroup.Key,
            //               StudentCount = dateGroup.Count()
            //           };

            // SQL version of the above LINQ code.
            ViewBag.CurrentSort = sortOrder;

             ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
   ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
  // var students = from s in db.Students
    //              select s;
   if (searchString != null)
   {
       page = 1;
   }
   else
   {
       searchString = currentFilter;
   }

   ViewBag.CurrentFilter = searchString;


   string query = "SELECT isnull(cast(c.Course_DatesID as nvarchar(40)),0) as Course_DatesID,pp.LastName + ' ' + pp.FirstName as Trainer,c.Dept as Venue,isnull(c.DF,0) as DateFrom,RIGHT('00'+CAST(FromHours AS VARCHAR(2)),2) as FromHours,RIGHT('00'+CAST(FromMinutes AS VARCHAR(2)),2) as FromMinutes,RIGHT('00'+CAST(ToHours AS VARCHAR(2)),2) as ToHours,RIGHT('00'+CAST(ToMinutes AS VARCHAR(2)),2) as ToMinutes,c.Course_Title as Course,isnull(c.Capacity,0) as Capacity, COUNT(p.ID) AS StudentCount " +
"FROM Course_Dates c  left join Person p on c.Course_DatesID = p.CourseID left join CourseInstructor ci on c.Course_DatesID  = ci.CourseID left join person pp on ci.instructorID = pp.ID ";

 if ((!String.IsNullOrEmpty(searchString))|| (!String.IsNullOrEmpty(Course)))
   {
  query = query + " where ";
 }
   if (!String.IsNullOrEmpty(searchString))
   {
       query = query + "pp.LastName like '%" + searchString + "%' or pp.FirstName like '%" + searchString + "%' ";
           
    
   }

 if ((!String.IsNullOrEmpty(searchString)) && (!String.IsNullOrEmpty(Course)))

   {
     query = query + " and ";
 }


   if (!String.IsNullOrEmpty(Course))
   {
       query = query + "c.Course_Title like '%" + Course+ "%' ";


   }

  
  

            query = query + "GROUP BY Course_DatesID, pp.LastName+ ' ' + pp.FirstName,c.Course_DatesID,c.Dept,c.Course_Title,c.DF,FromHours,FromMinutes,ToHours,ToMinutes,c.Capacity";



 



   switch (sortOrder)
   {
      case "name_desc":
         query= query +  " order by Trainer desc";
         break;
      case "Date":
         query = query + " order by DateFrom";
         break;
      case "date_desc":
         query = query + " order by DateFrom desc";
         break;
      default:
         query = query + " order by Trainer";
         break;
}


   IEnumerable<EnrollmentDateGroup> data = db.Database.SqlQuery<EnrollmentDateGroup>(query);

   int pageSize = 8;
   int pageNumber = (page ?? 1);
   return View(data.ToPagedList(pageNumber, pageSize));

        }


        public ActionResult Live(string sortOrder, string currentFilter, string searchString, string Course, int? page)
        {
            // Commenting out LINQ to show  to do the same thing in SQL.
            //IQueryable<EnrollmentDateGroup> = from student in db.Students
            //           group student by student.EnrollmentDate into dateGroup
            //           select new EnrollmentDateGroup()
            //           {
            //               EnrollmentDate = dateGroup.Key,
            //               StudentCount = dateGroup.Count()
            //           };

            // SQL version of the above LINQ code.
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            // var students = from s in db.Students
            //              select s;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            string query = "SELECT isnull(cast(c.Course_DatesID as nvarchar(40)),0) as Course_DatesID,pp.LastName + ' ' + pp.FirstName as Trainer,c.Dept as Venue,isnull(c.DF,0) as DateFrom,RIGHT('00'+CAST(FromHours AS VARCHAR(2)),2) as FromHours,RIGHT('00'+CAST(FromMinutes AS VARCHAR(2)),2) as FromMinutes,RIGHT('00'+CAST(ToHours AS VARCHAR(2)),2) as ToHours,RIGHT('00'+CAST(ToMinutes AS VARCHAR(2)),2) as ToMinutes,c.Course_Title as Course,isnull(c.Capacity,0) as Capacity, COUNT(p.ID) AS StudentCount " +
         "FROM Course_Dates c  left join Person p on c.Course_DatesID = p.CourseID left join CourseInstructor ci on c.Course_DatesID  = ci.CourseID left join person pp on ci.instructorID = pp.ID where c.DF>=getdate() ";

          
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query + " and pp.LastName like '%" + searchString + "%' or pp.FirstName like '%" + searchString + "%' ";


            }

        


            if (!String.IsNullOrEmpty(Course))
            {
                query = query + " and c.Course_Title like '%" + Course + "%' ";


            }




            query = query + "GROUP BY Course_DatesID, pp.LastName+ ' ' + pp.FirstName,c.Course_DatesID,c.Dept,c.Course_Title,c.DF,FromHours,FromMinutes,ToHours,ToMinutes,c.Capacity";







            switch (sortOrder)
            {
                case "name_desc":
                    query = query + " order by Trainer desc";
                    break;
                case "Date":
                    query = query + " order by DateFrom";
                    break;
                case "date_desc":
                    query = query + " order by DateFrom desc";
                    break;
                default:
                    query = query + " order by Trainer";
                    break;
            }


            IEnumerable<EnrollmentDateGroup> data = db.Database.SqlQuery<EnrollmentDateGroup>(query);

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(data.ToPagedList(pageNumber, pageSize));

        }




        public ActionResult Thankyou()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult DontSend()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Email()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Cert()
        {
            return View();
        }

        [WordDocument]
        public ActionResult Certificate(int? id)
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
            ViewBag.Student = student.FullName;
            ViewBag.T = student.Course_Name;
            ViewBag.CourseID = student.CourseID;
            ViewBag.Date = DateTime.Today.ToShortDateString();
            string query = "SELECT p.FirstName + ' ' + p.LastName as Name from person p,CourseInstructor i where i.InstructorID=p.ID and i.CourseID=" + student.CourseID + " and i.InstructorID=p.ID";
            IEnumerable<TrainerForCourse> data = db.Database.SqlQuery<TrainerForCourse>(query);
            var kkk = data.FirstOrDefault();
            ViewBag.Trainer = "confirmed later.";
            if (kkk != null)
            {
                ViewBag.Trainer = kkk.Name.ToString();
            }



            return new ViewAsPdf("Certificate") { FileName = @ViewBag.Student + " " + @ViewBag.T + " " + DateTime.Now + ".pdf", PageOrientation = Rotativa.Options.Orientation.Landscape, PageMargins = new Margins(0, 0, 0, 0), CustomSwitches = "--disable-smart-shrinking", PageWidth = 210, PageHeight = 297, PageSize = Size.A4 };


        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }






    }

}