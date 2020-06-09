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
        void TransactionSuccess(dynamic inputData);

        string TransactionWhenException(dynamic inputData);
    }



}
