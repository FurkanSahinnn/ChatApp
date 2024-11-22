﻿// <auto-generated />
using ChatApp.API.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatApp.API.Persistence.Migrations
{
    [DbContext(typeof(JsonWebTokenContext))]
    [Migration("20241122193143_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChatApp.API.Core.Domain.RoleApp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RoleApps");
                });

            modelBuilder.Entity("ChatApp.API.Core.Domain.UserApp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleAppId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleAppId");

                    b.ToTable("UserApps");
                });

            modelBuilder.Entity("ChatApp.API.Core.Domain.UserApp", b =>
                {
                    b.HasOne("ChatApp.API.Core.Domain.RoleApp", "RoleApp")
                        .WithMany("UserApp")
                        .HasForeignKey("RoleAppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleApp");
                });

            modelBuilder.Entity("ChatApp.API.Core.Domain.RoleApp", b =>
                {
                    b.Navigation("UserApp");
                });
#pragma warning restore 612, 618
        }
    }
}
