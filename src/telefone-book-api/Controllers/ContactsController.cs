using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using telefone_book_api.Data;
using telefone_book_api.Models;
using telefone_book_api.Models.Dto;

namespace telefone_book_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly TelephoneBookDbContext _dbContext;
    private readonly IMapper _mapper;

    public ContactsController(TelephoneBookDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        string? currentUser = User.Identity?.Name;
        if (currentUser == null)
        {
            return BadRequest("Current user is undefined");
        }
        return Ok(await _dbContext.Contacts.Where(c => c.CurrentUser == currentUser).AsNoTracking().ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        string? currentUser = User.Identity?.Name;
        if (currentUser == null)
        {
            return BadRequest("Current user is undefined");
        }
        var contact = await _dbContext.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && c.CurrentUser == currentUser);

        if (contact == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ContactDto>(contact));
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContactDto contact)
    {
        string? currentUser = User.Identity?.Name;
        if (currentUser == null)
        {
            return BadRequest("Current user is undefined");
        }
        var contactToCreate = _mapper.Map<Contact>(contact);
        contactToCreate.CurrentUser = currentUser;

        _dbContext.Contacts.Add(contactToCreate);
        await _dbContext.SaveChangesAsync();

        var createdContact = _mapper.Map<Contact>(contactToCreate);

        return CreatedAtAction(nameof(Create), new { id = contactToCreate.Id }, contactToCreate);
    }


    [HttpPost("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, ContactDto newContact)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(newContact.CellPhoneNumber))
        {
            contact.CellPhoneNumber = newContact.CellPhoneNumber;
        }

        if (!string.IsNullOrEmpty(newContact.HomePhoneNumber))
        {
            contact.HomePhoneNumber = newContact.HomePhoneNumber;
        }

        if (!string.IsNullOrEmpty(newContact.Name))
        {
            contact.Name = newContact.Name;
        }

        if (!string.IsNullOrEmpty(newContact.Surname))
        {
            contact.Surname = newContact.Surname;
        }

        if (!string.IsNullOrEmpty(newContact.Email))
        {
            contact.Email = newContact.Email;
        }
        await _dbContext.SaveChangesAsync();

        return Ok(_mapper.Map<ContactDto>(contact));
    }

    [HttpDelete("{contactId}")]
    public async Task<IActionResult> Delete(int contactId)
    {
        var contactToDelete = await _dbContext.Contacts.FindAsync(contactId);
        if (contactToDelete == null)
            return NotFound();
        _dbContext.Remove(contactToDelete);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}
