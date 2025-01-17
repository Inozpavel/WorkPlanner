﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Tasks.Api.Data;

namespace Tasks.Api.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210408153910_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("RoomRoleUserInRoom", b =>
                {
                    b.Property<Guid>("RoomRolesRoomRoleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserInRoomsRoomId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserInRoomsUserId")
                        .HasColumnType("uuid");

                    b.HasKey("RoomRolesRoomRoleId", "UserInRoomsRoomId", "UserInRoomsUserId");

                    b.HasIndex("UserInRoomsRoomId", "UserInRoomsUserId");

                    b.ToTable("RoomRoleUserInRoom");
                });

            modelBuilder.Entity("Tasks.Api.Entities.Room", b =>
                {
                    b.Property<Guid>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("RoomDescription")
                        .HasColumnType("text");

                    b.Property<string>("RoomName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Tasks.Api.Entities.RoomRole", b =>
                {
                    b.Property<Guid>("RoomRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("RoomRoleDescription")
                        .HasColumnType("text");

                    b.Property<string>("RoomRoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RoomRoleId");

                    b.ToTable("RoomRoles");
                });

            modelBuilder.Entity("Tasks.Api.Entities.UserInRoom", b =>
                {
                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("RoomId", "UserId");

                    b.ToTable("UsersInRoom");
                });

            modelBuilder.Entity("RoomRoleUserInRoom", b =>
                {
                    b.HasOne("Tasks.Api.Entities.RoomRole", null)
                        .WithMany()
                        .HasForeignKey("RoomRolesRoomRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tasks.Api.Entities.UserInRoom", null)
                        .WithMany()
                        .HasForeignKey("UserInRoomsRoomId", "UserInRoomsUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tasks.Api.Entities.UserInRoom", b =>
                {
                    b.HasOne("Tasks.Api.Entities.Room", null)
                        .WithMany("UsersInRoom")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tasks.Api.Entities.Room", b =>
                {
                    b.Navigation("UsersInRoom");
                });
#pragma warning restore 612, 618
        }
    }
}
