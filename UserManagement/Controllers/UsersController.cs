using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagement.Models;
using UserManagement.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ILogger<UsersController> _logger;
        private IHttpContextAccessor _httpContextAccessor;

        public UsersController(ILogger<UsersController> logger, UserContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            if (!_context.Users.Any())
            {
                //for demo purposes:
                //------------------------------------------------------------------
                var user1 = new User
                {
                    Id = 1,
                    RoleId = 1,
                    FirstName = "Steven",
                    LastName = "Belcher",
                    EmailAddress = "sc.belcher@yahoo.com",
                    StatusId = 1,
                    CreateDate = DateTime.Parse("8/1/2020")
                };

                var user2 = new User
                {
                    Id = 2,
                    RoleId = 2,
                    FirstName = "John",
                    LastName = "Public",
                    EmailAddress = "jq.public@yahoo.com",
                    StatusId = 1,
                    CreateDate = DateTime.Parse("8/13/2020")
                };

                _context.Users.Add(user1);
                _context.Users.Add(user2);
                await _context.SaveChangesAsync();
                //------------------------------------------------------------------
            }

            return await _context.Users.Select(u => ConvertUser(u)).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return ConvertUser(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO userDTO)
        {
            //Use Identity Role to check if logged user is a superuser
            //Can wire up the AUTHORIZE attribute or custom check using the HttpClient object

            //Other option is to Use HttpContextAccessor
            //if (!_httpContextAccessor.HttpContext.User.IsInRole("SuperUser"))
            //{
            //    _logger.LogWarning("Hack Attempt");
            //    return BadRequest();     
            //}
            //Also need to validate that user is superuser for the employer

            if (id != userDTO.Id)
            {
                _logger.LogWarning("User Mismatch");
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.RoleId = userDTO.RoleId;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.EmailAddress = userDTO.EmailAddress;
            user.StatusId = userDTO.StatusId;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("Concurrency Error while updating user");

                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        //[Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userDTO)
        {
            //Use Identity Role to check if logged user is a superuser
            //Can wire up the AUTHORIZE attribute or custom check using the HttpClient object
            //if (currentUser.Role != superUser)
            //{
            //    _logger.LogWarning("Hack Attempt");
            //    return BadRequest();     
            //}

            var user = new User
            {
                RoleId = userDTO.RoleId,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                EmailAddress = userDTO.EmailAddress,
                StatusId = userDTO.StatusId,
                PasswordHash = HashPass(userDTO.Password) //depending on methods, password may be handled separately
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Data Access Error while creating user");
                throw;
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            //Use Identity Role to check if logged user is a superuser
            //Can wire up the AUTHORIZE attribute or custom check using the HttpClient object
            //if (currentUser.Role != superUser)
            //{
            //    _logger.LogWarning("Hack Attempt");
            //    return BadRequest();     
            //}

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Error while deleting user");
                throw;
            }

            return NoContent();
        }

        //Other methods that we will need for functional requirements

        // POST: api/Users/employ
        [HttpPost("employ")]
        //[Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<EmploymentDTO>> EmployUser(EmploymentDTO employmentDTO)
        {
            //Use Identity Role to check if logged user is a superuser
            //Can wire up the AUTHORIZE attribute or custom check using the HttpClient object
            //if (currentUser.Role != superUser)
            //{
            //    _logger.LogWarning("Hack Attempt");
            //    return BadRequest();     
            //}

            var employment = new Employment
            {
                UserId = employmentDTO.UserId,
                EmployerId = employmentDTO.EmployerId,
                IsSuperUser = employmentDTO.IsSuperUser,
                AssignmentDate = DateTime.UtcNow
            };

            try
            {
                _context.Employments.Add(employment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while Employing a User");
                throw;
            }

            return CreatedAtAction("EmployUser", new { id = employment.Id }, ConvertEmployment(employment));
        }

        // POST: api/Users/assignproduct
        [HttpPost("assignproduct")]
        //[Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<AssignedProductDTO>> ProvideAccess(AssignedProductDTO productDTO)
        {
            //Use Identity Role to check if logged user is a superuser
            //Can wire up the AUTHORIZE attribute or custom check using the HttpClient object
            //if (currentUser.Role != superUser)
            //{
            //    _logger.LogWarning("Hack Attempt");
            //    return BadRequest();     
            //}

            var product = await _context.ProductLicenses
                .FirstOrDefaultAsync(p => p.ProductId == productDTO.ProductId && p.EmployerId == productDTO.EmployerId && p.LicenseTypeId == productDTO.LicenseTypeId);
            if (product == null)
            {
                return NotFound();
            }

            var assigned = new AssignedProduct
            {
                Product = product,
                UserId = productDTO.UserId,
                AssignmentDate = DateTime.UtcNow
            };

            try
            {
                _context.AssignedProducts.Add(assigned);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred Assigning a product");
                throw;
            }

            return CreatedAtAction("ProvideAccess", new { id = assigned.Id }, ConvertAssignment(assigned));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private static UserDTO ConvertUser(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                RoleId = user.RoleId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                StatusId = user.StatusId
            };
        }

        private static EmploymentDTO ConvertEmployment(Employment employment)
        {
            return new EmploymentDTO
            {
                UserId = employment.UserId,
                EmployerId = employment.EmployerId,
                IsSuperUser = employment.IsSuperUser,
                AssignmentDate = employment.AssignmentDate,
            };
        }

        private static AssignedProductDTO ConvertAssignment(AssignedProduct assignment)
        {
            return new AssignedProductDTO
            {
                ProductId = assignment.Product.ProductId,
                EmployerId = assignment.Product.EmployerId,
                LicenseTypeId = assignment.Product.LicenseTypeId,
                MinimumRoleId = assignment.Product.MinimumRoleId,
                StartDate = assignment.Product.StartDate,
                EndDate = assignment.Product.EndDate,
                UserId = assignment.UserId,
                AssignmentDate = assignment.AssignmentDate
            };
        }

        private string HashPass(string password)
        {
            string hash = "";

            //convert password to Hash via Identity setup

            return hash;
        }
    }
}
