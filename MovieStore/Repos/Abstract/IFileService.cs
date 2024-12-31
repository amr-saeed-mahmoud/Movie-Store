namespace MovieStore.Repos.Abstract;

public interface IFileService
{
    Tuple<int, string> SaveImage(IFormFile ImageFile);
    bool DeleteImage(string ImageFileName);
}