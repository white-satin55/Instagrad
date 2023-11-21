﻿// <auto-generated />
using System;
using Instagrad.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Instagrad.Infrastructure.Migrations
{
    [DbContext(typeof(InstagradDbContext))]
    [Migration("20231121123158_AddMediaType")]
    partial class AddMediaType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Instagrad.Domain.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PublisherLogin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PublisherLogin");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Instagrad.Domain.User", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("UserLogin")
                        .HasColumnType("text");

                    b.Property<string>("_password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Login");

                    b.HasIndex("UserLogin");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<string>("IncomingFrendshipRequestsLogin")
                        .HasColumnType("text");

                    b.Property<string>("OutgoingFrendshipRepuestsLogin")
                        .HasColumnType("text");

                    b.HasKey("IncomingFrendshipRequestsLogin", "OutgoingFrendshipRepuestsLogin");

                    b.HasIndex("OutgoingFrendshipRepuestsLogin");

                    b.ToTable("UserUser");
                });

            modelBuilder.Entity("Instagrad.Domain.Image", b =>
                {
                    b.HasOne("Instagrad.Domain.User", null)
                        .WithMany("Images")
                        .HasForeignKey("PublisherLogin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Instagrad.Domain.User", b =>
                {
                    b.HasOne("Instagrad.Domain.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserLogin");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("Instagrad.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("IncomingFrendshipRequestsLogin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Instagrad.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("OutgoingFrendshipRepuestsLogin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Instagrad.Domain.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
