using BlocksCore.Application.Abstratctions;
using BlocksCore.Web.Abstractions.Route;
using Microsoft.AspNetCore.Mvc;

namespace BlocksCore.Test.WebApi.Model
{
    
    [Area("TestModule")]
    [HttpArea("TestModule1")]
    public class TestWebApiService : IAppService
    {
        
    }
}