using System.Collections.Generic;
namespace ToDoApp.Models
{
    public class PagedResponse<T>
    {
        public List<T>? Data { get; set; }
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}
