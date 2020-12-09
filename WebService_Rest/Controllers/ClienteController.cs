using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebService_Rest.Business;
using static WebService_Rest.Business.Entidades;

namespace WebService_Rest.Controllers
{
    public class ClienteController : ApiController 
    {


        [HttpPost]
        [Route("API/Login")]

        public RespuestaLogin Post(PeticionLogin peti)
        {
            Metodos metodo = new Metodos();


            return metodo.Login(peti);
        }


        [HttpPost]
        [Route("API/DashBoard")]

        public List<DashboardGen> Post(PeticionDasboard petidash)
        {
            Metodos metodo = new Metodos();


            return metodo.Dasboard(petidash);
        }
    }
}
