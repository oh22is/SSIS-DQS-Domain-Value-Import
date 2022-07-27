using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.MessageBox;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Pipeline;
using System.Xml;

namespace oh22is.SqlServer.DQS
{
    public class Helper
    {

        public static string INPUT_NAME = "SSIS DQS Domain Value Destination Input";
        public static string ERROR_NAME = "ErrorOutput";

        /// <summary>
        /// 
        /// </summary>
        public enum IncorrectValues
        {
            FailComponent,
            RedirectRows,
            Ignore
        };

        public enum Publish
        {
            Never,
            Always,
            WithNoError
        };
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userFriendlyMessage"></param>
        /// <param name="ex"></param>
        public static void ExceptionMessageBox(string userFriendlyMessage, Exception ex, System.Windows.Forms.IWin32Window owner)
        {
            ApplicationException exTop = new ApplicationException(userFriendlyMessage, ex);
            exTop.Source = "SSIS DQS Component";
            ExceptionMessageBox box = new ExceptionMessageBox(exTop, ExceptionMessageBoxButtons.OK, ExceptionMessageBoxSymbol.Error);
            box.Show(owner);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComponentMetaData"></param>
        /// <param name="Component"></param>
        /// <param name="Message"></param>
        public static void FireInformation(IDTSComponentEvents ComponentMetaData, string Component, string Message)
        {
            bool bOut = true;
            ComponentMetaData.FireInformation(0, Component, Message, "", 0, ref bOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComponentMetaData"></param>
        /// <param name="Component"></param>
        /// <param name="Message"></param>
        public static void FireInformation(IDTSComponentMetaData100 ComponentMetaData, string Component, string Message)
        {
            bool bOut = true;
            ComponentMetaData.FireInformation(0, Component, Message, "", 0, ref bOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentEvents"></param>
        /// <param name="Component"></param>
        /// <param name="Message"></param>
        public static void FireError(IDTSComponentEvents componentEvents, string Component, string Message)
        {
            componentEvents.FireError(-1, Component, Message, "", 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComponentMetaData"></param>
        /// <param name="Component"></param>
        /// <param name="Message"></param>
        public static void FireError(IDTSComponentMetaData100 ComponentMetaData, string Component, string Message)
        {
            bool bOut = false;
            ComponentMetaData.FireError(-1, Component, Message, null, 0, out bOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentEvents"></param>
        /// <param name="Component"></param>
        /// <param name="Message"></param>
        public static void FireWarning(IDTSComponentEvents componentEvents, string Component, string Message)
        {
            componentEvents.FireWarning(0, Component, Message, "", 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComponentMetaData"></param>
        /// <param name="Component"></param>
        /// <param name="Message"></param>
        public static void FireWarning(IDTSComponentMetaData100 ComponentMetaData, string Component, string Message)
        {
            ComponentMetaData.FireWarning(0, Component, Message, "", 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentEvents"></param>
        /// <param name="progressDescription"></param>
        /// <param name="percentComplete"></param>
        /// <param name="progressCountLow"></param>
        /// <param name="progressCountHigh"></param>
        /// <param name="component"></param>
        public static void FireProgress(IDTSComponentEvents componentEvents, string progressDescription, int percentComplete, int progressCountLow, int progressCountHigh, string component)
        {
            bool bOut = true;
            componentEvents.FireProgress(progressDescription, percentComplete, progressCountLow, progressCountHigh, component, ref bOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComponentMetaData"></param>
        /// <param name="progressDescription"></param>
        /// <param name="percentComplete"></param>
        /// <param name="progressCountLow"></param>
        /// <param name="progressCountHigh"></param>
        /// <param name="component"></param>
        public static void FireProgress(IDTSComponentMetaData100 ComponentMetaData, string progressDescription, int percentComplete, int progressCountLow, int progressCountHigh, string component)
        {
            bool bOut = true;
            ComponentMetaData.FireProgress(progressDescription, percentComplete, progressCountLow, progressCountHigh, component, ref bOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComponentMetaData"></param>
        /// <returns></returns>
        public static IDTSInput100 GetInput(IDTSComponentMetaData100 ComponentMetaData)
        {
            IDTSInput100 input = null;

            foreach (IDTSInput100 possibleInput in ComponentMetaData.InputCollection)
            {
                if (possibleInput.Name == INPUT_NAME)
                {
                    input = possibleInput;
                }
            }

            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IncorrectValues SetIncorrectValues(string value)
        {
            IncorrectValues retValue = IncorrectValues.FailComponent;
            switch(value)
            {
                case "Fail Component":
                    retValue = IncorrectValues.FailComponent;
                    break;
                case "Ignore Failure":
                    retValue = IncorrectValues.Ignore;
                    break;
                case "Redirect rows to error output":
                    retValue = IncorrectValues.RedirectRows;
                    break;
                default:
                    retValue = IncorrectValues.FailComponent;
                    break;
            }
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Publish SetPublishValue(string value)
        {
            Publish retValue = Publish.Never;
            switch (value)
            {
                case "Never Publish":
                    retValue = Publish.Never;
                    break;
                case "Always Publish":
                    retValue = Publish.Always;
                    break;
                case "Publish When There Is No Error":
                    retValue = Publish.WithNoError;
                    break;
                default:
                    retValue = Publish.Never;
                    break;
            }
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="Name"></param>
        /// <param name="dataType"></param>
        /// <param name="Length"></param>
        /// <param name="Precision"></param>
        /// <param name="Scale"></param>
        /// <param name="CodePage"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public static IDTSOutputColumn100 BuildColumn(IDTSOutput100 output, string Name, DataType dataType, int Length, int Precision, int Scale, int CodePage, string Description)
        {
            IDTSOutputColumn100 Column = output.OutputColumnCollection.New();
            Column.Name = Name;
            Column.SetDataTypeProperties(dataType,
                                       Length,
                                       Precision,
                                       Scale,
                                       CodePage);
            Column.Description = Description;
            return Column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalCollection"></param>
        /// <param name="column"></param>
        public static void CreateExternalMetaDataColumn(IDTSExternalMetadataColumnCollection100 externalCollection, IDTSOutputColumn100 column)
        {
            // For each output column create an external meta data columns.
            IDTSExternalMetadataColumn100 eColumn = externalCollection.New();
            eColumn.Name = column.Name;
            eColumn.DataType = column.DataType;
            eColumn.Precision = column.Precision;
            eColumn.Length = column.Length;
            eColumn.Scale = column.Scale;

            // wire the output column to the external metadata
            column.ExternalMetadataColumnID = eColumn.ID;
        }

    }
}
