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
    [Migration("20231018041117_AddedTeachers")]
    partial class AddedTeachers
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
                            CreatedAt = new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1158),
                            Name = "Madhav",
                            Weight = 50.399999999999999
                        },
                        new
                        {
                            Id = 2,
                            Class = 5,
                            CreatedAt = new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1169),
                            Name = "Suresh",
                            Weight = 30.600000000000001
                        },
                        new
                        {
                            Id = 3,
                            Class = 7,
                            CreatedAt = new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1171),
                            Name = "Deepak",
                            Weight = 40.399999999999999
                        },
                        new
                        {
                            Id = 4,
                            Class = 12,
                            CreatedAt = new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1173),
                            Name = "Hemant",
                            Weight = 55.0
                        },
                        new
                        {
                            Id = 5,
                            Class = 3,
                            CreatedAt = new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1174),
                            Name = "Shashank",
                            Weight = 25.399999999999999
                        });
                });

            modelBuilder.Entity("Students_API.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("HiringDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teachers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            HiringDate = new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1409),
                            Name = "Rakesh",
                            Rating = 4.5,
                            Salary = 45000,
                            Subject = "Math"
                        },
                        new
                        {
                            Id = 2,
                            HiringDate = new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1412),
                            Name = "Ranjan",
                            Rating = 3.3999999999999999,
                            Salary = 34000,
                            Subject = "Science"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
