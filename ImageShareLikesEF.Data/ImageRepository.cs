using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ImageShareLikesEF.Data
{
    public class ImageRepository
    {
        private string _connectionString;
        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddImage(string imagePath, string fileName, string title)
        {
            var context = new ImageDbContext(_connectionString);
            var image = new Image
            {
                ImagePath = imagePath,
                FileName = fileName,
                Title = title,
                Views = 0
            };
            context.Images.Add(image);
            context.SaveChanges();
        }
        public List<Image>GetImages()
        {
            var context = new ImageDbContext(_connectionString);
            var images = context.Images.ToList();
            return images;
        }
        public Image GetImageById(int id)
        {
            var context = new ImageDbContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }
        public void IncreaseViews(int id)
        {
            var context = new ImageDbContext(_connectionString);
            var imageViews = context.Images.FirstOrDefault(i => i.Id == id).Views;
            imageViews++;
            context.Images.FirstOrDefault(i => i.Id == id).Views = imageViews;
            context.SaveChanges();

        }
    }
}