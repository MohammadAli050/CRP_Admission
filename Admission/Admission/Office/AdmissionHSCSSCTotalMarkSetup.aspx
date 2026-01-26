<%@ Page Title="SSC and HSC Total Marks Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionHSCSSCTotalMarkSetup.aspx.cs" Inherits="Admission.Admission.Office.AdmissionHSCSSCTotalMarkSetup" %>

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
                    <h4>SSC and HSC Total Marks Setup</h4>
                </div>
            </div>
        </div>
    </div>
    <hr />


    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="panel panel-default">
                <div class="panel-body">

                    <asp:HiddenField ID="hdnSetupId" runat="server" />

                    <div class="row">
                        <div class="col-lf-2 col-md-2 col-sm-2">
                            <b>Exam Type</b>
                            <asp:DropDownList ID="ddlExamType" AutoPostBack="true" OnSelectedIndexChanged="ddlExamType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-lf-2 col-md-2 col-sm-2">
                            <b>Board</b>
                            <asp:DropDownList ID="ddlEducationBoard" AutoPostBack="true" OnSelectedIndexChanged="ddlEducationBoard_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>

                        </div>
                        <div class="col-lf-2 col-md-2 col-sm-2">
                            <b>Group/Subject</b>
                            <asp:DropDownList ID="ddlGroup" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>

                        </div>
                        <div class="col-lf-2 col-md-2 col-sm-2">
                            <b>Passing Year</b>
                            <asp:DropDownList ID="ddlPassingYear" AutoPostBack="true" OnSelectedIndexChanged="ddlPassingYear_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>

                        </div>
                        <div class="col-lf-2 col-md-2 col-sm-2">
                            <b>Total Mark</b>
                            <asp:TextBox ID="txtTotalMark" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>

                        </div>
                        <div class="col-lf-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" Text="Load Information" CssClass="btn btn-info" Width="100%" OnClick="btnLoad_Click" />
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px">

                        <div class="col-lf-2 col-md-2 col-sm-2" runat="server" id="divSave">
                            <asp:Button ID="btnSave" runat="server" Text="Save Information" CssClass="btn btn-success" Width="100%" OnClick="btnSave_Click" />
                        </div>
                        <div class="col-lf-2 col-md-2 col-sm-2" runat="server" id="divCancel">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel Update" CssClass="btn btn-danger" Width="100%" OnClick="btnCancel_Click" />
                        </div>
                    </div>

                </div>
            </div>


            <div class="panel panel-default" style="margin-top: 10px" runat="server" id="divSetup">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-10 col-md-10 col-sm-10">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:LinkButton ID="lnkDownloadExcel" runat="server" CssClass="btn-primary btn-sm" Height="33px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 20px" Font-Bold="true" Text="Download Excel" OnClick="lnkDownloadExcel_Click" OnClientClick="jsShowHideProgress();"></asp:LinkButton>
                        </div>
                    </div>
                    <br />
                    <asp:GridView runat="server" ID="gvSetupList" AutoGenerateColumns="False" AllowPaging="false"
                        PagerSettings-Mode="NumericFirstLast" Width="100%"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                        ShowHeader="true" CssClass="gridCss">
                        <HeaderStyle BackColor="Green" ForeColor="White" />
                        <RowStyle BackColor="#ecf0f0" />
                        <AlternatingRowStyle BackColor="#ffffff" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL#">
                                <ItemTemplate>
                                    <b><%# Container.DataItemIndex + 1 %></b>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Exam Type">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblExamType" Text='<%# Eval("Attribute3") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Board">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBoard" Text='<%# Eval("Attribute4") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Group">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblGroup" Text='<%# Eval("Remarks") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Passing Year">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblYear" Text='<%# Eval("Year") %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Marks">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMark" Text='<%# Eval("TotalMarks") %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>


                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding: 5px; text-align: center">
                                        <asp:LinkButton ID="EditItem" ToolTip="Edit" ForeColor="Blue" OnClientClick="this.style.display = 'none'" CommandArgument='<%#Eval("Id")%>' runat="server" OnClick="EditItem_Click">
                                                                             <strong><i class="fas fa-pencil-alt"></i></strong>
                                        </asp:LinkButton>
                                    </div>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding: 5px; text-align: center">
                                        <asp:LinkButton ID="DeleteItem" ToolTip="Remove" ForeColor="Red" CommandArgument='<%#Eval("Id")%>' runat="server" OnClientClick="return confirm('Are you sure you want to Remove this Entry ?');" OnClick="DeleteItem_Click">                         
                                                                                <strong><i class="fas fa-trash"></i></strong>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:GridView>

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
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />
                    <EnableAction AnimationTarget="btnSave" Enabled="false" />
                    <EnableAction AnimationTarget="btnCancel" Enabled="false" />
                    <EnableAction AnimationTarget="lnkDownloadExcel" Enabled="false" />

                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                    <EnableAction   AnimationTarget="btnSave" Enabled="true" />
                    <EnableAction   AnimationTarget="btnCancel" Enabled="true" />
                    <EnableAction   AnimationTarget="lnkDownloadExcel" Enabled="true" />


                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
