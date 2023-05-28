
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace LogicAPI.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options):base(options){

        }

        public DbSet<Dictionary> Dictionaries {get;set;}


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var builder = WebApplication.CreateBuilder();
            // connection string
            var host = builder.Configuration["DBHOST"] ?? builder.Configuration.GetConnectionString("DBHOST");
            var port = builder.Configuration["DBPORT"] ?? builder.Configuration.GetConnectionString("DBPORT");
            var password = builder.Configuration["MYSQL_PASSWORD"] ?? builder.Configuration.GetConnectionString("MYSQL_PASSWORD");
            var userid = builder.Configuration["MYSQL_USER"] ?? builder.Configuration.GetConnectionString("MYSQL_USER");
            var userDB = builder.Configuration["MYSQL_DATABASE"] ?? builder.Configuration.GetConnectionString("MYSQL_DATABASE");

            var connString = $"server={host}; userid={userid}; pwd={password}; port={port}; database={userDB}";
            
            optionsBuilder.UseMySQL(connString);
        }
    }
}