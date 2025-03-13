using System;
using System.Collections.Generic;

namespace Bestshop.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Authors { get; set; } = null!;
        public string Isbn { get; set; } = null!;
        public int NumPages { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageFilename { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
