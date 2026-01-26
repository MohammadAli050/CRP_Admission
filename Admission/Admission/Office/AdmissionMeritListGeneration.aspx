<%@ Page Title="Admission Merit List Generation" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionMeritListGeneration.aspx.cs" Inherits="Admission.Admission.Office.AdmissionMeritListGeneration" %>


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
            document.getElementById("blurOverlay").style.display = "block";
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
            document.getElementById("blurOverlay").style.display = "none";
        }


        function jsShowHideProgress() {
            setTimeout(function () {
                document.getElementById('divProgress').style.display = 'block';
                document.getElementById("blurOverlay").style.display = "block";
            }, 200);
            deleteCookie();

            var timeInterval = 500; // milliseconds (checks the cookie for every half second )

            var loop = setInterval(function () {
                if (IsCookieValid()) {
                    document.getElementById('divProgress').style.display =
                        'none'; document.getElementById("blurOverlay").style.display = "none"; clearInterval(loop)
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

        <div id="blurOverlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; backdrop-filter: blur(5px); background-color: rgba(255, 255, 255, 0.3); z-index: 1000000;">
</div>
    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Please wait.Processing your request.................." ForeColor="Red" Font-Bold="true"></asp:Label>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Admission Merit List Generation</h4>
                </div>
            </div>
        </div>
    </div>
    <hr />

    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="panel panel-default">
                <div class="panel-body">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Session</strong></label>
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
                                <label><strong>Faculty</strong></label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:LinkButton ID="lnkLoad" runat="server" CssClass="btn-info btn-sm" Height="33px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 25px" Font-Bold="true" Text="Load Admission Result" OnClick="lnkLoad_Click"></asp:LinkButton>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:LinkButton ID="lnkMeritListGenerate" runat="server" CssClass="btn-danger btn-sm" Height="33px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 25px" Font-Bold="true" Text="Generate Merit Position" OnClick="lnkMeritListGenerate_Click" OnClientClick="jsShowHideProgress();"></asp:LinkButton>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:LinkButton ID="lnkProgramAssignOld" runat="server" CssClass="btn-primary btn-sm" Visible="false" Height="33px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 25px" Font-Bold="true" Text="Assign Program" OnClick="lnkProgramAssign_Click" OnClientClick="jsShowHideProgress();"></asp:LinkButton>
                            <asp:LinkButton ID="lnkProgramAssign" runat="server" CssClass="btn-primary btn-sm" Height="33px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 25px" Font-Bold="true" Text="Assign Program" OnClick="lnkProgramAssign_ClickNew" OnClientClick="jsShowHideProgress();"></asp:LinkButton>

                        </div>

                    </div>
                </div>
            </div>


            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12" style="font-weight: bold">
                            <ul>
                                <li style="color: blue">Step 1 : Please click "Load Admission Result" Button to get all candidate list</li>
                                <li style="color: red">Step 2 : Please click "Generate Merit Position" Button to assign position</li>
                                <li style="color: blue">Step 3 : Please click "Assign Program" Button to assign a program based on position and Program priority</li>
                            </ul>
                        </div>
                    </div>

                </div>
            </div>

            <div class="panel panel-default" style="margin-top: 10px" runat="server" id="divSeat">
                <div class="panel-body">

                    <h3>Seat Capacity Setup List</h3>

                    <asp:GridView runat="server" ID="gvSeatCapacity" AutoGenerateColumns="False" AllowPaging="false"
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

                            <asp:TemplateField HeaderText="Program">
                                <ItemTemplate>

                                    <asp:Label runat="server" ID="lblProgram" Text='<%# Eval("Attribute1") %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Total Seat">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblTotal" Text='<%# Eval("TotalSeat") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>




                            <asp:TemplateField HeaderText="Merit Seat">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblMeritSeat" Text='<%# Eval("MeritSeat") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Quota Seat">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQuota" Text='<%# Eval("Attribute2") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Total Setup">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblOccupied" Text='<%# Eval("Attribute3") %>' ForeColor='<%#  Convert.ToInt32(Eval("Attribute3"))>=Convert.ToInt32(Eval("TotalSeat")) ? System.Drawing.Color.Red : System.Drawing.Color.Black %>'
                                            Font-Bold="true"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Occupied Seat">
                                <ItemTemplate>
                                    <div style="text-align: center">

                                        <asp:Label runat="server" ID="lblOccupied" Text='<%# Eval("CreatedBy") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remaining">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblRemaining" Text='<%# Convert.ToInt32(Eval("TotalSeat"))-Convert.ToInt32(Eval("CreatedBy")) %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>


                        </Columns>
                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:GridView>

                </div>
            </div>

            <div class="panel panel-default" style="margin-top: 5px" runat="server" id="divGrid">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <br />
                            <asp:Label runat="server" ID="lblTotalCandidate" Text="" ForeColor="Crimson" Font-Bold="true"></asp:Label>
                        </div>
                    </div>

                    <div class="row" style="margin-top:10px">

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Program</b>
                            <asp:DropDownList ID="ddlProgramFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProgramFilter_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
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

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Selected By</b>
                            <asp:DropDownList ID="ddlFilterEligibleBy" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterEligibleBy_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="-5">-All-</asp:ListItem>
                                <asp:ListItem Value="0">Merit</asp:ListItem>
                                <asp:ListItem Value="1">Quota</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:LinkButton ID="lnkDownloadExcel" runat="server" CssClass="btn-success btn-sm" Height="33px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 20px" Font-Bold="true" Text="Download Excel" OnClick="lnkDownloadExcel_Click" OnClientClick="jsShowHideProgress();"></asp:LinkButton>
                        </div>

                    </div>

                    <br />
                    <asp:GridView runat="server" ID="gvCandidateList" AutoGenerateColumns="False" AllowPaging="true" PageSize="1000" OnPageIndexChanging="gvCandidateList_PageIndexChanging"
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

                            <asp:TemplateField HeaderText="Admission Roll">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbTestRoll" Text='<%# Eval("StudentTestRoll") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Subject 1 & 2">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbSub12" Text='<%# "Sub 1 : "+ Eval("Subject1") +"<br />"+"Sub 2 : "+ Eval("Subject2") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Subject 3 & 4">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbSub34" Text='<%# "Sub 3 : "+ Eval("Subject3") +"<br />"+"Sub 4 : "+ Eval("Subject4") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Subject 5 & 6">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbSub56" Text='<%# "Sub 5 : "+ Eval("Subject5") +"<br />"+"Sub 6 : "+ Eval("Subject6") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Subject 7 & 8">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbSub78" Text='<%# "Sub 7 : "+ Eval("Subject7") +"<br />"+"Sub 8 : "+ Eval("Subject8") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Subject 9 & 10">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbSub910" Text='<%# "Sub 9 : "+ Eval("Subject9") +"<br />"+"Sub 10 : "+ Eval("Subject10") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Test Total">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lbSubTotal" Text='<%# Eval("TotalMark") %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SSC">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbSSC" Text='<%# "Obtd Marks : "+ Eval("SSCAchivedMark") +"<br />"+"Wtg : "+ Eval("SSCActualMark") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="HSC">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbHSC" Text='<%# "Obtd Marks : "+ Eval("HSCAchivedMark") +"<br />"+"Wtg : "+ Eval("HSCActualMark") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbTotal" Text='<%# "Total : "+ Eval("TotalAchivedMarks") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Merit Position">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lbMerit" Text='<%# Eval("MeritPosition") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Eligible">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblEligibleProgram" Text='<%# Convert.ToInt32(Eval("StatusId"))<=0 ? "" :Eval("Program") +"<br />" +"Priority : "+Eval("ProgramPriority") +"<br />"+Eval("MeritName") %>' ForeColor='<%# Convert.ToInt32(Eval("PositionQuotaTypeId"))>0 ? System.Drawing.Color.Red : System.Drawing.Color.Black %>' Font-Bold="true"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                                <HeaderStyle />
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
                    <EnableAction AnimationTarget="lnkLoad" Enabled="false" />
                    <EnableAction AnimationTarget="lnkMeritListGenerate" Enabled="false" />
                    <EnableAction AnimationTarget="lnkProgramAssign" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="lnkLoad" Enabled="true" />
                    <EnableAction   AnimationTarget="lnkMeritListGenerate" Enabled="true" />
                    <EnableAction   AnimationTarget="lnkProgramAssign" Enabled="true" />

                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
