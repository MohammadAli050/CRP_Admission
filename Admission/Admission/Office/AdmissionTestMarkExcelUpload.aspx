<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionTestMarkExcelUpload.aspx.cs" Inherits="Admission.Admission.Office.AdmissionTestMarkExcelUpload" %>

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

    </script>

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
                    <h4>Admission Marks Upload Setup</h4>
                </div>
            </div>
        </div>
        <hr />
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
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
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Exam Taken</strong></label>

                                <asp:TextBox runat="server" ID="txtTotalMark" class="form-control" Style="width: 100%;" TextMode="number"></asp:TextBox>
                                <%--   <input type="text" id="txtTotalMark" class="form-control" style="width: 100%;" />--%>
                            </div>
                        </div>

                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Convert To</strong></label>
                                <asp:TextBox runat="server" ID="txtConvertMark" class="form-control" Style="width: 100%;" TextMode="number"></asp:TextBox>
                                <%--<input type="text" id="txtConvertMark1" runat="server" class="form-control" style="width: 100%;" />--%>
                            </div>
                        </div>


                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <%--  <label><strong>Convert To</strong></label>--%>
                                <asp:Button runat="server" ID="btnLoad" class="form-control" Style="width: 100%; margin-top: 23px" Text="Load" CssClass="btn btn-primary" OnClick="btnLoad_Click" />
                                <%-- <asp:TextBox runat="server" ID="TextBox1" class="form-control" style="width: 100%;" TextMode ="number"></asp:TextBox>--%>
                                <%--<input type="text" id="txtConvertMark1" runat="server" class="form-control" style="width: 100%;" />--%>
                            </div>
                        </div>

                        <%--   <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Exam Type</strong></label>
                                <asp:DropDownList ID="ddlExamType" Width="100%" CssClass="form-control" runat="server" Enabled="false">
                                    <asp:ListItem Enabled="true" Text="-All-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="O Level" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="A Level" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Diploma" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                        <%--  <div class="col-sm-1 col-md-1 col-lg-1">
                            <div class="form-group">
                                <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn btn-info"
                                    OnClick="btnLoad_Click"
                                    Style="width: 100%; margin-top: 24px;" />
                            </div>
                        </div>
                        <div class="col-sm-1 col-md-1 col-lg-1">
                            <div class="form-group">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success"
                                    OnClick="btnAdd_Click"
                                    Style="width: 100%; margin-top: 24px;" />
                            </div>
                        </div>--%>

                        <%-- new code for button --%>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <hr />
                            <asp:Label ID="Label2" runat="server" Text="Select Excel File" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <br />

                            <asp:FileUpload ID="ExcelUpload" runat="server" accept=".xlsx,.xls" CssClass="btn btn-primary" Width="100%" Style="margin-bottom: 5px" ClientIDMode="Static" Height="58%" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2" runat="server" visible="false">
                            <hr />
                            <asp:Label ID="Label3" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <br />
                            <asp:LinkButton ID="lnkExcelUpload" runat="server" CssClass="btn-info btn-sm" Height="35px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 0px" Font-Bold="true" Text="Load Excel Data" OnClick="btnExcelUpload_Click"
                                ClientIDMode="Static" CausesValidation="false"></asp:LinkButton>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <hr />
                            <asp:Label ID="Label4" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <br />
                            <asp:LinkButton ID="lnkSampleExcel" runat="server" CssClass="btn-info btn-sm" Height="35px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 0px" Font-Bold="true" Text="Sample Excel" OnClick="lnkSampleExcel_Click"
                                ClientIDMode="Static" CausesValidation="false"></asp:LinkButton>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3" runat="server" visible="false">
                            <hr />
                            <asp:Label ID="Label5" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <br />
                            <asp:LinkButton ID="lnkStudentMigrateButton" runat="server" CssClass="btn-primary btn-sm" Height="35px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 0px" Font-Bold="true" Visible="true" Text="Migrate Result with Load Data" OnClick="lnkStudentMigrateButton_Click"></asp:LinkButton>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <hr />
                            <asp:Label ID="Label6" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <br />
                            <asp:LinkButton ID="lnkStudentMigrateButtonDirect" runat="server" OnClientClick="jsShowHideProgress()" CssClass="btn-danger btn-sm" Height="35px" Style="display: inline-block; width: 100%; text-align: center; font-size: 17px; margin-top: 0px" Font-Bold="true" Visible="true" Text="Migrate Result Direct" OnClick="lnkStudentMigrateButtonDirect_Click" ClientIDMode="Static" CausesValidation="false"></asp:LinkButton>
                        </div>


                        <%-- new code ends --%>
                    </div>
                </div>
            </div>
        </div>
        <hr />


        <%-- GvTotalStudentList new code --%>
        <br />


        <div class="Panel panel-default">
            <div class="panel-body">

                <div class="row">
                    <div class="col-lg-6 col-md-6 col-sm-6" runat="server" id="DivTotalStudent" style="text-align: center">

                        <div class="Panel panel-default">
                            <div class="panel-body">

                                <div class="row">

                                    <asp:Label ID="lblTotalStudent" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>


                                </div>
                                <div class="row" style="margin-top: 10px; margin-left: 5px">
                                    <asp:GridView ID="GVTotalStudentList" runat="server">
                                        <HeaderStyle BackColor="SeaGreen" ForeColor="White" />
                                        <RowStyle BackColor="#ecf0f0" />
                                        <AlternatingRowStyle BackColor="#ffffff" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                    </div>




                    <%--   </div>

                    </div>
                </div>--%>
                    <%-- New code ends --%>
                    <div class="col-lg-6 col-md-6 col-sm-6" runat="server" id="DivNotUploadedStudent" style="text-align: center">

                        <div class="card">
                            <div class="card-body">

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6">
                                        <asp:Label ID="lblNotMigratedStudent" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label></b>
                                    </div>
                                    <%--  <div class="col-lg-4 col-md-4 col-sm-4">
                                                <asp:LinkButton ID="lnkDownloadExcel" runat="server" CssClass="btn-info btn-sm" Style="display: inline-block; width: 100%; text-align: center; font-size: 13px; " Font-Bold="true" Visible="false" Text="Download Excel" OnClick="lnkDownloadExcel_Click"
                                                    ClientIDMode="Static" CausesValidation="false"></asp:LinkButton>
                                            </div>--%>
                                </div>
                                <div class="row" style="margin-top: 10px">
                                    <asp:GridView ID="GVNotUploadedStudentList" runat="server">
                                        <HeaderStyle BackColor="SeaGreen" ForeColor="White" />
                                        <RowStyle BackColor="#ecf0f0" />
                                        <AlternatingRowStyle BackColor="#ffffff" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                    </div>


                </div>

            </div>
        </div>





        <%-- new code --%>


        <div class="col-md-12" style="overflow: scroll;">

            <div class=" panel panel-default" style="">
                <div class="table-responsive " style="border-radius: 8px;">
                    <asp:GridView ID="GvStudent" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="table table-hover table-default" Style="border: 0 !important; margin-bottom: 0"
                        AllowPaging="True" PageSize="1000" OnPageIndexChanging="GvStudent_PageIndexChanging"
                        PagerSettings-Mode="NextPreviousFirstLast"
                        PagerSettings-FirstPageText="&lt;&lt; First"
                        PagerSettings-LastPageText="Last &gt;&gt;"
                        PagerSettings-NextPageText="Next &gt;"
                        PagerSettings-PreviousPageText="&lt; Previous"
                        PagerStyle-CssClass="pagination-container"
                        CellPadding="4">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="StudentId" Visible="false" HeaderText="Id">
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Test Roll">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("StudentTestRoll") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>


                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 1">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject1") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>


                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 2">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject2") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 3">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject3") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 4">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject4") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 5">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject5") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 6">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject6") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 7">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject7") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 8">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject8") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 9">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject9") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Subject 10">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("Subject10") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Converted Total Mark">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("ConvertedTotalMark") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Converted Percentage">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("ConvertedMarkPercentage") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Total Mark">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("StudentMark") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>


                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Exam Taken">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestRoll" Text='<%#Eval("TotalMark") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>




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

        <%-- new code ends --%>


        <triggers>
            <asp:PostBackTrigger ControlID="lnkExcelUpload" />

        </triggers>


    </div>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            </ContentTemplate>
            </asp:UpdatePanel>

        <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel2" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnInsCancel" Enabled="false" />                   
                </Parallel>
            </OnUpdating>
                <OnUpdated>
                    <Parallel duration="0">
                        <ScriptAction Script="onComplete();" />
                        <EnableAction   AnimationTarget="btnInsCancel" Enabled="true" />
                    </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>


</asp:Content>
