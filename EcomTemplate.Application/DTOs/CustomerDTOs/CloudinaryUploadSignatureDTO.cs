
namespace GrocerySupermarket.Application.DTOs;
public class CloudinaryUploadSignatureDTO
{
    public string Signature { get; set; } = string.Empty;
    public long Timestamp { get; set; }
    public string Folder { get; set; } = string.Empty;
    public long MaxFileSize { get; set; }
}
