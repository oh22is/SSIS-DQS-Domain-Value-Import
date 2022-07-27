using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Design;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Design;
using System.Windows.Forms;

namespace oh22is.SqlServer.DQS
{
    class KBPublishTaskUI : IDtsTaskUI
    {

        private TaskHost _taskHost;
        private IServiceProvider _serviceProvider;

        #region IDtsComponentUI Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskHost"></param>
        /// <param name="serviceProvider"></param>
        public void Initialize(TaskHost taskHost, IServiceProvider serviceProvider)
        {
            this._taskHost = taskHost;
            this._serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContainerControl GetView()
        {
            return new frmKBPublishTaskUI(_serviceProvider, _taskHost);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentWindow"></param>
        public void New(IWin32Window parentWindow)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentWindow"></param>
        public void Delete(IWin32Window parentWindow)
        {
            return;
        }

        #endregion

    }
}
