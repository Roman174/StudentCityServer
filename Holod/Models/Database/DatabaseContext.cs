﻿using Holod.Models.Database.Configs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Holod.Models.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<StudentCity> StudentCities { get; set; }
        public DbSet<Hostel> Hostels { get; set; }
        public DbSet<Stuff> Stuffs { get; set; }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new HostelConfig());
        }
    }
}