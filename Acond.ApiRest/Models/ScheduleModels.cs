using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SisCob.Entity;

namespace Acond.ApiRest.Models
{
    public class ScheduleModel
    {
        public Int32 ID_REG { get; set; }
        public Int32 ID_LOTE { get; set; }
        public int ST_TP_REG { get; set; }
        public Int32 FKCOND { get; set; }
        public Int32 FKUNID { get; set; }
        public Int32 FKLOC { get; set; }
        public Int32 FKINQ { get; set; }
        public Int32 NU_RECIBO { get; set; }
    }
}