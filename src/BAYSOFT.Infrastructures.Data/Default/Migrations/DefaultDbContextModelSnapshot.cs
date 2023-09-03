﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace BAYSOFT.Infrastructures.Data.Default.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    partial class DefaultDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Default");
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BAYSOFT.Core.Domain.Default.Samples.Entities", b =>
            {
                b.Property<int>("Id")
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(512)");

                b.HasKey("Id");

                b.ToTable("Samples", (string)null);
            });
#pragma warning restore 612, 618
        }
    }
}