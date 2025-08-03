﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kventin.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FileLinkedWith : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkedWith",
                table: "FileRecords",
                type: "int",
                nullable: false,
                defaultValue: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedWith",
                table: "FileRecords");
        }
    }
}
