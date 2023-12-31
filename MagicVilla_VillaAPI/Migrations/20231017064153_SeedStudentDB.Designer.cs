﻿// <auto-generated />
using System;
using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Students_API.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20231017064153_SeedStudentDB")]
    partial class SeedStudentDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_VillaAPI.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Class")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Class = 10,
                            CreatedAt = new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7139),
                            Name = "Madhav",
                            Weight = 50.399999999999999
                        },
                        new
                        {
                            Id = 2,
                            Class = 5,
                            CreatedAt = new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7153),
                            Name = "Suresh",
                            Weight = 30.600000000000001
                        },
                        new
                        {
                            Id = 3,
                            Class = 7,
                            CreatedAt = new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7155),
                            Name = "Deepak",
                            Weight = 40.399999999999999
                        },
                        new
                        {
                            Id = 4,
                            Class = 12,
                            CreatedAt = new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7157),
                            Name = "Hemant",
                            Weight = 55.0
                        },
                        new
                        {
                            Id = 5,
                            Class = 3,
                            CreatedAt = new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7159),
                            Name = "Shashank",
                            Weight = 25.399999999999999
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
