using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace LightDataTable
{
    /// <summary>
    /// Base class for light data column, that wraps <see cref="DataColumn"/>
    /// </summary>
    public abstract class LightDataColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="column">Column of <see cref="DataTable"/> that should be wrapped to <see cref="LightDataColumn"/></param>
        /// <param name="table">Table that will contains this column</param>
        public LightDataColumn(DataColumn column, LightDataTable table)
        {
            Table = table;
            ColumnName = column.ColumnName;
            AllowDbNull = column.AllowDBNull;
            Ordinal = column.Ordinal;
            Initialize(column);
        }

        /// <summary>
        /// Zero-based column order number
        /// </summary>
        public readonly int Ordinal;

        /// <summary>
        /// Column name
        /// </summary>
        public readonly string ColumnName;

        /// <summary>
        /// Does column allows <c>null</c> values
        /// </summary>
        public readonly bool AllowDbNull;

        /// <summary>
        /// Table that contains column
        /// </summary>
        public readonly LightDataTable Table;

        /// <summary>
        /// Type of data stored in the column
        /// </summary>
        public abstract Type DataType { get; }

        /// <summary>
        /// Indexer for access to the column values
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        public abstract object this[int rowIndex] { get; set; }

        /// <summary>
        /// Indicates whether the column has at least one null value
        /// </summary>
        public bool HasNulls
        {
            get
            {
                if (DbNullBits == null) return false;

                for (int index = 0; index < Table.RowsCount; ++index)
                    if (DbNullBits.Get(index)) return true;

                return false;
            }
        }

        /// <summary>
        /// Number of null elements in the column.
        /// </summary>
        public virtual int NullsCount
        {
            get
            {
                if (DbNullBits == null) return 0;

                int count = 0;
                for (int index = 0; index < Table.RowsCount; ++index)
                    if (DbNullBits.Get(index))
                        ++count;

                return count;
            }
        }

        /// <summary>
        /// Indicates whether the column has null value in the specified row
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>True, if cell is null, otherwise - false. </returns>
        public virtual bool IsNull(int rowIndex) { return DbNullBits != null && DbNullBits.Get(rowIndex); }

        /// <summary>
        /// Set the null value in the specified row
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        public virtual void SetNull(int rowIndex) { SetNull(rowIndex, true); }

        /// <summary>
        /// Execute aggregate operation for current column
        /// </summary>
        /// <param name="kind">Kind of aggregation</param>
        /// <returns>Aggregation result</returns>
        public virtual object Aggregate(AggregateType kind) { return null; }

        /// <summary>
        /// Storage of null values in the column cells
        /// </summary>
        protected BitArray DbNullBits { get; set; }

        /// <summary>
        /// Set the null value in the <see cref="DbNullBits"/> for the specified row
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <param name="isNull">Value</param>
        protected void SetNull(int rowIndex, bool isNull)
        {
            if (DbNullBits == null)
            {
                if (Table.RowsCount == 0)
                    throw new InvalidOperationException("Table doesn`t contains rows");
                DbNullBits = new BitArray(Table.RowsCount);
            }
            DbNullBits.Set(rowIndex, isNull);
        }

        /// <summary>
        /// Initialize <see cref="LightDataColumn"/> from <see cref="DataColumn"/>
        /// </summary>
        /// <param name="column">Source column</param>
        /// <param name="storage">Existing storage</param>
        protected abstract void Initialize(DataColumn column, object storage);

        /// <summary>
        /// Initialize <see cref="LightDataColumn"/> from <see cref="DataColumn"/>
        /// </summary>
        /// <param name="column">Source column</param>
        private void Initialize(DataColumn column)
        {
            var storage = StorageField.GetValue(column);
            if (storage != null)
                DbNullBits = (BitArray)BitsField.GetValue(storage);
            if (DbNullBits != null)
                OptimizeDbNullBits();
            Initialize(column, storage);
        }

        /// <summary>
        /// Optimize the storage of the null values mask
        /// </summary>
        private void OptimizeDbNullBits()
        {
            for (int i = 0; i < Table.RowsCount; ++i)
                if (DbNullBits.Get(i))
                    return;

            DbNullBits = null;
        }

        /// <summary>
        /// Provides access to <see cref="DataColumn"/>
        /// </summary>
        private static readonly FieldInfo StorageField = typeof(DataColumn).GetField("_storage", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Provides access to <see cref="System.Data.Common.DataStorage"/>
        /// </summary>
        private static readonly FieldInfo BitsField = typeof(DataTable).Assembly.GetType("System.Data.Common.DataStorage").GetField("dbNullBits", BindingFlags.Instance | BindingFlags.NonPublic);
    }
}
