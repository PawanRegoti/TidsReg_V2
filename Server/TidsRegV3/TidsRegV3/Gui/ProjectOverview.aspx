<%@ page title="" language="C#" masterpagefile="~/Gui/Layout.Master" autoeventwireup="true" codebehind="ProjectOverview.aspx.cs" inherits="TidsRegV3.Gui.ProjectOverview" %>

<%@ register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div align="center">
    <table style="width: 100%;">
      <tr>
        <td style="width: 30%">
          <div>
            <asp:DropDownList CssClass="button" ID="ProjectDropDown" runat="server"></asp:DropDownList>

            <div style="margin: 20px">
              <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" CellPadding="5" Font-Names="Century Gothic" Font-Size="8pt" ForeColor="#003399" Height="300px" Width="300px" SelectionMode="DayWeekMonth">
                <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                <OtherMonthDayStyle ForeColor="#999999" />
                <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                <WeekendDayStyle BackColor="#CCCCFF" />
              </asp:Calendar>
            </div>
          </div>
        </td>

        <td style="width: 70%;" align="right">
          <div align="right">
            <asp:Chart ID="Chart1" runat="server" OnLoad="Chart1_Load" Height="500px" Width="1100px">
              <Series>
                <asp:Series Name="Series1"></asp:Series>
              </Series>
              <ChartAreas>
                <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
              </ChartAreas>
            </asp:Chart>
          </div>
        </td>
      </tr>
      <tr>
        <td style="width: 30%; align-content: center">
          <div>
            <asp:Button ID="CreateGrid" runat="server" Text="Go" Width="100px" CssClass="button" OnClick="CreateGrid_Click" />
          </div>
        </td>

        <td style="width: 70%; margin: 50px" align="right">
          <div>
            <asp:DropDownList CssClass="button" ID="ChartViewDropDown" runat="server" Visible="False"></asp:DropDownList>
            <asp:Button ID="UpdateChart" runat="server" Text="Go" Width="100px" CssClass="button" Visible="False" OnClick="UpdateChart_Click" />
            <asp:Button ID="Export" runat="server" Text="Export" CssClass="button" OnClick="Export_Click" Visible="False" />
          </div>
        </td>
      </tr>
    </table>
  </div>
</asp:Content>
