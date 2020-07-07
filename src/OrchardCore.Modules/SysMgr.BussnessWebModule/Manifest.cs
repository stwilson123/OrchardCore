
using BlocksCore.Abstractions.Extensions;

[assembly: Module(
    Name = "SysMgrBussenssModule",
    Author = "The Orchard Team",
    Website = "https://orchardproject.net",
    Version = "2.0.0",
    Description = "Creates an admin section for the site.",
    ChildNames = new[] { "SysMgt.BussnessApplicationModule", "SysMgt.BussnessDTOModule", "SysMgt.BussnessDomainModule", "SysMgt.BussnessRespositoryModule", "SysMgr.EntityModule" }
)]
