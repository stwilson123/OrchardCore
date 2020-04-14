using System.Collections.ObjectModel;
using System.Reflection;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Test.WebApi.Model;
using BlocksCore.WebAPI;
using BlocksCore.WebAPI.Controllers;
using BlocksCore.WebAPI.Controllers.Manager;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Moq;
using Xunit;

namespace BlocksCore.Test.WebApi.Conventions
{
    public class ControllerModelConventionTests
    {
        private MvcControllerManager _mvcControllerManager;
        public ControllerModelConventionTests()
        {
            var mockMvcControllerManager = new Mock<MvcControllerManager>();
                mockMvcControllerManager.Setup(m => m.GetAll())
                .Returns(new Collection<DefaultControllerInfo<MvcControllerActionInfo>>()
                {
                    new DefaultControllerInfo<MvcControllerActionInfo>("TestModule", nameof(TestWebApiService),
                         typeof(TestWebApiService),typeof(IAppService),typeof(IAppService),null
                        )
                    
                });
            _mvcControllerManager = mockMvcControllerManager.Object;
        }
        
        [Fact]
        public void RouteAttrbute_should_ConvertTo_Mvc_Attrute_Others_justCopy()
        {
            //var convention = new ControllerModelConvention(_mvcControllerManager);
            //var controllerModel = new ControllerModel(typeof(TestWebApiService).GetTypeInfo(),new object[]{});

            //convention.Apply(controllerModel);
        }
    }
}