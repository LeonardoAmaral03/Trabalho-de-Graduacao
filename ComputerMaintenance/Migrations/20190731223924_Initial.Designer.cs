﻿// <auto-generated />
using System;
using ComputerMaintenance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComputerMaintenance.Migrations
{
    [DbContext(typeof(AppContextModel))]
    [Migration("20190731223924_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ComputerMaintenance.Models.Computer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Computers");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand");

                    b.Property<string>("Description");

                    b.Property<string>("Model");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.ItemComputer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AcquisitionDate");

                    b.Property<Guid?>("ComputerId");

                    b.Property<Guid?>("ItemId");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemComputers");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.Maintenance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActiActivity");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Maintenances");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.MaintenanceItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ItemId");

                    b.Property<Guid?>("MaintenanceId");

                    b.Property<int>("Period");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("MaintenanceId");

                    b.ToTable("MaintenanceItems");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.ItemComputer", b =>
                {
                    b.HasOne("ComputerMaintenance.Models.Computer", "Computer")
                        .WithMany("ItemComputers")
                        .HasForeignKey("ComputerId");

                    b.HasOne("ComputerMaintenance.Models.Item", "Item")
                        .WithMany("ItemComputers")
                        .HasForeignKey("ItemId");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.MaintenanceItem", b =>
                {
                    b.HasOne("ComputerMaintenance.Models.Item", "Item")
                        .WithMany("MaintenanceItems")
                        .HasForeignKey("ItemId");

                    b.HasOne("ComputerMaintenance.Models.Maintenance", "Maintenance")
                        .WithMany("MaintenanceItems")
                        .HasForeignKey("MaintenanceId");
                });
#pragma warning restore 612, 618
        }
    }
}
