

//using BlocksCore.Data.Linq2DB.Test.Cache;
//using Xunit;

//namespace BlocksCore.Data.Linq2DB.Test.Linq
//{
//    public class LinqQueryableTest 
//    {
//        [Fact]
//        public void joinGenSql()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(),context);
//                var testEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID, (TESTENTITY2 b) => b.Id);
                    
//                var default1Sql = defaultLinqQuery.ToString();

//                defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id);
//                var default2Sql = defaultLinqQuery.ToString();
//                //Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);
                
//                defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoin2Entity = defaultLinqQuery
//                    .LeftJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id);
//                var default3Sql = defaultLinqQuery.ToString();
//            }
//        }

//        [Fact]
//        public void whereGenSql()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var constKeyId = "123";
//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntity = defaultLinqQuery.Where((TESTENTITY t) => t.Id == constKeyId);
                    
//                var default1Sql = defaultLinqQuery.ToString();

//                defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY t) => t.Id == constKeyId);
//                var default2Sql = defaultLinqQuery.ToString();
//                //Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);
//            }
//        }
//        [Fact]
//        public void whereMultTableGenSql()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var constKeyId = "123";
                
//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY t, TESTENTITY2 b) => (t.Id == constKeyId) ||(b.Id == constKeyId));
//                var default2Sql = defaultLinqQuery.ToString();
//                //Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);
                
//                var defaultLinqQuery1 = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity1 = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY t, TESTENTITY2 b) => (t.Id == constKeyId) ||(b.Id == constKeyId));
//                var default2Sql1 = defaultLinqQuery.ToString();
//            }
//        }

//        [Fact]
//        public void selectGenSql()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var constKeyId = "123";
//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                 var testEntity = defaultLinqQuery.InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .SelectToDynamicList((TESTENTITY t) => new { Id =  t.Id, TestEntity2 = new { Id =  t.TESTENTITY2ID } });

//                var default1Sql = defaultLinqQuery.ToString();

//                var defaultLinqQuery1 = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntity1 = defaultLinqQuery1.InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                   .SelectToDynamicList((TESTENTITY t,TESTENTITY2 b) => new { Id = t.Id, TestEntity2 = new { Id = b.Id } });

//                var default1Sql1 = defaultLinqQuery1.ToString();


//                defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY2 t) => t.Id == constKeyId);
//                var default2Sql = defaultLinqQuery.ToString();
//                //Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);
//            }
//        }

//        [Fact]
//        public void orderByGenSql()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var constKeyId = "123";

//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntity = defaultLinqQuery.OrderBy(t => t.Id);

//                var default1Sql = defaultLinqQuery.ToString();


//                var defaultLinqQuery1 = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntity1 = defaultLinqQuery1.OrderBy(t =>new { t.Id ,t.TESTENTITY2ID});

//                var default1Sql1 = defaultLinqQuery1.ToString();

//                defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY2 t) => t.Id == constKeyId)
//                    .OrderBy(t => t.Id);
//                var default2Sql = defaultLinqQuery.ToString();


//               var defaultLinqQuery3 = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity3 = defaultLinqQuery3
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY t) => t.Id == constKeyId)
//                    .OrderBy(t =>new { t.Id,t.TESTENTITY2ID });
//                var default3Sql = defaultLinqQuery3.ToString();
//                //Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);


//                var defaultLinqQuery4 = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity4 = defaultLinqQuery4
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY t) => t.Id == constKeyId)
//                    .OrderByDescending(t => new { t.Id, t.TESTENTITY2ID });
//                var default4Sql = defaultLinqQuery4.ToString();


//                var defaultLinqQuery5 = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity5 = defaultLinqQuery5
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY t) => t.Id == constKeyId)
//                    .OrderBy(t => new { t.Id, t.TESTENTITY2ID })
//                    .ThenBy(t => new { t.CREATER });
//                var default5Sql = defaultLinqQuery5.ToString();



//                var defaultLinqQuery6 = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity6 = defaultLinqQuery6
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY t) => t.Id == constKeyId)
//                    .OrderBy(t => new { t.Id, t.TESTENTITY2ID })
//                    .ThenByDescending(t => new { t.CREATER });
//                var default6Sql = defaultLinqQuery6.ToString();
//            }



//        }


//        [Fact]
//        public void pageGenSql()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var constKeyId = "123";
//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntity = defaultLinqQuery.InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                   .Paging((TESTENTITY t) => new { Id = t.Id, TestEntity2 = new { Id = t.TESTENTITY2ID } }, new Data.Pager.Page()
//                   {
//                       page = 2,
//                       pageSize = 10,
//                        sortColumn = "Id",
//                         sortOrder = "asc"
                         
//                   });

//                var default1Sql = defaultLinqQuery.ToString();

//                var defaultLinqQuery1 = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntity1 = defaultLinqQuery1.InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                   .SelectToDynamicList((TESTENTITY t, TESTENTITY2 b) => new { Id = t.Id, TestEntity2 = new { Id = b.Id } });

//                var default1Sql1 = defaultLinqQuery1.ToString();


//                defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY2 t) => t.Id == constKeyId);
//                var default2Sql = defaultLinqQuery.ToString();
//                //Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);
//            }
//        }
        
        
//        [Fact]
//        public void countGenSql()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var constKeyId = "123";
//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntityLinq = defaultLinqQuery.InnerJoin((TESTENTITY t) => t.TESTENTITY2ID, (TESTENTITY2 b) => b.Id);

//                var entityCount = testEntityLinq.Count();

//                var entityCount2 = testEntityLinq.SelectToDynamicList((TESTENTITY t,TESTENTITY2 b) => new{ t.TESTENTITY2ID, b.Id});
//            }
//        }


//        [Fact]
//        public void joinManyGenSql()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID, (TESTENTITY2 b) => b.Id);

//                var default1Sql = defaultLinqQuery.ToString();

//                defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testLeftJoinEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 b) => b.Id);
//                var default2Sql = defaultLinqQuery.ToString();
//                //Assert.NotEqual(testEntity.TESTENTITY2ID, newGuid);

//                var testLeftJoin2Entity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID_NULLABLE, (TESTENTITY2 t2) => t2.Id)
//                    .InnerJoin((TESTENTITY2 t2) => t2.Id, (TESTENTITY3 t3) => t3.Id)
//                    .SelectToList((TESTENTITY t, TESTENTITY2 t2, TESTENTITY3 t3) =>
//                    new testDTO
//                    {
//                        Id = t.Id,
//                        Text = t2.Text,
//                        TESTENTITYID =  t3.TESTENTITYID
//                    });
//                var default3Sql = defaultLinqQuery.ToString();
//            }
//        }

//        [Fact]
//        public void joinWhere()
//        {
//            using (var context = new BlocksDbContext())
//            {
//                var defaultLinqQuery = new DefaultLinqQueryable<TESTENTITY>(context.TestEntity.AsQueryable(), context);
//                var testEntity = defaultLinqQuery
//                    .InnerJoin((TESTENTITY t) => t.TESTENTITY2ID, (TESTENTITY2 b) => b.Id)
//                    .Where((TESTENTITY t) => t.TESTENTITY2ID == "123")
//                     .SelectToList((TESTENTITY b) => new TESTENTITY(){ Id = b.Id});
 
//            }
//        }
//        class testDTO 
//        {
//            public string Id { get; set; }
//            public string Text { get; set; }
//            public string TESTENTITYID { get; set; }

//        }
//    }
//}
