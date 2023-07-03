using PectraForms.WebApplication.BackEnd.API.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PectraForms.WebApplication.BackEnd.API.db
{
    public class DbContext
    {
        private const string DBName = @"db\forms.sqlite3";

        private static SQLiteConnection GetInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", Path.Combine(HostingEnvironment.MapPath("~"), DBName))
            );

            db.Open();

            return db;
        }

        public static ExpandoObject GetFormDefinition(int piFormularioId)
        {
            dynamic oForm = new ExpandoObject();
            using (var ctx = DbContext.GetInstance())
            {
                var query = "SELECT * FROM Formulario WHERE FormularioId=" + piFormularioId;
                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oForm.FormularioId = Convert.ToInt32(reader["FormularioId"]);
                            oForm.ActId = Convert.ToInt32(reader["ActId"]);
                            oForm.Nombre = reader["Nombre"].ToString();
                            oForm.EvitaHacerActivityEnd = Convert.ToBoolean(reader["EvitaHacerActivityEnd"]);
                            #region Secciones
                            string qrySec = "SELECT * FROM Seccion WHERE FormularioId=" + oForm.FormularioId.ToString();
                            using (var cmdSec = new SQLiteCommand(qrySec, ctx))
                            {
                                using (var rdrSec = cmdSec.ExecuteReader())
                                {
                                    oForm.Secciones = new List<object>();
                                    while (rdrSec.Read())
                                    {
                                        dynamic oSec = new ExpandoObject();
                                        oSec.Nombre = rdrSec["Nombre"].ToString();
                                        #region Filas
                                        string qryRow = "SELECT * FROM Fila WHERE SeccionId = '" + rdrSec["SeccionId"].ToString() + "' AND FormularioId=" + oForm.FormularioId.ToString();
                                        using (var cmdRow = new SQLiteCommand(qryRow, ctx))
                                        {
                                            using (var rdrRow = cmdRow.ExecuteReader())
                                            {
                                                oSec.Filas = new List<object>();
                                                while (rdrRow.Read())
                                                {
                                                    dynamic oRow = new ExpandoObject();
                                                    #region Controles
                                                    string qryCtr = "SELECT * FROM Control WHERE FilaId = " + rdrRow["FilaId"];
                                                    using (var cmdCtr = new SQLiteCommand(qryCtr, ctx))
                                                    {
                                                        using (var rdrCtr = cmdCtr.ExecuteReader())
                                                        {
                                                            oRow.Controles = new List<object>();
                                                            while (rdrCtr.Read())
                                                            {
                                                                dynamic oCtr = new ExpandoObject();
                                                                oCtr.ControlId = rdrCtr["ControlId"].ToString();
                                                                oCtr.ControlTypeId = rdrCtr["ControlTypeId"].ToString();
                                                                oCtr.Nombre = rdrCtr["Nombre"].ToString();
                                                                oCtr.InsideGrid = Convert.ToBoolean(rdrCtr["InsideGrid"]);
                                                                oCtr.Descripcion = rdrCtr["Descripcion"].ToString();
                                                                oCtr.SoloLectura = rdrCtr["SoloLectura"] != DBNull.Value ? (bool?)Convert.ToBoolean(rdrCtr["SoloLectura"]) : null;
                                                                oCtr.Requerido = rdrCtr["Requerido"] != DBNull.Value ? (bool?)Convert.ToBoolean(rdrCtr["Requerido"]) : null;
                                                                oCtr.MultiLinea = rdrCtr["MultiLinea"] != DBNull.Value ? (bool?)Convert.ToBoolean(rdrCtr["MultiLinea"]) : null;
                                                                oCtr.MaxLength = rdrCtr["MaxLength"] != DBNull.Value ? (int?)Convert.ToInt32(rdrCtr["MaxLength"]) : null;
                                                                #region OrigenDatos
                                                                oCtr.OrigenDatos = new ExpandoObject();
                                                                oCtr.OrigenDatos.Nombre = rdrCtr["OrigenDatos.Nombre"].ToString();
                                                                oCtr.OrigenDatos.Campo = rdrCtr["OrigenDatos.Campo"].ToString();
                                                                oCtr.OrigenDatos.TipoDato = rdrCtr["OrigenDatos.TipoDato"].ToString();
                                                                #endregion OrigenDatos
                                                                #region Lista
                                                                if (rdrCtr["TipoDeLista"] != DBNull.Value)
                                                                {
                                                                    oCtr.Lista = new ExpandoObject();
                                                                    oCtr.Lista.Tipo = rdrCtr["TipoDeLista"].ToString();
                                                                    oCtr.Lista.Ordenamiento = rdrCtr["Lista.Ordenamiento"] != DBNull.Value ? rdrCtr["Lista.Ordenamiento"].ToString() : null;
                                                                    switch ((string)oCtr.Lista.Tipo)
                                                                    {
                                                                        case "ListaFija":
                                                                            oCtr.Lista.ListaFija = new ExpandoObject();
                                                                            oCtr.Lista.ListaFija.Operacion = string.Format("Getsel{0}{1}", Common.MakeValidIdentifierName(oCtr.Nombre), rdrCtr["Sec"]);
                                                                            break;
                                                                        case "ListaDinamica":
                                                                            oCtr.Lista.ListaDinamica = new ExpandoObject();
                                                                            oCtr.Lista.ListaDinamica.Tabla = rdrCtr["ListaDinamica.Tabla"].ToString();
                                                                            oCtr.Lista.ListaDinamica.CampoMostrar = rdrCtr["ListaDinamica.CampoMostrar"].ToString();
                                                                            oCtr.Lista.ListaDinamica.CampoValor = rdrCtr["ListaDinamica.CampoValor"].ToString();
                                                                            oCtr.Lista.ListaDinamica.ParentControlId = rdrCtr["ListaDinamica.ParentControlId"] != DBNull.Value ? rdrCtr["ListaDinamica.ParentControlId"].ToString() : null;
                                                                            oCtr.Lista.ListaDinamica.CampoFiltro = rdrCtr["ListaDinamica.CampoFiltro"] != DBNull.Value ? rdrCtr["ListaDinamica.CampoFiltro"].ToString() : null;
                                                                            break;
                                                                    }
                                                                }
                                                                #endregion Lista

                                                                #region Validacion
                                                                if (rdrCtr["TipoValidacion"] != DBNull.Value && !(rdrCtr["TipoValidacion"].ToString()).Equals("SinValidacion"))
                                                                {
                                                                    oCtr.Validacion = new ExpandoObject();
                                                                    oCtr.Validacion.Tipo = rdrCtr["TipoValidacion"].ToString();
                                                                    switch ((string)oCtr.Validacion.Tipo)
                                                                    {
                                                                        case "PorComparacion":
                                                                            break;
                                                                        case "PorRango":
                                                                            break;
                                                                        case "PorExpresionRegular":
                                                                            oCtr.Validacion.Expresion = rdrCtr["Validacion.Expresion"].ToString();
                                                                            break;
                                                                    }
                                                                }
                                                                #endregion Validacion

                                                                oRow.Controles.Add(oCtr);
                                                            }
                                                        }
                                                    }
                                                    #endregion Controles
                                                    oSec.Filas.Add(oRow);
                                                }
                                            }
                                        }
                                        #endregion Filas
                                        oForm.Secciones.Add(oSec);
                                    }
                                }
                            }
                            #endregion Secciones
                        }
                    }
                }
            }
            return oForm;
        }


    }
}