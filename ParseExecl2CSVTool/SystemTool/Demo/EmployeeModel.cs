using System.Collections.Generic;
using System.Linq;

namespace Demo
{
    public class EmployeeModel : DbHelper
    {
        public static List<Employee> GetAll()
        {
            using (var db = new DbDemoDataContext(ConnectionString))
            {
                var emp = from x in db.Employees select x;
                return emp.ToList();
            }
        }
    }
}