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
    public partial class BooleBox_ArchivosDAC
    {
        public static List<BooleBox_ArchivosBE> GetAll()
        {
            List<BooleBox_ArchivosBE> lstBooleBox_Archivos = new List<BooleBox_ArchivosBE>();

            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Get Values
            using (IDbConnection db = new SqlConnection(szCnnString))
            {
                lstBooleBox_Archivos = db.Query<BooleBox_ArchivosBE>("SELECT * FROM [BooleBox_Archivos]").ToList();
            }

            return lstBooleBox_Archivos;
        }

        public static List<BooleBox_ArchivosBE> GetByParam(Dictionary<string, object> pConditionValues)
        {
            string szSqlWhere = string.Empty;
            List<BooleBox_ArchivosBE> lstBooleBox_Archivos = new List<BooleBox_ArchivosBE>();

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
                    lstBooleBox_Archivos = db.Query<BooleBox_ArchivosBE>("SELECT * FROM [BooleBox_Archivos] " + szSqlWhere, oParameters).ToList();
                }
            }

            return lstBooleBox_Archivos;
        }

        public static void Insert(BooleBox_ArchivosBE pBooleBox_ArchivosBE)
        {
            if (pBooleBox_ArchivosBE != null)
            {
                // Get ConnectionString
                string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

                // Insert
                using (IDbConnection db = new SqlConnection(szCnnString))
                {
                                        string szSqlQuery = "INSERT INTO [BooleBox_Archivos] ([ArchivoId], [Nombre], [CarpetaId], [ExternalFileId], [ExternalDateCreated], [ExternalDateModified]) VALUES (@ArchivoId, @Nombre, @CarpetaId, @ExternalFileId, @ExternalDateCreated, @ExternalDateModified)";
                                        db.Execute(szSqlQuery, pBooleBox_ArchivosBE);
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
                        string szSqlUpdate = string.Format("UPDATE [BooleBox_Archivos] SET {0} WHERE {1}", szSqlSet, szSqlWhere);
                        db.Execute(szSqlUpdate, oParameters);
                    }
                }
            }
        }

        
        public static void Delete(object ArchivoId)
        {
            // Get ConnectionString
            string szCnnString = CnnStringProvider.GetDecryptedCnnStringByCnnKey("ConnectionStringKey");

            // Get Values
            using (IDbConnection db = new SqlConnection(szCnnString))
            {
                string szSqlQuery = "DELETE FROM [BooleBox_Archivos] WHERE [ArchivoId] = @ArchivoId";
                db.Execute(szSqlQuery, new { ArchivoId });
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
                    string szSqlQuery = "DELETE FROM [BooleBox_Archivos] " + szSqlWhere;
                    db.Execute(szSqlQuery, oParameters);
                }
            }
        }

        
    }
}
