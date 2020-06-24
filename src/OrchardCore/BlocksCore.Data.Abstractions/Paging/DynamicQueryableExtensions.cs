//using System;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Linq.Expressions;
//using System.Reflection;
//using Blocks.Framework.Data.Pager;
//using BlocksCore.SyntacticAbstractions.Types;

//namespace Blocks.Framework.Data.Paging
//{
//    public static class DynamicQueryableExtensions
//    {
//        public static PageList<TSource> PageResult<TSource>(this IQueryable source, int page, int pageSize)
//        {
//            Check.NotNull<IQueryable>(source, nameof (source));
//            Check.Condition<int>(page, (Predicate<int>) (p => p > 0), nameof (page));
//            Check.Condition<int>(pageSize, (Predicate<int>) (ps => ps > 0), nameof (pageSize));
//            var pagedResult = new PageList<TSource>()
//            {
//                PagerInfo = new Page()
//                {
//                    page = page,
//                    pageSize = pageSize,
//                    records = source.Count(),
//                },
//            };
//            // pagedResult.PagerInfo.total = (int)Math.Ceiling((double)pagedResult.PagerInfo.records / (double)pageSize);
//            pagedResult.Rows = Page(source, page, pageSize).ToDynamicList<TSource>();
//            return pagedResult;
//        }

//        public static IQueryable Page(this IQueryable source, int page, int pageSize)
//        {
//            Check.NotNull<IQueryable>(source, nameof (source));
//            Check.Condition<int>(page, (Predicate<int>) (p => p > 0), nameof (page));
//            Check.Condition<int>(pageSize, (Predicate<int>) (ps => ps > 0), nameof (pageSize));
             
//            return Skip(source,(page - 1) * pageSize).Take(pageSize);
//        }
//        public static IQueryable Skip(this IQueryable source, int count)
//        {
//            Check.NotNull<IQueryable>(source, nameof (source));
//            Check.Condition<int>(count, (Predicate<int>) (x => x >= 0), nameof (count));

//            var typeDynamicQuery = typeof(System.Linq.Dynamic.Core.DynamicQueryableExtensions);
//            var GetMethodType = typeDynamicQuery.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
//                .LastOrDefault(t => t.Name == "GetMethod" && !t.IsGenericMethod);
//            var SkipMethod = GetMethodType.Invoke(null, new object[] { "Skip", 1, (Func<MethodInfo, bool>) null});
//            var CreateQueryType = typeDynamicQuery.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
//                .FirstOrDefault(t => t.Name == "CreateQuery"&& !t.IsGenericMethod && t.GetParameters().Length == 3 && 
//                                     t.GetParameters()[0].ParameterType == typeof(MethodInfo) &&
//                    t.GetParameters()[1].ParameterType == typeof(IQueryable) &&
//                    t.GetParameters()[2].ParameterType == typeof(Expression)

//                    );
//            var result = CreateQueryType.Invoke("CreateQuery", new object[] { SkipMethod, source, (Expression) Expression.Constant((object) count)});
            
//            return result as IQueryable ;
//        }
//    }
//}