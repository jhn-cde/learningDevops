using Microsoft.AspNetCore.Mvc;
using MicroserviceOne.BusinessService;
using MySqlConnector;

using Microsoft.EntityFrameworkCore;

namespace MicroserviceOne.Controllers;
[ApiController]
[Route("mysqlapi")]
public class RootController: ControllerBase
{
  public RootController(){

  }

  [HttpGet]
  public string GetDictionaries(){
    
    var builder = WebApplication.CreateBuilder();
    var host = builder.Configuration["DBHOST"] ?? builder.Configuration.GetConnectionString("DBHOST");
    var port = builder.Configuration["DBPORT"] ?? builder.Configuration.GetConnectionString("DBPORT");
    var password = builder.Configuration["MYSQL_PASSWORD"] ?? builder.Configuration.GetConnectionString("MYSQL_PASSWORD");
    var userid = builder.Configuration["MYSQL_USER"] ?? builder.Configuration.GetConnectionString("MYSQL_USER");
    var userDB = builder.Configuration["MYSQL_DATABASE"] ?? builder.Configuration.GetConnectionString("MYSQL_DATABASE");

    var connString = $"server={host}; userid={userid}; pwd={password}; port={port}; database={userDB}";
    try
    { 
      var mySqlConnection = new MySqlConnection(connString);
      mySqlConnection.Open();
      var mySqlCommand = new MySqlCommand("show databases;", mySqlConnection);
      var mySqlReader = mySqlCommand.ExecuteReader();
      mySqlConnection.Close();
      return "Hello World, connection to the db is working!"; 
    }
    catch (Exception err)
    {
      return "Error! connecting the database, RootController"+"\n" + err.Message;
    }
  }
}