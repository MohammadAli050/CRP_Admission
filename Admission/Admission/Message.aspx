<%@ Page Title="Message" Language="C#" AutoEventWireup="true" CodeBehind="Message.aspx.cs" Inherits="Admission.Admission.Message" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/Site.css" rel="stylesheet" />
    <title>Oops...</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <br />
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                    </asp:Panel>
                    
                    <div>
                        <a href="Home.aspx" class="btn btn-primary">Go To Homepage.</a>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>



