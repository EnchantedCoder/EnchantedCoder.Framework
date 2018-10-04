namespace Havit.Data.Entity.Patterns.UnitOfWorks
{
	/// <summary>
	/// Zm�ny objekt� v UnitOfWork.
	/// </summary>
	public class Changes
	{
		/// <summary>
		/// Registrovan� objekty pro Insert.
		/// </summary>
		public object[] Inserts { get; set; }

		/// <summary>
		/// Registrovan� objekty pro Update.
		/// </summary>
		public object[] Updates { get; set; }

		/// <summary>
		/// Registrovan� objekty pro Delete.
		/// </summary>
		public object[] Deletes { get; set; }
	}
}