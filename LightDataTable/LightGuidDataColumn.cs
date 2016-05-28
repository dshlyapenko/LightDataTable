using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace LightDataTable
{
    /// <summary>
    /// Light column with Guid values
    /// </summary>
    public sealed class LightGuidDataColumn : LightDataColumn, ILightColumn<Guid>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="column">Column of <see cref="DataTable"/> that should be wrapped to <see cref="LightDataColumn"/></param>
        /// <param name="table">Table that will contains this column</param>
        public LightGuidDataColumn(DataColumn column, LightDataTable table) : base(column, table) { }

        /// <summary>
        /// Type of data stored in the column
        /// </summary>
        public override Type DataType { get { return typeof(Guid); } }

        /// <summary>
        /// Indexer for access to the column values
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        public override object this[int rowIndex]
        {
            get { return IsNull(rowIndex) ? null : (object)_values[rowIndex]; }
            set
            {
                if (value == null)
                {
                    _values[rowIndex] = Guid.Empty;
                    SetNull(rowIndex, true);
                }
                else
                {
                    _values[rowIndex] = (Guid)value;
                    SetNull(rowIndex, false);
                }
            }
        }

        /// <summary>
        /// Gets cell value
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        Guid ILightColumn<Guid>.this[int rowIndex]
        {
            get { return _values[rowIndex]; }
            set
            {
                _values[rowIndex] = value;
                SetNull(rowIndex, false);
            }
        }

        /// <summary>
        /// Copies a range of elements from column to destination array
        /// </summary>
        /// <param name="destination">Destination array</param>
        /// <param name="destinationIndex">A integer that represents the index in the destination array which storing begins</param>
        public void CopyTo(Guid[] destination, int destinationIndex = 0) { _values.CopyTo(destination, destinationIndex); }

        /// <summary>
        /// Initialize <see cref="LightDataColumn"/> from <see cref="DataColumn"/>
        /// </summary>
        /// <param name="column">Source column</param>
        /// <param name="storage">Existing storage</param>
        protected override void Initialize(DataColumn column, object storage)
        {
            if (Table.RowsCount > 0)
            {
                DbNullBits = new BitArray(Table.RowsCount);
                _values = new Guid[Table.RowsCount];
                var store = (object[])ValuesField.GetValue(storage);
                bool hasNulls = false;
                for (int i = 0; i < Table.RowsCount; ++i)
                    if (store[i] != null)
                        _values[i] = (Guid)store[i];
                    else
                    {
                        hasNulls = true;
                        DbNullBits.Set(i, true);
                    }

                // memory usage optimization
                if (!hasNulls)
                    DbNullBits = null;
            }
        }

        /// <summary>
        /// Provides access to <see cref="System.Data.Common.ObjectStorage"/>
        /// </summary>
        private static readonly FieldInfo ValuesField = typeof(DataTable).Assembly.GetType("System.Data.Common.ObjectStorage").GetField("values", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Empty column values
        /// </summary>
        private static readonly Guid[] Empty = new Guid[0];

        /// <summary>
        /// Column values
        /// </summary>
        private Guid[] _values = Empty;
    }
}
