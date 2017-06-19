using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace alpine.database.Models
{
    public partial class alpineContext : DbContext
    {
        public virtual DbSet<Audiences> Audiences { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserPasswordResetLinks> UserPasswordResetLinks { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public alpineContext()
        { }

        public alpineContext( DbContextOptions<alpineContext> options )
             : base( options )
        { }

        public static string ConnectionString { get; set; }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder.UseSqlServer( ConnectionString );
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.Entity<Audiences>( entity =>
             {
                 entity.ToTable( "Audiences", "identity" );

                 entity.HasIndex( e => e.ClusterId )
                     .HasName( "CIX_Users" )
                     .IsUnique();

                 entity.Property( e => e.Id )
                     .HasColumnName( "id" )
                     .ValueGeneratedNever();

                 entity.Property( e => e.Base64Secret )
                     .IsRequired()
                     .HasColumnName( "base64Secret" );

                 entity.Property( e => e.ClusterId )
                     .HasColumnName( "clusterId" )
                     .ValueGeneratedOnAdd();

                 entity.Property( e => e.Name )
                     .IsRequired()
                     .HasColumnName( "name" )
                     .HasMaxLength( 256 );
             } );

            modelBuilder.Entity<Clients>( entity =>
             {
                 entity.ToTable( "Clients", "identity" );

                 entity.Property( e => e.Id )
                     .HasColumnName( "id" )
                     .HasMaxLength( 128 );

                 entity.Property( e => e.Active ).HasColumnName( "active" );

                 entity.Property( e => e.AllowedOrigin )
                     .IsRequired()
                     .HasColumnName( "allowedOrigin" )
                     .HasMaxLength( 128 );

                 entity.Property( e => e.ApplicationType ).HasColumnName( "applicationType" );

                 entity.Property( e => e.Name )
                     .IsRequired()
                     .HasColumnName( "name" );

                 entity.Property( e => e.RefreshTokenLifetime ).HasColumnName( "refreshTokenLifetime" );

                 entity.Property( e => e.Secret )
                     .IsRequired()
                     .HasColumnName( "secret" );
             } );

            modelBuilder.Entity<RefreshTokens>( entity =>
             {
                 entity.ToTable( "RefreshTokens", "identity" );

                 entity.HasIndex( e => e.ClusterId )
                     .HasName( "CIX_RefreshTokens" )
                     .IsUnique();

                 entity.Property( e => e.Id )
                     .HasColumnName( "id" )
                     .HasMaxLength( 128 );

                 entity.Property( e => e.ClientId )
                     .IsRequired()
                     .HasColumnName( "clientId" )
                     .HasMaxLength( 128 );

                 entity.Property( e => e.ClusterId )
                     .HasColumnName( "clusterId" )
                     .ValueGeneratedOnAdd();

                 entity.Property( e => e.ExpiresUtc )
                     .HasColumnName( "expiresUtc" )
                     .HasColumnType( "datetime" );

                 entity.Property( e => e.IssuedUtc )
                     .HasColumnName( "issuedUtc" )
                     .HasColumnType( "datetime" );

                 entity.Property( e => e.ProtectedTicket ).HasColumnName( "protectedTicket" );

                 entity.Property( e => e.Subject )
                     .IsRequired()
                     .HasColumnName( "subject" )
                     .HasMaxLength( 128 );
             } );

            modelBuilder.Entity<Roles>( entity =>
             {
                 entity.ToTable( "Roles", "identity" );

                 entity.Property( e => e.Id ).HasColumnName( "id" );

                 entity.Property( e => e.Name )
                     .IsRequired()
                     .HasColumnName( "name" )
                     .HasMaxLength( 256 );
             } );

            modelBuilder.Entity<UserPasswordResetLinks>( entity =>
             {
                 entity.ToTable( "UserPasswordResetLinks", "identity" );

                 entity.Property( e => e.Id )
                     .HasColumnName( "id" )
                     .ValueGeneratedNever();

                 entity.Property( e => e.Guid ).HasColumnName( "guid" );

                 entity.Property( e => e.LinkExpiration )
                     .HasColumnName( "linkExpiration" )
                     .HasColumnType( "datetime" );

                 entity.Property( e => e.LinkType )
                     .IsRequired()
                     .HasColumnName( "linkType" )
                     .HasColumnType( "varchar(10)" );

                 entity.Property( e => e.UserId ).HasColumnName( "userId" );

                 entity.HasOne( d => d.User )
                     .WithMany( p => p.UserPasswordResetLinks )
                     .HasForeignKey( d => d.UserId )
                     .OnDelete( DeleteBehavior.Restrict )
                     .HasConstraintName( "FK__UserPassw__userI__5CD6CB2B" );
             } );

            modelBuilder.Entity<Users>( entity =>
             {
                 entity.ToTable( "Users", "identity" );

                 entity.HasIndex( e => e.ClusterId )
                     .HasName( "CIX_Users" )
                     .IsUnique();

                 entity.Property( e => e.Id )
                     .HasColumnName( "id" )
                     .ValueGeneratedNever();

                 entity.Property( e => e.AccessFailedCount ).HasColumnName( "accessFailedCount" );

                 entity.Property( e => e.Active ).HasColumnName( "active" );

                 entity.Property( e => e.Avatar ).HasColumnName( "avatar" );

                 entity.Property( e => e.ClusterId )
                     .HasColumnName( "clusterId" )
                     .ValueGeneratedOnAdd();

                 entity.Property( e => e.CompanyId ).HasColumnName( "companyId" );

                 entity.Property( e => e.CreatedByUserId ).HasColumnName( "createdByUserId" );

                 entity.Property( e => e.CreatedDate )
                     .HasColumnName( "createdDate" )
                     .HasColumnType( "datetime" );

                 entity.Property( e => e.Email )
                     .IsRequired()
                     .HasColumnName( "email" )
                     .HasMaxLength( 256 );

                 entity.Property( e => e.FirstName )
                     .IsRequired()
                     .HasColumnName( "firstName" )
                     .HasMaxLength( 100 );

                 entity.Property( e => e.LastName )
                     .IsRequired()
                     .HasColumnName( "lastName" )
                     .HasMaxLength( 100 );

                 entity.Property( e => e.Password )
                     .IsRequired()
                     .HasColumnName( "password" );

                 entity.Property( e => e.PasswordLastUpdated )
                     .HasColumnName( "passwordLastUpdated" )
                     .HasColumnType( "datetime" );

                 entity.Property( e => e.PhoneNumber )
                     .HasColumnName( "phoneNumber" )
                     .HasColumnType( "varchar(20)" );

                 entity.Property( e => e.RoleId ).HasColumnName( "roleId" );

                 entity.HasOne( d => d.Role )
                     .WithMany( p => p.Users )
                     .HasForeignKey( d => d.RoleId )
                     .OnDelete( DeleteBehavior.Restrict )
                     .HasConstraintName( "FK__Users__roleId__3E52440B" );
             } );
        }
    }
}