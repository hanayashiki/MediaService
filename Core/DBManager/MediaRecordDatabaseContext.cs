using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.DBManager
{
    [Obsolete]
    public class MediaRecordDatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite("Data Source=../../../../Core/media_record.db");
            optionsBuilder.UseSqlite("Data Source=C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/media_record.db");
        }
        public DbSet<Image> Image { get; set; }
    }
}