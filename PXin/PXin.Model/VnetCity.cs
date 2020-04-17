using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model
{
    public partial class VnetCity
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ProvinceId { get; set; }
        public int Status { get; set; }
    }
}
