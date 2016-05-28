namespace LightDataTable
{
    /// <summary>
    /// Represents interface to access column data.
    /// </summary>
    /// <remarks>Required for reducing boxing/unboxing operations</remarks>
    /// <typeparam name="T">Column data type</typeparam>
    public interface ILightColumn<T>
    {
        /// <summary>
        /// Gets cell value
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Cell value</returns>
        T this[int rowIndex] { get; set; }

        /// <summary>
        /// Copies a range of elements from column to destination array
        /// </summary>
        /// <param name="destination">Destination array</param>
        /// <param name="destinationIndex">A integer that represents the index in the destination array which storing begins</param>
        void CopyTo(T[] destination, int destinationIndex = 0);
    }
}
