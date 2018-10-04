using System;

namespace Havit.Data.Entity.Patterns.DataLoaders.Internal
{
	/// <summary>
	/// Informace o vlastnosti, kter� m� byt DataLoaderem na�tena.
	/// </summary>
	public class PropertyToLoad
	{
		/// <summary>
		/// N�zev na��tan� vlasntosti.
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// Typ, jeho� vlastnost je na��t�na.
		/// </summary>
		public Type SourceType { get; set; }

		/// <summary>
		/// Typ na��tan� vlasnosti.
		/// V p��pad� kolekc� jde o kolekci prvk� (nap�. pro LoginAccount.Roles bude obsahovat List&lt;Role&gt;.
		/// </summary>
		public Type TargetType { get; set; }

		/// <summary>
		/// Typ prvku na��tan� kolekce.
		/// </summary>
		public Type CollectionItemType { get; set; }

		/// <summary>
		/// Indikuje, zda je na��t�na kolekce. V opa�n�m p��pad� je na��t�na reference.
		/// </summary>
		public bool IsCollection => CollectionItemType != null;
	}
}