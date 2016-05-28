using System;
using System.Collections.Generic;
using System.Data;

namespace LightDataTable
{
    /// <summary>
    /// Light analog of <c>System.Data.DataTable</c>
    /// </summary>
    public sealed class LightDataTable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="table"><see cref="DataTable"/> that should be wrapped to <see cref="LightDataTable"/></param>
        public LightDataTable(DataTable table)
        {
            TableName = table.TableName;
            RowsCount = table.Rows.Count;
            var list = new List<LightDataColumn>(table.Columns.Count);
            foreach (var column in table.Columns)
                list.Add(Create((DataColumn)column));
            Columns = new LightDataColumnCollection(list);
        }

        /// <summary>
        /// Table name
        /// </summary>
        public readonly string TableName;

        /// <summary>
        /// Total number of rows in the current table
        /// </summary>
        public readonly int RowsCount;

        /// <summary>
        /// Table columns
        /// </summary>
        public readonly LightDataColumnCollection Columns;
        
        /// <summary>
        /// Indexer to access to the specified table cell
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        public object this[int columnIndex, int rowIndex]
        {
            get { return Columns[columnIndex][rowIndex]; }
            set { Columns[columnIndex][rowIndex] = value; }
        }

        /// <summary>
        /// Indexer to access to the specified table cell
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="rowIndex">Row index</param>
        /// <returns> Значение. </returns>
        public object this[string columnName, int rowIndex]
        {
            get { return Columns[columnName][rowIndex]; }
            set { Columns[columnName][rowIndex] = value; }
        }

        /// <summary>
        /// Typed access to the cell value of the specified column and row
        /// </summary>
        /// <typeparam name="T">Column type</typeparam>
        /// <param name="rowIndex">Row index</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Cell value</returns>
        public T Field<T>(int rowIndex, string columnName)
        {
            return (T) Columns[columnName][rowIndex];
        }

        /// <summary>
        /// Light column factory
        /// </summary>
        /// <param name="column"><see cref="DataColumn"/> that should be wrapped</param>
        /// <returns>Light column</returns>
        private LightDataColumn Create(DataColumn column)
        {
            var type = column.DataType;
            if (type == typeof(string))
                return new LightStringDataColumn(column, this);
            if (type == typeof(Guid))
                return new LightGuidDataColumn(column, this);
            if (type == typeof(Int16))
                return new LightInt16DataColumn(column, this);
            if (type == typeof(Int32))
                return new LightInt32DataColumn(column, this);
            if (type == typeof(Int64))
                return new LightInt64DataColumn(column, this);
            if (type == typeof(Decimal))
                return new LightDecimalDataColumn(column, this);
            if (type == typeof(bool))
                return new LightBooleanDataColumn(column, this);
            if (type == typeof(DateTime))
                return new LightDateTimeDataColumn(column, this);
            if (type == typeof(byte))
                return new LightByteDataColumn(column, this);
            if (type == typeof(byte[]))
                return new LightByteArrayDataColumn(column, this);
            if (type == typeof(double))
                return new LightDoubleDataColumn(column, this);

            throw new NotImplementedException($"Type {type} is not supported.");
        }
    }
}
