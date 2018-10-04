namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Re�im renderov�n� struktury HTML DateTimeBoxu.
	/// </summary>
	public enum DateTimeBoxContainerRenderMode
	{
		/// <summary>
		/// Zaji��uje renderov�n� standardn� struktury - textbox pro hodnotu, tvrd� mezera a obr�zek/ikonka pro v�b�r kalend��e.
		/// </summary>
		Standard,

		/// <summary>
		/// Viz BootstrapInputGroupButtonOnRight, av�ak ikonka pro v�b�r kalend��e je nalevo.
		/// </summary>
		BootstrapInputGroupButtonOnLeft,

		/// <summary>
		/// Zaji��uje renderov�n� struktury dle konvenc� Bootstrapu pro Input Group a Input Group Button.
		/// T��dy input-group, input-group-btn pro DIVy a form-control pro textbox jsou p�id�ny automaticky.
		/// Ikonka pro v�b�r kalend��e je napravo.
		/// </summary>
		BootstrapInputGroupButtonOnRight
	}
}