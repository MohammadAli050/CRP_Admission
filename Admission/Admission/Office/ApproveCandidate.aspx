<%@ Page Title="Bachelors - Approve Candidate" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveCandidate.aspx.cs" Inherits="Admission.Admission.Office.ApproveCandidate" %>

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


        #MainContent_gvUndergraduateApplicantInfo > tbody > tr.pagingDiv {
            background-color: #f2f2f2;
        }

            #MainContent_gvUndergraduateApplicantInfo > tbody > tr.pagingDiv table {
                padding-left: 10px;
                width: 50%;
            }

                #MainContent_gvUndergraduateApplicantInfo > tbody > tr.pagingDiv table td {
                    display: inline;
                }

        .pagingDiv a, .pagingDiv span {
            display: inline-block;
            padding: 0px 9px;
            margin-right: 4px;
            border-radius: 3px;
            border: solid 1px #c0c0c0;
            background: #e9e9e9;
            box-shadow: inset 0px 1px 0px rgba(255,255,255, .8), 0px 1px 3px rgba(0,0,0, .1);
            font-size: .875em;
            font-weight: bold;
            text-decoration: none;
            color: #717171;
            text-shadow: 0px 1px 0px rgba(255,255,255, 1);
        }

            .pagingDiv a:hover {
                background: #fefefe;
                background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#FEFEFE), to(#f0f0f0));
                background: -moz-linear-gradient(0% 0% 270deg,#FEFEFE, #f0f0f0);
            }

            .pagingDiv a.active {
                border: none;
                background: #616161;
                box-shadow: inset 0px 0px 8px rgba(0,0,0, .5), 0px 1px 0px rgba(255,255,255, .8);
                color: #f0f0f0;
                text-shadow: 0px 0px 3px rgba(0,0,0, .5);
            }

        .pagingDiv span {
            color: #f0f0f0;
            background: #616161;
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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />


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

                        <asp:Button ID="btnApproveUG" runat="server" Text="Approve For UG" Visible="false" CssClass="float-right"
                            ForeColor="Green" OnClick="btnApproveUG_Click" />
                        <asp:Button ID="btnApprovePG" runat="server" Text="Approve For PG" Visible="false" CssClass="float-right"
                            ForeColor="Green" OnClick="btnApprovePG_Click" />
                    </div>
                    <br />
                    <div>

                        <asp:Panel ID="undergradPanelForGridView" runat="server" Enabled="false" Visible="false">
                            <asp:GridView ID="gvUndergraduateApplicantInfo" runat="server"
                                CssClass="table table-responsive table-hover table-bordered"
                                AutoGenerateColumns="false" Width="100%"
                                OnRowDataBound="gvUndergraduateApplicantInfo_RowDataBound"
                                OnPageIndexChanging="gvUndergraduateApplicantInfo_PageIndexChanging"
                                AllowPaging="true"
                                PageSize="2000"
                                PagerStyle-CssClass="pagingDiv">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" Font-Size="Smaller" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PaymentID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGPaymentID"
                                                Text='<%#Eval("PaymentId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FormSL" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGFormSl"
                                                Text='<%#Eval("FormSerial") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Photo">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="imgUGPhoto"
                                                Text='<%#Eval("candidatePhoto") %>'></asp:Label>
                                            <%--<asp:Image runat="server" ID="imgUGPhoto" Height="50px" Width="45px"
                                                ImageUrl='<%#Eval("candidatePhoto") %>' />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:ImageField DataImageUrlField="canidatePhoto" ItemStyle-Height="50px" ItemStyle-Width="45px"></asp:ImageField>--%>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGCandidateName"
                                                Text='<%#Eval("candidateName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mobile">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGMobile"
                                                Text='<%#Eval("candidateSMSPhone") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil GPA">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGSSCGpa"
                                                Text='<%#Eval("sscResult") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil GPAW4S" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGSSCGpaW4s"
                                                Text='<%#Eval("sscResultGpaW4s") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil Exam Type">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGSSCBoard"
                                                Text='<%#Eval("sscEduBoard") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil Group">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGSSCGroup"
                                                Text='<%#Eval("sscGroupOrSub") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil Year">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGSSCYear"
                                                Text='<%#Eval("sscPassingYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim GPA">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGHSCGpa"
                                                Text='<%#Eval("hscResult") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil GPAW4S" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGHSCGpaW4s"
                                                Text='<%#Eval("hscResultGpaW4s") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim Exam Type">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGHSCBoard"
                                                Text='<%#Eval("hscEduBoard") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim Group">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGHSCGroup"
                                                Text='<%#Eval("hscGroupOrSub") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim Year">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGHSCYear"
                                                Text='<%#Eval("hscPassingYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GPA Total">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGGpaTotal"
                                                Text='<%#(Convert.ToDouble(Eval("sscResult"))
                                                    + Convert.ToDouble(Eval("hscResult"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quota">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGQuota"
                                                Text='<%#Eval("quota") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="chkUGSelectAll" Font-Bold="true" Text="<b> Select All</b>"
                                                OnCheckedChanged="chkUGSelectAll_CheckedChanged" AutoPostBack="true" />
                                        </HeaderTemplate>
                                        <HeaderStyle Font-Size="X-Small" />
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkUGApprove" />
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUGCandidateId" Text='<%#Eval("candidateID") %>'></asp:Label>
                                            <asp:Label runat="server" ID="lblUGAdmUnitId" Text='<%#Eval("admSetupAdmUnitID") %>'></asp:Label>
                                            <asp:Label runat="server" ID="lblUGAcaCalId" Text='<%#Eval("admSetupAcaCalId") %>'></asp:Label>
                                            <asp:Label runat="server" ID="lblUGAdmSetupId" Text='<%#Eval("admSetupID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>

                        <%-- ---------------------------------------------------------------------------------------------------- --%>

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
                                            <asp:Label runat="server" ID="imgPGPhoto"
                                                Text='<%#Eval("candidatePhoto") %>'></asp:Label>
                                            <%--<asp:Image runat="server" ID="imgPGPhoto" Height="50px" Width="45px"
                                                ImageUrl='<%#Eval("candidatePhoto") %>' />--%>
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

     <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanel1" runat="server">
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

