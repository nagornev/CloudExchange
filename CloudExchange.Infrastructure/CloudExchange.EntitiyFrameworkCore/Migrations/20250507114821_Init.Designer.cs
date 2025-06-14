﻿// <auto-generated />
using System;
using CloudExchange.EntitiyFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CloudExchange.EntitiyFrameworkCore.Migrations
{
    [DbContext(typeof(DescriptorContext))]
    [Migration("20250507114821_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CloudExchange.Domain.Entities.DescriptorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Lifetime")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Path")
                        .HasColumnType("longtext");

                    b.Property<long>("Uploaded")
                        .HasColumnType("bigint");

                    b.Property<long>("Weight")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Descriptors");
                });

            modelBuilder.Entity("CloudExchange.Domain.Entities.DescriptorEntity", b =>
                {
                    b.OwnsOne("CloudExchange.Domain.ValueObjects.DescriptorCredentialsValueObject", "Credentials", b1 =>
                        {
                            b1.Property<Guid>("DescriptorEntityId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Download")
                                .HasMaxLength(15)
                                .HasColumnType("varchar(15)")
                                .HasColumnName("Download");

                            b1.Property<string>("Root")
                                .HasMaxLength(20)
                                .HasColumnType("varchar(20)")
                                .HasColumnName("Root");

                            b1.HasKey("DescriptorEntityId");

                            b1.ToTable("Descriptors");

                            b1.WithOwner()
                                .HasForeignKey("DescriptorEntityId");
                        });

                    b.Navigation("Credentials");
                });
#pragma warning restore 612, 618
        }
    }
}
