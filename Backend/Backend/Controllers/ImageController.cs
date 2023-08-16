using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;
[ApiController]
[Route("images")]
public class ImageController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ImageController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpGet("{filename}")]
    public IActionResult GetImage(string filename)
    {
        var imagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "images", filename);

        if (!System.IO.File.Exists(imagePath))
        {
            return NotFound($"{imagePath}");
        }

        var image = System.IO.File.OpenRead(imagePath);

        return File(image, "image/jpeg");
    }
}
