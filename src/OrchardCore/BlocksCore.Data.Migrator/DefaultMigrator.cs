using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Infrastructure;
using FluentMigrator;

namespace BlocksCore.Data.Migrator
{
    [Migration(20200730121800)]
    public class DefaultMigrator : Migration
    {
        private readonly IDbContextServices dbContextServices;

        public DefaultMigrator(IDbContextServices dbContextServices)
        {
            this.dbContextServices = dbContextServices;
        }
        public override void Up()
        {
            this.Create.Schema("123");
        }


        public override void Down()
        {
            this.Delete.Schema("123");
        }

      
    }
}
