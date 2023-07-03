using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;

namespace PectraForms.WebApplication.BackEnd.API.Helpers
{
    /// <summary>
    /// This delegate describes the method on the interface to be called.
    /// </summary>
    /// <typeparam name="T">This is the type of the interface</typeparam>
    /// <param name="proxy">This is the method.</param>
    public delegate void UseServiceDelegate<T>(T proxy);

    public class ClientProxyHelper<T>
    {
        /// <summary>
        /// This is the channel proxy
        /// </summary>
        IClientChannel proxy = null;

        /// <summary>
        /// This is the store of the channel.
        /// </summary>
        private static IDictionary<string, ChannelFactory<T>> channelPool = new Dictionary<string, ChannelFactory<T>>();

        private static IDictionary<Tuple<string, string>, IDictionary<string, ChannelFactory<T>>> userPool = new Dictionary<Tuple<string,string>, IDictionary<string, ChannelFactory<T>>>();

        /// <summary>
        /// Returns an instance of the channel object. The channel is not yet open.
        /// </summary>
        /// <param name="WCFEndPoint">This is the end point</param>
        /// <param name="username">Usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>Return List of all the invoked proxies</returns>
        private ChannelFactory<T> GetChannelFactory(string WCFEndPoint, string username = null, string password = null)
        {
            ChannelFactory<T> channelFactory = null;
            IDictionary<string, ChannelFactory<T>> currentChannelPool = channelPool;

            if (!string.IsNullOrEmpty(username))
            {
                if (!userPool.TryGetValue(new Tuple<string, string>(username, password), out currentChannelPool))
                {
                    currentChannelPool = new Dictionary<string, ChannelFactory<T>>();
                    userPool.Add(new Tuple<string, string>(username, password), currentChannelPool);
                }
            }
            //Check if the channel factory exists
            //Create and return an instance of the channel
            if (!currentChannelPool.TryGetValue(WCFEndPoint, out channelFactory))
            {
                channelFactory = new ChannelFactory<T>(WCFEndPoint);

                if (!string.IsNullOrEmpty(username))
                {
                    if (ConfigurationManager.AppSettings["WindowsAuthentication_domain"] != null)
                        channelFactory.Credentials.Windows.ClientCredential.Domain = ConfigurationManager.AppSettings["WindowsAuthentication_domain"];
                    channelFactory.Credentials.Windows.ClientCredential.Password = password;
                    channelFactory.Credentials.Windows.ClientCredential.UserName = username;
                }

                currentChannelPool.Add(WCFEndPoint, channelFactory);
            }

            return channelFactory;
        }

        /// <summary>
        /// Invokes the method on the WCF interface with the given end point to 
        /// create a channel
        /// Usage
        /// new ProxyHelper&lt;InterfaceName&gt;().Use(serviceProxy => { 
        ///         value = serviceProxy.MethodName(params....); 
        ///     });
        /// </summary>
        /// <param name="codeBlock">The WCF interface method of interface of type T
        /// </param>
        /// <param name="WCFEndPoint">The end point.</param>
        public void Use(UseServiceDelegate<T> codeBlock, string WCFEndPoint, string username = null, string password = null)
        {
            try
            {
                //Create an instance of proxy
                this.proxy = GetChannelFactory(WCFEndPoint, username, password).CreateChannel() as IClientChannel;
                if (this.proxy != null)
                {
                    this.proxy.Open();
                    //Call the method
                    codeBlock((T)this.proxy);

                    this.proxy.Close();
                }
            }
            catch (FaultException faultException)
            {
                if (this.proxy != null)
                    this.proxy.Abort();

                if (faultException.Code.Name == "ExpiredSession" || faultException.Code.Name == "InvalidSession")
                {
                    throw new InvalidSessionException(faultException.Message);
                }
                else
                    throw; //new Exception(faultException.Message)
            }
            catch (CommunicationException commException)
            {
                if (this.proxy != null)
                    this.proxy.Abort();

                if (commException.InnerException is FaultException)
                {
                    FaultException exc = (FaultException)commException.InnerException;
                    if (exc.Code.SubCode.Name == "FailedAuthentication")
                        throw new FaultException(commException.Message);
                }

                throw new Exception("Error de comunicación");
            }
            catch (TimeoutException timeoutException)
            {
                if (this.proxy != null)
                    this.proxy.Abort();

                throw new Exception("Error de timeout");
            }
            catch (Exception ex)
            {
                if (this.proxy != null)
                    this.proxy.Abort();

                throw new Exception("Error general");
            }
        }
    }
}