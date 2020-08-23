using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Linq2DB.Entities;
using LinqToDB.Mapping;
using ColumnAttribute = LinqToDB.Mapping.ColumnAttribute;

namespace BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext
{
    public partial class TESTENTITY   : BlocksCore.Data.Abstractions.Entities.Entity
    {
 
        //[Column("ID")]
        public override string Id { set ; get ; }
        public string TESTENTITY2ID { set; get; }
        public decimal COLNUMINT { set; get; }
        public string TESTENTITY2ID_NULLABLE { set; get; }
        public decimal? COLNUMINT_NULLABLE { set; get; }
        public string? STRING { set; get; }
        public long ISACTIVE { set; get; }
        public string? COMMENT { set; get; }
        public DateTime? REGISTERTIME { set; get; }
      
        public TESTENTITY2 TESTENTITY2 { set; get; }

       // [Association(ThisKey = nameof(Id), OtherKey = nameof(TESTENTITY3.TESTENTITYID), CanBeNull = true, Relationship = Relationship.OneToOne)]
        public IList<TESTENTITY3> TESTENTITY3s { set; get; }
    }

    public partial class TESTENTITYConfiguration : IEntityTypeConfiguration<TESTENTITY>
    {
        public void Configure(EntityMappingBuilder<TESTENTITY> builder)
        {

            //builder.HasKey(x => x.Id);
            builder.HasPrimaryKey(x => x.Id);
            builder.Association(t => t.TESTENTITY2, (x, y) => x.TESTENTITY2ID == y.Id);
           // builder.Association(t => t.TESTENTITY3s,(x, y) => x.Id == y.TESTENTITYID) ;


            //builder.HasOne(t => t.TESTENTITY2).WithMany().HasForeignKey(t => t.TESTENTITY2ID);
            //builder.HasMany(t => t.TESTENTITY3s).WithOne().HasForeignKey(t => t.TESTENTITYID1);
            //builder.HasOne(t => t.TESTENTITY2).WithMany().HasForeignKey(t => t.TESTENTITY2ID);//.HasForeignKey(t => t.TESTENTITY2ID);
            //builder.HasMany(t => t.TESTENTITY3s).WithOne().HasForeignKey(t => t.TESTENTITYID1);
        }

    }
}