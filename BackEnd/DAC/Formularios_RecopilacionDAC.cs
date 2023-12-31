//------------------------------------------------------------------------------
// <auto-generated>
//   Este codigo fue generado por Pectra Forms
// 
//   Cambios en este archivo pueden ocasionar un comportamiento incorrecto.
// </auto-generated>
//------------------------------------------------------------------------------

using Dapper;
using Pectra.Forms.Utils.Encrypter;
using PectraForms.WebApplication.BackEnd.BusinessEntities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PectraForms.WebApplication.BackEnd.DataAccessComponents
{
    public partial class Formularios_RecopilacionDAC
    {
        public static List<Formularios_RecopilacionBE> GetAll()
        {
            List<Formularios_RecopilacionBE> lstFormularios_Recopilacion = new List<Formularios_RecopilacionBE>();

            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Get Values
            using (IDbConnection db = new SqlConnection(szCnnString))
            {
                lstFormularios_Recopilacion = db.Query<Formularios_RecopilacionBE>("SELECT * FROM [Formularios_Recopilacion]").ToList();
            }

            return lstFormularios_Recopilacion;
        }

        public static List<Formularios_RecopilacionBE> GetByParam(Dictionary<string, object> pConditionValues)
        {
            string szSqlWhere = string.Empty;
            List<Formularios_RecopilacionBE> lstFormularios_Recopilacion = new List<Formularios_RecopilacionBE>();

            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Set Sql Where
            if (pConditionValues != null && pConditionValues.Any())
            {
                DynamicParameters oParameters = new DynamicParameters();

                foreach (KeyValuePair<string, object> oKeyValuePair in pConditionValues)
                {
                    if (string.IsNullOrWhiteSpace(szSqlWhere) == false)
                        szSqlWhere += " AND ";

                    szSqlWhere = string.Format("{0} [{1}] = @{1}", szSqlWhere, oKeyValuePair.Key);

                    // dynamic parameters
                    oParameters.Add(oKeyValuePair.Key, oKeyValuePair.Value);
                }

                if (string.IsNullOrWhiteSpace(szSqlWhere) == false)
                    szSqlWhere = "WHERE " + szSqlWhere;

                // Get values
                using (IDbConnection db = new SqlConnection(szCnnString))
                {
                    // szSqlWhere => WHERE [FieldA] = @ValueA AND [FieldB] = @ValueB
                    lstFormularios_Recopilacion = db.Query<Formularios_RecopilacionBE>("SELECT * FROM [Formularios_Recopilacion] " + szSqlWhere, oParameters).ToList();
                }
            }

            return lstFormularios_Recopilacion;
        }

        public static void Insert(Formularios_RecopilacionBE pFormularios_RecopilacionBE)
        {
            if (pFormularios_RecopilacionBE != null)
            {
                // Get ConnectionString
                string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

                // Insert
                using (IDbConnection db = new SqlConnection(szCnnString))
                {
                                        string szSqlQuery = "INSERT INTO [Formularios_Recopilacion] ([TipoFormularioId], [FechaDesde], [FechaHasta], [JsonFormData], [FechaHoraRecopilacion]) VALUES (@TipoFormularioId, @FechaDesde, @FechaHasta, @JsonFormData, @FechaHoraRecopilacion)";
                                        szSqlQuery += "; SELECT CAST(SCOPE_IDENTITY() AS int)";
                    
                    pFormularios_RecopilacionBE.Id = db.ExecuteScalar<int>(szSqlQuery, pFormularios_RecopilacionBE);
                                    }
            }
        }

        public static void Update(Dictionary<string, object> pUpdateValues, Dictionary<string, object> pConditionValues)
        {
            string szSqlSet = string.Empty;
            string szSqlWhere = string.Empty;

            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Set the Sql Set clause & Where clause
            if (pUpdateValues != null && pUpdateValues.Any() && pConditionValues != null && pConditionValues.Any())
            {
                DynamicParameters oParameters = new DynamicParameters();

                // SET clause
                foreach (KeyValuePair<string, object> oKeyValuePair in pUpdateValues)
                {
                                        // exclude identity column
                    if (oKeyValuePair.Key == "Id")
                        continue;
                     
                    if (string.IsNullOrWhiteSpace(szSqlSet) == false)
                        szSqlSet += ",";

                    szSqlSet = string.Format("{0} [{1}] = @{1}", szSqlSet, oKeyValuePair.Key);

                    // dynamic parameters
                    oParameters.Add(oKeyValuePair.Key, oKeyValuePair.Value);
                }

                // WHERE clause
                foreach (KeyValuePair<string, object> oKeyValuePair in pConditionValues)
                {
                    if (string.IsNullOrWhiteSpace(szSqlWhere) == false)
                        szSqlWhere += " AND ";

                    szSqlWhere = string.Format("{0} [{1}] = @{1}", szSqlWhere, oKeyValuePair.Key);

                    // dynamic parameters
                    oParameters.Add(oKeyValuePair.Key, oKeyValuePair.Value);
                }

                // Update
                if (string.IsNullOrWhiteSpace(szSqlSet) == false && string.IsNullOrWhiteSpace(szSqlWhere) == false)
                {
                    using (IDbConnection db = new SqlConnection(szCnnString))
                    {
                        // szSqlSet => SET [FieldA] = @ValueA, [FieldB] = @ValueB || szSqlWhere => WHERE [FieldY] = @ValueY AND [FieldZ] = @ValueZ
                        string szSqlUpdate = string.Format("UPDATE [Formularios_Recopilacion] SET {0} WHERE {1}", szSqlSet, szSqlWhere);
                        db.Execute(szSqlUpdate, oParameters);
                    }
                }
            }
        }

        
        public static void Delete(object Id)
        {
            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Get Values
            using (IDbConnection db = new SqlConnection(szCnnString))
            {
                string szSqlQuery = "DELETE FROM [Formularios_Recopilacion] WHERE [Id] = @Id";
                db.Execute(szSqlQuery, new { Id });
            }
        }

        public static void DeleteByParam(Dictionary<string, object> pConditionValues)
        {
            string szSqlWhere = string.Empty;

            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Set Sql Where
            if (pConditionValues != null && pConditionValues.Any())
            {
                DynamicParameters oParameters = new DynamicParameters();

                foreach (KeyValuePair<string, object> oKeyValuePair in pConditionValues)
                {
                    if (string.IsNullOrWhiteSpace(szSqlWhere) == false)
                        szSqlWhere += " AND ";

                    szSqlWhere = string.Format("{0} [{1}] = @{1}", szSqlWhere, oKeyValuePair.Key);

                    // dynamic parameters
                    oParameters.Add(oKeyValuePair.Key, oKeyValuePair.Value);
                }

                if (string.IsNullOrWhiteSpace(szSqlWhere) == false)
                    szSqlWhere = "WHERE " + szSqlWhere;

                // Get values
                using (IDbConnection db = new SqlConnection(szCnnString))
                {
                    // szSqlWhere => WHERE [FieldA] = @ValueA AND [FieldB] = @ValueB
                    string szSqlQuery = "DELETE FROM [Formularios_Recopilacion] " + szSqlWhere;
                    db.Execute(szSqlQuery, oParameters);
                }
            }
        }

        
    }
}
