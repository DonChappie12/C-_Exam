using System;
using Microsoft.EntityFrameworkCore;

namespace c__exam.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> user { get; set; }
        public DbSet<Activities> activity { get; set; }
        public DbSet<Join> join { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    }
}