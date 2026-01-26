<%@ Page Title="Admission merit list Report" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RptAdmissionMeritList.aspx.cs" Inherits="Admission.Admission.Office.Reports.RptAdmissionMeritList" %>


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
                    <h4>Admission merit list Report</h4>
                </div>
            </div>
        </div>
    </div>
    <hr />

    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <b>Session</b>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSession_CV" runat="server"
                                    ControlToValidate="ddlSession"
                                    Display="Dynamic"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    ValueToCompare="-1"
                                    Font-Size="9pt"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1">
                                </asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <b>Faculty</b>
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Program</b>
                            <asp:DropDownList ID="ddlProgramFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProgramFilter_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Quota</b>
                            <asp:DropDownList ID="ddlFilterQuota" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterQuota_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Status</b>
                            <asp:DropDownList ID="ddlFilterStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterStatus_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="-5">-All-</asp:ListItem>
                                <asp:ListItem Value="1">Selected</asp:ListItem>
                                <asp:ListItem Value="0">Waiting</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>

                    <div class="row" style="margin-top: 5px">
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Eligible For</b>
                            <asp:DropDownList ID="ddlFilterEligibleBy" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterEligibleBy_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="-5">-All-</asp:ListItem>
                                <asp:ListItem Value="0">Merit</asp:ListItem>
                                <asp:ListItem Value="1">Quota</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:LinkButton ID="lnkDownloadExcel" runat="server" CssClass="btn-info btn-sm" Height="33px" OnClientClick="jsShowHideProgress();" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 25px" Font-Bold="true" Text="Download Report" OnClick="lnkDownloadExcel_Click"></asp:LinkButton>
                        </div>

                    </div>

                </div>
            </div>
        </ContentTemplate>

        
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkDownloadExcel" />
        </Triggers>

    </asp:UpdatePanel>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="lnkLoad" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="lnkLoad" Enabled="true" />

                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
