using Microsoft.Azure.Mobile.Server.Config;
using MyPoetryMobileService.DataObjects;
using MyPoetryMobileService.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyPoetryMobileService.Controllers
{
    [MobileAppController]
    public class CustomRegistrationController : ApiController
    {
        private const string connectionStringName = "Name=MyPoetryMobileContext";
        
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        // POST api/CustomRegistration
        public HttpResponseMessage Post(RegistrationRequest registrationRequest)
        {
            if (string.IsNullOrWhiteSpace(registrationRequest.Email) ||
                string.IsNullOrWhiteSpace(registrationRequest.Password) ||
                string.IsNullOrWhiteSpace(registrationRequest.PasswordConfirm) ||
                string.IsNullOrWhiteSpace(registrationRequest.Name) ||
                string.IsNullOrWhiteSpace(registrationRequest.Surname) ||
                string.IsNullOrWhiteSpace(registrationRequest.Gender) ||
                registrationRequest.RegistrationDate == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_R_1");
            }
            else if (registrationRequest.Password.Length < 8)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_R_2");
            }
            else if (!registrationRequest.Password.Equals(registrationRequest.PasswordConfirm))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_R_3");
            }
            else if (!registrationRequest.Email.Contains("@"))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_R_4");
            }
            else if (registrationRequest.Gender.Length != 1)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_R_5");
            }
            
            // SQL Database version using Entity Framework for data access.
            MyPoetryMobileContext context = new MyPoetryMobileContext(connectionStringName);
            User account = context.User.Where(a => a.Email == registrationRequest.Email.Trim().ToLower()).SingleOrDefault();
            
            // If there's already a confirmed account, return an error response.
            if (account != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_R_6");
            }
            else
            {
                byte[] salt = CustomLoginProviderUtils.GenerateSalt();

                // Generates random 6 digit number
                Random generator = new Random();
                String activationCode = generator.Next(0, 1000000).ToString("D6");

                User newUser = new Models.User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = registrationRequest.Email.Trim().ToLower(),
                    Salt = salt,
                    SaltedAndHashedPassword = CustomLoginProviderUtils.Hash(registrationRequest.Password, salt),
                    Name = registrationRequest.Name.Trim(),
                    Surname = registrationRequest.Surname.Trim(),
                    Gender = registrationRequest.Gender,
                    Photo = registrationRequest.Photo,
                    RegistrationDate = registrationRequest.RegistrationDate,
                    AccessesNumber = 0,
                    UseTime = 0,
                    IsActivated = false,
                    ActivationCode = activationCode
                };

                context.User.Add(newUser);

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }

                // Sends welcome email
                EmailHandler.SendEmail(
                    newUser.Email,
                    newUser.Name + " " + newUser.Surname,
                    "Welcome to MyPoetry",
                    "<p><span style=\"font - size:14px; \">Hi " + newUser.Name + " " + newUser.Surname + ",</span></p>" +
                    "<h1><strong>Welcome to <span style = \"color:#20B2AA;\">MyPoetry</span>!</strong></h1>" +
                    "<h2>The first app dedicated to poetry.</h2>" +
                    "<hr/>" +
                    "<p>Enter this activation&nbsp;code in&nbsp;the application to start using it immediately: <strong>" + newUser.ActivationCode + "</strong>.</p>" +
                    "<p>Start now to...</p>" +
                    "<ul>" +
                    "<li>Write poetry</li>" +
                    "<li>Use specific writing tools</li>" +
                    "<li>Share your texts</li>" +
                    "<li>Track your progress</li>" +
                    "<li>...And so much more </li>" +
                    "</ul>" +
                    "<p><strong>What are you waiting for?</strong></p>" +
                    "<p>&nbsp;</p>" +
                    "<p><em>This is an email generated automatically by MyPoetry. Do not respond to this email address.</em></p>",
                    true);

                // Return the success response.
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
        }
    }
}