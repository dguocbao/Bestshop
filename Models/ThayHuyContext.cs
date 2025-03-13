using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Bestshop.Models
{
    public partial class ThayHuyContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ThayHuyContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ThayHuyContext(DbContextOptions<ThayHuyContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
        }

        

        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<PasswordReset> PasswordResets { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=LAPTOP-37HO9028;Initial Catalog=ThayHuy;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
//            }
//        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("ThayHuyDatabase");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("books");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Authors)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("authors");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("category");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.ImageFilename)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("image_filename");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("isbn");

                entity.Property(e => e.NumPages).HasColumnName("num_pages");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Message1)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Subject)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("subject");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("delivery_address");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("order_status");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("payment_method");

                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("payment_status");

                entity.Property(e => e.ShippingFee)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("shipping_fee");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("unit_price");
            });

            modelBuilder.Entity<PasswordReset>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__password__AB6E616598CB5BAD");

                entity.ToTable("password_resets");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("token");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "UQ__users__AB6E6164C51EC31A")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
