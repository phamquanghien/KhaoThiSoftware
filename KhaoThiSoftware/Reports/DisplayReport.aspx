<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayReport.aspx.cs" Inherits="KhaoThiSoftware.Reports.DisplayReport" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div>
        <h3>Demo Crystal report viewer</h3>
    </div>
    </div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px" ToolbarImagesFolderUrl="" ToolPanelWidth="200px" Width="350px" />
    </form>
</body>
</html>
