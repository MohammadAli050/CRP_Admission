<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CertificatePurchaseForm.aspx.cs" Inherits="Admission.Admission.CertificateCandidate.CertificatePurchaseForm" %>







<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">


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
        .auto-style1 {
            height: 32px;
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4>Purchase Form</h4>
                </div>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-condensed table-striped">
                        <tr>
                            <td colspan="3" style="text-align: center; font-weight: bold; color: crimson; ">
                                <span class="spanAsterisk">( * )</span> <span style="color: crimson"> Indicate required fields.</span>
                            </td>
                        </tr>
                        
                        <%--<tr id="hideCalculater"  runat = "server" > 
                            <td colspan="3" style="text-align: left; font-weight: bold; color: crimson;" >
                                For English Medium Students please Calculate the Average Grade Points : <asp:button runat="server" Text = "O/A-Level Conversion" onclick="btnShowPopupClicked" ID="btnsPopup" class="btn btn-info"/>
                            </td>
                        </tr>--%>



                        <tr>
                            <td colspan="3" style="text-align: left; font-weight: bold; color: orangered">
                                Please note that this is not the final application. Candidate will be able to access application form after successful payment.
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Name <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td style="width: 50%">
                                <asp:RequiredFieldValidator runat="server"
                                    ID="NameReq"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtName"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Name Required" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Date Of Birth <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtDateOfBirth" runat="server" Width="50%" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                            </td>
                            <td style="width: 50%">
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                    TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" Animated="true" />
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator4"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtDateOfBirth"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Date of Birth Required" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Email <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control"
                                    TextMode="Email"></asp:TextBox>
                                <span style="color: darkorange; font-size: 9pt;">Please provide a valid email address.
                                    Candidate will not receive any email containing financial or payment information.
                                </span>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator1"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtEmail"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Email Required" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">SMS Mobile No. <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtSmsMobile" runat="server" Width="100%" CssClass="form-control" placeholder="+88017XXXXXXXX"></asp:TextBox>
                                <span style="color: darkorange; font-size: 9pt;">Please include country code, eg: +8801700000000. 
                                    Candidate will not recieve Username and Password 
                                    if number is in wrong format.
                                </span>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator2"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtSmsMobile"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="SMS Mobile Number Required" />
                                <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                    ValidationGroup="SUBMIT"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Bold="False"
                                    ErrorMessage="Invalid format"
                                    ControlToValidate="txtSmsMobile"
                                    ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Guardian Mobile No. <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtGuardianMobile" runat="server" Width="100%" CssClass="form-control" placeholder="+88017XXXXXXXX"></asp:TextBox>
                                <span style="color: darkorange; font-size: 9pt;">Please include country code, eg: +8801700000000. 
                                    Guardian will not recieve information if number is 
                                    in wrong format.
                                </span>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator3"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtGuardianMobile"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Guardian Mobile Number Required" />
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                    ValidationGroup="SUBMIT"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Bold="False"
                                    ErrorMessage="Invalid format"
                                    ControlToValidate="txtGuardianMobile"
                                    ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style_td">Gender <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlGender" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:CompareValidator ID="ddlGenderComV" runat="server"
                                    ControlToValidate="ddlGender" ErrorMessage="Gender required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Quota <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlQuota" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:CompareValidator ID="ddlQuotaComV" runat="server"
                                    ControlToValidate="ddlQuota" ErrorMessage="Quota Required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </td>
                        </tr>--%>
                        <%--<tr id="hidePassingYear"  runat = "server">
                            <td class="style_td">HSC/A-Level or Equivalent Passing Year <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlPassingYear" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:CompareValidator ID="ddlPassingYearComV" runat="server"
                                    ControlToValidate="ddlPassingYear" ErrorMessage="Passing Year Required."
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </td>
                        </tr>--%>

                        

                    </table>
                    


                    <table style="width: 100%">
                        <tr>
                            <td>
                                <img runat="server" id="imgCtrl" />&nbsp;
                                <asp:ImageButton ID="btnReLoadCaptcha" runat="server"
                                    Height="30"
                                    Width="30"
                                    ToolTip="Reload captcha"
                                    ImageUrl="~/Images/AppImg/reload6.png" />
                                <br />
                                <asp:Panel ID="captchaMsg" runat="server" Visible="false">
                                    <asp:Label ID="lblCaptcha" runat="server"
                                        CssClass="text-warning"
                                        Text="Sorry your text and image didn't match. Please try again."></asp:Label>
                                </asp:Panel>
                                <br />
                                <label>Enter the code shown.</label>
                                <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control"
                                    Width="25%"></asp:TextBox>
                                <br />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="2" class="auto-style1">
                                <asp:UpdatePanel ID="updatePanel_EligibleMessage" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblEligibleMsg" runat="server" Font-Bold="true"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" Text="Next"
                                    CssClass="btn btn-primary" ValidationGroup="SUBMIT"
                                    OnClick="btnSubmit_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </table>


                </div>
            </div>
            <%-- ROW --%>
        </div>
    </div>
</asp:Content>
