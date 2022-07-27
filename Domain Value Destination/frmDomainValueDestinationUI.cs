using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using Microsoft.Ssdqs.Component.Common.Logic;
using Microsoft.Ssdqs.Core.Service.KnowledgebaseManagement.Define;
using Microsoft.Ssdqs.DataService.Define;
using Microsoft.Ssdqs.Flow.Notification;
using Microsoft.Ssdqs.Proxy.EntryPoint;
using oh22is.SqlServer.DQS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#pragma warning disable 1587

namespace oh22is.SqlServer.DQS
{
    public partial class frmDomainValueDestinationUI : Form
    {
        public readonly Connections Connections;
        private readonly IServiceProvider _serviceProvider;
        private ConnectionManager _dqsConnection;
        public readonly IDTSComponentMetaData100 Component;
        private NotificationSessionInfo _clientSession;
        private Variables _variables;
        private DataTable _inputColumns;
        private BindingSource _bsSynonymValue;
        private BindingSource _bsLeadingValue;
        private BindingSource _bsValueType;

        /// <summary>
        /// COMPONENT PROPERTIES
        /// </summary>
        private string _dqsConnectionManager;
        private string _dqsKnowledgeBase;
        private string _dqsDomain;
        private bool _encryptConnection;
        private string _incorrectValues = "Fail Component";
        private const string SelectInputColumn = "Select an input column";
        private bool _writeLog;
        private string _publlishKb = "Publish When There Is No Error";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtsComponentMetadata"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="variables"></param>
        /// <param name="connections"></param>
        public frmDomainValueDestinationUI(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider, Variables variables, Connections connections)
        {
            InitializeComponent();
            Connections = connections;
            Component = dtsComponentMetadata;
            _serviceProvider = serviceProvider;
            _variables = variables;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDomainValueDestinationUI_Load(object sender, EventArgs e)
        {

            #region Set Connection Manager
            
            try
            {
                if (Component.RuntimeConnectionCollection.Count > 0)
                {
                    IDTSRuntimeConnection100 conn = Component.RuntimeConnectionCollection[0];
                    if (conn != null
                        && conn.ConnectionManagerID.Length > 0
                        && Connections.Contains(conn.ConnectionManagerID))
                    {
                        _dqsConnectionManager = Connections[conn.ConnectionManagerID].Name;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessageBox("An error occurred while retrieving the connection manager.", ex, this);
            }

            var dqcmExist = false;

            cbConnectionManager.Items.Clear();
            foreach (var cm in Connections.Cast<ConnectionManager>().Where(cm => DataQualityServices.IsDqsConnection(cm)))
            {
                this.cbConnectionManager.Items.Add(cm.Name);
                if (cm.Name == _dqsConnectionManager) dqcmExist = true;
            }

            if (dqcmExist)
            {
                cbConnectionManager.SelectedItem = cbConnectionManager.Items[cbConnectionManager.Items.IndexOf(_dqsConnectionManager)];
            }

            #endregion

            #region Set Input Columns

            _inputColumns = new DataTable("Columns");
            _inputColumns.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            _inputColumns.Columns.Add(new DataColumn("Column", Type.GetType("System.String")));

            _inputColumns.Rows.Add(0, SelectInputColumn);
            foreach (IDTSInputColumn100 vcol in Component.InputCollection[0].InputColumnCollection)
            {
                _inputColumns.Rows.Add(vcol.ID, vcol.Name);
            }

            _bsSynonymValue = new BindingSource {DataSource = new DataView(_inputColumns)};
            _bsLeadingValue = new BindingSource {DataSource = new DataView(_inputColumns)};
            _bsValueType = new BindingSource {DataSource = new DataView(_inputColumns)};

            cbLeadingValue.DataSource = _bsLeadingValue;
            cbLeadingValue.DisplayMember = "Column";
            cbLeadingValue.ValueMember = "ID";
            cbLeadingValue.SelectedIndex = -1;

            cbSynonymValue.DataSource = _bsSynonymValue;
            cbSynonymValue.DisplayMember = "Column";
            cbSynonymValue.ValueMember = "ID";
            cbSynonymValue.SelectedIndex = -1;

            cbValueType.DataSource = _bsValueType;
            cbValueType.DisplayMember = "Column";
            cbValueType.ValueMember = "ID";
            cbValueType.SelectedIndex = -1;

            #endregion

            #region Set Component Properties

            try
            {

                _dqsKnowledgeBase = (Component.CustomPropertyCollection["DqsKnowledgeBase"].Value != null ? Component.CustomPropertyCollection["DqsKnowledgeBase"].Value.ToString() : String.Empty);
                _dqsDomain = (Component.CustomPropertyCollection["DqsDomain"].Value != null ? Component.CustomPropertyCollection["DqsDomain"].Value.ToString() : String.Empty);
                _encryptConnection = (Component.CustomPropertyCollection["DqsEncryptedConnection"].Value != null ? Convert.ToBoolean(Component.CustomPropertyCollection["DqsEncryptedConnection"].Value.ToString()) : false);
                _incorrectValues = (Component.CustomPropertyCollection["DqsIncorrectValues"].Value != null ? Component.CustomPropertyCollection["DqsIncorrectValues"].Value.ToString() : "Fail Component");
                _writeLog = (Component.CustomPropertyCollection["DqsWriteLog"].Value != null ? Convert.ToBoolean(Component.CustomPropertyCollection["DqsWriteLog"].Value.ToString()) : false);
                _publlishKb = (Component.CustomPropertyCollection["DqsPublishKb"].Value != null ? Component.CustomPropertyCollection["DqsPublishKb"].Value.ToString() : "Publish When There Is No Error");

                if (!String.IsNullOrEmpty(_dqsKnowledgeBase))
                {
                    cbKnowledgeBase.SelectedItem = cbKnowledgeBase.Items[cbKnowledgeBase.Items.IndexOf(_dqsKnowledgeBase)];
                }
                
                if (!String.IsNullOrEmpty(_dqsDomain))
                {
                    cbDomain.SelectedItem = cbDomain.Items[cbDomain.Items.IndexOf(_dqsDomain)];
                }
                
                if (!String.IsNullOrEmpty(_incorrectValues))
                {
                    cbIncorrectValues.SelectedItem = cbIncorrectValues.Items[cbIncorrectValues.Items.IndexOf(_incorrectValues)];
                    cbLog.Enabled = !(cbIncorrectValues.Items[cbIncorrectValues.SelectedIndex].ToString() == "Redirect rows to error output");
                }

                if (!String.IsNullOrEmpty(_publlishKb))
                {
                    cbPublish.SelectedItem = cbPublish.Items[cbPublish.Items.IndexOf(_publlishKb)];
                }

                cbEncryptConnection.Checked = _encryptConnection;
                cbLog.Checked = _writeLog;

            }
            catch (Exception ex)
            {
                Helper.ExceptionMessageBox("The settings could not be loaded.", ex, this);
            }

            foreach (IDTSInputColumn100 inputColumn in Component.InputCollection[0].InputColumnCollection)
            {
                
                bool bLeadingVallue = (inputColumn.CustomPropertyCollection["LeadingValue"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["LeadingValue"].Value.ToString()) : false);
                if (bLeadingVallue)
                {
                    cbLeadingValue.SelectedValue = inputColumn.ID;
                }
                else if (cbLeadingValue.SelectedValue == null)
                {
                    cbLeadingValue.SelectedValue = 0;
                }

                bool bValueType = (inputColumn.CustomPropertyCollection["ValueType"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["ValueType"].Value.ToString()) : false);
                if (Convert.ToBoolean(inputColumn.CustomPropertyCollection["ValueType"].Value) == true)
                {
                    cbValueType.SelectedValue = inputColumn.ID;
                }
                else if (cbValueType.SelectedValue == null)
                {
                    cbValueType.SelectedValue = 0;
                }

                bool bSynonymValue = (inputColumn.CustomPropertyCollection["SynonymValue"].Value != null ? Convert.ToBoolean(inputColumn.CustomPropertyCollection["SynonymValue"].Value.ToString()) : false);
                if (Convert.ToBoolean(inputColumn.CustomPropertyCollection["SynonymValue"].Value) == true)
                {
                    cbSynonymValue.SelectedValue = inputColumn.ID;
                }
                else if (cbSynonymValue.SelectedValue == null)
                {
                    cbSynonymValue.SelectedValue = 0;
                }

            }

            #endregion
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {

                /// Creates a new Connection Service and opens the DQS Cleansing Connection Manager window
                var connService = (IDtsConnectionService)_serviceProvider.GetService(typeof(IDtsConnectionService));
                var created = connService.CreateConnection("DQS");

                foreach (ConnectionManager cm in created)
                {
                    cbConnectionManager.Items.Insert(0, cm.Name);
                }

                if (created.Count > 0)
                {
                    cbConnectionManager.SelectedIndex = 0;
                }
                else
                {
                    cbConnectionManager.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessageBox("An error occurred while creating a connection manager.", ex, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbConnectionManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string name = cbConnectionManager.Items[cbConnectionManager.SelectedIndex].ToString();

                foreach (ConnectionManager cm in Connections)
                {
                    if (cm.Name == name)
                    {
                        _dqsConnection = cm;
                        Component.RuntimeConnectionCollection["DQSConnectionManager"].ConnectionManager = DtsConvert.GetExtendedInterface(Connections[cbConnectionManager.SelectedItem]);
                        Component.RuntimeConnectionCollection["DQSConnectionManager"].ConnectionManagerID = Connections[cbConnectionManager.SelectedItem].ID;
                        break;
                    }
                }

                if (_dqsConnection != null)
                {

                    DataQualityServices.InitializeProxy(cbEncryptConnection.Checked);

                    _clientSession = DataQualityServices.CreateClientSession(_dqsConnection, DataQualityServices.DatabaseName);

                    var connectionStringParameters = DataQualityConnectorFinals.GetConnectionStringParameters(_dqsConnection.ConnectionString);

                    cbKnowledgeBase.Items.Clear();
                    var knowledgeBase = ProxyEntryPointFactory.GetKnowledgebaseManagementEntryPoint(DataQualityServices.ServerName(_dqsConnection), DataQualityServices.DatabaseName, _clientSession.SessionId);
                    {
                        var allKbs = knowledgeBase.KnowledgebaseGet();
                        cbKnowledgeBase.Items.Clear();

                        foreach (var kb in allKbs)
                        {
                            cbKnowledgeBase.Items.Add(kb.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessageBox("An error occurred while retrieving the connection manager.", ex, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbKnowledgeBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_dqsConnection != null)
                {

                    DataQualityServices.InitializeProxy(cbEncryptConnection.Checked);
                    var knowledgeBaseId = DataQualityServices.GetKnowledgeBaseIdByName(cbKnowledgeBase.Items[cbKnowledgeBase.SelectedIndex].ToString(), DataQualityServices.ServerName(_dqsConnection), DataQualityServices.DatabaseName, _clientSession.SessionId);

                    cbDomain.Items.Clear();
                    var metadataManagementEntryPoint = ProxyEntryPointFactory.GetMetadataManagementEntryPoint(DataQualityServices.ServerName(_dqsConnection), DataQualityServices.DatabaseName, _clientSession.SessionId, knowledgeBaseId);
                    {
                        var domains = metadataManagementEntryPoint.DomainGetAll();
                        cbDomain.Items.Clear();
                        foreach (var domain in domains)
                        {
                            cbDomain.Items.Add(domain.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessageBox("An error occurred while retrieving the knowledge base.", ex, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (PropertyValidate())
            {

                try
                {
                    Component.CustomPropertyCollection["DqsKnowledgeBase"].Value = cbKnowledgeBase.Items[cbKnowledgeBase.SelectedIndex].ToString();
                    Component.CustomPropertyCollection["DqsDomain"].Value = cbDomain.Items[cbDomain.SelectedIndex].ToString();
                    Component.CustomPropertyCollection["DqsEncryptedConnection"].Value = cbEncryptConnection.Checked.ToString();
                    Component.CustomPropertyCollection["DqsIncorrectValues"].Value = cbIncorrectValues.Items[cbIncorrectValues.SelectedIndex].ToString();
                    Component.CustomPropertyCollection["DqsWriteLog"].Value = cbLog.Checked.ToString();
                    Component.CustomPropertyCollection["DqsPublishKb"].Value = cbPublish.Items[cbPublish.SelectedIndex].ToString();

                    string columnLeadingValue = (_inputColumns.Select(String.Format("ID = {0}", cbLeadingValue.SelectedValue)))[0]["Column"].ToString();
                    string columnValueType = (_inputColumns.Select(String.Format("ID = {0}", cbValueType.SelectedValue)))[0]["Column"].ToString();
                    string columnSynonymValue = (_inputColumns.Select(String.Format("ID = {0}", cbSynonymValue.SelectedValue)))[0]["Column"].ToString();

                    foreach (IDTSInputColumn100 inputColumn in Component.InputCollection[0].InputColumnCollection)
                    {

                       inputColumn.CustomPropertyCollection["LeadingValue"].Value = inputColumn.Name == columnLeadingValue;
                       inputColumn.CustomPropertyCollection["ValueType"].Value = inputColumn.Name == columnValueType;
                       inputColumn.CustomPropertyCollection["SynonymValue"].Value = inputColumn.Name == columnSynonymValue;

                    }

                    DialogResult = DialogResult.OK;

                }
                catch (Exception ex)
                {
                    Helper.ExceptionMessageBox("The settings could not be saved.", ex, this);
                }
            }
            else
            {
                MessageBox.Show("The component is not fully configured.", Component.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBox1"></param>
        /// <param name="comboBox2"></param>
        /// <returns></returns>
        private string GetFilter(ComboBox comboBox1, ComboBox comboBox2)
        {

            string strComboBox1 = null;
            string strComboBox2 = null;

            if (comboBox1.SelectedIndex != -1 && comboBox1.SelectedIndex != 0 )
            {
                strComboBox1 = comboBox1.SelectedValue.ToString();
            }

            if (comboBox2.SelectedIndex != -1 && comboBox2.SelectedIndex != 0)
            {
                strComboBox2 = comboBox2.SelectedValue.ToString();
            }

            var strFilter = (String.IsNullOrEmpty(strComboBox1) ? "" : strComboBox1);
            strFilter += (String.IsNullOrEmpty(strFilter) || String.IsNullOrEmpty(strComboBox2) ? "" : ",");
            strFilter += (String.IsNullOrEmpty(strComboBox2) ? "" : strComboBox2);

            if (comboBox1.Name != "cbValueType"
                && comboBox2.Name != "cbValueType")
            {
                strFilter += (String.IsNullOrEmpty(strFilter) ? "" : ",");
                strFilter += GetValueTypeFilterColumns();
            }

            if (!String.IsNullOrEmpty(strFilter) && strFilter != ",")
            {
                strFilter = "ID not in (" + strFilter + ")";
            }

            return strFilter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLeadingValue_Click(object sender, EventArgs e)
        {
            
            cbLeadingValue.DataSource = _bsLeadingValue;
            _bsLeadingValue.Filter = GetFilter(cbSynonymValue, cbValueType);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbValueType_Click(object sender, EventArgs e)
        {
            cbValueType.DataSource = _bsValueType;
            _bsValueType.Filter = GetFilter(cbLeadingValue, cbSynonymValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSynonymValue_Click(object sender, EventArgs e)
        {
            cbSynonymValue.DataSource = _bsSynonymValue;
            _bsSynonymValue.Filter = GetFilter(cbLeadingValue, cbValueType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool PropertyValidate()
        {
            if (cbConnectionManager.SelectedIndex == -1) return false;
            if (cbKnowledgeBase.SelectedIndex == -1) return false;
            if (cbDomain.SelectedIndex == -1) return false;
            switch (cbLeadingValue.SelectedIndex)
            {
                case -1:
                    return false;
                case 0:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetValueTypeFilterColumns()
        {
            string value = String.Empty;
            foreach (IDTSInputColumn100 col in Component.InputCollection[0].InputColumnCollection)
            {
                if (col.DataType != Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I1
                    && col.DataType != Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I2
                    && col.DataType != Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I4
                    && col.DataType != Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I8)
                {
                    if (!String.IsNullOrWhiteSpace(col.ID.ToString()))
                    {
                        value += (String.IsNullOrEmpty(value) ? "" : ", ");
                        value += col.ID.ToString();
                    }
                }
            }
            return value;
        }

        private void cbIncorrectValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLog.Enabled = !(cbIncorrectValues.Items[cbIncorrectValues.SelectedIndex].ToString() == "Redirect rows to error output");
        }

        private void linkCodeplex_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://domainvalueimport.codeplex.com/");
        }

        private void linkOH22_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
           System.Diagnostics.Process.Start("http://www.oh22.is");
        }
    }
}
