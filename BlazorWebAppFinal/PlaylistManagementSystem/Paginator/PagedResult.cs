using System.Collections.Generic;
using System.ComponentModel;


namespace PlaylistManagementSystem.Paginator
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public T[] Results { get; set; }
    }
}