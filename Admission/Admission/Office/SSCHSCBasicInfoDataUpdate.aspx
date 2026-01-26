<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true"
    CodeBehind="SSCHSCBasicInfoDataUpdate.aspx.cs" Inherits="Admission.Admission.Office.SSCHSCBasicInfoDataUpdate" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <style type="text/css">
        .style_td {
            font-weight: bold;
            text-align: left;
            font-size: 9pt;
        }

        .style_td1 {
            font-weight: bold;
            text-align: right;
            font-size: 9pt;
        }

        .style_td_secondCol {
            border-left: dotted;
            border-color: gray;
            border-width: 1px;
        }

        .spanAsterisk {
            color: crimson;
            font-size: 12pt;
        }

        .style_thead {
            text-align: center;
            /*background-color: lightgrey;*/
            font-family: Calibri;
            font-size: 12pt;
            font-weight: bold;
        }

        .panelBody_edu_marginBottom {
            margin-bottom: -3%;
        }



        .modalBackground {
            background-color: #2a2d2a;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
        }

        .auto-style2 {
            display: block;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            border-radius: 0;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            border: 1px solid #ccc;
            padding: 6px 12px;
            background-color: #fff;
            background-image: none;
        }
    </style>


    <style type="text/css">
        /* On screens that are 600px wide or less, the background color is olive */
        @media screen and (max-width: 600px) {
            #MainContent_btnSSCHSC {
                font-size: smaller;
                margin-bottom: 5px;
            }

            .captcha {
                width: 100% !important;
            }

            #MainContent_btnSubmit {
                width: 100% !important;
            }
        }
    </style>


    <script type="text/javascript">

        function onlyDotsAndNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                return true;
            }
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
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
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />

    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-primary">
                <div class="panel-heading text-center">
                    <h4>Get HSC, SSC Information From Education Board Database</h4>
                    <div class="row" style="display: none;">
                        <%--<div class="col-lg-12" style="text-align: right;">
                            <asp:Button ID="btnAdminAssignTestValue" runat="server" Text="Assign Test Value" CssClass="btn btn-danger" OnClick="btnAdminAssignTestValue_Click" />
                        </div>--%>
                    </div>
                </div>

                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server">
                        <ContentTemplate>

                            <asp:Panel runat="server">
                                <div class="panel panel-warning">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="panel panel-info">
                                                    <div class="panel-body">

                                                        <div class="row">
                                                            <%--======================== Examination ========================--%>
                                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                                <div class="form-group">
                                                                    <label>Examination<span class="spanAsterisk">*</span></label>
                                                                    <asp:DropDownList ID="ddlExamType" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlExamType_SelectedIndexChanged">
                                                                        <asp:ListItem Enabled="true" Text="--Select Exam--" Value="-1"></asp:ListItem>
                                                                        <asp:ListItem Text="SSC, SSC (Vocational), Dakhil" Value="ssc" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="HSC, HSC (Vocational), Alim" Value="hsc"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                                        ControlToValidate="ddlExamType" ErrorMessage="Required"
                                                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                            <%--======================== END Examination ========================--%>
                                                            <%--========================  Roll ========================--%>
                                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                                <div class="form-group">
                                                                    <label>Roll Number<span class="spanAsterisk">*</span></label>
                                                                    <asp:TextBox ID="txtRoll" runat="server" Width="100%" CssClass="form-control" type="number" AutoPostBack="true" OnTextChanged="txtRoll_TextChanged">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server"
                                                                        ID="txtRollValidator"
                                                                        ValidationGroup="SUBMITVerifyInformation"
                                                                        ControlToValidate="txtRoll"
                                                                        Display="Dynamic"
                                                                        Font-Size="9pt"
                                                                        ForeColor="Crimson"
                                                                        Font-Bold="false"
                                                                        ErrorMessage="Required" />
                                                                </div>
                                                            </div>
                                                            <%--======================== END Roll ========================--%>

                                                            <%--======================== Reg. Number ========================--%>
                                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                                <div class="form-group">
                                                                    <label>Reg. Number<span class="spanAsterisk">*</span></label>
                                                                    <asp:TextBox ID="txtReg" runat="server" Width="100%" CssClass="form-control" type="number" AutoPostBack="true" OnTextChanged="txtReg_TextChanged"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server"
                                                                        ID="RequiredFieldValidator10"
                                                                        ValidationGroup="SUBMITVerifyInformation"
                                                                        ControlToValidate="txtReg"
                                                                        Display="Dynamic"
                                                                        Font-Size="9pt"
                                                                        ForeColor="Crimson"
                                                                        Font-Bold="false"
                                                                        ErrorMessage="Required" />
                                                                </div>
                                                            </div>
                                                            <%--======================== END Reg. Number ========================--%>

                                                            <%--======================== Year ========================--%>
                                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                                <div class="form-group">
                                                                    <label>Passing Year<span class="spanAsterisk">*</span></label>
                                                                    <asp:DropDownList ID="ddlPassYear" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPassYear_SelectedIndexChanged"></asp:DropDownList>
                                                                    <asp:CompareValidator ID="PassYearvalidator" runat="server"
                                                                        ControlToValidate="ddlPassYear" ErrorMessage="Required"
                                                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>

                                                                </div>
                                                            </div>
                                                            <%--======================== END Year ========================--%>

                                                            <%--======================== Board ========================--%>
                                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                                <div class="form-group">
                                                                    <label>Board<span class="spanAsterisk">*</span></label>
                                                                    <asp:DropDownList ID="ddlBoard" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBoard_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:CompareValidator ID="Boardvalidator" runat="server"
                                                                        ControlToValidate="ddlBoard" ErrorMessage="Required"
                                                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                            <%--======================== END Board ========================--%>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-4">
                                            <div class="col-lg-10 col-md-10 col-sm-10">
                                                <div class="alert alert-danger">
                                                    <p>You can modify the data through this interface only before applying the application. Any changes made after application will not take effect.
                                                        <br />If you update the basic information then please update for both SSC and HSC.
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                <asp:Button ID="btnVerifyInformation" runat="server" Text="Get Data"
                                                    class="btn btn-danger pull-right form-control" ValidationGroup="SUBMITVerifyInformation" OnClientClick="this.style.display = 'none'" OnClick="btnVerifyInformation_Click" />
                                            </div>
                                        </div>



                                    </div>
                                </div>

                                <div class="panel panel-primary" style="margin-top: 10px" runat="server" id="panelInformation">
                                    <div class="panel-body">


                                        <div class="panel panel-success" style="margin-top: 10px">
                                            <div class="panel-body">
                                                <h3 style="color: blue">Basic Information</h3>

                                                <div class="row">
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Student's Name</b>
                                                        <asp:TextBox ID="txtName" CssClass="form-control" runat="server" Text=""></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Father's Name</b>
                                                        <asp:TextBox ID="txtFatherName" CssClass="form-control" runat="server" Text=""></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Mother's Name</b>
                                                        <asp:TextBox ID="txtMotherName" CssClass="form-control" runat="server" Text=""></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Gender</b>
                                                        <asp:DropDownList ID="ddlGender" CssClass="form-control" runat="server">
                                                            <asp:ListItem Value="MALE">MALE</asp:ListItem>
                                                            <asp:ListItem Value="FEMALE">FEMALE</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Group</b>
                                                        <asp:TextBox ID="txtGroup" CssClass="form-control" runat="server" Text=""></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>GPA</b>
                                                        <asp:TextBox ID="txtGPA" CssClass="form-control" TextMode="Number" step=".01" runat="server" Text=""></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="margin-top: 10px">

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Result</b>
                                                        <asp:TextBox ID="txtResult" CssClass="form-control" runat="server" Text=""></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Total Marks</b>
                                                        <asp:TextBox ID="txtTotalMarks" CssClass="form-control" TextMode="Number" step=".01" runat="server" Text=""></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Obtained Marks</b>
                                                        <asp:TextBox ID="txtObtained" CssClass="form-control" TextMode="Number" runat="server" step=".01" Text=""></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <br />

                                                        <%--Create a button to update the basic info--%>
                                                        <asp:Button ID="btnUpdateBasicInfo" runat="server" Text="Update Info" OnClientClick="this.style.display = 'none'"
                                                            class="btn btn-success" OnClick="btnUpdateBasicInfo_Click" />
                                                    </div>

                                                </div>



                                            </div>
                                        </div>

                                        <div class="panel panel-success" style="margin-top: 10px">
                                            <div class="panel-body">
                                                <h3 style="color: blue">Subject Wise Result Information</h3>

                                                <asp:GridView runat="server" ID="gvSubjectResult" AutoGenerateColumns="False" AllowPaging="false"
                                                    PagerSettings-Mode="NumericFirstLast" Width="100%"
                                                    PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                                                    ShowHeader="true" CssClass="table table-bordered">
                                                    <HeaderStyle BackColor="#0D2D62" ForeColor="White" />
                                                    <RowStyle BackColor="#ecf0f0" />
                                                    <AlternatingRowStyle BackColor="#ffffff" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL#">
                                                            <ItemTemplate>
                                                                <b><%# Container.DataItemIndex + 1 %></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Subject Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubectCode" runat="server" Text='<%#Eval("subCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Subject Grade">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSubjectGarde" CssClass="form-control" Text='<%#Eval("grade") %>' runat="server"></asp:TextBox>
                                                                <asp:Label ID="lblSubSeq" Text='<%#Eval("subName") %>' runat="server" Visible="false"></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <%--add a button to update subject wise grade--%>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnUpdateSubjectGrade" runat="server" Text="Update Grade" OnClientClick="this.style.display = 'none'"
                                                                    class="btn btn-primary" OnClick="btnUpdateSubjectGrade_Click" CommandArgument='<%# Eval("subCode") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                    </Columns>
                                                </asp:GridView>


                                            </div>
                                        </div>

                                    </div>
                                </div>


                            </asp:Panel>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanelAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSubmit" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnSubmit" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
