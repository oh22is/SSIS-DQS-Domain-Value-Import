using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.Ssdqs.Component.Common.Utilities;
using Microsoft.Ssdqs.Core.Abstract;
using Microsoft.Ssdqs.Core.Service.KnowledgebaseManagement.Define;
using Microsoft.Ssdqs.DataValueService.Define;
using Microsoft.Ssdqs.Infra.Codes;
using Microsoft.Ssdqs.Proxy.EntryPoint;
using oh22is.SqlServer.DQS.Utilities;
#pragma warning disable 1587

namespace oh22is.SqlServer.DQS
{
#if SQL2012
    [DtsPipelineComponent(
        DisplayName = "DQS Domain Value Import",
        Description = "SSIS DQS Domain Value Import",
        IconResource = "oh22is.SqlServer.DQS.DQSDomainValueDestination.ico",
        UITypeName = "oh22is.SqlServer.DQS.DomainValueDestinationUI, oh22is.SqlServer.DQS.DomainValueDestination, Version=1.2.0.0, Culture=neutral, PublicKeyToken=24d42f83e07154e4",
        ComponentType = ComponentType.DestinationAdapter,
        CurrentVersion = 0)]
#elif SQL2014
    [DtsPipelineComponent(
        DisplayName = "DQS Domain Value Import",
        Description = "SSIS DQS Domain Value Import",
        IconResource = "oh22is.SqlServer.DQS.DQSDomainValueDestination.ico",
        UITypeName = "oh22is.SqlServer.DQS.DomainValueDestinationUI, oh22is.SqlServer2014.DQS.DomainValueDestination, Version=1.2.0.0, Culture=neutral, PublicKeyToken=24d42f83e07154e4",
        ComponentType = ComponentType.DestinationAdapter,
        CurrentVersion = 0)]
#elif SQL2016
    [DtsPipelineComponent(
        DisplayName = "DQS Domain Value Import",
        Description = "SSIS DQS Domain Value Import",
        IconResource = "oh22is.SqlServer.DQS.DQSDomainValueDestination.ico",
        UITypeName = "oh22is.SqlServer.DQS.DomainValueDestinationUI, oh22is.SqlServer2016.DQS.DomainValueDestination, Version=1.2.0.0, Culture=neutral, PublicKeyToken=24d42f83e07154e4",
        ComponentType = ComponentType.DestinationAdapter,
        CurrentVersion = 0)]
#endif
    public class DomainValueDestination : PipelineComponent
    {
        /// <summary>
        /// The DQS ConnectionManager
        /// </summary>
        private ConnectionManager _dqsConnection;
        //private NotificationSessionInfo _ClientSession;
        private long _sessionId;

        private bool _encryptetdConnection;
        private string _knowledgeBase;
        private long _knowledgeBaseId;
        private string _domain;
        private long _domainId;
        private int _totalRowsProcessed;
        private bool _writeLog;
        private Helper.IncorrectValues _incorrectValues = Helper.IncorrectValues.FailComponent;
        private int _countIncorrectValues;
        private Helper.Publish _publishKb = Helper.Publish.Never;

        private string _columnNameLeadingValue;
        private int _columnIdLeadingValue = -1;
        private int _columnIndexLeadingValue = -1;
        
        private string _columnNameValueType;
        private int _columnIdValueType = -1;
        private int _columnIndexValueType = -1;
        
        private string _columnNameSynonymValue;
        private int _columnIdSynonymValue = -1;
        private int _columnIndexSynonymValue = -1;

        private string _columnNameExceptionMessage;
        private int _columnIdExceptionMessage = -1;

        private int _errorColumnId = -1;

        /// <summary>
        /// 
        /// </summary>
        public override void ProvideComponentProperties()
        {
            // Reset the component.
            RemoveAllInputsOutputsAndCustomProperties();
            ComponentMetaData.RuntimeConnectionCollection.RemoveAll();
            ComponentMetaData.InputCollection.RemoveAll();

            ComponentMetaData.UsesDispositions = true;

            IDTSInput100 input = ComponentMetaData.InputCollection.New();
            input.Name = Helper.INPUT_NAME;
            input.HasSideEffects = true;

            // Connection Manager
            var connectionManager = ComponentMetaData.RuntimeConnectionCollection.New();
            connectionManager.Name = "DQSConnectionManager";

            // Create Custom Property Knowledge Base
            IDTSCustomProperty100 idtsKnowledgeBase = ComponentMetaData.CustomPropertyCollection.New();
            idtsKnowledgeBase.Name = "DqsKnowledgeBase";
            idtsKnowledgeBase.Description = "DqsKnowledgeBase";
            idtsKnowledgeBase.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;

            // Create Custom Property Domain
            IDTSCustomProperty100 idtsDomain = ComponentMetaData.CustomPropertyCollection.New();
            idtsDomain.Name = "DqsDomain";
            idtsDomain.Description = "DqsDomain";
            idtsDomain.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;

            // Create Custom Property Encrypted Connection 
            IDTSCustomProperty100 idtsEncryptedConnection = ComponentMetaData.CustomPropertyCollection.New();
            idtsEncryptedConnection.Name = "DqsEncryptedConnection";
            idtsEncryptedConnection.Description = "DqsEncryptedConnection";
            idtsEncryptedConnection.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;

            // Create Custom Property Incorrect Values 
            IDTSCustomProperty100 idtsIncorrectValues = ComponentMetaData.CustomPropertyCollection.New();
            idtsIncorrectValues.Name = "DqsIncorrectValues";
            idtsIncorrectValues.Description = "DqsIncorrectValues";
            idtsIncorrectValues.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;

            // Create Custom Property Publish Kb 
            IDTSCustomProperty100 idtsPublishKb = ComponentMetaData.CustomPropertyCollection.New();
            idtsPublishKb.Name = "DqsPublishKb";
            idtsPublishKb.Description = "DqsPublishKb";
            idtsPublishKb.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;

            // Create Custom Property Incorrect Values 
            IDTSCustomProperty100 idtsWriteLog = ComponentMetaData.CustomPropertyCollection.New();
            idtsWriteLog.Name = "DqsWriteLog";
            idtsWriteLog.Description = "DqsWriteLog";
            idtsWriteLog.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;

            // Create Custom Error Output
            IDTSOutput100 errorOutput = ComponentMetaData.OutputCollection.New();
            errorOutput.IsErrorOut = true;
            errorOutput.Name = Helper.ERROR_NAME;
            errorOutput.SynchronousInputID = input.ID;
            errorOutput.ExclusionGroup = 1;
            errorOutput.Description = "Error output rows are directed to this output.";

            var exceptionColumn = Helper.BuildColumn(errorOutput, "ExceptionMessage", DataType.DT_WSTR, 4000, 0, 0, 0, "");
            _columnNameExceptionMessage = exceptionColumn.Name;
            _columnIdExceptionMessage = exceptionColumn.ID;
            
        }

        /// <summary>
        /// AcquireConnections is called during both component design and execution. 
        /// </summary>
        /// <param name="transaction"></param>
        public override void AcquireConnections(object transaction)
        {
            if (ComponentMetaData.RuntimeConnectionCollection["DQSConnectionManager"].ConnectionManager != null)
            {
                _dqsConnection = null;
                if (ComponentMetaData.RuntimeConnectionCollection.Count > 0)
                {
                    if (ComponentMetaData.RuntimeConnectionCollection["DQSConnectionManager"].ConnectionManager != null)
                    {
                        /// Create a DQS connection
                        _dqsConnection = DtsConvert.GetWrapper(ComponentMetaData.RuntimeConnectionCollection["DQSConnectionManager"].ConnectionManager);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputId"></param>
        /// <param name="virtualInput"></param>
        /// <param name="lineageId"></param>
        /// <param name="usageType"></param>
        /// <returns></returns>
        public override IDTSInputColumn100 SetUsageType(int inputId, IDTSVirtualInput100 virtualInput, int lineageId, DTSUsageType usageType)
        {
            var inputColumn = base.SetUsageType(inputId, virtualInput, lineageId, usageType);

            var custPropLeadingValue = inputColumn.CustomPropertyCollection.New();
            custPropLeadingValue.Name = "LeadingValue";
            custPropLeadingValue.Value = (object)false;

            var custPropValueType = inputColumn.CustomPropertyCollection.New();
            custPropValueType.Name = "ValueType";
            custPropValueType.Value = (object)false;

            var custPropSynonymValue = inputColumn.CustomPropertyCollection.New();
            custPropSynonymValue.Name = "SynonymValue";
            custPropSynonymValue.Value = (object)false;

            return inputColumn;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PreExecute()
        {

            #region Create client session and define DQS properties
            
            _encryptetdConnection = Convert.ToBoolean(ComponentMetaData.CustomPropertyCollection["DqsEncryptedConnection"].Value.ToString());
            _writeLog = Convert.ToBoolean(ComponentMetaData.CustomPropertyCollection["DqsWriteLog"].Value.ToString());
            DataQualityServices.InitializeProxy(_encryptetdConnection);
            _sessionId = DataQualityServices.CreateClientSessionId(_dqsConnection, DataQualityServices.DatabaseName);

            _knowledgeBase = ComponentMetaData.CustomPropertyCollection["DqsKnowledgeBase"].Value.ToString();

            if (DataQualityServices.IsOpen(_knowledgeBase, DataQualityServices.ServerName(_dqsConnection), DataQualityServices.DatabaseName, _sessionId))
            {
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "It's not possible to add domain values to a Domain, when the knowledge base is already opened and not in state \"domain management\".");
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "Publish the knowledge base before import new domain values.");
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "Make sure that no one makes changes to the knowledge base during package execution.");
                throw new Exception("Open Knowledge Base");
            }
            else
            {

                _knowledgeBaseId = DataQualityServices.GetKnowledgeBaseIdByName(_knowledgeBase, DataQualityServices.ServerName(_dqsConnection), DataQualityServices.DatabaseName, _sessionId);

                _domain = ComponentMetaData.CustomPropertyCollection["DqsDomain"].Value.ToString();
                _domainId = DataQualityServices.GetDomainIdByName(_domain, DataQualityServices.ServerName(_dqsConnection), DataQualityServices.DatabaseName, _sessionId, _knowledgeBaseId);

                _incorrectValues = Helper.SetIncorrectValues(ComponentMetaData.CustomPropertyCollection["DqsIncorrectValues"].Value.ToString());

                _publishKb = Helper.SetPublishValue(ComponentMetaData.CustomPropertyCollection["DqsPublishKb"].Value.ToString());

                Knowledgebase knowledgeBase = DataQualityServices.GetKnowledgeBaseByName(_knowledgeBase, DataQualityServices.ServerName(_dqsConnection), DataQualityServices.DatabaseName, _sessionId);

                var knowledgeBaseManagementEntryPoint = ProxyEntryPointFactory.GetKnowledgebaseManagementEntryPoint(DataQualityServices.ServerName(_dqsConnection), DataQualityServices.DatabaseName, _sessionId);
                {
                    knowledgeBase.InWorkPhase = KnowledgebaseInWorkPhase.DomainManagement;
                    var knowledgebaseOpenResult = knowledgeBaseManagementEntryPoint.KnowledgebaseOpen(knowledgeBase);
                    var id = (Knowledgebase)knowledgebaseOpenResult.Knowledgebase;
                    Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, String.Format("OpenState: {0}", knowledgebaseOpenResult.OpenState.ToString()));
                    id.InWorkPhase = KnowledgebaseInWorkPhase.DomainManagement;
                    knowledgeBaseManagementEntryPoint.KnowledgebaseUpdate(id);
                }

                #region Set Input Column

                var columnIndex = 0;

                var input = Helper.GetInput(ComponentMetaData);

                foreach (IDTSInputColumn100 inputColumn in ComponentMetaData.InputCollection[0].InputColumnCollection)
                {
                    bool bLeadingVallue = (inputColumn.CustomPropertyCollection["LeadingValue"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["LeadingValue"].Value.ToString()) : false);
                    if (bLeadingVallue)
                    {
                        _columnIdLeadingValue = inputColumn.ID;
                        _columnIndexLeadingValue = BufferManager.FindColumnByLineageID(input.Buffer, inputColumn.LineageID);
                        _columnNameLeadingValue = inputColumn.Name;
                        _errorColumnId = inputColumn.ID;
                    }

                    bool bValueType = (inputColumn.CustomPropertyCollection["ValueType"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["ValueType"].Value.ToString()) : false);
                    if (bValueType)
                    {
                        _columnIdValueType = inputColumn.ID;
                        _columnIndexValueType = BufferManager.FindColumnByLineageID(input.Buffer, inputColumn.LineageID);
                        _columnNameValueType = inputColumn.Name;
                    }

                    bool bSynonymValue = (inputColumn.CustomPropertyCollection["SynonymValue"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["SynonymValue"].Value.ToString()) : false);
                    if (bSynonymValue)
                    {
                        _columnIdSynonymValue = inputColumn.ID;
                        _columnIndexSynonymValue = BufferManager.FindColumnByLineageID(input.Buffer, inputColumn.LineageID);
                        _columnNameSynonymValue = inputColumn.Name;
                    }

                    columnIndex++;
                }

                #endregion

            }
               
            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        public override void PostExecute()
        {
            
            switch (_publishKb)
            {
                case Helper.Publish.Never:
                    Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, "The knowledge base was not published.");
                    break;
                case Helper.Publish.Always:
                    DataQualityServices.PublishKnowledgeBase(_knowledgeBase, DataQualityServices.ServerName(_dqsConnection), _sessionId, true, ComponentMetaData);
                    break;
                case Helper.Publish.WithNoError:
                    if (_countIncorrectValues == 0)
                    {
                        DataQualityServices.PublishKnowledgeBase(_knowledgeBase, DataQualityServices.ServerName(_dqsConnection), _sessionId, true, ComponentMetaData);
                    }
                    else
                    {
                        Helper.FireWarning(ComponentMetaData, ComponentMetaData.Name, "Some errors occured. The knowledge base was not published.");
                    }
                    break;
            }

            
            Helper.FireProgress(ComponentMetaData, String.Format("{0} total rows processed into domain {1}", _totalRowsProcessed, _domain), 100, 0, 100, ComponentMetaData.Name);
            if (_countIncorrectValues > 0)
            {
                Helper.FireWarning(ComponentMetaData, ComponentMetaData.Name, String.Format("{0} total rows processed with errors", _countIncorrectValues));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputId"></param>
        /// <param name="buffer"></param>
        public override void ProcessInput(int inputId, PipelineBuffer buffer)
        {
            
            var rowsProcessed = 0;

            /// Create a ReadOnlyString Collection to upload incoming data to DQS
            List<DomainValue> domain = new List<DomainValue>();

            while (buffer.NextRow())
            {
                rowsProcessed++;
                _totalRowsProcessed++;
                
                var strLeadingValue = GetBufferString(buffer, _columnIndexLeadingValue);
                var strSynonymValue = GetBufferString(buffer, _columnIndexSynonymValue);
                var strDomainValueType = GetBufferString(buffer, _columnIndexValueType);
                var intDomainValueType = GetBufferInt(buffer, _columnIndexValueType);
                var domainValueStatus = DomainValueStatus.Correct;
                DomainValue leadingValue = new DomainValue();
                DomainValue synonymValue = new DomainValue();
                
                if (_columnIndexValueType != -1)
                {
                    domainValueStatus = DataQualityServices.GetDomainValueStatus(intDomainValueType);
                }
                
                if (domainValueStatus == DomainValueStatus.None && String.IsNullOrEmpty(strSynonymValue))
                {
                    switch (_incorrectValues)
                    {
                        case Helper.IncorrectValues.FailComponent:
                            strDomainValueType = (String.IsNullOrEmpty(strDomainValueType) ? "NULL" : strDomainValueType);
                            Helper.FireError(ComponentMetaData, ComponentMetaData.Name, String.Format("{0} is not a correct Domain Value Type.", strDomainValueType));
                            break;
                        case Helper.IncorrectValues.RedirectRows:

                            var errorOutputId = -1;
                            var errorOutputIndex = -1;
                            int dtsEErrortriggeredredirection;
                            unchecked
                            {
                                dtsEErrortriggeredredirection = (int)0xC020401E;
                            }

                            GetErrorOutputInfo(ref errorOutputId, ref errorOutputIndex);
                            buffer.DirectErrorRow(ComponentMetaData.OutputCollection[0].ID, dtsEErrortriggeredredirection, _errorColumnId);
                            buffer.SetString(buffer.ColumnCount - 1, "Invalid Domain Value Type.");

                            _countIncorrectValues++;
                            break;
                        case Helper.IncorrectValues.Ignore:
                            strDomainValueType = (String.IsNullOrEmpty(strDomainValueType) ? "NULL" : strDomainValueType);
                            Helper.FireWarning(ComponentMetaData, ComponentMetaData.Name, String.Format("{0} is not a correct Domain Value Type.", strDomainValueType));
                            break;
                    }
                }
                else
                {
                    try
                    {
                        if (String.IsNullOrEmpty(strSynonymValue))
                        {
                            leadingValue = new DomainValue(strLeadingValue, _domainId, domainValueStatus);
                            domain.Add(leadingValue);
                        }
                        else
                        {
                            synonymValue = new DomainValue(strSynonymValue, _domainId, DomainValueStatus.NotValid, strLeadingValue);
                            domain.Add(synonymValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.FireError(ComponentMetaData, ComponentMetaData.Name, String.Format("Domain values could not be created. Please check Input values."));
                        throw new Exception("Domain values could not be created. Please check Input values.");
                    }

                    if (domain.Count > 0)
                    {
                        switch (_incorrectValues)
                        {
                            case Helper.IncorrectValues.RedirectRows:
                                var serviceResult = DataQualityServices.SetDomainValues(new ReadOnlyCollection<DomainValue>(domain), DataQualityServices.ServerName(_dqsConnection), _sessionId, _knowledgeBaseId, _domainId);
                                domain.Clear();

                                if (serviceResult[0].Exception != null)
                                {

                                    var errorOutputId = -1;
                                    var errorOutputIndex = -1;
                                    int dtsEErrortriggeredredirection;
                                    unchecked
                                    {
                                        dtsEErrortriggeredredirection = (int)0xC020401E;
                                    }

                                    GetErrorOutputInfo(ref errorOutputId, ref errorOutputIndex);
                                    buffer.DirectErrorRow(ComponentMetaData.OutputCollection[0].ID, dtsEErrortriggeredredirection, _errorColumnId);
                                    buffer.SetString(buffer.ColumnCount - 1, serviceResult[0].Exception.Message);

                                    _countIncorrectValues++;

                                }
                                break;
                            case Helper.IncorrectValues.FailComponent:
                            case Helper.IncorrectValues.Ignore:
                                if (domain.Count >= 1000)
                                {
                                    ProcessDomains(domain);
                                    domain.Clear();
                                }
                                break;
                        }
                    }
                }
            }

            if (domain.Count > 0)
            {
                ProcessDomains(domain);
                domain.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        private void ProcessDomains(List<DomainValue> domain)
        {
            int errorCounter = 0;

            try
            {
                var serviceResult = DataQualityServices.SetDomainValues(new ReadOnlyCollection<DomainValue>(domain), DataQualityServices.ServerName(_dqsConnection), _sessionId, _knowledgeBaseId, _domainId);
                
                foreach (ServiceResult srvR in serviceResult)
                {
                    if (srvR.Exception != null)
                    {
                        errorCounter++;
                        if (_writeLog)
                        {
                            Helper.FireWarning(ComponentMetaData, ComponentMetaData.Name, srvR.Exception.Message);
                        }
                    }
                }

                _countIncorrectValues += errorCounter;

                if (errorCounter > 0 && _incorrectValues == Helper.IncorrectValues.FailComponent)
                {
                    Helper.FireError(ComponentMetaData, ComponentMetaData.Name, String.Format("During the batch upload {0} error occurred.", errorCounter));
                    throw new Exception(String.Format("During the batch upload {0} error occurred.", errorCounter));
                }
            }
            catch (Exception ex)
            {
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, String.Format("Errors occurred during import. The data have not been processed."));
                throw new Exception(String.Format("During the batch upload {0} error occurred.", errorCounter));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetString(object value)
        {
            return value != null ? value.ToString() : String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DTSValidationStatus Validate()
        {
            DTSValidationStatus dtsValidationStatus = ComponentValidation.ValidateComponentInitialData(ComponentMetaData);

            if (dtsValidationStatus != DTSValidationStatus.VS_ISVALID)
            {
                return dtsValidationStatus;
            }

            if (ComponentMetaData.InputCollection[0].InputColumnCollection.Count
                != ComponentMetaData.InputCollection[0].GetVirtualInput().VirtualInputColumnCollection.Count)
            {
                ReinitializeMetaData();
            }

            if (ComponentMetaData.InputCollection.Count == 0) return DTSValidationStatus.VS_ISCORRUPT;

            try
            {
                _incorrectValues =
                    Helper.SetIncorrectValues(
                        ComponentMetaData.CustomPropertyCollection["DqsIncorrectValues"].Value.ToString());
                switch (_incorrectValues)
                {
                    case Helper.IncorrectValues.FailComponent:
                        ComponentMetaData.InputCollection[0].ErrorRowDisposition = DTSRowDisposition.RD_FailComponent;
                        break;
                    case Helper.IncorrectValues.Ignore:
                        ComponentMetaData.InputCollection[0].ErrorRowDisposition = DTSRowDisposition.RD_IgnoreFailure;
                        break;
                    case Helper.IncorrectValues.RedirectRows:
                        ComponentMetaData.InputCollection[0].ErrorRowDisposition = DTSRowDisposition.RD_RedirectRow;
                        break;
                    default:
                        ComponentMetaData.InputCollection[0].ErrorRowDisposition = DTSRowDisposition.RD_NotUsed;
                        break;
                }
            }
            catch(Exception ex)
            {
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, ex.ToString());
            }

            if (!DataQualityServices.InitializeProxy(_encryptetdConnection))
            {
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "The Proxy could not be initialized.");
                return DTSValidationStatus.VS_ISBROKEN;
            }

            #region Validate Component Properties

            //CommonNoMappingsSpecified
            if (ComponentMetaData.CustomPropertyCollection["DqsKnowledgeBase"].Value == null)
            {
                Helper.FireInformation(ComponentMetaData, ComponentMetaData.Name, "A knowledge base is not defined.");
                return DTSValidationStatus.VS_ISBROKEN;
            }

            //CommonNoMappingsSpecified
            if (ComponentMetaData.CustomPropertyCollection["DqsDomain"].Value == null)
            {
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "A domain base is not defined.");
                return DTSValidationStatus.VS_ISBROKEN;
            }

            //CommonNoMappingsSpecified
            bool bLeadingValue = false;
            foreach (IDTSInputColumn100 inputColumn in ComponentMetaData.InputCollection[0].InputColumnCollection)
            {
                bLeadingValue = (inputColumn.CustomPropertyCollection["LeadingValue"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["LeadingValue"].Value.ToString()) : false);
                if (bLeadingValue)
                {
                    break;
                }
            }
            if (bLeadingValue == false)
            {
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "A leading value is not defined.");
                return DTSValidationStatus.VS_ISBROKEN;
            }

            if (ComponentMetaData.CustomPropertyCollection["DqsIncorrectValues"].Value == null)
            {
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "Handling incorrect values seems to be corrupt.");
                return DTSValidationStatus.VS_ISBROKEN;
            }

            if (Helper.SetIncorrectValues(ComponentMetaData.CustomPropertyCollection["DqsIncorrectValues"].Value.ToString()) == Helper.IncorrectValues.RedirectRows
                && ComponentMetaData.OutputCollection[0].IsAttached == false)
            {
                Helper.FireWarning(ComponentMetaData, ComponentMetaData.Name, "Handling incorrect values seems to be corrupt.");
                return DTSValidationStatus.VS_ISBROKEN;
            }

            #endregion

            #region DQS related validations

            /// checks the DQS server settings
            try
            {
                if (_dqsConnection != null)
                {
                    VerificationResult verificationResult = ComponentUtility.VerifyServer(_dqsConnection.ConnectionString, DataQualityServices.DatabaseName);
                    if (verificationResult == VerificationResult.ServerVerified)
                    {

                        /// checks if the DQS communication is valid
                        if (!ComponentUtility.IsCommunicationValid(ComponentMetaData, DataQualityServices.DatabaseName))
                        {
                            return DTSValidationStatus.VS_ISBROKEN;
                        }

                    }
                    switch (verificationResult)
                    {

                        case VerificationResult.ClientNotCompatible:
                            {
                                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "");
                                break;
                            }
                        case VerificationResult.FrameworkVersionNotCompatible:
                            {
                                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "DqsInstaller.exe -upgrade");
                                break;
                            }
                    }
                }
                else
                {
                    Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "");
                    return DTSValidationStatus.VS_ISBROKEN;
                }
            }
            catch (Exception ex)
            {
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, "An error occurred while retrieving the connection manager.");
                Helper.FireError(ComponentMetaData, ComponentMetaData.Name, ex.ToString());
                return DTSValidationStatus.VS_ISBROKEN;
            }

            #endregion

            return base.Validate();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ReleaseConnections()
        {
            try
            {
                if (_dqsConnection != null)
                {
                    _dqsConnection.Dispose();
                }
                _dqsConnection = null;
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputId"></param>
        public override void OnInputPathAttached(int inputId)
        {

            IDTSInput100 input = ComponentMetaData.InputCollection.GetObjectByID(inputId);
            input.InputColumnCollection.RemoveAll();

            ReinitializeMetaData();

        }
     
        /// <summary>
        /// 
        /// </summary>
        public override void ReinitializeMetaData()
        {

            IDTSInput100 input = ComponentMetaData.InputCollection[0];

            foreach (IDTSInputColumn100 inputColumn in ComponentMetaData.InputCollection[0].InputColumnCollection)
            {
                bool bLeadingVallue = (inputColumn.CustomPropertyCollection["LeadingValue"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["LeadingValue"].Value.ToString()) : false);
                if (bLeadingVallue)
                {
                    _columnNameLeadingValue = inputColumn.Name;
                }

                bool bValueType = (inputColumn.CustomPropertyCollection["ValueType"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["ValueType"].Value.ToString()) : false);
                if (bValueType)
                {
                    _columnNameValueType = inputColumn.Name;
                }

                bool bSynonymValue = (inputColumn.CustomPropertyCollection["SynonymValue"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["SynonymValue"].Value.ToString()) : false);
                if (bSynonymValue)
                {
                    _columnNameSynonymValue = inputColumn.Name;
                }
            }


            input.InputColumnCollection.RemoveAll();

            IDTSVirtualInput100 vInput = input.GetVirtualInput();
            
            foreach (IDTSVirtualInputColumn100 vCol in vInput.VirtualInputColumnCollection)
            {
                SetUsageType(input.ID, vInput, vCol.LineageID, DTSUsageType.UT_READONLY);
            }

            foreach (IDTSInputColumn100 inputColumn in ComponentMetaData.InputCollection[0].InputColumnCollection)
            {
                if (inputColumn.Name == _columnNameLeadingValue) inputColumn.CustomPropertyCollection["LeadingValue"].Value = "true";
                if (inputColumn.Name == _columnNameValueType) inputColumn.CustomPropertyCollection["ValueType"].Value = "true";
                if (inputColumn.Name == _columnNameSynonymValue) inputColumn.CustomPropertyCollection["SynonymValue"].Value = "true";
            }

            if (!ComponentMetaData.AreInputColumnsValid)
            {
                foreach (IDTSInputColumn100 column in input.InputColumnCollection)
                {
                    IDTSVirtualInputColumn100 vColumn = vInput.VirtualInputColumnCollection.GetVirtualInputColumnByLineageID(column.LineageID);

                    if (vColumn == null)
                    {
                        input.InputColumnCollection.RemoveObjectByID(column.ID);
                    }
                }
            }

            base.ReinitializeMetaData();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private string GetBufferString(PipelineBuffer buffer, int columnIndex)
        {
            if (columnIndex == -1) return String.Empty;
            if (buffer[columnIndex] == null) return String.Empty;
            if (String.IsNullOrWhiteSpace(buffer[columnIndex].ToString())) return String.Empty;

            var retValue = buffer[columnIndex].ToString();

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private int GetBufferInt(PipelineBuffer buffer, int columnIndex)
        {
            if (columnIndex == -1) return -1;
            if (buffer[columnIndex] == null) return -1;

            var retValue = buffer.GetInt32(columnIndex);

            return retValue;
        }
    }
}
