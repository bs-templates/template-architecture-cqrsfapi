﻿// <auto-generated />
using BAYSOFT.Infrastructures.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BAYSOFT.Presentations.WebAPI.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    [Migration("20201207024221_InitialDefaultDb")]
    partial class InitialDefaultDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("BAYSOFT.Core.Domain.Entities.Default.Sample", b =>
                {
                    b.Property<int>("SampleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SampleID");

                    b.ToTable("Samples");
                });
#pragma warning restore 612, 618
        }
    }
}