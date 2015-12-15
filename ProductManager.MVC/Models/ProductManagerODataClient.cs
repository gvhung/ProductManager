using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.MVC.Models
{
    /// <summary>
    /// Classe estática para gerar clientes OData
    /// </summary>
    public static class ProductManagerODataClient
    {
        //Configurar aqui o endereço por defeito do web service
        private static string _address = "http://localhost:63574/odata/";

        public static ODataClient GetDefaultClient()
        {
            return new ODataClient(_address);
        }

        public static ODataClient GetCustomClient(string address)
        {
            return new ODataClient(address);
        }
    }
}
