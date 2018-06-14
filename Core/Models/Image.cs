using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace Core.Models
{
    public class MediaRecordDatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite("Data Source=../../../../Core/media_record.db");
            optionsBuilder.UseSqlite("Data Source=C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/media_record.db");
        }
        public DbSet<Image> Image { get; set; }
    }
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public long MD5 { get; set; }
        [Required]
        public string BlobName { get; set; }
    }
}
