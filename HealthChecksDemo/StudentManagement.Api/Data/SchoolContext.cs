using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Api.Data {
    public class SchoolContext : DbContext {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public SchoolContext(DbContextOptions options): base(options) {

        }
    }
}
