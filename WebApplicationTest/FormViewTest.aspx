﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormViewTest.aspx.cs" Inherits="WebApplicationTest.FormViewTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
	<asp:ScriptManager runat="server" />
    <div>	
		<havit:FormViewExt ID="MyFormView" ItemType="Havit.BusinessLayerTest.Subjekt" DefaultMode="Edit" RenderOuterTable="false" runat="server">
			<ItemTemplate>
				Nazev: <%#: Item.Nazev %>
				<%--<asp:Button CommandName="New" Text="New" runat="server" />--%>
				<asp:Button CommandName="Edit" Text="Edit" runat="server" />
				<asp:Button CommandName="Delete" Text="Delete" runat="server" />
			</ItemTemplate>	
			<EditItemTemplate>
				<asp:Panel ID="MyPanel" runat="server">
					<asp:UpdatePanel RenderMode="Inline" runat="server">
						<ContentTemplate>
							Nazev: <asp:TextBox ID="NazevTextBox" Text="<%# BindItem.Nazev %>" runat="server" />
						</ContentTemplate>
					</asp:UpdatePanel>
				</asp:Panel>
				<asp:Button CommandName="Update" Text="Update" runat="server" />
				<asp:Button CommandName="Cancel" Text="Cancel" runat="server" />
			</EditItemTemplate>
<%--			<InsertItemTemplate>
				Nazev: <asp:TextBox ID="NazevTextBox" Text="<%# BindItem.Nazev %>" runat="server" />
				<asp:Button CommandName="Insert" Text="Insert" runat="server" />
				<asp:Button CommandName="Cancel" Text="Cancel" runat="server" />
			</InsertItemTemplate>--%>
		</havit:FormViewExt>
		<asp:Button ID="UpdateButton" Text="Update" Visible="false" runat="server" />
    </div>
    </form>
</body>
</html>
