using DatingAPI.Data;
using DatingAPI.DTOs;
using DatingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingAPI.Controllers
{
    public class AccountController: BaseAPIController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<AppUser>> Register(UserDTO userDto)
        {   
            if(await UserExists(userDto.userName))
            {
                return BadRequest("User already exist");
            }
            else
            {
                using var hmac = new HMACSHA512();
                var user = new AppUser
                {
                    UserName = userDto.userName.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.password)),
                    PasswordSalt = hmac.Key
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult<AppUser>> Login(UserDTO userDto)
        {
            var user =await _context.Users.SingleOrDefaultAsync(user => user.UserName == userDto.userName);
            if(user == null) { return Unauthorized("User do not exist"); }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var hashedResult = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.password));
            for(int i=0; i<hashedResult.Length; i++)
            {
                if (hashedResult[i] != user.PasswordHash[i])
                    return Unauthorized("Incorrect Password");
            }
            return Ok(user);
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.UserName.ToLower() == username.ToLower());
        }

    }
}
