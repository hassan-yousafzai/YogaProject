using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Helper
{
    /// <summary>
    /// This is a utility class that has been developed to have the most common methods that are used in many places
    /// some of these methods are getting a logged in user id or sending an email when a customer books or cancels an event
    /// This class and its methods are all static so therefore there is no need for creating an object for it to access its methods
    /// </summary>
    public static class Utility
    {
        //this method gets the logged in user Id 
        //it makes it easy as then approprate data can be shown to only the correct user
        public static string GetLoggedInUserId()
        {
            try
            {
                ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                             .FindById(HttpContext.Current.User.Identity.GetUserId());
                return user.Id;
            }
            catch (NullReferenceException e)
            {
                return e + " ";
            }
        }

        //this method converts a string to title case
        public static string ConvertToTitleCase(string str)
        {
            return str = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        //this method checks user age - mainly used when registering users
        public static bool CheckUserAge(DateTime date)
        {
            var status = false;

            var currentYear = DateTime.Today.Year;
            var selectedDate = date.Year;

            var difference = (currentYear - selectedDate);
            if (difference >= 16)
                status = true;

            return status;
        }

        //this method takes a customer and class objects with a string as the email subject
        //this method sends a booking email to a customer who books the event
        //this method also sends the amount of days a customer has till he can cancel the booking 
        //if there are less than 3 days than the customer can't cancel the booking
        public static void SendBookingMail(Customer customer, Class vClass, string subject)
        {
            DateTime start = vClass.StartDate.Date;
            DateTime currentDate = DateTime.Now.Date;

            var numberOfDaysLeftToCancel = (start - currentDate).Days - 3;
            string canCancelBooking = "";
            if (numberOfDaysLeftToCancel > 3)
                canCancelBooking = "You can cancel your booking within " + numberOfDaysLeftToCancel + " days. </ br> ";
            else
                canCancelBooking = "You cannot cancel your booking anymore. </ br> ";

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            // setup Smtp authentication
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
            msg.To.Add(new MailAddress(customer.Email));
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<html><head></head><body><b>" +
                "The below is the summary of your recent booking with the Yoga Fitness Club. " + canCancelBooking +
                "<table border=" + 1 + ">" +
                    "<thead>" +
                        "<tr>" + "<th>Tutor</th>" + "<th>Start</th>" + "<th>End</th>" + "<th>Price</th>" + "<th>Location</th>" + "</tr>" +
                    "</thead>" +
                    "<tbody>" +
                        "<tr>" + "<td>" + customer.Name + "</td>" + "<td>" + vClass.StartDate + "</td>" + "<td>" + vClass.EndDate + "</td>" + "<td> £" + vClass.ClassType.Price + "</td>" +
                            "<td>" + vClass.Room.Branch.Address + ", " + vClass.Room.Branch.Postcode + ", Room Number: " + vClass.Room.RoomNumber + "</td>" +
                        "</tr>" +
                    "</tbody>" +
                "</table>" +
                "</b></body>");
            client.Send(msg);
        }

        //this method takes a customer object and a string as the email subject
        //this method sends a cancellation email to a customer who cancels a booking he made
        public static void SendCancellationMail(Customer customer, string subject)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            // setup Smtp authentication
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
            msg.To.Add(new MailAddress(customer.Email));
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<html><head></head><body>Your booking has been removed and you have been refunded if you have paid.</body>");

            client.Send(msg);
        }
    }
}

