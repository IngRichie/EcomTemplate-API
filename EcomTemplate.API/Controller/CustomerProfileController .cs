using Microsoft.AspNetCore.Mvc;
using GrocerySupermarket.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet;
using EcomTemplate.API.HelperFunctions;


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
        var customerId = UserHelper.GetUserId(User);
        Console.WriteLine("This is the customer id: ", customerId);
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
        var customerId = UserHelper.GetUserId(User);
        var profile = await _service.CreateOrUpdateAsync(customerId, dto);
        return Ok(profile);
    }

    // 🔹 DELETE profile (rare but needed)
    [HttpDelete]
    public async Task<IActionResult> DeleteProfile()
    {
        var customerId = UserHelper.GetUserId(User);
        await _service.DeleteAsync(customerId);
        return NoContent();
    }




[Authorize]
[HttpPost("profile/image/signature")]
public IActionResult GetProfileImageUploadSignature()
{
    var customerId = UserHelper.GetUserId(User);

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
        folder = "EcomTemplate_Customers_Profile_Pictures",
        maxFileSize = 5 * 1024 * 1024 // 5MB
    });
}


[Authorize]
[HttpPut("profile/image")]
public async Task<IActionResult> SaveProfileImageUrl(
    [FromBody] string imageUrl)
{
 

    var customerId = UserHelper.GetUserId(User);

    await _service.UpdateProfileImageAsync(customerId, imageUrl);

    return Ok(new { profileImageUrl = imageUrl });
}





}
