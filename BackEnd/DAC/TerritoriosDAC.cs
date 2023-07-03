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
    public partial class TerritoriosDAC
    {
        public static List<TerritoriosBE> GetAll()
        {
            List<TerritoriosBE> lstTerritorios = new List<TerritoriosBE>();

            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Get Values
            using (IDbConnection db = new SqlConnection(szCnnString))
            {
                lstTerritorios = db.Query<TerritoriosBE>("SELECT * FROM [Territorios]").ToList();
            }

            return lstTerritorios;
        }

        public static List<TerritoriosBE> GetByParam(Dictionary<string, object> pConditionValues)
        {
            string szSqlWhere = string.Empty;
            List<TerritoriosBE> lstTerritorios = new List<TerritoriosBE>();

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
                    lstTerritorios = db.Query<TerritoriosBE>("SELECT * FROM [Territorios] " + szSqlWhere, oParameters).ToList();
                }
            }

            return lstTerritorios;
        }

        public static void Insert(TerritoriosBE pTerritoriosBE)
        {
            if (pTerritoriosBE != null)
            {
                // Get ConnectionString
                string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

                // Insert
                using (IDbConnection db = new SqlConnection(szCnnString))
                {
                                        string szSqlQuery = "INSERT INTO [Territorios] ([Descripcion]) VALUES (@Descripcion)";
                                        szSqlQuery += "; SELECT CAST(SCOPE_IDENTITY() AS int)";
                    
                    pTerritoriosBE.TerritorioId = db.ExecuteScalar<int>(szSqlQuery, pTerritoriosBE);
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
                    if (oKeyValuePair.Key == "TerritorioId")
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
                        string szSqlUpdate = string.Format("UPDATE [Territorios] SET {0} WHERE {1}", szSqlSet, szSqlWhere);
                        db.Execute(szSqlUpdate, oParameters);
                    }
                }
            }
        }

        
        public static void Delete(object TerritorioId)
        {
            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Get Values
            using (IDbConnection db = new SqlConnection(szCnnString))
            {
                string szSqlQuery = "DELETE FROM [Territorios] WHERE [TerritorioId] = @TerritorioId";
                db.Execute(szSqlQuery, new { TerritorioId });
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
                    string szSqlQuery = "DELETE FROM [Territorios] " + szSqlWhere;
                    db.Execute(szSqlQuery, oParameters);
                }
            }
        }

        
    }
}
