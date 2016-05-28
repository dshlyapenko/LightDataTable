using System;
using System.Data;
using System.Reflection;

namespace LightDataTable
{
    /// <summary>
    /// Light column with Double values
    /// </summary>
    public sealed class LightDoubleDataColumn : LightDataColumn, ILightColumn<Double>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="column">Column of <see cref="DataTable"/> that should be wrapped to <see cref="LightDataColumn"/></param>
        /// <param name="table">Table that will contains this column</param>
        public LightDoubleDataColumn(DataColumn column, LightDataTable table) : base(column, table) { }
        
        /// <summary>
        /// Type of data stored in the column
        /// </summary>
        public override Type DataType { get { return typeof(Double); } }

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
                    _values[rowIndex] = (Double)value;
                    SetNull(rowIndex, false);
                }
            }
        }

        /// <summary>
        /// Gets cell value
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        Double ILightColumn<Double>.this[int rowIndex]
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
        public void CopyTo(Double[] destination, int destinationIndex = 0) { _values.CopyTo(destination, destinationIndex); }

        /// <summary>
        /// Initialize <see cref="LightDataColumn"/> from <see cref="DataColumn"/>
        /// </summary>
        /// <param name="column">Source column</param>
        /// <param name="storage">Existing storage</param>
        protected override void Initialize(DataColumn column, object storage)
        {
            if (Table.RowsCount > 0)
                Array.Copy((Double[])ValuesField.GetValue(storage), _values = new Double[Table.RowsCount], Table.RowsCount);
        }

        /// <summary>
        /// Provides access to <see cref="System.Data.Common.DoubleStorage"/>
        /// </summary>
        private static readonly FieldInfo ValuesField = typeof(DataTable).Assembly.GetType("System.Data.Common.DoubleStorage").GetField("values", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Default value for the column cells
        /// </summary>
        private const Double Default = 0.0;

        /// <summary>
        /// Empty column values
        /// </summary>
        private static readonly Double[] Empty = new Double[0];

        /// <summary>
        /// Column values
        /// </summary>
        private Double[] _values = Empty;
    }
}
