//------------------------------------------------------------------------------
// <auto-generated>
//   Este codigo fue generado por Pectra Forms
// 
//   Cambios en este archivo pueden ocasionar un comportamiento incorrecto.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using OBPIClient = PectraForms.WebApplication.BackEnd.API.PectraOBPIReference.PectraOBPIServiceClient;
using BPClient = PectraForms.WebApplication.BackEnd.API.PectraBPReference.BPServiceClient;
using PectraForms.WebApplication.BackEnd.API.PectraOBPIReference;

namespace PectraForms.WebApplication.BackEnd.API.Helpers
{
    /// <summary>
    /// The helper class to Pectra OBPI client proxy.
    /// </summary>
    class PectraOBPIHelper
    {
        #region [- OBPI Client Configuration -]
        /// <summary>
        /// Open the OBPI client.
        /// </summary>
        private static void OpenOBPIClient(OBPIClient m_oOBPIClient)
        {
            if (m_oOBPIClient != null)
                m_oOBPIClient.Open();  // Open the client
        }

        /// <summary>
        /// Close the OBPI client.
        /// </summary>
        private static void CloseOBPIClient(OBPIClient m_oOBPIClient)
        {
            if (m_oOBPIClient != null)
            {
                m_oOBPIClient.Close(); // Close the client
                m_oOBPIClient = null;
            }
        }

        #endregion

        #region [- ActivityEnd -]
        public static string ActivityEnd(string pszTrxId, string pszSetAttributes,
                                    long plTrxIdExt, long plPngId, long plSubProId, long plActId, long plInsId, long plPngInsId,
                                    string pszUsrId, string pszInsOrigin, long plPriority, bool pbAttachmentsDocuments)
        {
            ActivityEndSecureRequest oRequest;
            #region [ - Attributes - ]
            XmlDocument oDom = new XmlDocument();

            if (!string.IsNullOrEmpty(pszSetAttributes))
                oDom.LoadXml(pszSetAttributes);
            List<PectraOBPIReference.Attribute> plstSetAttributes = new List<PectraOBPIReference.Attribute>();
            foreach (XmlNode oNod in oDom.SelectNodes("//Attribute"))
            {
                PectraOBPIReference.Attribute oAtt = new PectraOBPIReference.Attribute();
                oAtt.Id = oNod.SelectSingleNode("AtrId").InnerText;
                oAtt.Value = oNod.SelectSingleNode("AtrValue").InnerText;
                oAtt.AttrExtValue = new AttributeExtendedValue();
                oAtt.AttrExtValue.ContentType = oNod.SelectSingleNode("AtrExtendedValue/ContentType").InnerText;
                oAtt.AttrExtValue.Data = Convert.FromBase64String(oNod.SelectSingleNode("AtrExtendedValue/Data").InnerText);
                oAtt.AttrExtValue.ExtensionData =
                oAtt.ExtensionData = oAtt.AttrExtValue.ExtensionData;

                plstSetAttributes.Add(oAtt);
            }
            #endregion
            oRequest = new ActivityEndSecureRequest();
            oRequest.SecureTrxID = Guid.Parse(pszTrxId);
            oRequest.Attributes = plstSetAttributes;
            oRequest.TrxIdExt = plTrxIdExt;
            oRequest.PngId = plPngId;
            oRequest.SubProId = plSubProId;
            oRequest.ActId = plActId;
            oRequest.InsId = plInsId;
            oRequest.PngInsId = plPngInsId;
            oRequest.UserId = pszUsrId;
            oRequest.InsOrigin = pszInsOrigin;
            MessagePriority msgPriority;
            switch (plPriority)
            {
                case 0:
                    msgPriority = MessagePriority.Lowest;
                    break;
                case 1:
                    msgPriority = MessagePriority.VeryLow;
                    break;
                case 2:
                    msgPriority = MessagePriority.Low;
                    break;
                case 3:
                    msgPriority = MessagePriority.Normal;
                    break;
                case 4:
                    msgPriority = MessagePriority.AboveNormal;
                    break;
                case 5:
                    msgPriority = MessagePriority.High;
                    break;
                case 6:
                    msgPriority = MessagePriority.VeryHigh;
                    break;
                case 7:
                    msgPriority = MessagePriority.Highest;
                    break;
                default:
                    msgPriority = MessagePriority.Normal;
                    break;
            }
            oRequest.MessagePriority = msgPriority;
            oRequest.AttachmentsDocuments = pbAttachmentsDocuments;

            OBPIClient m_oOBPIClient = new OBPIClient("BasicHttpBinding_IPectraOBPIService");

            // Open the client
            OpenOBPIClient(m_oOBPIClient);

            // Invoke  method
            ActivityEndResponse oResponse = m_oOBPIClient.ActivityEndSecure(oRequest);

            // Close the client
            CloseOBPIClient(m_oOBPIClient);

            // Translate
            string strReturn = oResponse.GUID;

            // Liberacion de objetos
            oResponse = null;

            //- Return the value
            return strReturn;

        }

        public static string ActivityEnd(string pszTrxId, string pszSetAttributes)
        {
            return ActivityEnd(pszTrxId, pszSetAttributes, 0, 0, 0, 0, 0, 0, string.Empty, string.Empty, 3, false);
        }
        #endregion

        #region [- AttributeGettValue -]
        public static string AttributeGetValue(string szTrxId, string pszAtrId)
        {
            AttributeGetValueSecureRequest oRequest;

            oRequest = new AttributeGetValueSecureRequest();
            oRequest.SecureTrxID = Guid.Parse(szTrxId);
            oRequest.AtrId = pszAtrId;

            OBPIClient m_oOBPIClient = new OBPIClient("BasicHttpBinding_IPectraOBPIService");

            // Open the client
            OpenOBPIClient(m_oOBPIClient);

            // Invoke method
            AttributeGetValueResponse oResponse = m_oOBPIClient.AttributeGetValueSecure(oRequest);

            // Close the client
            CloseOBPIClient(m_oOBPIClient);

            // Translate
            string strReturn = oResponse.AttributeValue;

            // Liberacion de objetos
            oResponse = null;

            //- Return the value
            return strReturn;
        }

        public static string AttributeGetValueFromXmlObject(string pszAtrId, string pszSetAttributes)
        {
            string szXPath;
            string strReturn = string.Empty;
            XmlNode nodSetAttr;
            XmlDocument xmlDom = new XmlDocument();
            try
            {
                xmlDom.LoadXml(pszSetAttributes);
                nodSetAttr = xmlDom.SelectSingleNode("SetAttributes");
                szXPath = string.Format("Attribute[AtrId=\'{0}\']", pszAtrId);
                // Valido si existe el atributo en el xml
                if (nodSetAttr.SelectSingleNode(szXPath) != null)
                    // Seteo el valor del atributo para retornarlo
                    strReturn = nodSetAttr.SelectSingleNode(szXPath).SelectSingleNode("AtrValue").InnerText;
                return strReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                nodSetAttr = null;
                xmlDom = null;
                strReturn = null;
            }

        }

        #endregion

        #region [- AttributeSetValue -]
        /// <summary>
        /// Set the value of an attribute.
        /// </summary>
        /// <param name="pszAtrId">Attribute ID.</param>
        /// <param name="pszAtrValue">Attribute value.</param>
        /// <param name="pszSetAttributes">Set attributes.</param>
        /// <param name="poExtendedValue">Valor extendido</param>
        /// <returns></returns>

        public static string AttributeSetValue(string pszAtrId, string pszAtrValue, string pszSetAttributes, object poExtendedValue)
        {
            string szXPath;
            XmlNode nodSetAttr, nodAttr;
            XmlDocument xmlDom = new XmlDocument();
            try
            {
                // - Limita el largo de los atributos
                // --------------------------------------------------------------------------------
                pszAtrValue = pszAtrValue.Substring(0, (pszAtrValue.Length > 250 ? 250 : pszAtrValue.Length));

                pszAtrValue = RemoveSpecialCharacters(pszAtrValue);

                // - Valida si ya hay creado un Nodo Root, si no esta, lo crea
                // --------------------------------------------------------------------------------
                try
                {
                    xmlDom.LoadXml(pszSetAttributes);
                    nodSetAttr = xmlDom.SelectSingleNode("SetAttributes");
                }
                catch
                {
                    nodSetAttr = xmlDom.CreateElement("SetAttributes");
                }
                // - Valida si ya existe el Atributo, si existe lo elimina
                // --------------------------------------------------------------------------------
                szXPath = string.Format("Attribute[AtrId=\'{0}\']", pszAtrId);
                if (nodSetAttr.SelectSingleNode(szXPath) != null)
                {
                    nodSetAttr.RemoveChild(nodSetAttr.SelectSingleNode(szXPath));
                }
                #region [ - Crea el nodo Attribute - ]
                // --------------------------------------------------------------------------------
                // - Crea el nodo del Atributo, con la siguiente Estructura
                // 
                //   <Attribute>
                //     <AtrId/>
                //     <AtrValue/>
                //     <AtrExtendedValue>
                //       <ContentType />
                //       <Data />
                //     </AtrExtendedValue>
                //   </Attribute>
                // --------------------------------------------------------------------------------
                //- Attribute
                nodAttr = xmlDom.CreateElement("Attribute");
                //- AtrId
                nodAttr.AppendChild(xmlDom.CreateElement("AtrId"));
                //- AtrValue
                nodAttr.AppendChild(xmlDom.CreateElement("AtrValue"));
                //- AtrExtendedValue
                nodAttr.AppendChild(xmlDom.CreateElement("AtrExtendedValue"));
                //- AtrExtendedValue/ContentType
                nodAttr.SelectSingleNode("AtrExtendedValue").AppendChild(xmlDom.CreateElement("ContentType"));
                //- AtrExtendedValue/Data
                nodAttr.SelectSingleNode("AtrExtendedValue").AppendChild(xmlDom.CreateElement("Data"));
                #endregion

                // - Inserta los Valores
                // --------------------------------------------------------------------------------
                nodAttr.SelectSingleNode("AtrId").InnerText = pszAtrId;
                nodAttr.SelectSingleNode("AtrValue").InnerText = pszAtrValue;

                //- Datos del valor extendido
                //- Convierte el Extended Value a byte[], en el caso de que no sea de ese tipo
                if (poExtendedValue != null && poExtendedValue.GetType() == typeof(string) &&
                    poExtendedValue.GetType() != typeof(byte[]) && poExtendedValue.ToString().Length > 0)
                {
                    poExtendedValue = Encoding.ASCII.GetBytes(poExtendedValue.ToString());
                }
                //-
                if (poExtendedValue != null && poExtendedValue.GetType() == typeof(byte[]))
                {
                    nodAttr.SelectSingleNode("AtrExtendedValue/ContentType").InnerText = "bin.base64";
                    nodAttr.SelectSingleNode("AtrExtendedValue/Data").InnerText = Convert.ToBase64String((byte[])poExtendedValue);
                }

                nodSetAttr.AppendChild(nodAttr);
                xmlDom.AppendChild(nodSetAttr);

                return xmlDom.OuterXml;
            }
            catch
            {
                throw;
            }
            finally
            {
                nodSetAttr = null;
                nodAttr = null;
                xmlDom = null;
            }
        }

        public static string AttributeSetValue(string pszAtrId, string pszAtrValue, string pszSetAttributes)
        {
            string szXPath;
            XmlNode nodSetAttr, nodAttr;
            XmlDocument xmlDom = new XmlDocument();
            try
            {
                // - Limita el largo de los atributos
                // --------------------------------------------------------------------------------
                pszAtrValue = pszAtrValue.Substring(0, (pszAtrValue.Length > 250 ? 250 : pszAtrValue.Length));

                pszAtrValue = RemoveSpecialCharacters(pszAtrValue);

                // - Valida si ya hay creado un Nodo Root, si no esta, lo crea
                // --------------------------------------------------------------------------------
                try
                {
                    xmlDom.LoadXml(pszSetAttributes);
                    nodSetAttr = xmlDom.SelectSingleNode("SetAttributes");
                }
                catch
                {
                    nodSetAttr = xmlDom.CreateElement("SetAttributes");
                }
                // - Valida si ya existe el Atributo, si existe lo elimina
                // --------------------------------------------------------------------------------
                szXPath = string.Format("Attribute[AtrId=\'{0}\']", pszAtrId);
                if (nodSetAttr.SelectSingleNode(szXPath) != null)
                {
                    nodSetAttr.RemoveChild(nodSetAttr.SelectSingleNode(szXPath));
                }
                #region [ - Crea el nodo Attribute - ]
                // --------------------------------------------------------------------------------
                // - Crea el nodo del Atributo, con la siguiente Estructura
                // 
                //   <Attribute>
                //     <AtrId/>
                //     <AtrValue/>
                //     <AtrExtendedValue>
                //       <ContentType />
                //       <Data />
                //     </AtrExtendedValue>
                //   </Attribute>
                // --------------------------------------------------------------------------------
                //- Attribute
                nodAttr = xmlDom.CreateElement("Attribute");
                //- AtrId
                nodAttr.AppendChild(xmlDom.CreateElement("AtrId"));
                //- AtrValue
                nodAttr.AppendChild(xmlDom.CreateElement("AtrValue"));
                //- AtrExtendedValue
                nodAttr.AppendChild(xmlDom.CreateElement("AtrExtendedValue"));
                //- AtrExtendedValue/ContentType
                nodAttr.SelectSingleNode("AtrExtendedValue").AppendChild(xmlDom.CreateElement("ContentType"));
                //- AtrExtendedValue/Data
                nodAttr.SelectSingleNode("AtrExtendedValue").AppendChild(xmlDom.CreateElement("Data"));
                #endregion

                // - Inserta los Valores
                // --------------------------------------------------------------------------------
                nodAttr.SelectSingleNode("AtrId").InnerText = pszAtrId;
                nodAttr.SelectSingleNode("AtrValue").InnerText = pszAtrValue;
                nodAttr.SelectSingleNode("AtrExtendedValue/ContentType").InnerText = "bin.base64";

                nodSetAttr.AppendChild(nodAttr);
                xmlDom.AppendChild(nodSetAttr);

                return xmlDom.OuterXml;
            }
            catch
            {
                throw;
            }
            finally
            {
                nodSetAttr = null;
                nodAttr = null;
                xmlDom = null;
            }
        }

        /// <summary>
        /// Attributes the set value.
        /// </summary>
        /// <param name="pszAtrId">The PSZ atr id.</param>
        /// <param name="pszAtrValue">The PSZ atr value.</param>
        /// <param name="pszSetAttributes">The PSZ set attributes.</param>
        /// <param name="poExtendedValue">The po extended value.</param>
        /// <param name="pszContentType">Type of the PSZ content.</param>
        /// <returns></returns>

        public static string AttributeSetValue(string pszAtrId, string pszAtrValue, string pszSetAttributes,
                                        object poExtendedValue, string pszContentType)
        {
            string szXPath;
            XmlNode nodSetAttr, nodAttr;
            XmlDocument xmlDom = new XmlDocument();
            try
            {
                // - Limita el largo de los atributos
                // --------------------------------------------------------------------------------
                pszAtrValue = pszAtrValue.Substring(0, (pszAtrValue.Length > 250 ? 250 : pszAtrValue.Length));

                pszAtrValue = RemoveSpecialCharacters(pszAtrValue);

                // - Valida si ya hay creado un Nodo Root, si no esta, lo crea
                // --------------------------------------------------------------------------------
                try
                {
                    xmlDom.LoadXml(pszSetAttributes);
                    nodSetAttr = xmlDom.SelectSingleNode("SetAttributes");
                }
                catch
                {
                    nodSetAttr = xmlDom.CreateElement("SetAttributes");
                }
                // - Valida si ya existe el Atributo, si existe lo elimina
                // --------------------------------------------------------------------------------
                szXPath = string.Format("Attribute[AtrId=\'{0}\']", pszAtrId);
                if (nodSetAttr.SelectSingleNode(szXPath) != null)
                {
                    nodSetAttr.RemoveChild(nodSetAttr.SelectSingleNode(szXPath));
                }
                #region [ - Crea el nodo Attribute - ]
                // --------------------------------------------------------------------------------
                // - Crea el nodo del Atributo, con la siguiente Estructura
                // 
                //   <Attribute>
                //     <AtrId/>
                //     <AtrValue/>
                //     <AtrExtendedValue>
                //       <ContentType />
                //       <Data />
                //     </AtrExtendedValue>
                //   </Attribute>
                // --------------------------------------------------------------------------------
                //- Attribute
                nodAttr = xmlDom.CreateElement("Attribute");
                //- AtrId
                nodAttr.AppendChild(xmlDom.CreateElement("AtrId"));
                //- AtrValue
                nodAttr.AppendChild(xmlDom.CreateElement("AtrValue"));
                //- AtrExtendedValue
                nodAttr.AppendChild(xmlDom.CreateElement("AtrExtendedValue"));
                //- AtrExtendedValue/ContentType
                nodAttr.SelectSingleNode("AtrExtendedValue").AppendChild(xmlDom.CreateElement("ContentType"));
                //- AtrExtendedValue/Data
                nodAttr.SelectSingleNode("AtrExtendedValue").AppendChild(xmlDom.CreateElement("Data"));
                #endregion

                // - Inserta los Valores
                // --------------------------------------------------------------------------------
                nodAttr.SelectSingleNode("AtrId").InnerText = pszAtrId;
                nodAttr.SelectSingleNode("AtrValue").InnerText = pszAtrValue;
                nodAttr.SelectSingleNode("AtrExtendedValue/ContentType").InnerText = "bin.base64";

                //- Datos del valor extendido
                //- Convierte el Extended Value a byte[], en el caso de que no sea de ese tipo
                if (poExtendedValue != null && poExtendedValue.GetType() == typeof(string) &&
                    poExtendedValue.GetType() != typeof(byte[]) && poExtendedValue.ToString().Length > 0)
                {
                    #region [ - Extended Value - ]
                    poExtendedValue = Encoding.ASCII.GetBytes(poExtendedValue.ToString());
                    #endregion
                }
                //-
                if (poExtendedValue != null && poExtendedValue.GetType() == typeof(byte[]))
                    nodAttr.SelectSingleNode("AtrExtendedValue/Data").InnerText = Convert.ToBase64String((byte[])poExtendedValue);

                nodSetAttr.AppendChild(nodAttr);
                xmlDom.AppendChild(nodSetAttr);

                return xmlDom.OuterXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                nodSetAttr = null;
                nodAttr = null;
                xmlDom = null;
            }
        }

        public static string RemoveSpecialCharacters(string str)
        {
            string strRet = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                Encoding iso_encoding = Encoding.GetEncoding("ISO-8859-1");
                Encoding unicode_encoding = Encoding.Unicode;

                byte[] unicode_bytes = unicode_encoding.GetBytes(str);
                byte[] iso_bytes = Encoding.Convert(unicode_encoding, iso_encoding, unicode_bytes);

                char[] iso_chars = new char[iso_encoding.GetCharCount(iso_bytes, 0, iso_bytes.Length)];

                iso_encoding.GetChars(iso_bytes, 0, iso_bytes.Length, iso_chars, 0);

                strRet = new string(iso_chars);
            }
            return strRet;
        }

        #endregion

        #region [- InstanceLockUnLock -]

        public static Int64 InstanceLockUnLockBP(string pszTrxId, bool pbLockState, string SessionToken, string ProfId, string LanguageId)
        {
            Int32 lInsId = 0;
            lInsId = Convert.ToInt32(AttributeGetValue(pszTrxId, "InsId"));
            if (lInsId > 0)
            {
                PectraBPReference.InstanceLockUnLockRequest oRequest;

                oRequest = new PectraBPReference.InstanceLockUnLockRequest();
                //oRequest.SecureTrxID = Guid.Parse(pszTrxId);
                oRequest.Lock = pbLockState;
                oRequest.UsrLock = AttributeGetValue(pszTrxId, "LoguedUsrId");
                oRequest.PngId = Convert.ToInt32(AttributeGetValue(pszTrxId, "PngId"));
                oRequest.VersionId = Convert.ToInt32(AttributeGetValue(pszTrxId, "VersionId"));
                oRequest.SubProId = Convert.ToInt32(AttributeGetValue(pszTrxId, "SubProId"));
                oRequest.ActId = Convert.ToInt32(AttributeGetValue(pszTrxId, "ActId"));
                oRequest.SessionToken = SessionToken;
                oRequest.InsId = lInsId;
                oRequest.ProfId = ProfId;
                oRequest.LanguageId = LanguageId;


                BPClient m_Client = new BPClient("WSHttpBinding_IBPService");

                if (m_Client != null)
                    m_Client.Open();  // Open the client


                // Invoke method
                PectraBPReference.InstanceLockUnLockResponse oResponse = m_Client.InstanceLockUnLock(oRequest);

                // Close the client
                if (m_Client != null)
                {
                    m_Client.Close();
                    m_Client = null;
                }

                //- Return the value
                return 1;
            }
            else
                return 0;
        }
        public static Int64 InstanceLockUnLock(string pszTrxId, bool pbLockState)
        {
            Int32 lInsId = 0;
            lInsId = Convert.ToInt32(AttributeGetValue(pszTrxId, "InsId"));
            if (lInsId > 0)
            {
                InstanceLockUnLockSecureRequest oRequest;

                oRequest = new InstanceLockUnLockSecureRequest();
                oRequest.SecureTrxID = Guid.Parse(pszTrxId);
                oRequest.Lock = pbLockState;
                oRequest.UsrLock = AttributeGetValue(pszTrxId, "LoguedUsrId");
                oRequest.PngId = Convert.ToInt32(AttributeGetValue(pszTrxId, "PngId"));
                oRequest.VersionId = Convert.ToInt32(AttributeGetValue(pszTrxId, "VersionId"));
                oRequest.SubProId = Convert.ToInt32(AttributeGetValue(pszTrxId, "SubProId"));
                oRequest.ActId = Convert.ToInt32(AttributeGetValue(pszTrxId, "ActId"));
                oRequest.InsId = lInsId;

                OBPIClient m_oOBPIClient = new OBPIClient("BasicHttpBinding_IPectraOBPIService");

                // Open the client
                OpenOBPIClient(m_oOBPIClient);

                // Invoke method
                InstanceLockUnLockResponse oResponse = m_oOBPIClient.InstanceLockUnLockSecure(oRequest);

                // Close the client
                CloseOBPIClient(m_oOBPIClient);

                //- Return the value
                return oResponse.Result;
            }
            else
                return 0;
        }

        public static Int64 InstanceLockUnLock(string pszTrxId, bool pbLockState, string pszUsrLock, int plPngId, int plVersionId,
                                        int plSubProId, int plActId, int plInsId)
        {
            InstanceLockUnLockSecureRequest oRequest;

            oRequest = new InstanceLockUnLockSecureRequest();
            oRequest.SecureTrxID = Guid.Parse(pszTrxId);
            oRequest.Lock = pbLockState;
            oRequest.UsrLock = pszUsrLock;
            oRequest.PngId = plPngId;
            oRequest.VersionId = plVersionId;
            oRequest.SubProId = plSubProId;
            oRequest.ActId = plActId;
            oRequest.InsId = plInsId;

            OBPIClient m_oOBPIClient = new OBPIClient("BasicHttpBinding_IPectraOBPIService");

            // Open the client
            OpenOBPIClient(m_oOBPIClient);

            // Invoke method
            InstanceLockUnLockResponse oResponse = m_oOBPIClient.InstanceLockUnLockSecure(oRequest);

            // Close the client
            CloseOBPIClient(m_oOBPIClient);

            //- Return the value
            return oResponse.Result;
        }

        public static Int64 InstanceLockUnLock(string pszTrxId, bool pbLockState, string pszUsrLock)
        {
            InstanceLockUnLockSecureRequest oRequest;

            oRequest = new InstanceLockUnLockSecureRequest();
            oRequest.SecureTrxID = Guid.Parse(pszTrxId);
            oRequest.Lock = pbLockState;
            oRequest.UsrLock = pszUsrLock;

            oRequest.PngId = Convert.ToInt16(AttributeGetValue(pszTrxId, "PngId"));
            oRequest.VersionId = Convert.ToInt16(AttributeGetValue(pszTrxId, "VersionId"));
            oRequest.ActId = Convert.ToInt16(AttributeGetValue(pszTrxId, "ActId"));
            oRequest.InsId = Convert.ToInt16(AttributeGetValue(pszTrxId, "InsId"));
            oRequest.SubProId = Convert.ToInt16(AttributeGetValue(pszTrxId, "SubProId"));

            OBPIClient m_oOBPIClient = new OBPIClient("BasicHttpBinding_IPectraOBPIService");

            // Open the client
            OpenOBPIClient(m_oOBPIClient);

            // Invoke DownloadDocumentResponse method
            InstanceLockUnLockResponse oResponse = m_oOBPIClient.InstanceLockUnLockSecure(oRequest);

            // Close the client
            CloseOBPIClient(m_oOBPIClient);

            //- Return the value
            return oResponse.Result;
        }
        #endregion

        #region [- PackageDelOne -]
        /// <summary>
        /// Borra un paquete
        /// </summary>
        /// <param name="plTrxId"></param>
        public static bool PackageDelOne(string pszTrxId)
        {
            ActivityCancelSecureRequest oRequest;
            oRequest = new ActivityCancelSecureRequest();
            oRequest.SecureTrxID = Guid.Parse(pszTrxId);

            try
            {
                OBPIClient m_oOBPIClient = new OBPIClient("BasicHttpBinding_IPectraOBPIService");

                // Open the client
                OpenOBPIClient(m_oOBPIClient);
                ActivityCancelResponse oResponse = m_oOBPIClient.ActivityCancelSecure(oRequest);
                // Close the client
                CloseOBPIClient(m_oOBPIClient);
                // Liberacion de objetos
                return oResponse.Result;
            }
            catch (Exception ex)
            {
                if (ex.Message != "TrxIdNotFound")
                    throw;
                else
                    return true;
            }
            finally
            {
                oRequest = null;
            }
        }
        #endregion

        #region [- PackageGetOne -]
        public static void PackageGetOne(string pszTrxId)
        {
            ActivityCancelSecureRequest oRequest;
            oRequest = new ActivityCancelSecureRequest();
            oRequest.SecureTrxID = Guid.Parse(pszTrxId);

            try
            {
                OBPIClient m_oOBPIClient = new OBPIClient("BasicHttpBinding_IPectraOBPIService");

                // Open the client
                OpenOBPIClient(m_oOBPIClient);
                //PackageGetOneResponse oResponse = m_oOBPIClient.p (oRequest);
                // Close the client
                CloseOBPIClient(m_oOBPIClient);
                // Liberacion de objetos
                //return oResponse.Result;
            }
            catch (Exception ex)
            {
                if (ex.Message != "TrxIdNotFound")
                    throw;
                //else
                //    return true;
            }
            finally
            {
                oRequest = null;
            }
        }
        #endregion

    } //-End Class-
}