using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebApplicationTest
{
	public partial class NumericBoxTest : System.Web.UI.Page
	{
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (Page.IsPostBack)
			{
				CallBackButton.Text = Page.IsValid.ToString();
			}
		}
	}
}
