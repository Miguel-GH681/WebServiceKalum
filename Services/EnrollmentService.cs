using FinalAdoSoap.Models;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace FinalAdoSoap.Services{
    public class EnrollmentService : IEnrollmentService
    {
        private AppLog appLog = new AppLog();

        private SqlConnection connection = new SqlConnection("Server=localhost;Database=kalum_tarea;User Id=sa;Password=INICIO.2022;");

        public EnrollmentResponse EnrollmentProcess(EnrollmentRequest er)
        {
            appLog.ResponseTime = Convert.ToInt16(DateTime.Now.ToString("fff"));
            EnrollmentResponse respuesta = null;
            Aspirante AspiranteEncontrado = BuscarAspirante(er.NoExpediente);

            if(AspiranteEncontrado == null){
                respuesta = new EnrollmentResponse {Codigo=204, Respuesta=$"No existe el aspirante con el número de expediente {er.NoExpediente}"};
                ImprimirLog(204, $"No existen registros para el número de expediente {er.NoExpediente}", "Information");
            } else{
                respuesta = EjecutarProcedimiento(er);
            }
            return respuesta;
        }

        private EnrollmentResponse EjecutarProcedimiento(EnrollmentRequest er){
            EnrollmentResponse respuesta = null;
            SqlCommand cmd = new SqlCommand("sp_EnrollmentProcess", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@NoExpediente", er.NoExpediente));
            cmd.Parameters.Add(new SqlParameter("@Ciclo", er.Ciclo));
            cmd.Parameters.Add(new SqlParameter("@MesInicioPago", er.MesInicioPago));
            cmd.Parameters.Add(new SqlParameter("@CarreraId", er.CarreraId));
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();
                while(reader.Read()){
                    respuesta = new EnrollmentResponse(){ Respuesta = reader.GetValue(0).ToString(),
                                                         Carne = reader.GetValue(1).ToString() };
                    appLog.DateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    if(reader.GetValue(0).ToString().Equals("TRANSACTION SUCCESS")){
                        respuesta.Codigo = 201;
                        ImprimirLog(201, reader.GetValue(0).ToString(),"Information");
                    } else if(reader.GetValue(0).ToString().Equals("TRANSACTION ERROR")){
                        respuesta.Codigo = 503;
                        ImprimirLog(503, reader.GetValue(0).ToString(),"Error");
                    } else{
                        respuesta.Codigo = 503;
                        ImprimirLog(503, "Error al momento de llamar al procedimiento almacenado","Error");
                    }
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                respuesta = new EnrollmentResponse{Codigo=503, Respuesta="Error al momento de llamar al procedimiento almacenado", Carne = "0"};
                ImprimirLog(503, "Error al momento de llamar al procedimiento almacenado", "Error");
            } finally{
                connection.Close();
            }
            return respuesta;
        }

        private Aspirante BuscarAspirante(string noExpediente){
            Aspirante resultado = null;
            SqlDataAdapter daAspirante = new SqlDataAdapter($"select * from Aspirante a where a.NoExpediente = '{noExpediente}';", connection);
            DataSet dsAspirante = new DataSet();
            daAspirante.Fill(dsAspirante, "Aspirante");
            if(dsAspirante.Tables["Aspirante"].Rows.Count > 0){
                resultado = new Aspirante{
                    NoExpediente = dsAspirante.Tables["Aspirante"].Rows[0][0].ToString(),
                    Apellidos = dsAspirante.Tables["Aspirante"].Rows[0][1].ToString(),
                    Nombres = dsAspirante.Tables["Aspirante"].Rows[0][2].ToString(),
                    Direccion = dsAspirante.Tables["Aspirante"].Rows[0][3].ToString(),
                    Telefono = dsAspirante.Tables["Aspirante"].Rows[0][4].ToString(),
                    Email = dsAspirante.Tables["Aspirante"].Rows[0][5].ToString(),
                    Estatus = dsAspirante.Tables["Aspirante"].Rows[0][6].ToString(),
                    ExamenId = dsAspirante.Tables["Aspirante"].Rows[0][7].ToString(),
                    CarreraId = dsAspirante.Tables["Aspirante"].Rows[0][8].ToString(),
                    JornadaId = dsAspirante.Tables["Aspirante"].Rows[0][9].ToString()
                };
            }
            return resultado;
        }

        public string Test(string s)
        {
            Console.WriteLine("Test Method Executed");
            return s;
        }

        private void ImprimirLog(int responseCode, string message, string typeLog){
            appLog.ResponseCode = responseCode;
            appLog.Message = message;
            appLog.ResponseTime = Convert.ToInt16(DateTime.Now.ToString("fff")) - appLog.ResponseTime;
            if(typeLog.Equals("Information")){
                appLog.Level = 20;
                Log.Information(JsonSerializer.Serialize(appLog));
            } else if(typeLog.Equals("Error")){
                appLog.Level = 40;
                Log.Error(JsonSerializer.Serialize(appLog));
            } else if(typeLog.Equals("Debug")){
                appLog.Level = 10;
                Log.Debug(JsonSerializer.Serialize(appLog));
            }
        }
    }
}