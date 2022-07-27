using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace oh22is.SqlServer.DQS
{
    /// <summary>
    /// Helper class for validating the columns in input objects. 
    /// </summary>
    public class ComponentValidation
    {

        /// <summary>
        /// 
        /// </summary>
        public ComponentValidation() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool DoesInputColumnMatchVirtualInputColumns(IDTSInput100 input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            var vInput = input.GetVirtualInput();
            var areAllColumnsValid = true;

            //	Verify that the columns in the input, have the same column metadata 
            // as the matching virtual input column.
            foreach (var column in from IDTSInputColumn100 column in input.InputColumnCollection let vColumn = vInput.VirtualInputColumnCollection.GetVirtualInputColumnByLineageID(column.LineageID) where !DoesColumnMetaDataMatch(column, vColumn) select column)
            {
                areAllColumnsValid = false;
                bool cancel;
                input.Component.FireError(
                    0,
                    input.Component.Name,
                    @"The input column metadata for column" + column.IdentificationString + @" does not match its upstream column.",
                    @"",
                    0,
                    out cancel);
            }

            return areAllColumnsValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="vColumn"></param>
        /// <returns></returns>
        private static bool DoesColumnMetaDataMatch(IDTSInputColumn100 column, IDTSVirtualInputColumn100 vColumn)
        {
            if (vColumn.DataType == column.DataType
                && vColumn.Precision == column.Precision
                && vColumn.Length == column.Length
                && vColumn.Scale == column.Scale)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        private static void FixInvalidInputColumnMetaData(IDTSInput100 input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            IDTSVirtualInput100 vInput = input.GetVirtualInput();

            foreach (IDTSInputColumn100 inputColumn in input.InputColumnCollection)
            {
                IDTSVirtualInputColumn100 vColumn = vInput.VirtualInputColumnCollection.GetVirtualInputColumnByLineageID(inputColumn.LineageID);

                if (!DoesColumnMetaDataMatch(inputColumn, vColumn))
                {
                    vInput.SetUsageType(vColumn.LineageID, inputColumn.UsageType);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentMetaData"></param>
        /// <returns></returns>
        public static DTSValidationStatus ValidateComponentInitialData(IDTSComponentMetaData100 componentMetaData)
        {

            var dtsInput = componentMetaData.InputCollection[0];
            var virtualInput = dtsInput.GetVirtualInput();
            
            if (componentMetaData.InputCollection.Count != 1)
            {
                Helper.FireError(componentMetaData, componentMetaData.Name, "The component needs one input.");
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (!dtsInput.IsAttached)
            {
                Helper.FireError(componentMetaData, componentMetaData.Name, "No input path is attached to the component.");
                return DTSValidationStatus.VS_ISBROKEN;
            }

            return DTSValidationStatus.VS_ISVALID;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentMetaData"></param>
        /// <returns></returns>
        private static bool DoesInputColumnsMatchDomains(IDTSComponentMetaData100 componentMetaData)
        {
            const bool retValue = true;
            string[] mapping = componentMetaData.CustomPropertyCollection["MappingValues"].Value.ToString().Split(';');
            var virtualInput = componentMetaData.InputCollection[0].GetVirtualInput();

            for (var i = 0; i < mapping.Count(); i++)
            {
                try
                {
                    var vCol = virtualInput.VirtualInputColumnCollection.GetVirtualInputColumnByName("", mapping[i].Split('|')[0]);
                    if (vCol == null) return false;
                }
                catch
                {
                    return false;
                }
            }

            return retValue;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool DoesInputColumDataTypesAreValid(IDTSVirtualInput100 virtualInput)
        {
            return virtualInput.VirtualInputColumnCollection.Cast<IDTSVirtualInputColumn100>().All(vCol => vCol.DataType != DataType.DT_IMAGE && vCol.DataType != DataType.DT_BYTES && vCol.DataType != DataType.DT_NTEXT && vCol.DataType != DataType.DT_FILETIME && vCol.DataType != DataType.DT_TEXT);
        }
    }
}
