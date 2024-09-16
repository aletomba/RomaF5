namespace RomaF5.Models
{
    public class PginationModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public int ItemsInPage { get; set; }
        public int ItemsInPreviousPage { get; set; }
        public int ItemsInNextPage { get; set; }
    }
}
