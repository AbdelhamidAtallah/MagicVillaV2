using MagicVilla_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id=1,
                    Name="maison",
                    Description = "ocean no view",
                    Superficie = 200,
                    NbRoom = 20,
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/09/17/18/15/lost-places-2759275_960_720.jpg",
                    Rate = 5,
                    Created_date = DateTime.Now
                    
                },
                new Villa()
                {
                    Id = 2,
                    Name = "dar",
                    Description = "view no ocean",
                    Superficie = 600,
                    NbRoom = 5,
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/04/10/22/28/residence-2219972_960_720.jpg",
                    Rate = 5,
                    Created_date = DateTime.Now
                }
                );
            

               
                
        }

    }
}
