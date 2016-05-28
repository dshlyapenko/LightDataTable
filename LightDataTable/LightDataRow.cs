namespace LightDataTable
{
    /// <summary>
    /// Light data row, that wraps <c>System.Data.DataRow</c>
    /// </summary>
    public sealed class LightDataRow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="table">Table</param>
        /// <param name="index">Row index</param>
        public LightDataRow(LightDataTable table, int index)
        {
            this.Table = table;
            RowIndex = index;
        }

        /// <summary>
        /// Table that contains current row
        /// </summary>
        public readonly LightDataTable Table;

        /// <summary>
        /// Row index
        /// </summary>
        public readonly int RowIndex;

        /// <summary>
        /// Indexer for access cells of the current row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns>Cell value</returns>
        public object this[int columnIndex]
        {
            get
            {
                return Table.Columns[columnIndex][RowIndex];
            }
            set
            {
                Table.Columns[columnIndex][RowIndex] = value;
            }
        }

        /// <summary>
        /// Indexer for access cells of the current row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>Cell value</returns>
        public object this[string columnName]
        {
            get
            {
                return this[Table.Columns[columnName].Ordinal];
            }
            set
            {
                this[Table.Columns[columnName].Ordinal] = value;
            }
        }

        /// <summary>
        /// Typed access to the cell value of the specified column in the current row
        /// </summary>
        /// <typeparam name="T">Column type</typeparam>
        /// <param name="columnName">Column name</param>
        /// <returns>Cell value</returns>
        public T Field<T>(string columnName) { return (T)this[columnName]; }
    }
}
