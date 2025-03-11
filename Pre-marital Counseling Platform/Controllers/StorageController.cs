using FirebaseAdmin;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class StorageController : ControllerBase
{
    private readonly FirebaseStorage _firebaseStorage;

    public StorageController()
    {
        _firebaseStorage = new FirebaseStorage("student-51e6a.appspot.com");
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { Error = "No file uploaded" });
        }

        try
        {
            var currentUser = HttpContext.User;
            var userEmail = currentUser.FindFirst("Email")?.Value;

            var fileName = $"{userEmail}_{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                var task = _firebaseStorage
                    .Child("images")
                    .Child(fileName)
                    .PutAsync(stream);

                string downloadUrl = await task;
                return Ok(new { Url = downloadUrl, Message = "Image uploaded successfully" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadImage(string fileName)
    {
        try
        {
            var url = await _firebaseStorage
                .Child("images")
                .Child(fileName)
                .GetDownloadUrlAsync();

            return Ok(new { Url = url });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}