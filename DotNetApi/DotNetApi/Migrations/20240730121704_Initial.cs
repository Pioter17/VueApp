using DotNetApi.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;

#nullable disable

namespace DotNetApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tasks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                });

            var filePathApp = Path.Combine("Data", "applications.json");

            var jsonDataApp = File.ReadAllText(filePathApp);
            var apps = JsonConvert.DeserializeObject<List<Application>>(jsonDataApp);

            foreach (var app in apps)
            {
              migrationBuilder.InsertData(
                  table: "Apps",
                  columns: new[] { "Id", "Name", "Date", "Edition", "Server", "ServerId", "Tasks" },
                  values: new object[] { app.Id, app.Name, app.Date, app.Edition, app.Server, app.ServerId, app.Tasks });
            }

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Applications = table.Column<int>(type: "int", nullable: false),
                    Tasks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });
                var filePathServer = Path.Combine("Data", "servers.json");

                var jsonDataServer = File.ReadAllText(filePathServer);
                var servers = JsonConvert.DeserializeObject<List<AppServer>>(jsonDataServer);

                foreach (var server in servers)
                {
                  migrationBuilder.InsertData(
                      table: "Servers",
                      columns: new[] { "Id", "Name", "Date", "Edition", "Tasks", "Applications" },
                      values: new object[] { server.Id, server.Name, server.Date, server.Edition, server.Tasks, server.Applications });
                }

                migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

                var filePath = Path.Combine("Data", "tasks.json");

                var jsonData = File.ReadAllText(filePath);
                var tasks = JsonConvert.DeserializeObject<List<AppTask>>(jsonData);

                foreach (var task in tasks)
                {
                  migrationBuilder.InsertData(
                      table: "Tasks",
                      columns: new[] { "Id", "Name", "Date", "Edition", "Server", "Application", "ServerId", "ApplicationId" },
                      values: new object[] { task.Id, task.Name, task.Date, task.Edition, task.Server, task.Application, task.ServerId, task.ApplicationId });
                }
    }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apps");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
