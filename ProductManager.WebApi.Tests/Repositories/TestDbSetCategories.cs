using ProductManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.WebApi.Tests
{
    public class TestDbSetCategories : TestDbSet<Category>
    {
        public override Category Find(params object[] keyValues)
        {
            return this.SingleOrDefault(category => category.CategoryId == (int)keyValues.Single());
        }
    }
}
