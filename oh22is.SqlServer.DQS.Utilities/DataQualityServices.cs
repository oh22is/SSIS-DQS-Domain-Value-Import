using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.Ssdqs.Component.Common.Logic;
using Microsoft.Ssdqs.Core.Abstract;
using Microsoft.Ssdqs.Core.Service.KnowledgebaseManagement.Define;
using Microsoft.Ssdqs.DataService.Define;
using Microsoft.Ssdqs.DataValueService.Define;
using Microsoft.Ssdqs.Flow.Notification;
using Microsoft.Ssdqs.Infra.Codes;
using Microsoft.Ssdqs.Infra.Exceptions;
using Microsoft.Ssdqs.Proxy.EntryPoint;
#pragma warning disable 1587

namespace oh22is.SqlServer.DQS.Utilities
{
    public class DataQualityServices
    {
        public const string DatabaseName = "DQS_MAIN";

        /// <summary>
        /// 
        /// </summary>
        public static bool InitializeProxy(bool encrypted)
        {
            try
            {
                ProxyInit.UseEncryption = encrypted;
                if (!ProxyInit.IsInit)
                {
                    ProxyInit.Init(new SsisProxyInitParameters());
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;   
            }
        }

        /// <summary>
        /// Creates a session with the DQS server and returns the current session id
        /// </summary>
        /// <param name="connectionManager">The DQS connection manager</param>
        /// <param name="databaseName">The DQS database name</param>
        /// <returns>The current session id</returns>
        public static NotificationSessionInfo CreateClientSession(ConnectionManager connectionManager, string databaseName)
        {
            if (connectionManager != null)
            {
                if (databaseName != null)
                {
                    Dictionary<string, string> connectionStringParameters = DataQualityConnectorFinals.GetConnectionStringParameters(connectionManager.ConnectionString);
                    NotificationEntryPointClient notificationEntryPoint = ProxyEntryPointFactory.GetNotificationEntryPoint(connectionStringParameters["ServerName"], databaseName);
                    NotificationSessionInfo notificationSessionInfo = notificationEntryPoint.NotificationSessionCreate(ClientType.SsisClient);                                       
                    return notificationSessionInfo;
                }
                else
                {
                    throw new ArgumentNullException("databaseName");
                }
            }
            else
            {
                throw new ArgumentNullException("connectionManager");
            }

        }

        /// <summary>
        /// Creates a session with the DQS server and returns the current session id
        /// </summary>
        /// <param name="connectionManager">The DQS connection manager</param>
        /// <param name="databaseName">The DQS database name</param>
        /// <returns>The current session id</returns>
        public static long CreateClientSessionId(ConnectionManager connectionManager, string databaseName)
        {
            if (connectionManager != null)
            {
                if (databaseName != null)
                {
                    Dictionary<string, string> connectionStringParameters = DataQualityConnectorFinals.GetConnectionStringParameters(connectionManager.ConnectionString);
                    NotificationEntryPointClient notificationEntryPoint = ProxyEntryPointFactory.GetNotificationEntryPoint(connectionStringParameters["ServerName"], databaseName);
                    NotificationSessionInfo notificationSessionInfo = notificationEntryPoint.NotificationSessionCreate(ClientType.SsisClient);

                    return notificationSessionInfo.SessionId;
                }
                else
                {
                    throw new ArgumentNullException("databaseName");
                }
            }
            else
            {
                throw new ArgumentNullException("connectionManager");
            }

        }

        /// <summary>
        /// checks whether the connection manager is a DQS connection manager or not
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public static bool IsDqsConnection(ConnectionManager cm)
        {
            return cm.CreationName.StartsWith("DQS");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionManager"></param>
        /// <returns></returns>
        public static string ServerName (ConnectionManager connectionManager)
        {
            Dictionary<string, string> connectionStringParameters = DataQualityConnectorFinals.GetConnectionStringParameters(connectionManager.ConnectionString);
            return (connectionStringParameters["ServerName"] == "." ? "localhost" : connectionStringParameters["ServerName"]);
        }

        /// <summary>
        /// Wird nicht länger verwendet...
        /// </summary>
        /// <param name="knowledgeBaseName"></param>
        /// <param name="serverName"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public static bool OpenKnowledgeBaseWithInWorkPhase(string knowledgeBaseName, string serverName, string DatabaseName, long SessionID)
        {
            Knowledgebase knowledgeBase = GetKnowledgeBaseByName(knowledgeBaseName, serverName, DatabaseName, SessionID);
            if (knowledgeBase.LockedClientId != SessionID && knowledgeBase.LockedClientId != 0)
            {
                return false;
            }
            else
            {
                using (var knowledgeBaseManagementEntryPoint = ProxyEntryPointFactory.GetKnowledgebaseManagementEntryPoint(serverName, DataQualityServices.DatabaseName, SessionID))
                {
                    knowledgeBase.InWorkPhase = KnowledgebaseInWorkPhase.DomainManagement;
                    var knowledgebaseOpenResult = knowledgeBaseManagementEntryPoint.KnowledgebaseOpen(knowledgeBase);
                    var id = (Knowledgebase)knowledgebaseOpenResult.Knowledgebase;
                    id.InWorkPhase = KnowledgebaseInWorkPhase.DomainManagement;
                    id = knowledgeBaseManagementEntryPoint.KnowledgebaseUpdate(id);
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="knowledgeBaseName"></param>
        /// <param name="serverName"></param>
        /// <param name="databaseName"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static bool IsOpen(string knowledgeBaseName, string serverName, string databaseName, long sessionId)
        {

            var retOpen = false;

            using (var knowledgeBase = ProxyEntryPointFactory.GetKnowledgebaseManagementEntryPoint(serverName, databaseName, sessionId))
            {
                var allKbs = knowledgeBase.KnowledgebaseGet();
                if (allKbs.Where(kb => kb.Name == knowledgeBaseName).Any(kb => kb.InWorkPhase != KnowledgebaseInWorkPhase.None))
                {
                    retOpen = true;
                }
            }

            return retOpen;
        }

        /// <summary>
        /// GetKnowledgeBaseIdByName
        /// </summary>
        /// <param name="knowledgeBaseName">Name of the knowledge base</param>
        /// <param name="serverName"></param>
        /// <param name="databaseName"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static long GetKnowledgeBaseIdByName(string knowledgeBaseName, string serverName, string databaseName, long sessionId)
        {
            return GetKnowledgeBaseByName(knowledgeBaseName, serverName, databaseName, sessionId).Id;
        }

        /// <summary>
        /// GetKnowledgeBaseIdByName
        /// </summary>
        /// <param name="knowledgeBaseName">Name of the knowledge base</param>
        /// <param name="serverName"></param>
        /// <param name="databaseName"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static Knowledgebase GetKnowledgeBaseByName(string knowledgeBaseName, string serverName, string databaseName, long sessionId)
        {

            /// GetKnowledgebaseManagementEntryPoint
            using (var knowledgeBase = ProxyEntryPointFactory.GetKnowledgebaseManagementEntryPoint(serverName, databaseName, sessionId))
            {
                var allKbs = knowledgeBase.KnowledgebaseGet();
                foreach (var kb in allKbs)
                {
                    if (kb.Name == knowledgeBaseName)
                    {
                        return kb;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="serverName"></param>
        /// <param name="databaseName"></param>
        /// <param name="sessionId"></param>
        /// <param name="knowledgeBaseId"></param>
        /// <returns></returns>
        public static long GetDomainIdByName(string domainName, string serverName, string databaseName, long sessionId, long knowledgeBaseId)
        {
            return GetDomainByName(domainName, serverName, databaseName, sessionId, knowledgeBaseId).Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="serverName"></param>
        /// <param name="databaseName"></param>
        /// <param name="sessionId"></param>
        /// <param name="knowledgeBaseId"></param>
        /// <returns></returns>
        public static Domain GetDomainByName(string domainName, string serverName, string databaseName, long sessionId, long knowledgeBaseId)
        {           
            using (var metadata = ProxyEntryPointFactory.GetMetadataManagementEntryPoint(serverName, databaseName, sessionId, knowledgeBaseId))
            {
                
                return metadata.DomainGetByName(domainName);
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DomainValueStatus GetDomainValueStatus(int value)
        {
            DomainValueStatus domainValueStatus;
            switch (value)
            {
                case 0:
                    domainValueStatus = DomainValueStatus.Correct;
                    break;
                case 1:
                    domainValueStatus = DomainValueStatus.Error;
                    break;
                case 2:
                    domainValueStatus = DomainValueStatus.NotValid;
                    break;
                default:
                    domainValueStatus = DomainValueStatus.None;
                    break;
            }
            return domainValueStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="records"></param>
        public static ReadOnlyCollection<ServiceResult> SetDomainValues(ReadOnlyCollection<DomainValue> domainValue, string ServerName, long SessionId, long KnowledgeBaseId, long DomainId)
        {
            ReadOnlyCollection<ServiceResult> serviceResult;

            using (var DataManagementEntryPoint = ProxyEntryPointFactory.GetDataManagementEntryPoint(ServerName, DatabaseName, SessionId, KnowledgeBaseId))
            {
                serviceResult = DataManagementEntryPoint.DomainValueAdd(DomainId, domainValue);
            }

            return serviceResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainValue"></param>
        /// <param name="ServerName"></param>
        /// <param name="SessionId"></param>
        /// <param name="KnowledgeBaseId"></param>
        public static void DeleteDomainValue(ReadOnlyCollection<DomainValue> domainValue, string ServerName, long SessionId, long KnowledgeBaseId)
        {
            using (var DataManagementEntryPoint = ProxyEntryPointFactory.GetDataManagementEntryPoint(ServerName, DatabaseName, SessionId, KnowledgeBaseId))
            {
                DataManagementEntryPoint.DomainValueDelete(domainValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="knowledgeBaseName"></param>
        /// <param name="serverName"></param>
        /// <param name="sessionId"></param>
        /// <param name="FireInformation"></param>
        /// <param name="ComponentMetaData"></param>
        /// <returns></returns>
        public static bool PublishKnowledgeBase(string knowledgeBaseName, string serverName, long sessionId, bool FireInformation, IDTSComponentMetaData100 ComponentMetaData)
        {
            Knowledgebase knowledgeBase = GetKnowledgeBaseByName(knowledgeBaseName, serverName, DatabaseName, sessionId);

            switch (knowledgeBase.InWorkPhase)
            {
                case KnowledgebaseInWorkPhase.DomainManagement:
                    using (var KnowledgeBaseManagementEntryPoint = ProxyEntryPointFactory.GetKnowledgebaseManagementEntryPoint(serverName, DatabaseName, sessionId))
                    {

                        try
                        {

                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("Name: {0}", knowledgeBase.Name.ToString()));

                            if (!String.IsNullOrEmpty(knowledgeBase.Description))
                            {
                                Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("Description: {0}", knowledgeBase.Description.ToString()));
                            }

                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("KnowledgebaseType: {0}", knowledgeBase.KnowledgebaseType.ToString()));

                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("CreateBy: {0}", knowledgeBase.CreateBy.ToString()));
                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("CreateDate: {0}", knowledgeBase.CreateDate.ToString()));

                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("PublishedBy: {0}", knowledgeBase.PublishedBy.ToString()));
                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("PublishedDate: {0}", knowledgeBase.PublishedDate.ToString()));

                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("InWorkPhase: {0}", knowledgeBase.InWorkPhase.ToString()));
                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("WorkStatus: {0}", knowledgeBase.WorkStatus.ToString()));

                            if (knowledgeBase.LockedBy != null)
                            {
                                Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("LockedBy: {0}", knowledgeBase.LockedBy.ToString()));
                                Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("LockedClientId: {0}", knowledgeBase.LockedClientId.ToString()));
                                Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("LockedDate: {0}", knowledgeBase.LockedDate.ToString()));
                            }


                            knowledgeBase.InWorkPhase = KnowledgebaseInWorkPhase.DomainManagement;
                            KnowledgebaseOpenResult knowledgebaseOpenResult = KnowledgeBaseManagementEntryPoint.KnowledgebaseOpen(knowledgeBase);
                            Knowledgebase id = (Knowledgebase)knowledgebaseOpenResult.Knowledgebase;
                            Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("OpenState: {0}", knowledgebaseOpenResult.OpenState.ToString()));

                            id.InWorkPhase = KnowledgebaseInWorkPhase.DomainManagement;
                            id = KnowledgeBaseManagementEntryPoint.KnowledgebaseUpdate(id);
                            id = KnowledgeBaseManagementEntryPoint.KnowledgebasePublish(id);

                        }
                        catch (ProxyException ex)
                        {
                            Helper.FireError(ComponentMetaData, ComponentMetaData.Name, ex.ToString());
                        }
                        catch (ServiceException ex)
                        {
                            Helper.FireError(ComponentMetaData, ComponentMetaData.Name, ex.ToString());
                        }
                        catch (EntryPointException ex)
                        {
                            Helper.FireError(ComponentMetaData, ComponentMetaData.Name, ex.ToString());
                        }
                        catch (Exception ex)
                        {
                            Helper.FireError(ComponentMetaData, ComponentMetaData.Name, ex.ToString());
                        }
                    }
                    return true;
                    break;
                case KnowledgebaseInWorkPhase.None:
                    return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
    
    }
}
