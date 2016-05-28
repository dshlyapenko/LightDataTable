using System;
using System.Data;
using System.Reflection;

namespace LightDataTable
{
    /// <summary>
    /// Light column with short values
    /// </summary>
    public sealed class LightInt16DataColumn : LightDataColumn, ILightColumn<Int16>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="column">Column of <see cref="DataTable"/> that should be wrapped to <see cref="LightDataColumn"/></param>
        /// <param name="table">Table that will contains this column</param>
        public LightInt16DataColumn(DataColumn column, LightDataTable table) : base(column, table) { }

        /// <summary>
        /// Type of data stored in the column
        /// </summary>
        public override Type DataType { get { return typeof(Int16); } }

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
                    _values[rowIndex] = Default;
                    SetNull(rowIndex, true);
                }
                else
                {
                    _values[rowIndex] = (Int16)value;
                    SetNull(rowIndex, false);
                }
            }
        }

        /// <summary>
        /// Gets cell value
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        Int16 ILightColumn<Int16>.this[int rowIndex]
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
        public void CopyTo(Int16[] destination, int destinationIndex = 0) { _values.CopyTo(destination, destinationIndex); }

        /// <summary>
        /// Initialize <see cref="LightDataColumn"/> from <see cref="DataColumn"/>
        /// </summary>
        /// <param name="column">Source column</param>
        /// <param name="storage">Existing storage</param>
        protected override void Initialize(DataColumn column, object storage)
        {
            if (Table.RowsCount > 0)
                Array.Copy((Int16[])ValuesField.GetValue(storage), _values = new Int16[Table.RowsCount], Table.RowsCount);
        }

        /// <summary>
        /// Provides access to <see cref="System.Data.Common.Int16Storage"/>
        /// </summary>
        private static readonly FieldInfo ValuesField = typeof(DataTable).Assembly.GetType("System.Data.Common.Int16Storage").GetField("values", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Default value for the column cells
        /// </summary>
        private const Int16 Default = 0;

        /// <summary>
        /// Empty column values
        /// </summary>
        private static readonly Int16[] Empty = new Int16[0];

        /// <summary>
        /// Column values
        /// </summary>
        private Int16[] _values = Empty;
    }
}