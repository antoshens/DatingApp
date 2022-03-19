﻿// <auto-generated />
using System;
using DatingApp.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatingApp.Core.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220319142719_Add-UserLikes")]
    partial class AddUserLikes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DatingApp.Core.Model.AuditInfo", b =>
                {
                    b.Property<int>("AuditInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuditInfoId"), 1L, 1);

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PrimaryKey")
                        .HasColumnType("int");

                    b.HasKey("AuditInfoId");

                    b.ToTable("AuditInfoes");
                });

            modelBuilder.Entity("DatingApp.Core.Model.Photo", b =>
                {
                    b.Property<int>("PhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhotoId"), 1L, 1);

                    b.Property<int>("AuditInfoId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMain")
                        .HasColumnType("bit");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PhotoId");

                    b.HasIndex("AuditInfoId");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("DatingApp.Core.Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<int>("AuditInfoId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Interests")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LookingFor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte?>("Sex")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("AuditInfoId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DatingApp.Core.Model.UserLike", b =>
                {
                    b.Property<int>("SourceUserId")
                        .HasColumnType("int");

                    b.Property<int>("LikedUserId")
                        .HasColumnType("int");

                    b.Property<int>("AuditInfoId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("UserLikeId")
                        .HasColumnType("int");

                    b.HasKey("SourceUserId", "LikedUserId");

                    b.HasIndex("AuditInfoId");

                    b.HasIndex("LikedUserId");

                    b.ToTable("UserLikes");
                });

            modelBuilder.Entity("DatingApp.Core.Model.Photo", b =>
                {
                    b.HasOne("DatingApp.Core.Model.AuditInfo", "AuditInfo")
                        .WithMany()
                        .HasForeignKey("AuditInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DatingApp.Core.Model.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuditInfo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DatingApp.Core.Model.User", b =>
                {
                    b.HasOne("DatingApp.Core.Model.AuditInfo", "AuditInfo")
                        .WithMany()
                        .HasForeignKey("AuditInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuditInfo");
                });

            modelBuilder.Entity("DatingApp.Core.Model.UserLike", b =>
                {
                    b.HasOne("DatingApp.Core.Model.AuditInfo", "AuditInfo")
                        .WithMany()
                        .HasForeignKey("AuditInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DatingApp.Core.Model.User", "LikedUser")
                        .WithMany("LikedByUsers")
                        .HasForeignKey("LikedUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DatingApp.Core.Model.User", "SourceUser")
                        .WithMany("LikedUsers")
                        .HasForeignKey("SourceUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AuditInfo");

                    b.Navigation("LikedUser");

                    b.Navigation("SourceUser");
                });

            modelBuilder.Entity("DatingApp.Core.Model.User", b =>
                {
                    b.Navigation("LikedByUsers");

                    b.Navigation("LikedUsers");

                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
