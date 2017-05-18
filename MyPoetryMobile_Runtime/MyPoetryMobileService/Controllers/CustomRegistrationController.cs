using MyPoetryMobileService.DataObjects;
using MyPoetryMobileService.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyPoetryMobileService.Controllers
{
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
            if (registrationRequest.Password.Length < 8)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Invalid password (at least 8 chars required)");
            }
            else if (!registrationRequest.Email.Contains("@"))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Please supply a valid email address");
            }
            
            // SQL Database version using Entity Framework for data access.
            MyPoetryMobileContext context = new MyPoetryMobileContext(connectionStringName);
            User account = context.User.Where(a => a.Email == registrationRequest.Email).SingleOrDefault();
            
            // If there's already a confirmed account, return an error response.
            if (account != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    "That username already exists.");
            }
            else
            {
                byte[] salt = CustomLoginProviderUtils.GenerateSalt();

                User newUser = new Models.User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = registrationRequest.Email,
                    Salt = salt,
                    SaltedAndHashedPassword = CustomLoginProviderUtils.Hash(registrationRequest.Password, salt),
                    Name = registrationRequest.Name,
                    Surname = registrationRequest.Surname,
                    Gender = registrationRequest.Gender,
                    Photo = registrationRequest.Photo,
                    RegistrationDate = registrationRequest.RegistrationDate,
                    AccessesNumber = 0,
                    UseTime = 0
                };

                context.User.Add(newUser);
                context.SaveChanges();

                // Return the success response.
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
        }
    }
}