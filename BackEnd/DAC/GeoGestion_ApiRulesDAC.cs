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
    public partial class GeoGestion_ApiRulesDAC
    {
        public static List<GeoGestion_ApiRulesBE> GetAll()
        {
            List<GeoGestion_ApiRulesBE> lstGeoGestion_ApiRules = new List<GeoGestion_ApiRulesBE>();

            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Get Values
            using (IDbConnection db = new SqlConnection(szCnnString))
            {
                lstGeoGestion_ApiRules = db.Query<GeoGestion_ApiRulesBE>("SELECT * FROM [GeoGestion_ApiRules]").ToList();
            }

            return lstGeoGestion_ApiRules;
        }

        public static List<GeoGestion_ApiRulesBE> GetByParam(Dictionary<string, object> pConditionValues)
        {
            string szSqlWhere = string.Empty;
            List<GeoGestion_ApiRulesBE> lstGeoGestion_ApiRules = new List<GeoGestion_ApiRulesBE>();

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
                    lstGeoGestion_ApiRules = db.Query<GeoGestion_ApiRulesBE>("SELECT * FROM [GeoGestion_ApiRules] " + szSqlWhere, oParameters).ToList();
                }
            }

            return lstGeoGestion_ApiRules;
        }

        public static void Insert(GeoGestion_ApiRulesBE pGeoGestion_ApiRulesBE)
        {
            if (pGeoGestion_ApiRulesBE != null)
            {
                // Get ConnectionString
                string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

                // Insert
                using (IDbConnection db = new SqlConnection(szCnnString))
                {
                                        string szSqlQuery = "INSERT INTO [GeoGestion_ApiRules] ([ConsultasPorSegundo], [ConsultasPorUsuarioPorHora], [MaximoResultados]) VALUES (@ConsultasPorSegundo, @ConsultasPorUsuarioPorHora, @MaximoResultados)";
                                        szSqlQuery += "; SELECT CAST(SCOPE_IDENTITY() AS int)";
                    
                    pGeoGestion_ApiRulesBE.Id = db.ExecuteScalar<int>(szSqlQuery, pGeoGestion_ApiRulesBE);
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
                        string szSqlUpdate = string.Format("UPDATE [GeoGestion_ApiRules] SET {0} WHERE {1}", szSqlSet, szSqlWhere);
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
                string szSqlQuery = "DELETE FROM [GeoGestion_ApiRules] WHERE [Id] = @Id";
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
                    string szSqlQuery = "DELETE FROM [GeoGestion_ApiRules] " + szSqlWhere;
                    db.Execute(szSqlQuery, oParameters);
                }
            }
        }

        
    }
}
