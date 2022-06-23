using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIWorkOut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("GetCustomerDetails")]
        [Authorize]
        public ActionResult<IEnumerable<object>> GetCustomerDetails()
        {
            List<Employee> lstEmployee = new List<Employee>
            { new Employee { id=1, Name="Gana",DepId=1,Salary=1000 },
                new Employee { id = 2, Name = "Mani", DepId = 2,Salary=2000 },
            new Employee { id=3, Name="Praba",DepId=3,Salary=2500 },
            new Employee { id=4, Name="Parvesh",DepId=4,Salary=3000 },
            new Employee { id=5, Name="Ajay",DepId=1,Salary=3000 },
            new Employee { id=6, Name="Jadeja",DepId=2,Salary=4000 }
            };

            List<Department> lstDepartment = new List<Department>
            {
                new Department{ id=1,Name="HR" },
                new Department{ id=2,Name="Sales"} 
            };

            var res = lstEmployee.GroupJoin(lstDepartment,
                emp => emp.DepId,
                dep => dep.id,
            (Emp, Dep) => new { EMP = Emp, DEP = Dep }).SelectMany(DepE => DepE.DEP.DefaultIfEmpty(),
            (ABC, DEP) => new { ABC.EMP.id, ABC.EMP.Name, DEPID =(DEP == null?0: DEP.id), DEPNAME = (DEP == null ? "" : DEP.Name) }
            ).ToArray();


            var res1 = (from emp in lstEmployee
                        join dep in lstDepartment on emp.DepId equals dep.id
into EmpWithDepartment
                        from EmpWithDep in EmpWithDepartment.DefaultIfEmpty()
                        select new { emp.id, emp.Name, DepId = (EmpWithDep == null) ? 0 : EmpWithDep.id, DepartmentName = (EmpWithDep == null) ? "" : EmpWithDep.Name }).ToList();


            var departmentwiseHighSalary = lstEmployee.Join(lstDepartment, em => em.DepId, de => de.id,
            (x, y) => new { A = x, B = y }
                ).GroupBy(a =>new { a.B.id,a.B.Name }).Select(a => new { Employee = a.OrderByDescending(c => c.A.Salary).FirstOrDefault() }).ToList();

            return res;
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


    public class Employee {
        public int id { get; set; }
        public string Name { get; set; }
        public int DepId { get; set; }
        public int Salary { get; set; }
    }
    public class Department {
        public int id { get; set; }
        public string Name { get; set; }
    }
}
