using System.ComponentModel.DataAnnotations.Schema;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Linq2DB.Entities;
using LinqToDB.Mapping;

namespace BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext
{
    public partial class TESTENTITY2   : BlocksCore.Data.Abstractions.Entities.Entity
    {
 
        //[Column("ID")]
        public override string Id { set ; get ; }
        public string Text { set; get; }
    }
    public partial class TESTENTITY2Configuration : IEntityTypeConfiguration<TESTENTITY2>
    {
        public void Configure(EntityMappingBuilder<TESTENTITY2> builder)
        {

            builder.HasPrimaryKey(x => x.Id);

        }

    }
}