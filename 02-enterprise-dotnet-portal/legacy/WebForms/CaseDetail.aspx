<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaseDetail.aspx.cs" Inherits="LegacyPortal.CaseDetail" %>
<!doctype html>
<html lang="en"><head><title>Case detail — legacy Web Forms</title></head>
<body>
<form id="form1" runat="server">
  <h1>Case detail (Web Forms — still in the environment)</h1>
  <p>Reference: <asp:Label ID="lblReference" runat="server" /></p>
  <p>Status: <asp:Label ID="lblStatus" runat="server" /></p>
  <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
  <p><small>Migration note: this page stays working as-is. New pages go to MVC;
  this page migrates only when it needs a change, page by page — no big-bang rewrite.</small></p>
</form>
</body></html>
