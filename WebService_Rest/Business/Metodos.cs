using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_Rest.Models;
using static WebService_Rest.Business.Entidades;

namespace WebService_Rest.Business
{
    public class Metodos
    {
        Entidades ent;
        public Metodos()
        {
            ent = new Entidades();
        }

        public RespuestaLogin Login( PeticionLogin peti)
        {
            List<string> UserPasword = new List<string>();

            try
            {
                using (OracleConnection cn = new OracleConnection(Conexion.ConOra))
                {
                    cn.Open();

                    OracleCommand cmd = new OracleCommand("SELECT p.id_perfil FROM PERFIL P " +
                        "INNER JOIN USUARIO US ON P.ID_PERFIL = US.ID_PERFIL where US.NOMBRE_USUARIO = :usu AND CONTRASENA = :pass", cn);
                    OracleCommand comando = new OracleCommand("SELECT * FROM USUARIO WHERE NOMBRE_USUARIO = :usu AND CONTRASENA = :pass", cn);

                    cmd.Parameters.Add(":usu", peti.usu);
                    cmd.Parameters.Add(":pass", peti.pass);
                    // AddWithValue
                    comando.Parameters.Add(":usu", peti.usu);
                    comando.Parameters.Add(":pass", peti.pass);

                    OracleDataReader lector = comando.ExecuteReader();
                    OracleDataReader _reader = cmd.ExecuteReader();

                    if (lector.Read() == true)
                    {

                        var user = lector.GetString(9);
                        var psw = lector.GetString(10);

                        if (_reader.Read())
                        {
                            var Perfil = _reader.GetInt32(0);

                            UserPasword.Add(Perfil.ToString());
                        }

                        UserPasword.Add(user);

                        UserPasword.Add(psw);

                        RespuestaLogin resp = new RespuestaLogin { CodEstado = 1, Descripcion = "Usuario y Pass Correcto" };
                        cn.Close();
                        return resp;

                    }
                    else
                    {


                        RespuestaLogin resp = new RespuestaLogin { CodEstado = 0, Descripcion = "Usuario y Pass InCorrecto" };
                        cn.Close();
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {

                RespuestaLogin resp = new RespuestaLogin { CodEstado = -1, Descripcion = " "+ ex.InnerException +" "+ex.Message };
                return resp;
            }
           
        }


        public List<DashboardGen> Dasboard(PeticionDasboard petidash)
        {
            List<DashboardGen> list = new List<DashboardGen>();


            using (OracleConnection cn = new OracleConnection(Conexion.ConOra))
            {
                cn.Open();
                using (OracleCommand cmd = new OracleCommand("SP_DASHBOARD_GENERICO", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("P_RUT",  OracleDbType.Int32)).Value = petidash.rut;
                    cmd.Parameters.Add(new OracleParameter("P_EMPRESA", OracleDbType.Int32)).Value = petidash.rut_empresa;
                    //cmd.Parameters.Add(new OracleParameter("P_RUT", OracleType.Cursor)).Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new OracleParameter("P_CURSOR", OracleDbType.RefCursor)).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            DashboardGen uni = new DashboardGen();
                            uni.NOMBRE_UNIDAD = Convert.ToString(dr["NOMBRE_UNIDAD"]);
                            uni.FECHACREACION = Convert.ToDateTime(dr["FECHACREACION"]);
                            uni.FECHA_ESTIMADA = Convert.ToString(dr["FECHA_ESTIMADA"]);
                            uni.FECHA_TERMINO = Convert.ToString(dr["FECHA_TERMINO"]);
                            uni.Tareas_ter = Convert.ToInt32(dr["Cant_tareas_Ter"]);
                            uni.Cant_tareas_tot = Convert.ToInt32(dr["Cant_tareas_tot"]);
                            uni.procentaje = Convert.ToInt32(dr["Porcentaje"]);
                            uni.ESTADO = Convert.ToString(dr["ESTADO"]);
                            uni.ATRASO = Convert.ToInt32(dr["Atraso"]);
                            uni.nombre_usurio = Convert.ToString(dr["nombre_usuario"]);

                            list.Add(uni);
                        }
                        return list;
                    }

                }
            }

        }

    }
}