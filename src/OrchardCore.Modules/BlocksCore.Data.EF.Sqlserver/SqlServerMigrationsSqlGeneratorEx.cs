using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace BlocksCore.Data.EF.Sqlserver
{
    public class SqlServerMigrationsSqlGeneratorEx : SqlServerMigrationsSqlGenerator
    {
        public SqlServerMigrationsSqlGeneratorEx(MigrationsSqlGeneratorDependencies dependencies,IMigrationsAnnotationProvider migrationsAnnotations) : base(dependencies,migrationsAnnotations)
        {

        }

        protected override void Generate(DropForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
        {
            return;
        }


        protected override void Generate(AddForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
        {
            return;
        }

    }
}
