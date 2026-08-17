using GrocerySupermarket.Application.DTOs.ContactMessage;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.API.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactMessagesController : ControllerBase
{
    private readonly IContactMessageRepository _repository;

    public ContactMessagesController(
        IContactMessageRepository repository)
    {
        _repository = repository;
    }

    // =====================================
    // CREATE CONTACT MESSAGE
    // =====================================

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateContactMessageDTO dto)
    {
        var contact = new ContactMessage
        {
            ContactMessageId = Guid.NewGuid(),

            FullName = dto.FullName,

            Email = dto.Email,

            PhoneNumber = dto.PhoneNumber,

            Subject = dto.Subject,

            Message = dto.Message,

            IsRead = false,

            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(contact);

        return Ok(new
        {
            message = "Message sent successfully."
        });
    }

    // =====================================
    // GET ALL CONTACT MESSAGES
    // =====================================

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactMessageDTO>>> GetAll()
    {
        var contacts = await _repository.GetAllAsync();

        var result = contacts.Select(x => new ContactMessageDTO
        {
            ContactMessageId = x.ContactMessageId,

            FullName = x.FullName,

            Email = x.Email,

            PhoneNumber = x.PhoneNumber,

            Subject = x.Subject,

            Message = x.Message,

            IsRead = x.IsRead,

            CreatedAt = x.CreatedAt
        });

        return Ok(result);
    }

    // =====================================
    // DELETE CONTACT MESSAGE
    // =====================================

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Message not found."
            });
        }

        return Ok(new
        {
            message = "Message deleted successfully."
        });
    }
}