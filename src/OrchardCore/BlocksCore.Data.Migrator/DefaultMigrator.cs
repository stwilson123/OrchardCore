using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Infrastructure;
using FluentMigrator;

namespace BlocksCore.Data.Migrator
{
    [Migration(20200730121800,TransactionBehavior.None)]
    public class DefaultMigrator : Migration
    {
        private readonly IDbContextServices dbContextServices;

        private readonly ConnectionInfo connectionInfo;
        public DefaultMigrator(IDbContextServices dbContextServices)
        {
            this.dbContextServices = dbContextServices;
            connectionInfo = this.dbContextServices.GetConnetionInfo();
           
        }
        public override void Up()
        {
           ;
            //using (var connection = this.dbContextServices.CreateConnection(connectionInfo.ConnectionString.Replace(connectionInfo.Database, "master")))
            //{
            //    connection.Open();

            //    var command = connection.CreateCommand();
            //    command.CommandText = $"create database [{connectionInfo.Database}]";
            //    command.ExecuteNonQuery();
            //}
            
            IfDatabase("sqlserver").Execute.Sql($"create database {connectionInfo.Database}");
        }


        public override void Down()
        {
            IfDatabase("sqlserver").Execute.Sql($"drop database [{connectionInfo.Database}]");

           // this.Delete.Schema("123");
        }

      
    }
}
