using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskLKDS.Models
{
    public class DataBase
    {
        public List<Organization> Organizations { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Position> Positions { get; set; }
    }
}
