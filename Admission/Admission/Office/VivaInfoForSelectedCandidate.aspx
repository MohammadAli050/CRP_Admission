<%@ Page Title="VIVA Information" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="VivaInfoForSelectedCandidate.aspx.cs" Inherits="Admission.Admission.Office.VivaInfoForSelectedCandidate" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">




    <script type="text/javascript">

        //function jsShowHideProgress() {

        //    console.log('In jsShowHideProgress()');

        //    setTimeout(function () {

        //        console.log('In jsShowHideProgress() => setTimeout(function (200))');

        //        document.getElementById('divProgress').style.display = 'block';
        //        //$("#MainContent_btnLoad").attr('disabled', true);
        //        //$("#MainContent_btnUploadExcel").attr('disabled', true);
        //    }, 200);        //    deleteCookie();        //    var timeInterval = 500; // milliseconds (checks the cookie for every half second )        //    var loop = setInterval(function () {
        //        console.log('In jsShowHideProgress() => setTimeout(function (' + timeInterval + '))');

        //        if (IsCookieValid()) {
        //            document.getElementById('divProgress').style.display = 'none';
        //            //$("#MainContent_btnLoad").attr('disabled', false);
        //            //$("#MainContent_btnUploadExcel").attr('disabled', false);
        //            clearInterval(loop)
        //        }
        //    }, timeInterval);
        //}        //// cookies        //function deleteCookie() {
        //    var cook = getCookie('ExcelDownloadFlag');        //    if (cook != "") {
        //        document.cookie = "ExcelDownloadFlag=;Path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC";
        //    }
        //}        //function IsCookieValid() {
        //    var cook = getCookie('ExcelDownloadFlag');        //    return cook != '';
        //}        //function getCookie(cname) {
        //    var name = cname + "=";        //    var ca = document.cookie.split(';');        //    for (var i = 0; i < ca.length; i++) {
        //        var c = ca[i];        //        while (c.charAt(0) == ' ') {
        //            c = c.substring(1);
        //        }        //        if (c.indexOf(name) == 0) {
        //            return c.substring(name.length, c.length);
        //        }
        //    }        //    return "";
        //}


        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'inline';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <h3 style="margin-top: 10px; margin-bottom: 10px;">VIVA Information</h3>
            <hr style="margin: 10px 0px; color: #f2f2f2; background-color: #dfdcdc;" />
        </div>
    </div>


    <div class="row">
        <div class="col-sm-12">
            <asp:UpdatePanel ID="UpdatePanelMassage" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-3 col-md-3 col-lg-3">
                    <label>Faculty <span style="color: crimson">*</span></label>
                    <asp:DropDownList ID="ddlAdmUnit" runat="server" CssClass="form-control" Width="100%">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                        ControlToValidate="ddlAdmUnit" ErrorMessage="Required" Font-Size="9pt" Font-Bold="true"
                        ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                        ValidationGroup="gr1"></asp:CompareValidator>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3">
                    <label>Session <span style="color: crimson">*</span></label>
                    <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control" Width="100%"></asp:DropDownList>
                    <asp:CompareValidator ID="CompareValidator2" runat="server"
                        ControlToValidate="ddlSession" ErrorMessage="Required" Font-Size="9pt" Font-Bold="true"
                        ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                        ValidationGroup="gr1"></asp:CompareValidator>
                </div>
                <div class="col-sm-1 col-md-1 col-lg-1">
                    <br />
                    <asp:LinkButton ID="btnLoad" runat="server" OnClick="btnLoad_Click" ValidationGroup="gr1" CssClass="btn btn-primary" Style="margin-top: 4px;">
                            <i class='fa fa-cog' aria-hidden='true'></i>&nbsp;Load
                    </asp:LinkButton>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3">
                    <label>Viva Date <span style="color: crimson">*</span></label>
                    <asp:TextBox ID="txtDate" runat="server" Width="100%" CssClass="form-control" placeholder="dd/MM/yyyy" AutoCompleteType="Disabled"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                        TargetControlID="txtDate" Format="dd/MM/yyyy" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="txtDate" ErrorMessage="Required" ForeColor="Crimson"
                        Font-Size="9pt" Display="Dynamic" Font-Bold="true"
                        ValidationGroup="gr2"></asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2">
                    <br />
                    <asp:LinkButton ID="btnViewReport" runat="server" ValidationGroup="gr2" OnClick="btnViewReport_Click" CssClass="btn btn-info" Style="margin-top: 4px;">
                            <i class='fa fa-cog' aria-hidden='true'></i>&nbsp;View Report
                    </asp:LinkButton>
                </div>
            </div>

            <br />

            <asp:Panel ID="panelShowHide" runat="server" Visible="false">
                <hr style="margin: 10px 0px;" />
                <div class="row">
                    <div class="col-sm-3 col-md-3 col-lg-3">
                        <span>Excel Column: Serial, TestRoll, Name, Date, Time, Faculty</span>
                        <label>Upload File (Excel) <span style="color: crimson">*</span></label>
                        <br />
                        <asp:FileUpload runat="server" ID="fuExcel" />
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3">
                        <br />
                        <asp:LinkButton ID="btnUploadExcel" runat="server" OnClick="btnUploadExcel_Click" CssClass="btn btn-success" Style="margin-top: 4px;">
                            <i class="fas fa-upload"></i>&nbsp;Upload
                        </asp:LinkButton>
                        <%--OnClientClick="jsShowHideProgress();"--%>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3">
                        <%--<br />
                        <asp:LinkButton ID="btnAssignSession" runat="server" OnClick="btnAssignSession_Click" ValidationGroup="gr2" CssClass="btn btn-danger" Style="margin-top: 4px;">
                            <i class='fa fa-cog' aria-hidden='true'></i>&nbsp;Assign Session
                        </asp:LinkButton>--%>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3">
                        <%--<br />
                        <asp:LinkButton ID="btnRegistrationNoSave" runat="server" OnClick="btnRegistrationNoSave_Click" CssClass="btn btn-success" Style="margin-top: 4px;">
                            <i class="fas fa-save"></i>&nbsp;Save / Update
                        </asp:LinkButton>
                        <br />
                        <strong style="color: #31708f">Note: For Save/Update, Registration No must be filled out.</strong>--%>
                    </div>
                </div>
            </asp:Panel>



        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnLoad" />
            <asp:PostBackTrigger ControlID="btnUploadExcel" />
        </Triggers>
    </asp:UpdatePanel>


    <hr />
    <asp:UpdatePanel ID="updatePanel_Report" runat="server">
        <ContentTemplate>
            <div class="row">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                    AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="true">
                    <LocalReport ReportPath="Admission/Office/VivaInfoForSelectedCandidate.rdlc" EnableExternalImages="true">
                    </LocalReport>
                </rsweb:ReportViewer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <%--<asp:UpdatePanel ID="upGridView" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panelGridView" runat="server" Visible="false">
                <asp:GridView ID="gvVivaCandidateList" AllowSorting="True" runat="server" CssClass="table-bordered"
                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <FooterStyle BackColor="#737CA1" ForeColor="White" />
                    <HeaderStyle BackColor="#737CA1" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#F0F8FF" />

                    <Columns>
                        <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Serial">
                            <ItemTemplate>
                                <asp:Label ID="lblSerialId" runat="server" Text='<%#Eval("SerialId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Roll">
                            <ItemTemplate>
                                <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="txtName" Font-Bold="true" Text='<%#Eval("CandidateName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Viva Date">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblVivaDate" Text='<%#Eval("VivaDate","{0:dd-MM-yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Viva Time">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblVivaTime" Text='<%#Eval("VivaTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Faculty">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFacultyName" Text='<%#Eval("FacultyName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Phone">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPhoneNo" Text='<%#Eval("PhoneNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate>
                        No data found!                       
                    </EmptyDataTemplate>

                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                    <EditRowStyle BackColor="#7C6F57" />

                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>--%>



    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel1" runat="server">
        <Animations>
		<OnUpdating>
			<Parallel duration="0">
				<ScriptAction Script="InProgress();" />
				<EnableAction AnimationTarget="btnLoad" Enabled="false" />
			</Parallel>
		</OnUpdating>
		<OnUpdated>
			<Parallel duration="0">
				<ScriptAction Script="onComplete();" />
				<EnableAction   AnimationTarget="btnLoad" Enabled="true" />
			</Parallel>
		</OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
