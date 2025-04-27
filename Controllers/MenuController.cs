using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FoodDeliveryAdminWebsite.Models;
using System;

namespace FoodDeliveryAdminWebsite.Controllers
{
    [Route("api/restaurants/{restaurantId}/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/restaurants/5/menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems(int restaurantId)
        {
            return await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId)
                .ToListAsync();
        }

        // GET: api/restaurants/5/menu/2
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int restaurantId, int id)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id && m.RestaurantId == restaurantId);

            if (menuItem == null)
            {
                return NotFound();
            }

            return menuItem;
        }

        // POST: api/restaurants/5/menu
        [HttpPost]
        public async Task<ActionResult<MenuItem>> PostMenuItem(int restaurantId, MenuItem menuItem)
        {
            if (menuItem.RestaurantId != restaurantId)
            {
                return BadRequest();
            }

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenuItem", new { restaurantId, id = menuItem.Id }, menuItem);
        }

        // PUT: api/restaurants/5/menu/2
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenuItem(int restaurantId, int id, MenuItem menuItem)
        {
            if (id != menuItem.Id || restaurantId != menuItem.RestaurantId)
            {
                return BadRequest();
            }

            _context.Entry(menuItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuItemExists(id))
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

        // DELETE: api/restaurants/5/menu/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int restaurantId, int id)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id && m.RestaurantId == restaurantId);

            if (menuItem == null)
            {
                return NotFound();
            }

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}