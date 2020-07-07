
using BlocksCore.Abstractions.Extensions;

[assembly: Module(
    Name = "BussnessWebModule",
    Author = "The Orchard Team",
    Website = "https://orchardproject.net",
    Version = "2.0.0",
    Description = "Creates an admin section for the site.",
    Category = "Infrastructure",
    ChildNames = new[] { "Blocks.BussnessApplicationModule", "Blocks.BussnessDTOModule", "Blocks.BussnessDomainModule", "Blocks.BussnessRespositoryModule", "Blocks.EntityModule" }
)]
