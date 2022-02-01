
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Text;
using System.Web;

using Acond.ApiRest.Models;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SisCob.Business;
using SisCob.Entity;
using LPBInfo.DB;

using LPBinfo.Log;

namespace Acond.ApiRest.Controllers
{

    /// <summary>
    /// Controller Responsável por gerenciar todas as interações de Agendamento e Disparo de E-mail (Boleto)
    /// </summary>
    [RoutePrefix("api/Schedule")]
    public class ScheduleController : ApiController
    {

        ///<summary>
        ///Interface - Implantação de Lotes para Envio de E-mails.
        ///{"Cliente": ID do Cliente WebCond,
        ///"Lote de Recibos para Envio": Lista de Recibos para Envio}
        ///</summary>
        [AcceptVerbs("POST")]
        [Route("GravarLoteAgendamento")]
        public HttpResponseMessage GravarLoteAgendamento(HttpRequestMessage ObjRequest)
        {
            HttpResponseMessage response = null;

            try
            {
                string lstrRequest = ObjRequest.Content.ReadAsStringAsync().Result;

                if ((lstrRequest != "") && (!string.IsNullOrEmpty(lstrRequest)))
                {

                    WebAcondScheduleEnt ObjListModel = new  WebAcondScheduleEnt();
                    ObjListModel = (WebAcondScheduleEnt)JsonConvert.DeserializeObject(lstrRequest, ObjListModel.GetType());

                    if (ObjListModel != null)
                    {

                        AcondScheduleBLL ObjBussines = new AcondScheduleBLL();
                        ObjBussines.SetRegistroScheduleCond(ObjListModel);

                        if (ObjBussines.Erro == 0)
                        {
                            string lstrRetRespose = "Processo finalizado com sucesso [ Lote = " + ObjBussines.IDLOTE.ToString("00000000") + " | Qtd = " + ObjBussines.QTDLOTE.ToString("00000") + " ] ";
                            response = Request.CreateResponse(HttpStatusCode.OK);
                            response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                            return response;
                        }
                        else
                        {
                            string lstrRetRespose = lstrRequest;
                            response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                            response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                            return response;
                        }

                    }
                    else
                    {
                        string lstrRetRespose = "Request com erro - " + lstrRequest;
                        response = Request.CreateResponse(HttpStatusCode.NotImplemented);
                        response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                        return response;
                    }

                }
                else
                {
                    string lstrRetRespose = "Request NULL";
                    response = Request.CreateResponse(HttpStatusCode.NotImplemented);
                    response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                    return response;
                }
                
            }
            catch (Exception ex)
            {
                string lstrRetRespose = ex.Message;
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                return response;
            }
        }

        ///<summary>
        ///Interface - Envio de Boletos Agendados.
        ///{Processo é executado automáticamente pelo CRONJOB do Provedor:}
        ///</summary>
        [AcceptVerbs("POST")]
        [Route("ExecutarLoteAgendado")]
        public HttpResponseMessage ExecutarLoteAgendado()
        {
            HttpResponseMessage response = null;

            string lstrPathBase = HttpContext.Current.Server.MapPath("/api");

            try
            {
                AcondScheduleBLL ObjBLL = new AcondScheduleBLL();

                ObjBLL.SetExecutarAgendamento(lstrPathBase);

                if (ObjBLL.Erro == 0)
                {
                    string lstrRetRespose = "OK";
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                    return response;
                }
                else
                {
                    string lstrRetRespose = "Erro - " + ObjBLL.MsgErro;
                    response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                    response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                    LogSis ObjLog = new LogSis();
                    ObjLog.SetLogXML("ExecAgendaGeral", "Erro", lstrRetRespose, false, lstrPathBase + "/");

                    return response;

                }
               
            }
            catch (Exception ex)
            {
                string lstrRetRespose = ex.Message;
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                LogSis ObjLog = new LogSis();
                ObjLog.SetLogXML("ExecAgendaGeral", "Erro", lstrRetRespose, false, lstrPathBase + "/");

                return response;
            }
        }


        ///<summary>
        ///Interface - Envio de Boletos Agendados.
        ///{Processo é executado automáticamente pelo CRONJOB do Provedor:}
        ///</summary>
        [AcceptVerbs("GET")]
        [Route("GetLoteAgendado")]
        public HttpResponseMessage GetLoteAgendado()
        {
            HttpResponseMessage response = null;

            try
            {
                //AcondScheduleBLL ObjBLL = new AcondScheduleBLL();
                //List<WebAcondScheduleLoteEnt> ObjList = new List<WebAcondScheduleLoteEnt>();
                //ObjList = ObjBLL.GetLoteAgendamento();

                //if (ObjBLL.Erro == 0)
                //{
                //    string lstrRetJSON = JsonConvert.SerializeObject(ObjList, Formatting.None);
                //    string lstrRetRespose = "OK|" + lstrRetJSON;
                //    response = Request.CreateResponse(HttpStatusCode.OK);
                //    response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                //    return response;
                //}
                //else
                //{
                //    string lstrRetRespose = "Erro - " + ObjBLL.MsgErro;
                //    response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                //    response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                //    return response;

                //}

                BD.AbrirConexao();
                if (BD.Erro == 0)
                {
                    BD.FecharConexao();
                    string lstrRetRespose = "OK";
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                }
                else
                {
                    string lstrRetRespose = "Erro - " + BD.MsgErro;
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");
                }

                return response;

            }
            catch (Exception ex)
            {
                string lstrRetRespose = ex.Message;
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(lstrRetRespose, Encoding.UTF8, "application/json");

                return response;
            }
        }

    }
}