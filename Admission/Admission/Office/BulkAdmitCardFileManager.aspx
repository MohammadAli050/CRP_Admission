<%@ Page Title="Bulk Admit Card File Manager" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BulkAdmitCardFileManager.aspx.cs" Inherits="Admission.Admission.Office.BulkAdmitCardFileManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

    
    <script type="text/javascript">
     function jsShowHideProgress() {
            setTimeout(function () {
                document.getElementById('divProgress').style.display = 'block';
            }, 200);
            deleteCookie();

            var timeInterval = 500; // milliseconds (checks the cookie for every half second )

            var loop = setInterval(function () {
                if (IsCookieValid()) {
                    document.getElementById('divProgress').style.display =
                    'none'; clearInterval(loop)
                }

            }, timeInterval);
        }
        // cookies
        function deleteCookie() {
            var cook = getCookie('ExcelDownloadFlag');
            if (cook != "") {
                document.cookie = "ExcelDownloadFlag=;Path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC";
            }
        }

        function IsCookieValid() {
            var cook = getCookie('ExcelDownloadFlag');
            return cook != '';
        }

        function getCookie(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Please wait.Processing your request.................." ForeColor="Red" Font-Bold="true"></asp:Label>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Bulk Admit Card File Manager</h4>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="panelGridViewMessage" runat="server">
                        <asp:Label ID="lblMessageGv" runat="server" Text="No Message"></asp:Label>
                    </asp:Panel>

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-sm-8">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Minimum SL</b>
                            <asp:TextBox ID="txtMinTextRoll" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>

                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Maximum SL</b>
                            <asp:TextBox ID="txtMaxTextRoll" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" EmptyDataText="No directories found"
                        CssClass="table table-bordered table-condensed table-hover table-responsive table-striped">
                        <Columns>
                            <asp:TemplateField HeaderText="SL#" HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DirectoryName" HeaderText="Directory Name" />
                            <asp:BoundField DataField="CreationTime" HeaderText="Creation Time" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" Text="Download" runat="server" CommandArgument='<%# Eval("DirectoryName") %>' OnClientClick="jsShowHideProgress();"
                                        OnClick="lnkDownload_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CommandArgument='<%# Eval("DirectoryName") %>'
                                        OnClientClick="return confirm('Are you sure you want to Delete this directory?');)"
                                        OnClick="lnkDelete_Click"
                                        Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>


                </div>
            </div>
        </div>
    </div>

</asp:Content>
