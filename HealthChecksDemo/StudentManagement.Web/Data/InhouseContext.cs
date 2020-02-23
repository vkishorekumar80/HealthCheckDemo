using Microsoft.EntityFrameworkCore;
using StudentManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Web.Data {
    public class InhouseContext : DbContext {
        public DbSet<InhouseStudent> Students { get; set; }
        public DbSet<InhouseCourse> Courses { get; set; }

        public InhouseContext(DbContextOptions<InhouseContext> options): base(options) {

        }
    }
}
