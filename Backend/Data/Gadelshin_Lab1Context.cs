using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gadelshin_Lab1.Models;

namespace Gadelshin_Lab1.Data
{
    public class Gadelshin_Lab1Context : DbContext
    {
        public Gadelshin_Lab1Context (DbContextOptions<Gadelshin_Lab1Context> options)
            : base(options)
        {
        }

        public DbSet<Gadelshin_Lab1.Models.Book> Book { get; set; } = default!;
        public DbSet<Gadelshin_Lab1.Models.Author> Author { get; set; } = default!;
        public DbSet<Gadelshin_Lab1.Models.User> User { get; set; } = default!;
    }
}
