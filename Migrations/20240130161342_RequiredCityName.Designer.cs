﻿// <auto-generated />
using System;
using CitiesManager.WebAPI.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CitiesManager.WebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240130161342_RequiredCityName")]
    partial class RequiredCityName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CitiesManager.WebAPI.Models.City", b =>
                {
                    b.Property<Guid>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CityId");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            CityId = new Guid("ec01d452-7a30-4a23-be5c-516e9ded5783"),
                            CityName = "London"
                        },
                        new
                        {
                            CityId = new Guid("f2471756-a8b7-454a-9b10-3011fa4eec26"),
                            CityName = "Alicante"
                        },
                        new
                        {
                            CityId = new Guid("5a70b504-01ab-44eb-8ee4-d7e8dc29f6ed"),
                            CityName = "Berlin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
