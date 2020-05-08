using System;
using System.Collections.Generic;
using System.Linq;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlocksCore.Data.EF.Test.FunctionTest
{
    public class EntityUpdateTest : BlocksTest
    {
        public EntityUpdateTest() : base()
        {
            foreach (var contextOption in contextOptions)
            {
                using (var context = new TestBlocksDbContext(contextOption))
                {
                    context.TestEntity.AddRange(new List<TESTENTITY>()
                    {
                        new TESTENTITY() {Id = Guid.NewGuid().ToString()},
                        new TESTENTITY() {Id = Guid.NewGuid().ToString()},
                        new TESTENTITY() {Id = Guid.NewGuid().ToString()},
                    });
                    context.SaveChanges();
                }
            }
        }

        [Fact]
        public void DefaultConfigIsDetectChanges()
        {
            foreach (var contextOption in contextOptions)
            {
                using (var context = new TestBlocksDbContext(contextOption))
                {
                    var testEntity = context.TestEntity.Skip(0).Take(1).FirstOrDefault();

                    testEntity.TESTENTITY2ID = Guid.NewGuid().ToString();
                    Assert.Equal(EntityState.Modified, context.Entry(testEntity).State);
                }
            }
        }


        [Fact]
        public void CloseAutoDetectAllModifyIsunchaned_ButCanManalDetect()
        {
            foreach (var contextOption in contextOptions)
            {
                using (var context = new TestBlocksDbContext(contextOption))
                {
                    context.ChangeTracker.AutoDetectChangesEnabled = false;
                  //  context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    // context.Configuration.AutoDetectChangesEnabled = false;
                    var testEntities = context.TestEntity.Skip(0).Take(2).ToList();
                    var newGuid = Guid.NewGuid().ToString();
                    testEntities[0].TESTENTITY2ID = newGuid;
                    var dbEntry = context.Entry(testEntities[0]);
                    Assert.Equal(EntityState.Unchanged, dbEntry.State);


                    context.ChangeTracker.DetectChanges();
                    Assert.Equal(EntityState.Modified, context.Entry(testEntities[0]).State);
                }
            }
        }


        [Fact]
        public void GetDataWithNoTrackingIsDetached_notCache_notUpdate()
        {
            var id = String.Empty;
            var newGuid = String.Empty;
            foreach (var contextOption in contextOptions)
            {
                using (var context = new TestBlocksDbContext(contextOption))
                {
                    var testEntity = context.TestEntity.AsNoTracking().FirstOrDefault();
                    newGuid = Guid.NewGuid().ToString();
                    testEntity.TESTENTITY2ID = newGuid;
                    var EntityEntry = context.Entry(testEntity);
                    Assert.Equal(EntityState.Detached, EntityEntry.State);

                    id = testEntity.Id;
                    context.SaveChanges();
                    var newEntityNoTracking = context.TestEntity.AsNoTracking().FirstOrDefault(t => t.Id == id);
                    Assert.NotEqual(newEntityNoTracking.TESTENTITY2ID, newGuid);

                    var newEntity = context.TestEntity.FirstOrDefault(t => t.Id == id);
                    Assert.NotEqual(newEntity.TESTENTITY2ID, newGuid);
                }

                using (var context = new TestBlocksDbContext(contextOption))
                {
                    var testEntity = context.TestEntity.AsNoTracking().FirstOrDefault(t => t.Id == id);

                    Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);
                }
            }
        }

        [Fact]
        public void GetDataWithNoTrackingAttach()
        {
            var id = String.Empty;
            var newGuid = String.Empty;
            foreach (var contextOption in contextOptions)
            {
                using (var context = new TestBlocksDbContext(contextOption))
                {
                    var testEntity = context.TestEntity.AsNoTracking().FirstOrDefault();
                    newGuid = Guid.NewGuid().ToString();
                    testEntity.TESTENTITY2ID = newGuid;
                    var EntityEntry = context.Entry(testEntity);
                    Assert.Equal(EntityState.Detached, EntityEntry.State);
                    id = testEntity.Id;
                    context.SaveChanges();
                    var newEntity = context.TestEntity.AsNoTracking().FirstOrDefault(t => t.Id == id);
                    Assert.NotEqual(newEntity.TESTENTITY2ID, newGuid);
                }

                using (var context = new TestBlocksDbContext(contextOption))
                {
                    var testEntity = context.TestEntity.FirstOrDefault(t => t.Id == id);

                    Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);
                }
            }
        }
    }
}