﻿using DatingAPI.Data;
using DatingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DatingAPI.Controllers
{
    public class UsersController : BaseAPIController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        [Authorize]
        [HttpGet("id")]
        public async Task<ActionResult<AppUser>> GetUser(int id) => await _context.Users.FindAsync(id);

    }
}
