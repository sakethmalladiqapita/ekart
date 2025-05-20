namespace ekart.Models
{
    // Request model for paginated API endpoints
    public class PaginationQuery
    {
        public int PageNumber { get; set; } = 1; // Current page (default: 1)
        public int PageSize { get; set; } = 10;   // Items per page (default: 10)
    }
}
