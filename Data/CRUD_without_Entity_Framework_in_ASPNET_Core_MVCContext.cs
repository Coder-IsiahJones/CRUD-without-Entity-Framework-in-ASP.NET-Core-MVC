using Microsoft.EntityFrameworkCore;

namespace CRUD_without_Entity_Framework_in_ASP.NET_Core_MVC.Data
{
    public class CRUD_without_Entity_Framework_in_ASPNET_Core_MVCContext : DbContext
    {
        public CRUD_without_Entity_Framework_in_ASPNET_Core_MVCContext(DbContextOptions<CRUD_without_Entity_Framework_in_ASPNET_Core_MVCContext> options)
            : base(options)
        {
        }

        public DbSet<CRUD_without_Entity_Framework_in_ASP.NET_Core_MVC.Models.BookVM> BookVM { get; set; }
    }
}