using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using Microsoft.Ssdqs.Flow.Notification;
using Microsoft.Ssdqs.Proxy.EntryPoint;
using oh22is.SqlServer.DQS.Utilities;
#pragma warning disable 1587

namespace oh22is.SqlServer.DQS
{
    public partial class frmKBPublishTaskUI : Form
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly Connections _connections;
        private readonly TaskHost _taskHost;
        private ConnectionManager _dqsConnection = null;
        private NotificationSessionInfo _clientSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="taskHost"></param>
        public frmKBPublishTaskUI(IServiceProvider serviceProvider, TaskHost taskHost)
        {
            
            _serviceProvider = serviceProvider;
            _taskHost = taskHost;
             var cs = _serviceProvider.GetService(typeof(IDtsConnectionService)) as IDtsConnectionService;
            if (cs != null) _connections = cs.GetConnections();

            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBPublishUI_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (var cm in _connections.Cast<ConnectionManager>().Where(cm => DataQualityServices.IsDqsConnection(cm)))
                {
                    cbConnectionManager.Items.Add(cm.Name);
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessageBox("An error occurred while retrieving the connection manager.", ex, this);
            }

            cbException.Checked = (_taskHost.Properties["ThrowException"].GetValue(_taskHost) == null ? false : (bool)_taskHost.Properties["ThrowException"].GetValue(_taskHost));
            cbEncryptConnection.Checked = (_taskHost.Properties["Encrypted"].GetValue(_taskHost) == null ? false : (bool)_taskHost.Properties["Encrypted"].GetValue(_taskHost));
            cbConnectionManager.SelectedItem = (_taskHost.Properties["ConnectionManager"].GetValue(_taskHost) == null ? null : _taskHost.Properties["ConnectionManager"].GetValue(_taskHost).ToString());
            cbKnowledgeBase.SelectedItem = (_taskHost.Properties["KnowledgeBase"].GetValue(_taskHost) == null ? null : _taskHost.Properties["KnowledgeBase"].GetValue(_taskHost).ToString());

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

            if (cbConnectionManager.SelectedIndex == -1
                || cbKnowledgeBase.SelectedIndex == -1
                || String.IsNullOrEmpty(cbConnectionManager.Items[cbConnectionManager.SelectedIndex].ToString())
                || String.IsNullOrEmpty(cbKnowledgeBase.Items[cbKnowledgeBase.SelectedIndex].ToString()))
            {
                MessageBox.Show("Please define the Connection Manager and the Knowledge Base.", "SSIS DQS Component", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _taskHost.Properties["ConnectionManager"].SetValue(_taskHost, cbConnectionManager.Items[cbConnectionManager.SelectedIndex].ToString());
                _taskHost.Properties["KnowledgeBase"].SetValue(_taskHost, cbKnowledgeBase.Items[cbKnowledgeBase.SelectedIndex].ToString());
                _taskHost.Properties["Encrypted"].SetValue(_taskHost, cbEncryptConnection.Checked);
                _taskHost.Properties["ThrowException"].SetValue(_taskHost, cbException.Checked);
                DialogResult = DialogResult.OK;
            }

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

                foreach (ConnectionManager cm in _connections)
                {
                    if (cm.Name == name)
                    {
                        _dqsConnection = cm;       
                        break;
                    }
                }

                if (_dqsConnection != null)
                {

                    try
                    {
                        DataQualityServices.InitializeProxy(cbEncryptConnection.Checked);
                    }
                    catch (Exception ex)
                    {
                        Helper.ExceptionMessageBox("", ex, this);
                    }

                    _clientSession = DataQualityServices.CreateClientSession(_dqsConnection, DataQualityServices.DatabaseName);

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

            try
            {
                cbKnowledgeBase.SelectedItem = (_taskHost.Properties["KnowledgeBase"].GetValue(_taskHost) == null ? null : _taskHost.Properties["KnowledgeBase"].GetValue(_taskHost).ToString());
            }
            catch
            {
                cbKnowledgeBase.SelectedItem = null;
            }

        }

        private void linkCodeplex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://domainvalueimport.codeplex.com/");
        }

        private void linkOH22_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.oh22.is");
        }

    }
}
