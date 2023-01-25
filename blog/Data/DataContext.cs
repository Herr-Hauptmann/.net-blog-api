using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace blog.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Post> Posts {get;set;}
        public DbSet<Comment> Comments {get;set;}
        public DbSet<Tag> Tags {get;set;}
    }
}