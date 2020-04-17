using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model
{
    public partial class VnetRegion
    {
        public int Regionid { get; set; }
        public int CityId { get; set; }
        public string RegionName { get; set; }
        public int Status { get; set; }
    }
}
