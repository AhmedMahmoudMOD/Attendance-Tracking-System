﻿// <auto-generated />
using System;
using Attendance_Tracking_System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Attendance_Tracking_System.Migrations
{
    [DbContext(typeof(ITISysContext))]
    [Migration("20240330070356_first")]
    partial class first
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Attendance_Tracking_System.Models.ITIProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Program");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Intake", b =>
                {
                    b.Property<int>("No")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("No"));

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProgramID")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("No");

                    b.HasIndex("ProgramID");

                    b.ToTable("Intake");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Track", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProgramID")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ProgramID");

                    b.ToTable("Track");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("UserImage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("ITIProgramInstructor", b =>
                {
                    b.Property<int>("InstructorsId")
                        .HasColumnType("int");

                    b.Property<int>("ProgramsId")
                        .HasColumnType("int");

                    b.HasKey("InstructorsId", "ProgramsId");

                    b.HasIndex("ProgramsId");

                    b.ToTable("ITIProgramInstructor");
                });

            modelBuilder.Entity("InstructorIntake", b =>
                {
                    b.Property<int>("InstructorsId")
                        .HasColumnType("int");

                    b.Property<int>("IntakesNo")
                        .HasColumnType("int");

                    b.HasKey("InstructorsId", "IntakesNo");

                    b.HasIndex("IntakesNo");

                    b.ToTable("InstructorIntake");
                });

            modelBuilder.Entity("InstructorTrack", b =>
                {
                    b.Property<int>("InstructorsId")
                        .HasColumnType("int");

                    b.Property<int>("TracksId")
                        .HasColumnType("int");

                    b.HasKey("InstructorsId", "TracksId");

                    b.HasIndex("TracksId");

                    b.ToTable("InstructorTrack");
                });

            modelBuilder.Entity("IntakeTrack", b =>
                {
                    b.Property<int>("IntakesNo")
                        .HasColumnType("int");

                    b.Property<int>("TracksId")
                        .HasColumnType("int");

                    b.HasKey("IntakesNo", "TracksId");

                    b.HasIndex("TracksId");

                    b.ToTable("IntakeTrack");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Employee", b =>
                {
                    b.HasBaseType("Attendance_Tracking_System.Models.User");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Instructor", b =>
                {
                    b.HasBaseType("Attendance_Tracking_System.Models.User");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.ToTable("Instructor");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Student", b =>
                {
                    b.HasBaseType("Attendance_Tracking_System.Models.User");

                    b.Property<int>("AttendanceDegrees")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(250);

                    b.Property<string>("Faculty")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GraduationYear")
                        .HasColumnType("int");

                    b.Property<int>("IntakeNo")
                        .HasColumnType("int");

                    b.Property<int>("ProgramID")
                        .HasColumnType("int");

                    b.Property<string>("Specialization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrackID")
                        .HasColumnType("int");

                    b.Property<string>("University")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("IntakeNo");

                    b.HasIndex("ProgramID");

                    b.HasIndex("TrackID");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Intake", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.ITIProgram", "Program")
                        .WithMany("Intakes")
                        .HasForeignKey("ProgramID");

                    b.Navigation("Program");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Track", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.ITIProgram", "Program")
                        .WithMany("Tracks")
                        .HasForeignKey("ProgramID");

                    b.Navigation("Program");
                });

            modelBuilder.Entity("ITIProgramInstructor", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.Instructor", null)
                        .WithMany()
                        .HasForeignKey("InstructorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Attendance_Tracking_System.Models.ITIProgram", null)
                        .WithMany()
                        .HasForeignKey("ProgramsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InstructorIntake", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.Instructor", null)
                        .WithMany()
                        .HasForeignKey("InstructorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Attendance_Tracking_System.Models.Intake", null)
                        .WithMany()
                        .HasForeignKey("IntakesNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InstructorTrack", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.Instructor", null)
                        .WithMany()
                        .HasForeignKey("InstructorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Attendance_Tracking_System.Models.Track", null)
                        .WithMany()
                        .HasForeignKey("TracksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IntakeTrack", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.Intake", null)
                        .WithMany()
                        .HasForeignKey("IntakesNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Attendance_Tracking_System.Models.Track", null)
                        .WithMany()
                        .HasForeignKey("TracksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Employee", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Attendance_Tracking_System.Models.Employee", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Instructor", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Attendance_Tracking_System.Models.Instructor", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Student", b =>
                {
                    b.HasOne("Attendance_Tracking_System.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Attendance_Tracking_System.Models.Student", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Attendance_Tracking_System.Models.Intake", "Intake")
                        .WithMany("Students")
                        .HasForeignKey("IntakeNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Attendance_Tracking_System.Models.ITIProgram", "Program")
                        .WithMany("Students")
                        .HasForeignKey("ProgramID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Attendance_Tracking_System.Models.Track", "Track")
                        .WithMany("Students")
                        .HasForeignKey("TrackID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Intake");

                    b.Navigation("Program");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.ITIProgram", b =>
                {
                    b.Navigation("Intakes");

                    b.Navigation("Students");

                    b.Navigation("Tracks");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Intake", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("Attendance_Tracking_System.Models.Track", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
