using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Infrastructure;
using FluentMigrator;

namespace BlocksCore.Data.Migrator
{
    [Migration(20200730121800, TransactionBehavior.None)]

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

            //using (var connection = this.dbContextServices.CreateConnection(connectionInfo.ConnectionString.Replace(connectionInfo.Database, "master")))
            //{
            //    connection.Open();

            //    var command = connection.CreateCommand();
            //    command.CommandText = $"create database [{connectionInfo.Database}]";
            //    command.ExecuteNonQuery();
            //}
            var dbName = connectionInfo.Database;

            IfDatabase("sqlserver").Execute.Sql(@$"IF EXISTS(select top 1 * from sys.databases where name='{dbName}')
                                                    BEGIN
                                                     PRINT('数据库已存在!')
                                                    END
                                                   ELSE
                                                    BEGIN
                                                     CREATE DATABASE [{dbName}]
                                                     PRINT('数据库创建成功')
                                                    END");
            IfDatabase("oracle").Execute.Sql(@$"BEGIN
                                                    EXECUTE IMMEDIATE 'CREATE USER {dbName} identified by {dbName}';
                                                    EXECUTE IMMEDIATE 'CREATE TABLESPACE { dbName} datafile ''d:\{dbName}.dbf'' size 10m';
                                                    EXECUTE IMMEDIATE 'ALTER USER {dbName} DEFAULT TABLESPACE {dbName}';
                                                    EXECUTE IMMEDIATE 'GRANT CREATE session,create table,unlimited tablespace to {dbName}';
                                                END;".Replace("r\n", " ").Replace('\n', ' '));


        }


        public override void Down()
        {
            var dbName = connectionInfo.Database;

            IfDatabase("sqlserver").Execute.Sql(@$"
            ALTER DATABASE [{dbName}] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE --设置库单用户模式，和设置立即回滚
            DROP DATABASE [{dbName}]");
            
            var existSession = $"SELECT 'alter system kill session ' || '''' ||t.sid ||','||t.SERIAL#|| '''' as killSql FROM v$session t WHERE t.USERNAME='{dbName}'".Replace("'","''");
            IfDatabase("oracle").Execute.Sql(@$"   
            BEGIN
                BEGIN
                         FOR v_cur IN(SELECT sid, serial# FROM v$session WHERE username = '{dbName}') LOOP
                            EXECUTE IMMEDIATE ('ALTER SYSTEM  DISCONNECT SESSION ''' || v_cur.sid || ',' || v_cur.serial# || ''' IMMEDIATE');
                         END LOOP;
                        dbms_lock.sleep(10);
                END;
                EXECUTE IMMEDIATE 'DROP TABLESPACE {dbName} INCLUDING CONTENTS AND DATAFILES';
                EXECUTE IMMEDIATE 'DROP USER {dbName} CASCADE';
               
            END;
           ".Replace("r\n", " ").Replace('\n', ' '));
        }


    }
}
