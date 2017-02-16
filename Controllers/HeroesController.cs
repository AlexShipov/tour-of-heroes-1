using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using tour_of_heroes_1.Controllers.Helpers;
using tour_of_heroes_1.Models;

namespace tour_of_heroes_1.Controllers
{
    [Route("api/[controller]")]
    public class HeroesController: Controller
    {
        private static object _lock = new object();

        private static Dictionary<int, Hero> _fakeDb;
        static HeroesController()
        {
            var data = new[]{
            new Hero { Id= 11, Name= "Mr. Nice" },
            new Hero { Id= 12, Name= "Narco" },
            new Hero { Id= 13, Name= "Bombasto" },
            new Hero { Id= 14, Name= "Celeritas" },
            new Hero { Id= 15, Name= "Magneta" },
            new Hero { Id= 16, Name= "RubberMan" },
            new Hero { Id= 17, Name= "Dynama" },
            new Hero { Id= 18, Name= "Dr IQ" },
            new Hero { Id= 19, Name= "Magma" },
            new Hero { Id= 20, Name= "Tornado" }
            };

            _fakeDb = data.ToDictionary(hero => hero.Id);
        }

        // GET api/values/search
        [HttpGet("{id}")]        
        public IActionResult Get(int id)
        {
            if (_fakeDb.TryGetValue(id, out var retVal))
            {
                return Ok(new ApiResult<Hero> { Data = retVal });
            }
            else
            {
                return NotFound("No records match");
            }
        }
        // GET api/values
        [HttpGet]        
        public IActionResult Get([FromQuery]HeroSearch search)
        {
            ApiResult<IEnumerable<Hero>> data;

            if (search != null && search.Name != null)
            {
                var matched = _fakeDb.Where(item =>
                    item.Value.Name.IndexOf(search.Name,
                    StringComparison.CurrentCultureIgnoreCase) != -1)
                    .Select(item => item.Value)
                    .ToList();
                data = new ApiResult<IEnumerable<Hero>>
                {
                    Data = matched
                };
            }
            else
            {
                data = new ApiResult<IEnumerable<Hero>>
                {
                    Data = _fakeDb.Values.ToList()
                };
            }

            return Ok(data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Hero value)
        {
            if (_fakeDb.ContainsKey(id))
            {
                _fakeDb[id] = value;
                return Ok(value);
            }
            else
            {
                return NotFound("update failed no match");
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {            
            if (_fakeDb.ContainsKey(id))
            {
                _fakeDb.Remove(id);
                return NoContent();
            }
            else
            {
                return NotFound("delete failed no match");
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Hero value)
        {
            lock(_lock)
            {
                var exists = _fakeDb.Any(item => string.Equals(item.Value.Name, value.Name, StringComparison.CurrentCultureIgnoreCase));
                if(!exists)
                {
                    var nextId = _fakeDb.Keys.Any() ? _fakeDb.Keys.Max() + 1 : 1;
                    value.Id = nextId;
                    _fakeDb.Add(value.Id, value);
                    var newPath = $"{Request.Path.Value}/{nextId}";
                    return Created(newPath, new ApiResult<Hero> { Data = value });
                }
            }

            // if got here there is a conflict
            return this.Conflict("item already exists");
        }
    }
}
