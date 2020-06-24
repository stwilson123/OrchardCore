using BlocksCore.Application.Abstratctions.Datatransfer; 

namespace SysMgt.BussnessDTOModule.ThirdSystemType
{
    
    public class ThirdSystemTypePageResult 
    {
        public string ID { get; set; }
        public string SystemNo { get; set; }
        public string SystemName { get; set; }
        public string CreateDate { get; set; }
        public string Creater { get; set; } 
    }
}
