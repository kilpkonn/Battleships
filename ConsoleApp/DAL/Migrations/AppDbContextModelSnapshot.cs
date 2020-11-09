﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.BoardState", b =>
                {
                    b.Property<int>("BoardStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameSessionId")
                        .HasColumnType("int");

                    b.Property<int?>("GameSessionId1")
                        .HasColumnType("int");

                    b.HasKey("BoardStateId");

                    b.HasIndex("GameSessionId");

                    b.HasIndex("GameSessionId1");

                    b.ToTable("BoardState");
                });

            modelBuilder.Entity("Domain.BoardTile", b =>
                {
                    b.Property<int>("BoardTileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardStateId")
                        .HasColumnType("int");

                    b.Property<int?>("BoardStateId1")
                        .HasColumnType("int");

                    b.Property<int>("CoordX")
                        .HasColumnType("int");

                    b.Property<int>("CoordY")
                        .HasColumnType("int");

                    b.Property<int>("TileBlackHits")
                        .HasColumnType("int");

                    b.Property<int>("TileBlackShips")
                        .HasColumnType("int");

                    b.Property<int>("TileWhiteHits")
                        .HasColumnType("int");

                    b.Property<int>("TileWhiteShips")
                        .HasColumnType("int");

                    b.HasKey("BoardTileId");

                    b.HasIndex("BoardStateId");

                    b.HasIndex("BoardStateId1");

                    b.HasIndex("CoordX");

                    b.HasIndex("CoordY");

                    b.ToTable("BoardTile");
                });

            modelBuilder.Entity("Domain.Boat", b =>
                {
                    b.Property<int>("BoatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("GameSessionId")
                        .HasColumnType("int");

                    b.Property<int>("Lenght")
                        .HasColumnType("int");

                    b.HasKey("BoatId");

                    b.HasIndex("GameSessionId");

                    b.ToTable("Boats");
                });

            modelBuilder.Entity("Domain.GameSession", b =>
                {
                    b.Property<int>("GameSessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("BackToBackMovesOnHit")
                        .HasColumnType("bit");

                    b.Property<int>("BoardHeight")
                        .HasColumnType("int");

                    b.Property<int>("BoardWidth")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<int>("PlayerBlackId")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerWhiteId")
                        .HasColumnType("int");

                    b.Property<int>("TouchMode")
                        .HasColumnType("int");

                    b.HasKey("GameSessionId");

                    b.HasIndex("PlayerBlackId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayerWhiteId");

                    b.ToTable("GameSessions");
                });

            modelBuilder.Entity("Domain.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("PlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Domain.BoardState", b =>
                {
                    b.HasOne("Domain.GameSession", "GameSession")
                        .WithMany()
                        .HasForeignKey("GameSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.GameSession", null)
                        .WithMany("BoardStates")
                        .HasForeignKey("GameSessionId1");
                });

            modelBuilder.Entity("Domain.BoardTile", b =>
                {
                    b.HasOne("Domain.BoardState", "BoardState")
                        .WithMany()
                        .HasForeignKey("BoardStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.BoardState", null)
                        .WithMany("BoardTiles")
                        .HasForeignKey("BoardStateId1");
                });

            modelBuilder.Entity("Domain.Boat", b =>
                {
                    b.HasOne("Domain.GameSession", "GameSession")
                        .WithMany("Boats")
                        .HasForeignKey("GameSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.GameSession", b =>
                {
                    b.HasOne("Domain.Player", "PlayerBlack")
                        .WithMany()
                        .HasForeignKey("PlayerBlackId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Player", null)
                        .WithMany("GameSessions")
                        .HasForeignKey("PlayerId");

                    b.HasOne("Domain.Player", "PlayerWhite")
                        .WithMany()
                        .HasForeignKey("PlayerWhiteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
