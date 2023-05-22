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
    var host = builder.Configuration["DBHOST"] ?? "localhost";
    var port = builder.Configuration["DBPORT"] ?? "3307";
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
      var mySqlDatabases = "";
      while (mySqlReader.Read())
      {
        //mySqlDatabases += mySqlReader.GetString(0) + ",";
        string row = "";
        for (int i = 0; i < mySqlReader.FieldCount; i++)
          row += mySqlReader.GetValue(i).ToString() + ", ";
        mySqlDatabases += row;         
      }
      mySqlConnection.Close();
      return "Hello World, API is working!\nDatabases: " + mySqlDatabases +"\nconnStr: " + connString; 
    }
    catch (System.Exception ex)
    {
      return "Error! connecting the database, RootController"+"\nconnStr: " + connString;
    }
  }
}