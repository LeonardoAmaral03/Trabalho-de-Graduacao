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
    [Migration("20190921200132_CriadoSchedules")]
    partial class CriadoSchedules
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
                    b.Property<Guid>("ComputerId");

                    b.Property<Guid>("ItemId");

                    b.HasKey("ComputerId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemComputers");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.Maintenance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Activity");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Maintenances");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.MaintenanceItem", b =>
                {
                    b.Property<Guid>("ItemId");

                    b.Property<Guid>("MaintenanceId");

                    b.Property<int>("Period");

                    b.HasKey("ItemId", "MaintenanceId");

                    b.HasIndex("MaintenanceId");

                    b.ToTable("MaintenanceItems");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.Schedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("MaintenanceDate");

                    b.Property<Guid>("ScheduleMaintenanceItemId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleMaintenanceItemId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.ScheduleItemComputer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ComputerId");

                    b.Property<Guid>("ItemId");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.HasIndex("ItemId");

                    b.ToTable("ScheduleItemComputers");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.ScheduleMaintenanceItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("MaintenanceId");

                    b.Property<Guid>("ScheduleItemComputerId");

                    b.HasKey("Id");

                    b.HasIndex("MaintenanceId");

                    b.HasIndex("ScheduleItemComputerId");

                    b.ToTable("ScheduleMaintenanceItems");
                });

            modelBuilder.Entity("ComputerMaintenance.Models.ItemComputer", b =>
                {
                    b.HasOne("ComputerMaintenance.Models.Computer", "Computer")
                        .WithMany("ItemComputers")
                        .HasForeignKey("ComputerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ComputerMaintenance.Models.Item", "Item")
                        .WithMany("ItemComputers")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComputerMaintenance.Models.MaintenanceItem", b =>
                {
                    b.HasOne("ComputerMaintenance.Models.Item", "Item")
                        .WithMany("MaintenanceItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ComputerMaintenance.Models.Maintenance", "Maintenance")
                        .WithMany("MaintenanceItems")
                        .HasForeignKey("MaintenanceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComputerMaintenance.Models.Schedule", b =>
                {
                    b.HasOne("ComputerMaintenance.Models.ScheduleMaintenanceItem", "ScheduleMaintenanceItem")
                        .WithMany()
                        .HasForeignKey("ScheduleMaintenanceItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComputerMaintenance.Models.ScheduleItemComputer", b =>
                {
                    b.HasOne("ComputerMaintenance.Models.Computer", "Computer")
                        .WithMany()
                        .HasForeignKey("ComputerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ComputerMaintenance.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComputerMaintenance.Models.ScheduleMaintenanceItem", b =>
                {
                    b.HasOne("ComputerMaintenance.Models.Maintenance", "Maintenance")
                        .WithMany()
                        .HasForeignKey("MaintenanceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ComputerMaintenance.Models.ScheduleItemComputer", "ScheduleItemComputer")
                        .WithMany()
                        .HasForeignKey("ScheduleItemComputerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
