using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Web.Models {
    public class InhouseStudent {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsInhouse { get; set; }
    }

}
