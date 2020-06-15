using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [Route("api/users/forgotpassword")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly DatabaseService _fpService;

        public ForgotPasswordController(DatabaseService fpService)
        {
            _fpService = fpService;
        }
        
    

        public static string RandomToken()
        {
            Random random = new Random();
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string token = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return token;
        }

        // PUT: api/ForgotPassword/5
        [HttpPut]
        public IDictionary<string, string> Put([FromBody] IDictionary<string, string> request)
        {
            string email = request["email"];
            Console.WriteLine(email);
            List<User> users = _fpService.GetUsers();
            IMongoCollection<User> collection = _fpService.GetCollectionUser();

            if (users.Exists(x => x.email == email))
            {
                try
                {
                    var token = _fpService.GenerateJSONWebToken(request["email"], "password");
                    Console.WriteLine(token);
                    var filter = Builders<User>.Filter.Eq("email", email);
                    var update = Builders<User>.Update.Set("token", token);
                    collection.UpdateOne(filter, update);

                    string link = "http://localhost:4200/reset-password?token=" + token;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");


                    mail.From = new MailAddress(email+ "@info.uaic.ro");
                    mail.To.Add("vasile.lazar@info.uaic.ro");
                    mail.Subject = "Reset-password";
                    string htmlString = $@"<html>

                      <body>

                      <p>Buna ziua,</p>

                      <p>Link-ul de resetare parola este urmatorul: <a href = '{link}'> {link} </a></p>

                      <p>O zi buna in continuare,<br>- echipa Gaudeamus</br></p>

                      </body>

                      </html>

                     ";

                    mail.Body = htmlString;
                    mail.IsBodyHtml = true;

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("lazarvasile21091998@gmail.com", "itprogramator21");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    Console.WriteLine("Email sent");
                    Dictionary<string, string> dict = new Dictionary<string, string> { };
                    dict["response"] = "true";
                    return dict;
                }
                catch (Exception)
                {
                    Console.WriteLine("Not able to sent email");
                    Dictionary<string, string> dict = new Dictionary<string, string> { };
                    dict["response"] = "false";
                    return dict;
                }
            }
            else
            {
                Dictionary<string, string> dict = new Dictionary<string, string> { };
                dict["response"] = "false";
                return dict;
            }
            
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
