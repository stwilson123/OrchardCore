using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.Abstractions.DataBaseProvider
{
    public enum ProviderName
    {

        /// <summary>
        /// Microsoft Access OleDb provider (both JET or ACE).
        /// Used as configuration name for Access mapping schema <see cref="DataProvider.Access.AccessMappingSchema"/>.
        /// </summary>
        Access,

        /// <summary>
        /// Microsoft Access ODBC provider.
        /// Used as configuration name for Access mapping schema <see cref="DataProvider.Access.AccessMappingSchema"/>.
        /// </summary>
        AccessOdbc,

        /// <summary>
        /// IBM DB2 default provider (DB2 LUW).
        /// Used as configuration name for both DB2 base mapping schema <see cref="DataProvider.DB2.DB2MappingSchema"/>.
        /// </summary>
        DB2,
        /// <summary>
        /// IBM DB2 LUW provider.
        /// Used as configuration name for DB2 LUW mapping schema <see cref="DataProvider.DB2.DB2LUWMappingSchema"/>.
        /// </summary>
        DB2LUW,
        /// <summary>
        /// IBM DB2 for z/OS provider.
        /// Used as configuration name for DB2 z/OS mapping schema <see cref="DataProvider.DB2.DB2zOSMappingSchema"/>.
        /// </summary>
        DB2zOS,
        /// <summary>
        /// Firebird provider.
        /// Used as configuration name for Firebird mapping schema <see cref="DataProvider.Firebird.FirebirdMappingSchema"/>.
        /// </summary>
        Firebird,
        /// <summary>
        /// Informix IBM.Data.Informix provider (including IDS provider).
        /// Used as configuration name for Informix mapping schema <see cref="DataProvider.Informix.InformixMappingSchema"/>.
        /// </summary>
        Informix,
        /// <summary>
        /// Informix over IBM.Data.DB2 IDS provider.
        /// Used as configuration name for Informix mapping schema <see cref="DataProvider.Informix.InformixMappingSchema"/>.
        /// </summary>
        InformixDB2,
        /// <summary>
        /// Microsoft SQL Server default provider (SQL Server 2008).
        /// Used as configuration name for SQL Server base mapping schema <see cref="DataProvider.SqlServer.SqlServerMappingSchema"/>.
        /// </summary>
        SqlServer,
        /// <summary>
        /// Microsoft SQL Server 2000 provider.
        /// Used as configuration name for SQL Server 2000 mapping schema <see cref="DataProvider.SqlServer.SqlServer2000MappingSchema"/>.
        /// </summary>
        SqlServer2000,
        /// <summary>
        /// Microsoft SQL Server 2005 provider.
        /// Used as configuration name for SQL Server 2005 mapping schema <see cref="DataProvider.SqlServer.SqlServer2005MappingSchema"/>.
        /// </summary>
        SqlServer2005,
        /// <summary>
        /// Microsoft SQL Server 2008 provider.
        /// Used as configuration name for SQL Server 2008 mapping schema <see cref="DataProvider.SqlServer.SqlServer2008MappingSchema"/>.
        /// </summary>
        SqlServer2008,
        /// <summary>
        /// Microsoft SQL Server 2012 provider.
        /// Used as configuration name for SQL Server 2012 mapping schema <see cref="DataProvider.SqlServer.SqlServer2012MappingSchema"/>.
        /// </summary>
        SqlServer2012,
        /// <summary>
        /// Microsoft SQL Server 2012 provider.
        /// </summary>
        SqlServer2014,
        /// <summary>
        /// Microsoft SQL Server 2017 provider.
        /// Used as configuration name for SQL Server 2017 mapping schema <see cref="DataProvider.SqlServer.SqlServer2017MappingSchema"/>.
        /// </summary>
        SqlServer2017,
        /// <summary>
        /// MySql provider.
        /// Used as configuration name for MySql mapping schema <see cref="DataProvider.MySql.MySqlMappingSchema"/>.
        /// </summary>
        MySql,
        /// <summary>
        /// MySql provider.
        /// Used as configuration name for MySql mapping schema <see cref="DataProvider.MySql.MySqlMappingSchema"/>.
        /// </summary>
        MySqlOfficial,
        /// <summary>
        /// MySqlConnector provider.
        /// Used as configuration name for MySql mapping schema <see cref="DataProvider.MySql.MySqlMappingSchema"/>.
        /// </summary>
        MySqlConnector,
        /// <summary>
        /// Oracle ODP.NET autodetected provider (native or managed).
        /// Used as configuration name for Oracle base mapping schema <see cref="DataProvider.Oracle.OracleMappingSchema"/>.
        /// </summary>
        Oracle,
        /// <summary>
        /// Oracle ODP.NET native provider.
        /// Used as configuration name for Oracle native provider mapping schema <see cref="DataProvider.Oracle.OracleMappingSchema.NativeMappingSchema"/>.
        /// </summary>
        OracleNative,
        /// <summary>
        /// Oracle ODP.NET managed provider.
        /// Used as configuration name for Oracle managed provider mapping schema <see cref="DataProvider.Oracle.OracleMappingSchema.ManagedMappingSchema"/>.
        /// </summary>
        OracleManaged,
        /// <summary>
        /// PostgreSQL 9.2- data provider.
        /// Used as configuration name for PostgreSQL mapping schema <see cref="DataProvider.PostgreSQL.PostgreSQLMappingSchema"/>.
        /// </summary>
        PostgreSQL,
        /// <summary>
        /// PostgreSQL 9.2- data provider.
        /// </summary>
        PostgreSQL92,
        /// <summary>
        /// PostgreSQL 9.3+ data provider.
        /// </summary>
        PostgreSQL93,
        /// <summary>
        /// PostgreSQL 9.5+ data provider.
        /// </summary>
        PostgreSQL95,
        /// <summary>
        /// Microsoft SQL Server Compact Edition provider.
        /// Used as configuration name for SQL CE mapping schema <see cref="DataProvider.SqlCe.SqlCeMappingSchema"/>.
        /// </summary>
        SqlCe,
        /// <summary>
        /// SQLite provider.
        /// Used as configuration name for SQLite mapping schema <see cref="DataProvider.SQLite.SQLiteMappingSchema"/>.
        /// </summary>
        SQLite,
        /// <summary>
        /// System.Data.Sqlite provider.
        /// </summary>
        SQLiteClassic,
        /// <summary>
        /// Microsoft.Data.Sqlite provider.
        /// </summary>
        SQLiteMS,
        /// <summary>
        /// Native SAP/Sybase ASE provider.
        /// Used as configuration name for Sybase ASE mapping schema <see cref="DataProvider.Sybase.SybaseMappingSchema.NativeMappingSchema"/>.
        /// </summary>
        Sybase,
        /// <summary>
        /// Managed Sybase/SAP ASE provider from <a href="https://github.com/DataAction/AdoNetCore.AseClient">DataAction</a>.
        /// Used as configuration name for Sybase ASE mapping schema <see cref="DataProvider.Sybase.SybaseMappingSchema.ManagedMappingSchema"/>.
        /// </summary>
        SybaseManaged,
        /// <summary>
        /// SAP HANA provider.
        /// Used as configuration name for SAP HANA mapping schema <see cref="DataProvider.SapHana.SapHanaMappingSchema"/>.
        /// </summary>
        SapHana,
#if !NETSTANDARD2_0 && !NETSTANDARD2_1
        /// <summary>
        /// SAP HANA provider.
        /// Used as configuration name for SAP HANA mapping schema <see cref="DataProvider.SapHana.SapHanaMappingSchema.NativeMappingSchema"/>.
        /// </summary>
        SapHanaNative,
#endif
        /// <summary>
        /// SAP HANA ODBC provider.
        /// Used as configuration name for SAP HANA mapping schema <see cref="DataProvider.SapHana.SapHanaMappingSchema.OdbcMappingSchema"/>.
        /// </summary>
        SapHanaOdbc,
    }
}
