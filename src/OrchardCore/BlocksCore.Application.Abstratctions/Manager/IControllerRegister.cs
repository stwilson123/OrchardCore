using BlocksCore.Application.Abstratctions.Controller;

namespace BlocksCore.Application.Abstratctions.Manager
{
    public interface IControllerRegister
    {
        void Register(IControllerInfo controllerInfo);
    }
}