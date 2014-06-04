﻿<%@ Page Language="C#" MasterPageFile="Bootstrap.Master" CodeBehind="GridViewExtTest.aspx.cs" Inherits="WebApplicationTest.HavitWebBootstrapTests.GridViewExtTest" StyleSheetTheme="BootstrapTheme" %>

<asp:Content ContentPlaceHolderID="MainCPH" runat="server">
	
	<asp:UpdatePanel UpdateMode="Conditional" runat="server">
		<ContentTemplate>

			<havit:EnterpriseGridView ID="MainGV" AllowInserting="true" AutoCrudOperations="true" runat="server">
				<Columns>
					<havit:BoundFieldExt DataField="Nazev" SortExpression="Nazev" HeaderText="Název" />
					<havit:GridViewCommandField ShowEditButton="true" />
				</Columns>
			</havit:EnterpriseGridView>	
		</ContentTemplate>
	</asp:UpdatePanel>
	
	<bc:ModalEditorExtender ID="ModalEditorExtender" For="MainGV" ItemType="Havit.BusinessLayerTest.Subjekt" HeaderText="Editace subjektu" runat="server">
		<ContentTemplate>
			<asp:TextBox ID="NazevTB" Text="<%# BindItem.Nazev %>" runat="server" />
		</ContentTemplate>
		<FooterTemplate>
			<asp:Button CommandName="OK" Text="OK" runat="server" />
			<asp:Button CommandName="Save" Text="Save" runat="server" />
			<asp:Button CommandName="Cancel" Text="Cancel" runat="server" />
		</FooterTemplate>
	</bc:ModalEditorExtender>

</asp:Content>
