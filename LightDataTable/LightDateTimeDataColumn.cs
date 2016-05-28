using System;
using System.Data;
using System.Reflection;

namespace LightDataTable
{
    /// <summary>
    /// Light column with DateTime values
    /// </summary>
    public sealed class LightDateTimeDataColumn : LightDataColumn, ILightColumn<DateTime>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="column">Column of <see cref="DataTable"/> that should be wrapped to <see cref="LightDataColumn"/></param>
        /// <param name="table">Table that will contains this column</param>
        public LightDateTimeDataColumn(DataColumn column, LightDataTable table) : base(column, table) { }

        /// <summary>
        /// Type of data stored in the column
        /// </summary>
        public override Type DataType { get { return typeof(DateTime); } }

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
                    _values[rowIndex] = GetDateTime(value);
                    SetNull(rowIndex, false);
                }
            }
        }

        /// <summary>
        /// Gets cell value
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        DateTime ILightColumn<DateTime>.this[int rowIndex]
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
        public void CopyTo(DateTime[] destination, int destinationIndex = 0) { _values.CopyTo(destination, destinationIndex); }

        /// <summary>
        /// Initialize <see cref="LightDataColumn"/> from <see cref="DataColumn"/>
        /// </summary>
        /// <param name="column">Source column</param>
        /// <param name="storage">Existing storage</param>
        protected override void Initialize(DataColumn column, object storage)
        {
            if (Table.RowsCount > 0)
                Array.Copy((DateTime[])ValuesField.GetValue(storage), _values = new DateTime[Table.RowsCount], Table.RowsCount);
        }

        /// <summary>
        /// Provides access to <see cref="System.Data.Common.DateTimeStorage"/>
        /// </summary>
        private static readonly FieldInfo ValuesField = typeof(DataTable).Assembly.GetType("System.Data.Common.DateTimeStorage").GetField("values", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Default value for the column cells
        /// </summary>
        private static readonly DateTime Default = DateTime.MinValue;

        /// <summary>
        /// Empty column values
        /// </summary>
        private static readonly DateTime[] Empty = new DateTime[0];

        /// <summary>
        /// Column values
        /// </summary>
        private DateTime[] _values = Empty;

        /// <summary>
        /// Get typed value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        private DateTime GetDateTime(object value)
        {
            if (value is DateTime) return (DateTime)value;
            throw new NotSupportedException(value.GetType().FullName);
        }
    }
}
