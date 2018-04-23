<%@ Page Title="" Language="C#" MasterPageFile="~/Gui/Layout.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TidsRegV3.Gui.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <table style="width: 100%;">
            <tr>
                <td>
                    <ul class="centerlist">
                        <li style="font-size:x-large">Username</li>
                        <li>
                            <asp:TextBox ID="username" CssClass="textbox" runat="server" Font-Names="Century Gothic" Font-Size="Large" AutoCompleteType="Email" CausesValidation="True"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="usernameValidator" runat="server" ErrorMessage="Username cannot be empty." ValidationGroup="LoginGroup" ControlToValidate="username" Font-Size="Medium" ForeColor="#990033"></asp:RequiredFieldValidator>
                        </li>
                        <li style="font-size:x-large">Password</li>
                        <li>
                            <asp:TextBox ID="password" CssClass="textbox" runat="server" Font-Names="Century Gothic" Font-Size="Large" CausesValidation="True" TextMode="Password"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="passwordValidator" runat="server" ErrorMessage="Password cannot be empty." ValidationGroup="LoginGroup" ControlToValidate="password" Font-Size="Medium" ForeColor="#990033"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <asp:Button ID="login" runat="server" Text="Login" CssClass="button" Font-Names="Century Gothic" Font-Size="XX-Large" OnClick="login_Click" ValidationGroup="LoginGroup" />
                        </li>
                    </ul>
                </td>
                <td>
                    <ul class="centerlist">
                        <li style="font-size:x-large">Username</li>
                        <li>
                            <asp:TextBox ID="RegUsername" CssClass="textbox" runat="server" Font-Names="Century Gothic" Font-Size="Large" AutoCompleteType="Email" CausesValidation="True"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RegUsernameValidator" runat="server" ErrorMessage="Username cannot be empty." ValidationGroup="RegGroup" ControlToValidate="RegUsername" Font-Size="Medium" ForeColor="#990033"></asp:RequiredFieldValidator>
                        </li>
                        <li style="font-size:x-large">Password</li>
                        <li>
                            <asp:TextBox ID="RegPassword" CssClass="textbox" runat="server" Font-Names="Century Gothic" Font-Size="Large" CausesValidation="True" TextMode="Password"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RegPasswordValidator" runat="server" ErrorMessage="Password cannot be empty." ValidationGroup="RegGroup" ControlToValidate="RegPassword" Font-Size="Medium" ForeColor="#990033"></asp:RequiredFieldValidator>
                        </li>
                        <li style="font-size:x-large">Repeat Password</li>
                        <li>
                            <asp:TextBox ID="RegRepeatPassword" CssClass="textbox" runat="server" Font-Names="Century Gothic" Font-Size="Large" CausesValidation="True" TextMode="Password"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RegRepeatPasswordValidator" runat="server" ErrorMessage="Password cannot be empty." ValidationGroup="RegGroup" ControlToValidate="RegRepeatPassword" Font-Size="Medium" ForeColor="#990033"></asp:RequiredFieldValidator><br/>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password do not match." ControlToCompare="RegPassword" ValidationGroup="RegGroup" ControlToValidate="RegRepeatPassword" Font-Size="Medium" ForeColor="#990033"></asp:CompareValidator>
                        </li>
                        <li>
                            <asp:Button ID="Register" runat="server" Text="Register" CssClass="button" Font-Names="Century Gothic" Font-Size="XX-Large" OnClick="register_Click" ValidationGroup="RegGroup" />
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
