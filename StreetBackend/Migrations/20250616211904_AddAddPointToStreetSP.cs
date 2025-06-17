using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreetBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddAddPointToStreetSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFilePath = Path.Combine("SqlScripts", "AddPointToStreet.sql");
            var sql = File.ReadAllText(sqlFilePath);
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AddPointToStreet");
        }
    }
}
