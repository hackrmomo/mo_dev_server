namespace MoDev.Entities
{
    class Photograph
    {
        public int Id { get; set; }
        public string Iso { get; set; }
        public string Aperture { get; set; }
        public string FocalLength { get; set; }
        public string ShutterSpeed { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
        public string ImageUri { get; set; }
    }
}