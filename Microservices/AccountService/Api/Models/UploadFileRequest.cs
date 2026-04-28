
namespace Ecosystem.AccountService.Api.Models;

public class UploadFileRequest
{
    public IFormFile File { get; set; } = null!;
    public string Folder { get; set; } = null!;
    public string? FileName { get; set; }
}

