using System;
using System.Linq;
using System.Collections;
using HelloTestBO.DTO;
using System.Collections.Generic;

namespace HelloTestBO
{
    public class Service
    {
        public string GetHelloWorld()
        {
            return "Hi!!. Std2.0 EasyArchitect Test!!!.";
        }

        public IEnumerable<Employee> GetEmployees(Employee emp)
        {
            return new Employee[] {
                new Employee() {
                    Id = 1+emp.Id,
                    Name = $"Gelis Wu, And input={emp.Name}"
                }
            };
        }
    }
}
