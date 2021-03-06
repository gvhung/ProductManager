﻿using ProductManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.WebApi.Tests
{
    public class TestDbSetEmployees : TestDbSet<Employee>
    {
        public override Employee Find(params object[] keyValues)
        {
            return this.SingleOrDefault(employee => employee.EmployeeId == (int)keyValues.Single());
        }
    }
}
