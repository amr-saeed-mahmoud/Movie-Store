using System.ComponentModel;
using MovieStore.Repos.Abstract;

namespace MovieStore.Repos.Implemention;

public class FileService : IFileService
{

    private readonly IWebHostEnvironment _Environment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _Environment = webHostEnvironment;
    }

    public bool DeleteImage(string ImageFileName)
    {
        try
        {
            string UploadsPath = EnsureUploadDirectoryExists();
            string ImagePath = Path.Combine(UploadsPath, ImageFileName);
            if(File.Exists(ImagePath))
            {
                File.Delete(ImageFileName);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public Tuple<int, string> SaveImage(IFormFile ImageFile)
    {
        try
        {
            // check Uploads folder is exists if not create it
            string UploadsPath = EnsureUploadDirectoryExists();
            // Check the allowed extenstions
            var result = IsAllowedExtension(ImageFile);
            if(result.Item1 == 0)
            {
                return result; 
            }
            // create unique image name
            string UniqueName = Guid.NewGuid().ToString();
            string NewImageName = UniqueName + Path.GetExtension(ImageFile.FileName);
            string NewImagePath = Path.Combine(UploadsPath, NewImageName);

            using(var stream = new FileStream(NewImagePath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }
            return new Tuple<int, string>(1, NewImageName);
        }
        catch
        {
            return new Tuple<int, string>(0, "Error has occured");
        }
    }

    private string EnsureUploadDirectoryExists()
    {
        string wwwrootPath = _Environment.WebRootPath;
        string UploadsPath = Path.Combine(wwwrootPath, "Upload");
        if(!Directory.Exists(UploadsPath))
        {
            Directory.CreateDirectory(UploadsPath);
        }
        return UploadsPath;
    }

    private Tuple<int, string> IsAllowedExtension(IFormFile ImageFile)
    {

        string ImageExtension = Path.GetExtension(ImageFile.FileName);
        var AllowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
        string Message = "Successful Process";
        if(!AllowedExtensions.Contains(ImageExtension))
        {
            Message = string.Format("Only {0} extensions are allowed", string.Join(",", AllowedExtensions));
            return new Tuple<int, string>(0, Message);
        }
        return new Tuple<int, string>(1, Message);
    }

}