using BlocksCore.Application.Abstratctions;
using BlocksCore.Application.Abstratctions.Attributes;
using BlocksCore.Application.Abstratctions.Controller.Attributes;
using BlocksCore.Web.Abstractions.HttpMethod;

namespace BlocksCore.Test.Application.Controller.TestModel
{
    public interface ITestAppService : IAppService
    {
        [BlocksActionName("Default")]
        void Default();
        [BlocksActionName("TestGet"), HttpMethod(HttpVerb.Get)]
        void TestGet();

        [BlocksActionName("TestDelete")]
        void TestDelete();

        [BlocksActionName("TestIgnore"), RemoteService(false)]
        void TestIgnore();
        
        void TestNoActionName();
    }
    public class TestAppService : ITestAppService
    {
        public void Default()
        {
            throw new System.NotImplementedException();
        }

       
        public void TestGet()
        {
            throw new System.NotImplementedException();
        }

        [HttpMethod(HttpVerb.Delete)]
        public void TestDelete()
        {
            throw new System.NotImplementedException();
        }

        public void TestIgnore()
        {
            throw new System.NotImplementedException();
        }

        public void TestNoActionName()
        {
            throw new System.NotImplementedException();
        }
    }

   
}