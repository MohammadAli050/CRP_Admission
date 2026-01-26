<%@ Page Title="Masters - Approve Candidates" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveCandidateGrad.aspx.cs" Inherits="Admission.Admission.Office.ApproveCandidateGrad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />

    <style type="text/css">
        .modal {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .center {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .center img {
                height: 128px;
                width: 128px;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>Approve Candidates For Written Test</h4>
            </div>
            <div class="panel-body">
                <div>
                    <p><strong>Filter criteria</strong></p>
                </div>
                <asp:UpdatePanel ID="updatePanel_Filter" runat="server">
                    <ContentTemplate>
                        <table class="table_form table_fullwidth">
                            <tr>
                                <td class="style_td" style="width: 15%">Faculty/Program</td>
                                <td style="width: 35%">
                                    <asp:DropDownList ID="ddlSchoolProgram" runat="server" Width="95%"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="style_td style_td_secondCol" style="width: 15%">Session</td>
                                <td style="width: 35%">
                                    <asp:DropDownList ID="ddlSession" runat="server" Width="50%"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style_td">SSC/O-Level/Dakhil GPA >=</td>
                                <td>
                                    <asp:TextBox ID="txtSSCGpa" runat="server" Text="0" Width="50%"></asp:TextBox>
                                </td>
                                <td class="style_td style_td_secondCol">HSC/A-Level/Alim GPA >=</td>
                                <td>
                                    <asp:TextBox ID="txtHSCGpa" runat="server" Text="0" Width="50%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style_td">SSC Marks >=</td>
                                <td>
                                    <asp:TextBox ID="txtSSCMarks" runat="server" Text="0" Width="50%"></asp:TextBox>
                                </td>
                                <td class="style_td style_td_secondCol">HSC Marks >=</td>
                                <td>
                                    <asp:TextBox ID="txtHSCMarks" runat="server" Text="0" Width="50%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style_td">SSC/O-Level/Dakhil Group/Subject</td>
                                <td>
                                    <asp:DropDownList ID="ddlSSCGrpSub" runat="server" Width="50%"></asp:DropDownList>
                                </td>
                                <td class="style_td style_td_secondCol">HSC/A-Level/Alim Group/Subject</td>
                                <td>
                                    <asp:DropDownList ID="ddlHSCGrpSub" runat="server" Width="50%"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style_td">Undergrad GPA >=</td>
                                <td>
                                    <asp:TextBox ID="txtUndergradGpa" runat="server" Text="0" Width="50%"></asp:TextBox>
                                </td>
                                <td class="style_td style_td_secondCol">Graduate GPA >=</td>
                                <td>
                                    <asp:TextBox ID="txtGraduateGpa" runat="server" Text="0" Width="50%"></asp:TextBox>
                                </td>
                            </tr>
                            <%--<tr>
                                <td class="style_td">Sort By</td>
                                <td>
                                    <asp:DropDownList ID="ddlSortBy" runat="server" Width="50%"></asp:DropDownList>
                                </td>
                                <td class="style_td style_td_secondCol">Medium</td>
                                <td>
                                    <asp:DropDownList ID="ddlMediumType" runat="server" Width="50%" Enabled="false"></asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="style_td">Final Submit</td>
                                <td>
                                    <asp:DropDownList ID="ddlIsFinalSubmit" runat="server" Width="50%"></asp:DropDownList>
                                </td>
                                <td class="style_td style_td_secondCol">Approved</td>
                                <td>
                                    <asp:DropDownList ID="ddlIsApproved" runat="server" Width="50%"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <%-- END PANEL_DEFAULT FILTER --%>
    </div>
    <%-- END ROW --%>
    <%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>

    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>

            <div>
                <div>
                    <div>
                        <strong>
                            <asp:Label ID="lblType" runat="server"></asp:Label>
                            &nbsp; : &nbsp;
                            Records 
                        </strong>-
                        <asp:Label ID="lblCount" runat="server" CssClass="label label-info"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>

                        <asp:Button ID="btnApprovePG" runat="server" Text="Approve For PG" Visible="false" CssClass="float-right"
                            ForeColor="Green" OnClick="btnApprovePG_Click" />
                    </div>
                    <br />
                    <div>

                        <asp:Panel ID="graduatePanelForGridView" runat="server" Enabled="false" Visible="false">
                            <asp:GridView ID="gvGraduateApplicantInfo" runat="server" CssClass="table table-responsive table-hover table-bordered"
                                AutoGenerateColumns="false" Width="100%"
                                OnRowDataBound="gvGraduateApplicantInfo_RowDataBound">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" Font-Size="Smaller" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PaymentID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGPaymentID"
                                                Text='<%#Eval("PaymentId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FormSL" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGFormSl"
                                                Text='<%#Eval("FormSerial") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Photo">
                                        <ItemTemplate>
                                            <asp:Image runat="server" ID="imgPGPhoto" Height="50px" Width="45px"
                                                ImageUrl='<%#Eval("candidatePhoto") %>' />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGCandidateName"
                                                Text='<%#Eval("candidateName") %>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mobile">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGMobile"
                                                Text='<%#Eval("candidateSMSPhone") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil GPA, Marks, Board, Year" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <strong style="color: blue">GPA/Division: </strong>
                                            <asp:Label runat="server" ID="lblPGSSCGpa"
                                                Text='<%#Eval("sscResult") %>'></asp:Label><br />
                                            <strong style="color: blue">Marks: </strong>
                                            <asp:Label runat="server" ID="lblPGSSCMarks"
                                                Text='<%#Eval("sscMarks") %>'></asp:Label><br />
                                            <strong style="color: blue">Board: </strong>
                                            <asp:Label runat="server" ID="lblPGSSCBoard"
                                                Text='<%#Eval("sscEduBoard") %>'></asp:Label><br />
                                            <strong style="color: blue">Year: </strong>
                                            <asp:Label runat="server" ID="lblPGSSCYear"
                                                Text='<%#Eval("sscPassingYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil Marks" Visible="false" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <%-- hidden column --%>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil Board" Visible="false" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <%-- hidden column --%>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil Group" Visible="false" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <%-- hidden column --%>
                                            <asp:Label runat="server" ID="lblPGSSCGroup"
                                                Text='<%#Eval("sscGroupOrSub") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil Year" Visible="false" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <%-- hidden column --%>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim GPA, Marks, Board, Year" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <strong style="color: blue">GPA/Division: </strong>
                                            <asp:Label runat="server" ID="lblPGHSCGpa"
                                                Text='<%#Eval("hscResult") %>'></asp:Label><br />
                                            <strong style="color: blue">Marks: </strong>
                                            <asp:Label runat="server" ID="lblPGHSCMarks"
                                                Text='<%#Eval("hscMarks") %>'></asp:Label><br />
                                            <strong style="color: blue">Board: </strong>
                                            <asp:Label runat="server" ID="lblPGHSCBoard"
                                                Text='<%#Eval("hscEduBoard") %>'></asp:Label><br />
                                            <strong style="color: blue">Year: </strong>
                                            <asp:Label runat="server" ID="lblPGHSCYear"
                                                Text='<%#Eval("hscPassingYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim Marks" Visible="false" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <%-- hidden column --%>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim Board" Visible="false" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <%-- hidden column --%>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim Group" Visible="false" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGHSCGroup"
                                                Text='<%#Eval("hscGroupOrSub") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim Year" Visible="false" HeaderStyle-BackColor="#0e69ae">
                                        <ItemTemplate>
                                            <%-- hidden column --%>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" BackColor="#f0f0f0" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Undergrad GPA">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGUndergradGpa"
                                                Text='<%#Eval("undResult") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Small" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Undergrad Program">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGUndergradProgram"
                                                Text='<%#Eval("undProgram") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Undergrad Institute">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGUndergradInstitute"
                                                Text='<%#Eval("undInstitute") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Undergrad Year">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGUndergradYear"
                                                Text='<%#Eval("undPassingYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grad GPA">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGGradGpa"
                                                Text='<%#Eval("grdResult") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Small" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grad Program">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGGradProgram"
                                                Text='<%#Eval("grdProgram") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grad Institute">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGGradInstitute"
                                                Text='<%#Eval("grdInstitute") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grad Year">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGGradYear"
                                                Text='<%#Eval("grdPassingYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quota">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGQuota"
                                                Text='<%#Eval("quota") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="chkPGSelectAll" Font-Bold="true" Text="<b> Select All</b>"
                                                OnCheckedChanged="chkPGSelectAll_CheckedChanged" AutoPostBack="true" />
                                        </HeaderTemplate>
                                        <HeaderStyle Font-Size="X-Small" />
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkPGApprove" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPGCandidateId" Text='<%#Eval("candidateID") %>'></asp:Label>
                                            <asp:Label runat="server" ID="lblPGAdmUnitId" Text='<%#Eval("admSetupAdmUnitID") %>'></asp:Label>
                                            <asp:Label runat="server" ID="lblPGAcaCalId" Text='<%#Eval("admSetupAcaCalId") %>'></asp:Label>
                                            <asp:Label runat="server" ID="lblPGAdmSetupId" Text='<%#Eval("admSetupID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel1" runat="server">
        <Animations>
            <OnUpdating>
                <Sequence>
                    <Parallel duration=".02" Fps="24">
                        <FadeOut AnimationTarget="UpdatePanel1" minimumOpacity=".2" />
                    </Parallel>
                </Sequence>
            </OnUpdating>
            <OnUpdated>
                <Sequence>
                    <Parallel duration=".02" Fps="24">
                        <FadeIn AnimationTarget="UpdatePanel1" maximumOpacity="1.0" />
                    </Parallel>
                </Sequence>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>


</asp:Content>
