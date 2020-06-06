using System;
using System.Collections.Generic;
using System.Text;
using WebApiTestModule.DTO;
using BlocksCore.Application.Abstratctions;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTestModule
{
    public interface IDbAppService : IAppService
    {
        string TransactionSuccess(dynamic inputData);

    }



}
