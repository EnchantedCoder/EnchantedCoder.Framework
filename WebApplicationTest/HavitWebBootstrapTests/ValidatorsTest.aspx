﻿<%@ Page Title="" Language="C#" MasterPageFile="~/HavitWebBootstrapTests/Bootstrap.Master" AutoEventWireup="false" CodeBehind="ValidatorsTest.aspx.cs" Inherits="WebApplicationTest.HavitWebBootstrapTests.ValidatorsTest" %>

<%@ Register TagPrefix="uc" TagName="ValidationTargetTest" Src="~/HavitWebBootstrapTests/Controls/ValidationTargetTest.ascx" %>


<asp:Content ContentPlaceHolderID="MainCPH" runat="server">
	<style>
		.invalid + .tooltip > .tooltip-inner {
			border: 1px solid red;
			background: whitesmoke;
			color: black;
		}

		body {
			margin-left: 2em;
		}

		.invalid {
			border: 1px solid red;
		}
	</style>

	<asp:Panel runat="server">

		<h1>Validation Group A (More validators with the same Control to Validate)</h1>

		<bc:ValidationSummary ValidationGroup="A" runat="server" />
		<bc:ValidationSummary ValidationGroup="A" runat="server" />

		<asp:TextBox ID="TB1" runat="server" />
		<bc:RequiredFieldValidator ControlToValidate="TB1" ErrorMessage="Zadejte hodnotu do textboxu (1)." ValidationGroup="A" runat="server" />
		<bc:RequiredFieldValidator ControlToValidate="TB1" ErrorMessage="Zadejte hodnotu do textboxu (2)." ValidationGroup="A" runat="server" />
		<bc:RegularExpressionValidator ControlToValidate="TB1" ValidationExpression="^\d{3,5}$" ErrorMessage="Zadejte tři až pět číslic." ValidationGroup="A" runat="server" />
		<br />

		<bc:Button ID="SectionAButton" Text="Postback" ValidationGroup="A" runat="server" />
	</asp:Panel>

	<asp:UpdatePanel runat="server">
		<ContentTemplate>

			<h1>Validation Group B (UpdatePanel)</h1>

			<bc:ValidationSummary ValidationGroup="B" runat="server" />

			<asp:TextBox ID="TB3" runat="server" />
			<bc:RequiredFieldValidator ControlToValidate="TB3" ErrorMessage="Zadejte hodnotu do textboxu." ValidationGroup="B" runat="server" />
			<br />
			<bc:Button ID="SectionBButton" Text="Postback" ValidationGroup="B" runat="server" />

			<br />
			<br />

			<h1>Validation Group C (EnableClientScript=false and UpdatePanel)</h1>
			<bc:ValidationSummary ValidationGroup="C" runat="server" />
			<asp:TextBox ID="TB5" runat="server" />
			<bc:RequiredFieldValidator ControlToValidate="TB5" ErrorMessage="Zadejte hodnotu do textboxu. \zpetne lomitko 'apostrof" ValidationGroup="C" EnableClientScript="false" runat="server" />
			<br />
			<bc:Button Text="Postback" ValidationGroup="C" runat="server" />

			<br />
			<br />

			<h1>Validation Group D (CustomValidator, UpdatePanel)</h1>
			<bc:ValidationSummary ValidationGroup="D" runat="server" />
			<uc:ValidationTargetTest ID="ValidationTargetTestUC" runat="server" />
			<bc:CustomValidator ID="SectionDCustomValidator" ControlToValidate="ValidationTargetTestUC" ErrorMessage="Hodnota musí být 'bla'."  ValidationGroup="D" OnServerValidate="CustomValidator_ServerValidate" runat="server"/>
			<br />
			<bc:Button Text="Postback" ValidationGroup="D" runat="server" />
	
		</ContentTemplate>
	</asp:UpdatePanel>

	<br />
	<br />

	<h1>Validation Group E (CustomValidator)</h1>
	<bc:ValidationSummary ValidationGroup="E" runat="server" />
	<uc:ValidationTargetTest ID="ValidationTargetTest2UC" runat="server" />
	<bc:CustomValidator ControlToValidate="ValidationTargetTest2UC" ErrorMessage="Hodnota musí být 'bla'."  ValidationGroup="E" OnServerValidate="CustomValidator_ServerValidate" runat="server"/>
	<br />
	<bc:Button Text="Postback" ValidationGroup="E" runat="server" />

	<br />
	<br />


	<bc:Tooltip ToolTip="Tento tooltip by měl mít standardní barvu." runat="server">Normální tooltip.</bc:Tooltip>
</asp:Content>
