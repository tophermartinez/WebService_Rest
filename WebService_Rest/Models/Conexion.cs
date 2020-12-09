using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebService_Rest.Models
{
    public class Conexion
    {
        public static string ConOra = ConfigurationManager.ConnectionStrings["OraConexAmazon"].ConnectionString;
  
    }
}
