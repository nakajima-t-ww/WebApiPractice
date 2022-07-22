using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIPractice.Models;

namespace APIPractice.Data
{
    public class APIPracticeContext : DbContext
    {
        public APIPracticeContext (DbContextOptions<APIPracticeContext> options)
            : base(options)
        {
        }

        public DbSet<APIPractice.Models.Movie> Movie { get; set; } = default!;
    }
}
