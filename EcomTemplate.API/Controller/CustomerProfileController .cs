using Microsoft.AspNetCore.Mvc;
using GrocerySupermarket.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet;
// using GrocerySupermarket.Application.Services;


[ApiController]
[Authorize]
[Route("api/customer/profile")]
public class CustomerProfileController : ControllerBase
{
    private readonly CustomerProfileService _service;

    private readonly Cloudinary _cloudinary;

    public CustomerProfileController(CustomerProfileService service, Cloudinary cloudinary)
    {
        _service = service;
        _cloudinary = cloudinary;
    }

    // 🔹 GET profile
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var customerId = GetCustomerId(); // from JWT / API key
        var profile = await _service.GetProfileAsync(customerId);

        if (profile == null)
            return NotFound();

        return Ok(profile);
    }

    // 🔹 CREATE / UPDATE profile
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateCustomerProfileDTO dto)
    {
        var customerId = GetCustomerId();
        var profile = await _service.CreateOrUpdateAsync(customerId, dto);
        return Ok(profile);
    }

    // 🔹 DELETE profile (rare but needed)
    [HttpDelete]
    public async Task<IActionResult> DeleteProfile()
    {
        var customerId = GetCustomerId();
        await _service.DeleteAsync(customerId);
        return NoContent();
    }

    private Guid GetCustomerId()
{
    var customerIdClaim = User.FindFirst("customerId");

    if (customerIdClaim == null)
        throw new UnauthorizedAccessException("Customer is not authenticated.");

    return Guid.Parse(customerIdClaim.Value);
}


[Authorize]
[HttpPost("profile/image/signature")]
public IActionResult GetProfileImageUploadSignature()
{
    var customerId = GetCustomerId();

    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    // 🔒 BACKEND CONTROLS FOLDER
    var uploadParams = new SortedDictionary<string, object>
    {
        { "folder", "customers_folder" }, // ✅ FIXED FOLDER
        { "timestamp", timestamp },
        { "public_id", customerId.ToString() },
        { "overwrite", true },
        { "resource_type", "image" },
        { "allowed_formats", "jpg,jpeg,png,webp" }
    };

    var signature = _cloudinary.Api.SignParameters(uploadParams);

    return Ok(new
    {
        signature,
        timestamp,
        folder = "Sanvarich_Customers_Profile_Pictures",
        maxFileSize = 5 * 1024 * 1024 // 5MB
    });
}


[Authorize]
[HttpPut("profile/image")]
public async Task<IActionResult> SaveProfileImageUrl(
    [FromBody] string imageUrl)
{
    if (string.IsNullOrWhiteSpace(imageUrl))
        return BadRequest("Invalid image URL");

    // 🔐 Cloudinary domain check
    if (!imageUrl.Contains("res.cloudinary.com"))
        return BadRequest("Untrusted image source");

    // 🔐 Folder enforcement
    if (!imageUrl.Contains("/Sanvarich_Customers_Profile_Pictures/"))
        return BadRequest("Invalid upload folder");

    var customerId = GetCustomerId();

    await _service.UpdateProfileImageAsync(customerId, imageUrl);

    return Ok(new { profileImageUrl = imageUrl });
}





}
