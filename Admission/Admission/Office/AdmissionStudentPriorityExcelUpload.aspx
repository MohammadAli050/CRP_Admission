<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionStudentPriorityExcelUpload.aspx.cs" Inherits="Admission.Admission.Office.AdmissionStudentPriorityExcelUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .myFlexVerticleHorizentalCenter_FlexDirectionRow {
            display: flex;
            flex-direction: row;
            flex-wrap: wrap;
            justify-content: center;
            align-items: center;
            align-content: stretch;
            height: 55px;
        }

        .myFlexVerticleHorizentalCenter_FlexDirectionColumn {
            display: flex;
            flex-direction: column;
            flex-wrap: wrap;
            justify-content: center;
            align-items: center;
            align-content: stretch;
            height: 55px;
        }
    </style>


    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

    </script>

    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }


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
    <div class="panel-heading">
        <h4>Admission Excel Upload</h4>
    </div>
    <asp:UpdatePanel ID="UpdatePanel_Filter" runat="server">
        <ContentTemplate>
            <div style="color: crimson">
            </div>
            <asp:Panel ID="Panel_Master" runat="server">
                <asp:Label ID="lblMessage_Masters" runat="server"></asp:Label>
            </asp:Panel>

            <asp:Panel ID="Panel_GridView" runat="server">
                <div>
                    <div class="row">

                        <div class="row">
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Session</strong></label>
                                    <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:CompareValidator ID="ddlSession_CV" runat="server"
                                        ControlToValidate="ddlSession"
                                        Display="Dynamic"
                                        ErrorMessage="Session is required"
                                        ForeColor="Crimson"
                                        ValueToCompare="-1"
                                        Font-Size="9pt"
                                        Operator="NotEqual"
                                        ValidationGroup="gr1">
                                    </asp:CompareValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Faculty</strong></label>
                                    <asp:DropDownList ID="ddlFaculty" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>

                            <%-- new code for button --%>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <asp:Label ID="Label2" runat="server" Text="Select Excel File" Font-Bold="true" Font-Size="Large"></asp:Label>
                                <br />

                                <asp:FileUpload ID="ExcelUpload" runat="server" accept=".xlsx,.xls" CssClass="btn btn-primary" Width="100%" Style="margin-bottom: 5px" ClientIDMode="Static" Height="58%" />
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Label ID="Label3" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                                <br />
                                <asp:LinkButton ID="lnkExcelUpload" runat="server" CssClass="btn-info btn-sm" Height="35px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 0px" Font-Bold="true" Text="Load Excel Data" OnClick="lnkExcelUpload_Click"
                                    ClientIDMode="Static" CausesValidation="false"></asp:LinkButton>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-12" style="overflow: scroll;">

                    <div class=" panel panel-default" style="">
                        <div class="table-responsive " style="border-radius: 8px;">
                            <asp:GridView ID="GvStudent" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="table table-hover table-default" Style="border: 0 !important; margin-bottom: 0"
                                CellPadding="4">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="TestRoll" HeaderText="TestRoll" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                </Columns>
                                <EditRowStyle BackColor="" />
                                <FooterStyle BackColor="" Font-Bold="True" ForeColor="Black" />
                                <HeaderStyle BackColor="green" Font-Bold="True" ForeColor="white" />
                                <PagerStyle BackColor="" ForeColor="#5D7B9D" HorizontalAlign="left" />
                                <RowStyle BackColor="" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>


        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExcelUpload" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
