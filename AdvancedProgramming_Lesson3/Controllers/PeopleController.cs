using AdvancedProgramming_Lesson3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedProgramming_Lesson3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PersonContext _context;

        public PeopleController(PersonContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPeople()
        {
            return await _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDTO>> GetPerson(long id)
        {
            var person = await _context.TodoItems.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return ItemToDTO(person);
        }

        [HttpPost]
        [Route("UpdatePersonItem")]
        public async Task<ActionResult<PersonDTO>> UpdatePerson(PersonDTO personDTO)
        {
            var person = await _context.TodoItems.FindAsync(personDTO.Id);
            if (person == null)
            {
                return NotFound();
            }
            person.FirstName = personDTO.FirstName;
            person.LastName = personDTO.LastName;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PersonExists(personDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdatePerson),
                new { id = personDTO.Id },
                ItemToDTO(person));
        }

        [HttpPost]
        [Route("CreatePersonItem")]
        public async Task<ActionResult<PersonDTO>> CreatePerson(PersonDTO personDTO)
        {
            var personItem = new Person
            {
                FirstName = personDTO.FirstName,
                LastName = personDTO.LastName
            };

            _context.TodoItems.Add(personItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPerson),
                new { id = personItem.Id },
                ItemToDTO(personItem));
        }


        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePerson(long id)
        {
            var personItem = await _context.TodoItems.FindAsync(id);
            if (personItem == null)
            {
                return NotFound();
            }
            _context.TodoItems.Remove(personItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PersonExists(long id) =>
           _context.TodoItems.Any(e => e.Id == id);

        private static PersonDTO ItemToDTO(Person person) =>
            new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName
            };
    }


}
