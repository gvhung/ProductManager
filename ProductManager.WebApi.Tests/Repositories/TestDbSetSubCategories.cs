using ProductManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.WebApi.Tests
{
    public class TestDbSetSubCategories : TestDbSet<SubCategory>
    {
        public override SubCategory Find(params object[] keyValues)
        {
            return this.SingleOrDefault(subCategory => subCategory.SubCategoryId == (int)keyValues.Single());
        }
    }
}
