<%@ Page Title="" Language="C#" MasterPageFile="~/Gui/Layout.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="TidsRegV3.Gui.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div align="center">
    <h1 style="color: palevioletred">INTERNAL SERVER ERROR OCCURED</h1>
    <asp:Button ID="HomeButton" runat="server" CssClass="button" Font-Names="Century Gothic" Font-Size="XX-Large" Text="Home" OnClick="HomeButton_Click" />
  </div>
</asp:Content>
