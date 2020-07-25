using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BlocksCore.Data.Abstractions.Entities;

namespace BlocksCore.Data.EF.Test.TestModel.BlockTestContext
{
    public partial class TESTENTITY   : Entity   
    {
 
        [Column("ID")]
        public override string Id { set ; get ; }
        public string TESTENTITY2ID { set; get; }
        public decimal COLNUMINT { set; get; }
        public string TESTENTITY2ID_NULLABLE { set; get; }
        public decimal? COLNUMINT_NULLABLE { set; get; }
        public string STRING { set; get; }
        public long ISACTIVE { set; get; }
        public string COMMENT { set; get; }
        public DateTime? REGISTERTIME { set; get; }
      
        public TESTENTITY2 TESTENTITY2 { set; get; }
        public ICollection<TESTENTITY3> TESTENTITY3s { set; get; }
    }

    //public partial class TESTENTITYConfiguration : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<TESTENTITY>
    //{
    //    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TESTENTITY> builder)
    //    {

    //        builder.HasKey(x => x.Id);


    //        //builder.HasOne(t => t.TESTENTITY2).WithMany().HasForeignKey(t => t.TESTENTITY2ID);
    //        //builder.HasMany(t => t.TESTENTITY3s).WithOne().HasForeignKey(t => t.TESTENTITYID1);
    //        builder.HasOne(t => t.TESTENTITY2).WithMany().HasForeignKey(t => t.TESTENTITY2ID);//.HasForeignKey(t => t.TESTENTITY2ID);
    //        builder.HasMany(t => t.TESTENTITY3s).WithOne().HasForeignKey(t => t.TESTENTITYID1);
    //    }

    //}
}