using System.ComponentModel.DataAnnotations.Schema;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Linq2DB.Entities;
using LinqToDB.Mapping;

namespace BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext
{
    public partial class TESTENTITY3 : BlocksCore.Data.Abstractions.Entities.Entity
    {
        //[Column("ID")]
        public override string Id { get; set; }
        public string TESTENTITYID { set; get; }
        public string TESTENTITYID1 { get; set; }

       // public TESTENTITY TESTENTITY { get; set; }
    }

    public partial class TESTENTITY3Configuration : IEntityTypeConfiguration<TESTENTITY3>
    {
        public void Configure(EntityMappingBuilder<TESTENTITY3> builder)
        {

            builder.HasPrimaryKey(x => x.Id);


        }

    }
    //public class TestEntity3Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<TESTENTITY3>
    //{
    //    public TestEntity3Configuration() 
    //    {
    //        ToTable("TESTENTITY3");
    //        HasKey(x => x.Id);
    //        HasOptional(t => t.TESTENTITY).WithMany().HasForeignKey(t => t.TESTENTITYID1);
    //    }


    //}
}