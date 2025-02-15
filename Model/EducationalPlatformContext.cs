using System;
using System.Collections.Generic;
using Api_1.Entity.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api_1;

public partial class EducationalPlatformContext : IdentityDbContext<Student , UserRole , string>
{
    public EducationalPlatformContext()
    {
    }

    public EducationalPlatformContext(DbContextOptions<EducationalPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Regestration> Regestrations { get; set; }

    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<UserRole> AspNeRoles { get; set; }


    //public DbSet<IdentityUser> AspNetUsers { get; set; }
   // public DbSet<IdentityRole> AspNetRoles { get; set; }
    public DbSet<IdentityUserRole<string>> AspNetUserRoles { get; set; }
    public DbSet<IdentityUserClaim<string>> AspNetUserClaims { get; set; }
    public DbSet<IdentityUserLogin<string>> AspNetUserLogins { get; set; }
    public DbSet<IdentityRoleClaim<string>> AspNetRoleClaims { get; set; }
    public DbSet<IdentityUserToken<string>> AspNetUserTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = DESKTOP-GNTBPMJ ; Database = EducationalPlatform_API ; Integrated Security = SSPI ; TrustServerCertificate = True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admins__3214EC0755447CE2");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0777F78313");

            entity.Property(e => e.CatName).HasMaxLength(50);
           
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC07056A0E8A");

            entity.Property(e => e.CateId).HasColumnName("cate_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CourseName).HasMaxLength(50);
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Hours).HasColumnName("hours");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            //entity.Property(e => e.IsDelete)
            //    .HasMaxLength(10)
            //    .HasDefaultValue("foles")
            //    .IsFixedLength();
            entity.Property(e => e.Level)
                .HasMaxLength(50)
                .HasColumnName("level");
            entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Cate).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Courses__cate_id__1920BF5C");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Courses__instruc__1A14E395");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__instruct__3214EC0774467063");

            entity.ToTable("instructor");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
           
            entity.Property(e => e.JoinDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC077B3D4884");

            entity.ToTable("Material");

            entity.Property(e => e.CoursId).HasColumnName("cours_id");
            entity.Property(e => e.FilePath).HasColumnName("file_path");
            entity.Property(e => e.LecuerNumber)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.Cours).WithMany(p => p.Materials)
                .HasForeignKey(d => d.CoursId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Material__cours___1CF15040");
        });

       

        modelBuilder.Entity<Regestration>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId }).HasName("PK__Regestra__5A06028E7BAC8DD1");

            entity.ToTable("Regestration");

            entity.Property(e => e.StudentId).HasColumnName("Student_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.StarTdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("star_tdate");

            entity.HasOne(d => d.Course).WithMany(p => p.Regestrations)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Regestrat__cours__22AA2996");

            entity.HasOne(d => d.Student).WithMany(p => p.Regestrations)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Regestrat__Stude__21B6055D");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC07CEC34D09");

            //entity.Property(e => e.StudentId).HasColumnName("studentID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            
            entity.Property(e => e.JoinDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Phone).HasMaxLength(50);

            entity
            .OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

            //Add IdentityUser Roles to the model Admin

            var passwordHasher = new PasswordHasher<Student>();

            entity.HasData(new Student
            {
                Id = DefaultUsers.AdminId,
                Gender = "male",
                FullName = "Admin",
                UserName = DefaultUsers.AdminEmail,
                Email = DefaultUsers.AdminEmail,
                NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
                NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                Password = DefaultUsers.AdminPassword,
                SecurityStamp = DefaultUsers.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
            });



        });

        

        modelBuilder.Entity<UserRole>(entity =>
        {
            
            entity.ToTable("AspNetRoles");
            
            entity.Property(e => e.Id).HasMaxLength(450);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(256);
            entity.HasKey(e => e.Id);
            entity.HasData([
               new UserRole
               {
                    Id = DefaultRoles.AdminRoleId,
                    Name = DefaultRoles.Admin,
                    NormalizedName = DefaultRoles.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp ,
                    IsDefault = false,
                    IsDeleted = false

               },
               new UserRole
               {
                    Id = DefaultRoles.MemberRoleId,
                    Name = DefaultRoles.Member,
                    NormalizedName = DefaultRoles.Member.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.MemberRoleConcurrencyStamp,
                    IsDefault = true,
                    IsDeleted = false
               }
            ]);
        });

        // Configure IdentityUserLogin<TKey> (AspNetUserLogins)
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            entity.ToTable("IdentityUserLogin");

        });

        // Configure IdentityUserRole<TKey> (AspNetUserRoles)
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.ToTable("IdentityUserRole");
            entity.HasData(new IdentityUserRole<string>
            {
                UserId = DefaultUsers.AdminId,
                RoleId = DefaultRoles.AdminRoleId
            });

        });

        // Configure IdentityUserToken<TKey> (AspNetUserTokens)
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            entity.ToTable("IdentityUserToken");

        });

        // Configure IdentityRoleClaim<TKey> (AspNetRoleClaims)
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id); // Primary key is already defined, but adding explicitly for consistency
            entity.ToTable("IdentityRoleClaim");

            var permissions = Permissions.GetAllPermissions();
            var adminClaims = new List<IdentityRoleClaim<string>>();

            for (var i = 0; i < permissions.Count; i++)
            {
                adminClaims.Add(new IdentityRoleClaim<string>
                {
                    Id = i + 1,
                    ClaimType = Permissions.Type,
                    ClaimValue = permissions[i],
                    RoleId = DefaultRoles.AdminRoleId
                });
            }

            entity.HasData(adminClaims);

        });

        // Configure IdentityUserClaim<TKey> (AspNetUserClaims)
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id); // Primary key is already defined, but adding explicitly for consistency
            entity.ToTable("IdentityUserClaim");

        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
