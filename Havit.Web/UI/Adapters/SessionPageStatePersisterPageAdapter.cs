using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.Adapters;
using System.Web.UI;

namespace Havit.Web.UI.Adapters
{
	/// <summary>
	/// <see cref="PageAdapter"/>, kter� jako <see cref="PageStatePersister"/> v metod� <see cref="GetStatePersister()"/>
	/// vrac� <see cref="SessionPageStatePersister"/>.
	/// </summary>
	public class SessionPageStatePersisterPageAdapter : PageAdapter
	{
		/// <summary>
		/// Returns an object that is used by the Web page to maintain the control and view states. 
		/// </summary>
		/// <returns>
		/// An object derived from <see cref="PageStatePersister"/> that supports creating and extracting the combined control and view states for the <see cref="Page"/>.
		/// </returns>
		public override PageStatePersister GetStatePersister()
		{
			return new SessionPageStatePersister(this.Page);
		}
	}
}
