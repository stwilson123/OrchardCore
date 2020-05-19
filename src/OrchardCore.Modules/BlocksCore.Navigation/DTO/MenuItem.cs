using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Navigation.DTO
{
    public class MenuItem
    {
        public string uId { get; set; }


        public string Icon { get; set; }

        public long Order { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set;}
    }

}
