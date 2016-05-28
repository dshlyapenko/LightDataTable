using System;
using System.Data;
using System.Reflection;

namespace LightDataTable
{
    /// <summary>
    /// Light column with int values
    /// </summary>
    public sealed class LightInt32DataColumn : LightDataColumn, ILightColumn<Int32>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="column">Column of <see cref="DataTable"/> that should be wrapped to <see cref="LightDataColumn"/></param>
        /// <param name="table">Table that will contains this column</param>
        public LightInt32DataColumn(DataColumn column, LightDataTable table) : base(column, table) { }
        
        /// <summary>
        /// Type of data stored in the column
        /// </summary>
        public override Type DataType { get { return typeof(Int32); } }

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
                    _values[rowIndex] = (Int32)value;
                    SetNull(rowIndex, false);
                }
            }
        }

        /// <summary>
        /// Gets cell value
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        Int32 ILightColumn<Int32>.this[int rowIndex]
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
        public void CopyTo(Int32[] destination, int destinationIndex = 0) { _values.CopyTo(destination, destinationIndex); }

        /// <summary>
        /// Execute aggregate operation for current column
        /// </summary>
        /// <param name="kind">Kind of aggregation</param>
        /// <returns>Aggregation result</returns>
        public override object Aggregate(AggregateType kind)
        {
            long sum = 0;
            bool hasValue = false;

            switch (kind)
            {
                case AggregateType.Sum:
                    for (int i = 0; i < Table.RowsCount; ++i)
                        if (!IsNull(i))
                        {
                            sum += _values[i];
                            hasValue = true;
                        }
                    return hasValue ? (object)sum : null;

                case AggregateType.Mean:
                    int valuesCount = 0;
                    for (int i = 0; i < Table.RowsCount; ++i)
                        if (!IsNull(i))
                        {
                            sum += _values[i];
                            ++valuesCount;
                        }
                    return valuesCount == 0 ? null : (object)(int)(sum / valuesCount);

                case AggregateType.Min:
                    int min = int.MaxValue;
                    for (int i = 0; i < Table.RowsCount; ++i)
                        if (!IsNull(i))
                        {
                            if (_values[i] < min)
                                min = _values[i];
                            hasValue = true;
                        }
                    return hasValue ? (object)min : null;

                case AggregateType.Max:
                    int max = int.MinValue;
                    for (int i = 0; i < Table.RowsCount; ++i)
                        if (!IsNull(i))
                        {
                            if (_values[i] > max)
                                max = _values[i];
                            hasValue = true;
                        }
                    return hasValue ? (object)max : null;
              
                default:
                    throw new NotSupportedException(kind.ToString());
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
                Array.Copy((Int32[])ValuesField.GetValue(storage), _values = new Int32[Table.RowsCount], Table.RowsCount);
        }

        /// <summary>
        /// Provides access to <see cref="System.Data.Common.Int32Storage"/>
        /// </summary>
        private static readonly FieldInfo ValuesField = typeof(DataTable).Assembly.GetType("System.Data.Common.Int32Storage").GetField("values", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Default value for the column cells
        /// </summary>
        private const Int32 Default = 0;

        /// <summary>
        /// Empty column values
        /// </summary>
        private static readonly Int32[] Empty = new Int32[0];

        /// <summary>
        /// Column values
        /// </summary>
        private Int32[] _values = Empty;
    }
}
