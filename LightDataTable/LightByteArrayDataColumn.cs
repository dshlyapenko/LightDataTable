using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace LightDataTable
{
    /// <summary>
    /// Light column with byte array values
    /// </summary>
    public sealed class LightByteArrayDataColumn : LightDataColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="column">Column of <see cref="DataTable"/> that should be wrapped to <see cref="LightDataColumn"/></param>
        /// <param name="table">Table that will contains this column</param>
        public LightByteArrayDataColumn(DataColumn column, LightDataTable table) : base(column, table) { }

        /// <summary>
        /// Type of data stored in the column
        /// </summary>
        public override Type DataType { get { return typeof(byte[]); } }

        /// <summary>
        /// Indexer for access to the column values
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        public override object this[int rowIndex]
        {
            get { return _values[rowIndex]; }
            set { _values[rowIndex] = (byte[])value; }
        }

        /// <summary>
        /// Number of null elements in the column.
        /// </summary>
        public override int NullsCount
        {
            get
            {
                return _values.Count(item => item == null);
            }
        }

        /// <summary>
        /// Initialize <see cref="LightDataColumn"/> from <see cref="DataColumn"/>
        /// </summary>
        /// <param name="column">Source column</param>
        /// <param name="storage">Existing storage</param>
        protected override void Initialize(DataColumn column, object storage)
        {
            if (Table.RowsCount > 0)
            {
                _values = new byte[Table.RowsCount][];
                var source = (object[])ValuesField.GetValue(storage);
                for (int i=0; i<Table.RowsCount; ++i)
                    _values[i] = (byte[])source[i];
            }
        }

        /// <summary>
        /// Indicates whether the column has null value in the specified row
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>True, if cell is null, otherwise - false. </returns>
        public override bool IsNull(int rowIndex) { return _values[rowIndex] == null; }

        /// <summary>
        /// Set the null value in the specified row
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        public override void SetNull(int rowIndex) { _values[rowIndex] = null; }

        /// <summary>
        /// Provides access to <see cref="System.Data.Common.ObjectStorage"/>
        /// </summary>
        private static readonly FieldInfo ValuesField = typeof(DataTable).Assembly.GetType("System.Data.Common.ObjectStorage").GetField("values", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Empty column values
        /// </summary>
        private static readonly byte[][] Empty = new byte[0][];

        /// <summary>
        /// Column values
        /// </summary>
        private byte[][] _values = Empty;
    }
}
