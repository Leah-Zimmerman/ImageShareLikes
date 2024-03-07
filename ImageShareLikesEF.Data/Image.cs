namespace ImageShareLikesEF.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public int Views { get; set; }
    }
}