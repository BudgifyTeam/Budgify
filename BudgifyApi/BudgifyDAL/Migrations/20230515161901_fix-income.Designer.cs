﻿// <auto-generated />
using System;
using BudgifyDal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BudgifyDal.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230515161901_fix-income")]
    partial class fixincome
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BudgifyModels.Budget", b =>
                {
                    b.Property<int>("budget_id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("budget_id"));

                    b.Property<int>("users_id")
                        .HasColumnType("integer");

                    b.Property<int>("value")
                        .HasMaxLength(8)
                        .HasColumnType("integer");

                    b.HasKey("budget_id");

                    b.HasIndex("users_id");

                    b.ToTable("budget");
                });

            modelBuilder.Entity("BudgifyModels.Category", b =>
                {
                    b.Property<int>("category_id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("category_id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<int>("users_id")
                        .HasColumnType("integer");

                    b.HasKey("category_id");

                    b.HasIndex("users_id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("BudgifyModels.Expense", b =>
                {
                    b.Property<int>("expense_id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("expense_id"));

                    b.Property<int>("category_id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("pocket_id")
                        .HasColumnType("integer");

                    b.Property<int>("users_id")
                        .HasColumnType("integer");

                    b.Property<int>("value")
                        .HasMaxLength(8)
                        .HasColumnType("integer");

                    b.Property<int>("wallet_id")
                        .HasColumnType("integer");

                    b.HasKey("expense_id");

                    b.HasIndex("category_id");

                    b.HasIndex("pocket_id");

                    b.HasIndex("users_id");

                    b.HasIndex("wallet_id");

                    b.ToTable("expenses");
                });

            modelBuilder.Entity("BudgifyModels.Income", b =>
                {
                    b.Property<int>("income_id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("income_id"));

                    b.Property<int>("category_id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("users_id")
                        .HasColumnType("integer");

                    b.Property<int>("value")
                        .HasMaxLength(8)
                        .HasColumnType("integer");

                    b.Property<int>("wallet_id")
                        .HasColumnType("integer");

                    b.HasKey("income_id");

                    b.HasIndex("category_id");

                    b.HasIndex("users_id");

                    b.HasIndex("wallet_id");

                    b.ToTable("incomes");
                });

            modelBuilder.Entity("BudgifyModels.Pocket", b =>
                {
                    b.Property<int>("pocket_id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("pocket_id"));

                    b.Property<double>("goal")
                        .HasMaxLength(8)
                        .HasColumnType("double precision");

                    b.Property<string>("icon")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<double>("total")
                        .HasMaxLength(8)
                        .HasColumnType("double precision");

                    b.Property<int>("users_id")
                        .HasColumnType("integer");

                    b.HasKey("pocket_id");

                    b.HasIndex("users_id");

                    b.ToTable("pockets");
                });

            modelBuilder.Entity("BudgifyModels.Wallet", b =>
                {
                    b.Property<int>("wallet_id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("wallet_id"));

                    b.Property<string>("icon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<double>("total")
                        .HasMaxLength(8)
                        .HasColumnType("double precision");

                    b.Property<int>("users_id")
                        .HasColumnType("integer");

                    b.HasKey("wallet_id");

                    b.HasIndex("users_id");

                    b.ToTable("wallets");
                });

            modelBuilder.Entity("BudgifyModels.user", b =>
                {
                    b.Property<int>("users_id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("users_id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("icon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("publicaccount")
                        .IsRequired()
                        .HasColumnType("boolean");

                    b.Property<bool?>("status")
                        .IsRequired()
                        .HasColumnType("boolean");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("users_id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("BudgifyModels.Budget", b =>
                {
                    b.HasOne("BudgifyModels.user", "user")
                        .WithMany()
                        .HasForeignKey("users_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("BudgifyModels.Category", b =>
                {
                    b.HasOne("BudgifyModels.user", "user")
                        .WithMany()
                        .HasForeignKey("users_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("BudgifyModels.Expense", b =>
                {
                    b.HasOne("BudgifyModels.Category", "category")
                        .WithMany()
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgifyModels.Pocket", "pocket")
                        .WithMany()
                        .HasForeignKey("pocket_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgifyModels.user", "user")
                        .WithMany()
                        .HasForeignKey("users_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgifyModels.Wallet", "wallet")
                        .WithMany()
                        .HasForeignKey("wallet_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");

                    b.Navigation("pocket");

                    b.Navigation("user");

                    b.Navigation("wallet");
                });

            modelBuilder.Entity("BudgifyModels.Income", b =>
                {
                    b.HasOne("BudgifyModels.Category", "category")
                        .WithMany()
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgifyModels.user", "user")
                        .WithMany()
                        .HasForeignKey("users_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgifyModels.Wallet", "wallet")
                        .WithMany()
                        .HasForeignKey("wallet_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");

                    b.Navigation("user");

                    b.Navigation("wallet");
                });

            modelBuilder.Entity("BudgifyModels.Pocket", b =>
                {
                    b.HasOne("BudgifyModels.user", "user")
                        .WithMany()
                        .HasForeignKey("users_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("BudgifyModels.Wallet", b =>
                {
                    b.HasOne("BudgifyModels.user", "user")
                        .WithMany()
                        .HasForeignKey("users_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });
#pragma warning restore 612, 618
        }
    }
}