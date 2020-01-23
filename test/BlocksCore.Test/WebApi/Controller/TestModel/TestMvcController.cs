using BlocksCore.Application.Abstratctions.Attributes;
using BlocksCore.Application.Abstratctions.Controller.Attributes;
using BlocksCore.Web.Abstractions.HttpMethod;

namespace BlocksCore.Test.WebApi.Controller.TestModel
{
   
    public class TestMvcController : Microsoft.AspNetCore.Mvc.Controller
    {
        [BlocksActionName("Default")]
        public void Default()
        {
            throw new System.NotImplementedException();
        }

     
        [BlocksActionName("TestDelete"),HttpMethod(HttpVerb.Delete)]
        public void TestDelete()
        {
            throw new System.NotImplementedException();
        }

        [BlocksActionName("TestIgnore"), RemoteService(false)]
        public void TestIgnore()
        {
            throw new System.NotImplementedException();
        }

        public void TestNoActionName()
        {
            
        }
    }

   
}