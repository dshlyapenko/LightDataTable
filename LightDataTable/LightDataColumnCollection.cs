using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LightDataTable
{
    /// <summary>
    /// Light column collection
    /// </summary>
    public sealed class LightDataColumnCollection : ReadOnlyCollection<LightDataColumn>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columns">Light columns</param>
        internal LightDataColumnCollection(List<LightDataColumn> columns) : base(columns) { }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>Column</returns>
        public LightDataColumn this[string columnName]
        {
            get { return Items.FirstOrDefault(column => column.ColumnName == columnName); }
        }

        /// <summary>
        /// Determines where collection contains column with specified name
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>Existance indicator</returns>
        public bool Contains(string columnName)
        {
            return Items.Any(c => c.ColumnName == columnName);
        }
    }
}