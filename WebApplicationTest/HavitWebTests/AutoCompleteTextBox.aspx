﻿<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCompleteTextBox.aspx.cs" Inherits="WebApplicationTest.HavitWebTests.AutoCompleteTextBox" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../Content/site.css" rel="stylesheet" runat="server" />
</head>

<body>
	<form id="MainForm" runat="server">
		<div>

			<asp:ScriptManager EnablePartialRendering="true" runat="server">
				<Scripts>
					<asp:ScriptReference Name="jquery" />
					<asp:ScriptReference Name="jquery.ui.combined" />
					<%--Framework Scripts--%>
					<asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
					<asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
					<asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
					<asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
					<asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
					<asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
					<asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
					<asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
					<%--Site Scripts--%>
					<asp:ScriptReference Name="bootstrap" />
					<asp:ScriptReference Name="toastr" />
				</Scripts>
			</asp:ScriptManager>

			<asp:UpdatePanel runat="server">
				<ContentTemplate>
					<havit:MessengerControl runat="server" />
				</ContentTemplate>
			</asp:UpdatePanel>

			<div class="container">

				<havit:AutoCompleteTextBox
					ID="Test1ACTB"
					ServiceUrl="/Services/AutoCompleteTextBoxService.svc/GetSuggestions"
					AutoPostBack="false"
					UseClientCache="false"
					runat="server" />

				<bc:Button ID="ButtonBt" Text="Text" runat="server" />

			</div>
		</div>
	</form>
</body>
</html>