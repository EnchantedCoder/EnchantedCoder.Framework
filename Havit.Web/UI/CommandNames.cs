using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI
{
	/// <summary>
	/// T��da CommandNamesHelper obsahuje sd�len� n�zvy p��kaz� (CommandNames).
	/// Sd�l� se mezi prvky vyvol�vaj�c� ud�lost p��kazu (potomci GridViewImageButton)
	/// a EnterpriseGridView.
	/// </summary>
	internal static class CommandNames // zat�m nikde nepou�ito, rozmyslet, jestli nesta�� DataControlCommandName
	{
		/// <summary>
		/// Cancel.
		/// </summary>
		public const string Cancel = "Cancel";

		/// <summary>
		/// Delete.
		/// </summary>
		public const string Delete = "Delete";

		/// <summary>
		/// Edit.
		/// </summary>
		public const string Edit = "Edit";

		/// <summary>
		/// MoveDown.
		/// </summary>
		public const string MoveDown = "MoveDown";

		/// <summary>
		/// MoveUp.
		/// </summary>
		public const string MoveUp = "MoveUp";

		/// <summary>
		/// Update.
		/// </summary>
		public const string Update = "Update";

		/// <summary>
		/// Select.
		/// </summary>
		public const string Select = "Select";

		/// <summary>
		/// Detail.
		/// </summary>
		public const string Detail = "Detail";

		/// <summary>
		/// Report.
		/// </summary>
		public const string Report = "Report";
	}
}
