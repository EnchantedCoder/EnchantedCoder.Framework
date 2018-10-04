namespace Havit.GoPay.DataObjects
{
	/// <summary>
	/// Dodate�n� parametry platby
	/// </summary>
	public class GoPayAdditionalParameter
	{
		/// <summary>
		/// N�zev parametru
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Hodnota voliteln�ho parametru
		/// </summary>
		public string Value { get; set; }
	}
}