using System;
using System.Data;
using System.Reflection;

namespace LightDataTable
{
    /// <summary>
    /// Light column with byte values
    /// </summary>
    public sealed class LightByteDataColumn : LightDataColumn, ILightColumn<byte>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="column">column of <see cref="DataTable"/> that should be wrapped to <see cref="LightDataColumn"/></param>
        /// <param name="table">Table that will contains this column</param>
        public LightByteDataColumn(DataColumn column, LightDataTable table) : base(column, table) { }

        /// <summary>
        /// Type of data stored in the column
        /// </summary>
        public override Type DataType { get { return typeof(byte); } }

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
                    _values[rowIndex] = (byte)value;
                    SetNull(rowIndex, false);
                }
            }
        }

        /// <summary>
        /// Gets cell value
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        byte ILightColumn<byte>.this[int rowIndex]
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
        public void CopyTo(byte[] destination, int destinationIndex = 0) { _values.CopyTo(destination, destinationIndex); }

        /// <summary>
        /// Initialize <see cref="LightDataColumn"/> from <see cref="DataColumn"/>
        /// </summary>
        /// <param name="column">Source column</param>
        /// <param name="storage">Existing storage</param>
        protected override void Initialize(DataColumn column, object storage)
        {
            if (Table.RowsCount > 0)
                Array.Copy((byte[])ValuesField.GetValue(storage), _values = new byte[Table.RowsCount], Table.RowsCount);
        }

        /// <summary>
        /// Provides access to <see cref="System.Data.Common.ByteStorage"/>
        /// </summary>
        private static readonly FieldInfo ValuesField = typeof(DataTable).Assembly.GetType("System.Data.Common.ByteStorage").GetField("values", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Default value for the column cells
        /// </summary>
        private const byte Default = 0;

        /// <summary>
        /// Empty column values
        /// </summary>
        private static readonly byte[] Empty = new byte[0];

        /// <summary>
        /// column values
        /// </summary>
        private byte[] _values = Empty;
    }
}
