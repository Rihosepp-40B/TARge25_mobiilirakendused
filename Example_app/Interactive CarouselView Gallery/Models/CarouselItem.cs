namespace Interactive_CarouselView_Gallery.Models
{
    public class CarouselItem
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string? ShortSummary { get; set; }
        public List<string>? Ingredients { get; set; }
        //public int? PrepTimeMinutes { get; set; }
        //public int CookTimeMinutes { get; set; }
        //public List<Step>? DirectionSteps { get; set; } = new List<Step>();
    }
}
