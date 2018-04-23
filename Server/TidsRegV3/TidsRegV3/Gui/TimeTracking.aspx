<%@ Page Title="" Language="C#" MasterPageFile="~/Gui/Layout.Master" AutoEventWireup="true" CodeBehind="TimeTracking.aspx.cs" Inherits="TidsRegV3.Gui.TimeTracking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <table style="width: 100%;">
            <tr>
                <td style="width: 30%">
                    <ul class="centerlist">
                        <li style="font-size: x-large">Add New Project</li>
                        <li>
                            <asp:TextBox ID="NewProject" CssClass="textbox" runat="server" Font-Names="Century Gothic" Font-Size="Large" CausesValidation="True"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="NewProjectValidator" runat="server" ErrorMessage="Project Name cannot be empty." ValidationGroup="NewProjectGroup" ControlToValidate="NewProject" Font-Size="Medium" ForeColor="#990033"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <asp:Button ID="AddNewProject" runat="server" Text="Add New Project" CssClass="button" Font-Names="Century Gothic" Font-Size="XX-Large" ValidationGroup="NewProjectGroup" OnClick="AddNewProject_Click" />
                        </li>
                        <li></li>
                        <li>
                            <asp:Button ID="route" runat="server" Text="Route" CssClass="button" OnClick="route_Click" /><br/>
                            <asp:Button ID="Statistics" runat="server" Text="Statistics / Invoices" CssClass="button" OnClick="Statistics_Click" /><br/>
                        </li>
                    </ul>
                </td>
                <td style="width: 40%">
                    <div align="center">
                        <asp:Button ID="Backward" Text="<" CssClass="button" Width="70px" Font-Size="XX-Large" runat="server" OnClick="Backward_Click" />
                        <asp:Button ID="Forward" Text=">" CssClass="button" Width="70px" Font-Size="XX-Large" runat="server" OnClick="Forward_Click" />
                    </div>
                    <div align="center" style="margin: 30px">
                        <asp:GridView ID="GridView1" runat="server" CellPadding="8" CellSpacing="4" CssClass="grid" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:CommandField ShowEditButton="True" />
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
