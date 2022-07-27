using System;
using System.Globalization;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.Ssdqs.Core.Abstract;
using Microsoft.Ssdqs.Core.Service.KnowledgebaseManagement.Define;
using Microsoft.Ssdqs.Flow.Notification;
using Microsoft.Ssdqs.Infra.Codes;
using Microsoft.Ssdqs.Infra.Exceptions;
using Microsoft.Ssdqs.Proxy.EntryPoint;
using oh22is.SqlServer.DQS.Utilities;

namespace oh22is.SqlServer.DQS
{
#if SQL2012
    [DtsTask(
        Description = "Publish DQS Knowledge Base Task",
        DisplayName = "Publish DQS Knowledge Base Task",
        IconResource = "oh22is.SqlServer.DQS.DQSKBPublish.ico",
        UITypeName = "oh22is.SqlServer.DQS.KBPublishTaskUI, oh22is.SqlServer.DQS.KBPublishTask, Version=1.2.0.0,Culture=neutral,PublicKeyToken=517db833e4f65ba3",
        TaskContact = "oh22information services GmbH",
        RequiredProductLevel = DTSProductLevel.None
        )]
#elif SQL2014
    [DtsTask(
        Description = "Publish DQS Knowledge Base Task",
        DisplayName = "Publish DQS Knowledge Base Task",
        IconResource = "oh22is.SqlServer.DQS.DQSKBPublish.ico",
        UITypeName = "oh22is.SqlServer.DQS.KBPublishTaskUI, oh22is.SqlServer2014.DQS.KBPublishTask, Version=1.2.0.0,Culture=neutral,PublicKeyToken=517db833e4f65ba3",
        TaskContact = "oh22information services GmbH",
        RequiredProductLevel = DTSProductLevel.None
        )]
#elif SQL2016
    [DtsTask(
        Description = "Publish DQS Knowledge Base Task",
        DisplayName = "Publish DQS Knowledge Base Task",
        IconResource = "oh22is.SqlServer.DQS.DQSKBPublish.ico",
        UITypeName = "oh22is.SqlServer.DQS.KBPublishTaskUI, oh22is.SqlServer2016.DQS.KBPublishTask, Version=1.2.0.0,Culture=neutral,PublicKeyToken=517db833e4f65ba3",
        TaskContact = "oh22information services GmbH",
        RequiredProductLevel = DTSProductLevel.None
        )]
#endif
    public class KBPublishTask : Task
    {

        public string ConnectionManager { set; get; }
        public string KnowledgeBase { set; get; }
        public bool Encrypted { set; get; }
        public bool ThrowException { set; get; }

        private NotificationSessionInfo _clientSession;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections"></param>
        /// <param name="variableDispenser"></param>
        /// <param name="componentEvents"></param>
        /// <param name="log"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public override DTSExecResult Execute(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log, object transaction)
        {

            var cm = connections[ConnectionManager];
            
            DataQualityServices.InitializeProxy(Encrypted);

            _clientSession = DataQualityServices.CreateClientSession(cm, DataQualityServices.DatabaseName);

            var serverName = DataQualityServices.ServerName(cm);

            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("ServerCompatibilityVersion: {0}", _clientSession.ServerCompatibilityVersion));
            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("ServerVersion: {0}", _clientSession.ServerVersion));

            Knowledgebase knowledgeBase = DataQualityServices.GetKnowledgeBaseByName(this.KnowledgeBase, serverName, DataQualityServices.DatabaseName, _clientSession.SessionId);

            switch (knowledgeBase.InWorkPhase)
            {

                case KnowledgebaseInWorkPhase.DomainManagement:
                    using (var KnowledgeBaseManagementEntryPoint = ProxyEntryPointFactory.GetKnowledgebaseManagementEntryPoint(serverName, DataQualityServices.DatabaseName, _clientSession.SessionId))
                    {

                        try
                        {

                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("Name: {0}", knowledgeBase.Name));

                            if (!String.IsNullOrEmpty(knowledgeBase.Description))
                            {
                                Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("Description: {0}", knowledgeBase.Description));
                            }

                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("KnowledgebaseType: {0}", knowledgeBase.KnowledgebaseType));

                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("CreateBy: {0}", knowledgeBase.CreateBy));
                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("CreateDate: {0}", knowledgeBase.CreateDate.ToString(CultureInfo.InvariantCulture)));

                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("PublishedBy: {0}", knowledgeBase.PublishedBy));
                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("PublishedDate: {0}", knowledgeBase.PublishedDate));

                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("InWorkPhase: {0}", knowledgeBase.InWorkPhase));
                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("WorkStatus: {0}", knowledgeBase.WorkStatus));

                            if (knowledgeBase.LockedBy != null)
                            {
                                Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("LockedBy: {0}", knowledgeBase.LockedBy));
                                Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("LockedClientId: {0}", knowledgeBase.LockedClientId));
                                Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("LockedDate: {0}", knowledgeBase.LockedDate));
                            }


                            knowledgeBase.InWorkPhase = KnowledgebaseInWorkPhase.DomainManagement;
                            KnowledgebaseOpenResult knowledgebaseOpenResult = KnowledgeBaseManagementEntryPoint.KnowledgebaseOpen(knowledgeBase);
                            Knowledgebase id = (Knowledgebase)knowledgebaseOpenResult.Knowledgebase;
                            Helper.FireInformation(componentEvents, "SSIS DQS KB Publish Task", String.Format("OpenState: {0}", knowledgebaseOpenResult.OpenState));

                            id.InWorkPhase = KnowledgebaseInWorkPhase.DomainManagement;
                            id = KnowledgeBaseManagementEntryPoint.KnowledgebaseUpdate(id);
                            id = KnowledgeBaseManagementEntryPoint.KnowledgebasePublish(id);

                        }
                        catch (ProxyException ex)
                        {
                            Helper.FireError(componentEvents, "SSIS Publish DQS KB Task", ex.ToString());
                        }
                        catch (ServiceException ex)
                        {
                            Helper.FireError(componentEvents, "SSIS Publish DQS KB Task", ex.ToString());
                        }
                        catch (EntryPointException ex)
                        {
                            Helper.FireError(componentEvents, "SSIS Publish DQS KB Task", ex.ToString());
                        }
                        catch (Exception ex)
                        {
                            Helper.FireError(componentEvents, "SSIS Publish DQS KB Task", ex.ToString());
                        }
                    }
                    break;
                case KnowledgebaseInWorkPhase.None:
                    ExecuteResult(componentEvents, knowledgeBase.InWorkPhase, false);
                    break;
                default:
                    ExecuteResult(componentEvents, knowledgeBase.InWorkPhase, ThrowException);
                    break;
            }

            return base.Execute(connections, variableDispenser, componentEvents, log, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentEvents"></param>
        /// <param name="knowledgebaseInWorkPhase"></param>
        /// <param name="throwException"></param>
        private void ExecuteResult(IDTSComponentEvents componentEvents, KnowledgebaseInWorkPhase knowledgebaseInWorkPhase, bool throwException)
        {
            if (throwException)
            {
                Helper.FireError(componentEvents, "SSIS Publish DQS KB Task", String.Format("The KB has not been published. The KB InWorkPhase was {0} ", knowledgebaseInWorkPhase.ToString()));
            }
            else
            {
                Helper.FireWarning(componentEvents, "SSIS Publish DQS KB Task", String.Format("The KB has not been published. The KB InWorkPhase was {0}", knowledgebaseInWorkPhase.ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections"></param>
        /// <param name="variableDispenser"></param>
        /// <param name="events"></param>
        /// <param name="log"></param>
        /// <param name="eventInfos"></param>
        /// <param name="logEntryInfos"></param>
        /// <param name="refTracker"></param>
        public override void InitializeTask(Connections connections, VariableDispenser variableDispenser, IDTSInfoEvents events, IDTSLogging log, EventInfos eventInfos, LogEntryInfos logEntryInfos, ObjectReferenceTracker refTracker)
        {
            base.InitializeTask(connections, variableDispenser, events, log, eventInfos, logEntryInfos, refTracker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections"></param>
        /// <param name="variableDispenser"></param>
        /// <param name="componentEvents"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public override DTSExecResult Validate(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log)
        {

            if (String.IsNullOrEmpty(ConnectionManager))
            {
                Helper.FireError(componentEvents, "SSIS Publish DQS KB Task", "The ConnectionManager property must be configured.");
            }

            if (string.IsNullOrEmpty(KnowledgeBase))
            {
                Helper.FireError(componentEvents, "SSIS Publish DQS KB Task", "The Knowledgebase property must be configured.");
            }

            return base.Validate(connections, variableDispenser, componentEvents, log);
        }

    }
}
