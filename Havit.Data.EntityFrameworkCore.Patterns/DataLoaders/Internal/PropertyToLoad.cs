using System;

namespace Havit.Data.EntityFrameworkCore.Patterns.DataLoaders.Internal
{
	/// <summary>
	/// Informace o vlastnosti, kter� m� byt DataLoaderem na�tena.
	/// </summary>
	public class PropertyToLoad
	{
		/// <summary>
		/// N�zev na��tan� vlastnosti (po p��padn� substituci).
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// N�zev na��tan� vlastnosti (p�ed p��padnou substituc�).
		/// </summary>
		public string OriginalPropertyName { get; set; }

		/// <summary>
		/// Typ, jeho� vlastnost je na��t�na.
		/// </summary>
		public Type SourceType { get; set; }

		/// <summary>
		/// Typ na��tan� vlastnosti  (po p��padn� substituci).
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