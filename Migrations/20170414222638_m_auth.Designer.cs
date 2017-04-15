using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using vproker.Models;

namespace vproker.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170414222638_m_auth")]
    partial class m_auth
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("vproker.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("vproker.Models.Client", b =>
                {
                    b.Property<string>("ID");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<int>("DiscountPercent");

                    b.Property<byte[]>("DocumentData");

                    b.Property<string>("DocumentGivenBy")
                        .IsRequired();

                    b.Property<DateTime>("DocumentGivenWhen");

                    b.Property<string>("DocumentNumber")
                        .IsRequired();

                    b.Property<string>("DocumentSerial")
                        .IsRequired();

                    b.Property<string>("DocumentUnitCode");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("KnowSourceID")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("LivingAddress");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Note");

                    b.Property<string>("Phone1");

                    b.Property<string>("Phone2");

                    b.Property<string>("Phone3");

                    b.Property<string>("RegistrationAddress");

                    b.Property<string>("WorkingAddress");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("vproker.Models.KnowledgeSource", b =>
                {
                    b.Property<string>("ID");

                    b.Property<string>("Name");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("vproker.Models.Order", b =>
                {
                    b.Property<string>("ID");

                    b.Property<string>("ClientName")
                        .IsRequired();

                    b.Property<string>("ClientPhoneNumber")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EndDate");

                    b.Property<decimal>("PaidPledge");

                    b.Property<decimal?>("Payment");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("ToolID")
                        .IsRequired();

                    b.HasKey("ID");
                });

            modelBuilder.Entity("vproker.Models.Tool", b =>
                {
                    b.Property<string>("ID");

                    b.Property<decimal>("DayPrice");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Pledge");

                    b.Property<decimal>("Price");

                    b.Property<decimal>("WorkShiftPrice");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("vproker.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("vproker.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("vproker.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("vproker.Models.Client", b =>
                {
                    b.HasOne("vproker.Models.KnowledgeSource")
                        .WithMany()
                        .HasForeignKey("KnowSourceID");
                });

            modelBuilder.Entity("vproker.Models.Order", b =>
                {
                    b.HasOne("vproker.Models.Tool")
                        .WithMany()
                        .HasForeignKey("ToolID");
                });
        }
    }
}
