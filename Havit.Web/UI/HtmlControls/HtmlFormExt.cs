using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Reflection;
using Havit.Reflection;

namespace Havit.Web.UI.HtmlControls
{
	/// <summary>
	/// Roz���en� .NET controlu HtmlForm.
	/// </summary>
	public class HtmlFormExt : System.Web.UI.HtmlControls.HtmlForm
	{
		#region Data members
		/// <summary>
		/// Vr�t� nebo nastav� c�lov� URL formul��e. Atribut Action formul��e.
		/// Pokud nen� explicitn� nastaveno, vrac� automaticky Microsoft implementaci (v�etn� QueryStringu).
		/// </summary>
		/// <remarks>Ned�l� se ResolveUrl.</remarks>
		[
			Description("Vr�t� nebo nastav� c�lov� URL formul��e. Atribut Action formul��e."),
			Category("Behavior"),
			DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		]
		public virtual string Action
		{
			get
			{
				object action = ViewState["Action"];
				if (action != null)
				{
					return (string)action;
				}
				else
				{
					return this.GetBaseActionAttribute();
				}
			}
		}
		#endregion

		#region RenderAttributes
		/// <summary>
		/// Overriden. Zaji��uje vlastn� renderov�n� atributu Action
		/// </summary>
		/// <param name="writer"></param>
		protected override void RenderAttributes(System.Web.UI.HtmlTextWriter writer) 
		{
			writer.WriteAttribute("name", this.Name);
			this.Attributes.Remove("name");

			writer.WriteAttribute("method", this.Method);
			this.Attributes.Remove("method");

			writer.WriteAttribute("action", this.Action, true);
			this.Attributes.Remove("action");

			string submitEvent = this.Page_ClientOnSubmitEvent;
			if ((submitEvent != null) && (submitEvent.Length > 0))
			{
				if (this.Attributes["onsubmit"] != null) 
				{
					submitEvent = submitEvent + this.Attributes["onsubmit"];
					this.Attributes.Remove("onsubmit");
				}
				writer.WriteAttribute("language", "javascript");
				writer.WriteAttribute("onsubmit", submitEvent);
			}

			if (this.ID == null)
			{
				writer.WriteAttribute("id", this.ClientID);
			}
			
			// nelze volat base.RenderAttributes(), tak�e
			// vol�no z HtmlContainerControl
			this.ViewState.Remove("innerhtml");

			// vol�no v HtmlControl
			this.Attributes.Render(writer);
		}
		#endregion

		#region GetBaseActionAttribute, Page_ClientOnSubmitEvent
		/// <summary>
		/// Pomoc� reflexe vr�t� p�vodn� private base.GetActionAttribute()
		/// </summary>
		/// <returns>Microsoft implementace action atributu formul��e</returns>
		private string GetBaseActionAttribute() 
		{
			Type formType = typeof(System.Web.UI.HtmlControls.HtmlForm);
			MethodInfo actionMethod = formType.GetMethod("GetActionAttribute", BindingFlags.Instance | BindingFlags.NonPublic);
			object result = actionMethod.Invoke(this,null);
			return (string)result;
		}

		/// <summary>
		/// Pomoc� reflexe vr�t� internal ClientOnSubmitEvent vlastnost Page
		/// </summary>
		private string Page_ClientOnSubmitEvent 
		{
			get 
			{
				return (string)Reflector.GetPropertyValue(this.Page, typeof(System.Web.UI.Page), "ClientOnSubmitEvent");
			}
		}
		#endregion
	}
}
