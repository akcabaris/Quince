namespace ApplicationLLA.Server.Helper
{
    public class PostQueryObject
    {
        public string? City { get; set; }
        public string? County { get; set; }
        public string? Category { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
