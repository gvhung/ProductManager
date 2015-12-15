using ProductManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.WebApi.Tests
{
    public class TestDbSetProducts : TestDbSet<Product>
    {
        public override Product Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.ProductId == (int)keyValues.Single());
        }
    }
}
