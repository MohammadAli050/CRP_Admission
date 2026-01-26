<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PersonalInfoAppForm.aspx.cs" Inherits="Admission.Admission.Candidate.PersonalInfoAppForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
        #MainContent_RadioButtonList1 tr td label {
            margin-left:10px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" id="divFinalSubmit" runat="server" style="text-align:center;font-size:30px">
        <b style="color:red;font-weight:bold">You have already final submitted your application</b>
    </div>

    <div class="row" id="divMain" runat="server" style="margin-top:10px">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-info">
                <div class="panel-heading  text-center" style="background-color: black">
                    <h4 style="color: #c4bfbf">Personal Information</h4>
                </div>
                <div class="panel-body">
                    <%--======================== Full Name ========================--%>
                    <div class="form-group">

                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>First Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="NameReq"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtName"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Name Required"
                                    ValidationExpression="^[A-Za-z-._ ]*$" />
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" placeholder="Full Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Middle Name<span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="form-control" Width="100%" placeholder="Middle Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Last Name <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" Width="100%" placeholder="Last Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <div class="form-group">
                                    <label>Father's Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                    <asp:RequiredFieldValidator runat="server"
                                        ID="RequiredFieldValidator4"
                                        ValidationGroup="SUBMIT"
                                        ControlToValidate="txtName"
                                        Display="Dynamic"
                                        Font-Size="9pt"
                                        ForeColor="Crimson"
                                        Font-Bold="false"
                                        ErrorMessage="Name Required"
                                        ValidationExpression="^[A-Za-z-._ ]*$" />
                                    <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control" Width="100%" placeholder="Full Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">


                                <label>Mother's Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator5"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtName"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Name Required"
                                    ValidationExpression="^[A-Za-z-._ ]*$" />
                                <asp:TextBox ID="txtMotherName" runat="server" CssClass="form-control" Width="100%" placeholder="Full Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </div>

                        </div>
                    </div>



                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Date of Birth<span class="spanAsterisk">*</span></label>
                                <asp:TextBox ID="txtDateOfBirth" runat="server" Width="100%" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalenderExtender_DOB" runat="server"
                                    TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" />


                                <span id="txtDateOfBirthValidateMassage" runat="server" style="font-weight: bold; color: crimson;"></span>

                            </div>
                        </div>


                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Gender<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlGender" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator3" runat="server"
                                    ControlToValidate="ddlGender" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>

                    <%--======================== END Date of Birth ========================--%>
                    <%--======================== country religion ========================--%>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Country<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlCountry" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                    ControlToValidate="ddlCountry" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Religion<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlReligion" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator2" runat="server"
                                    ControlToValidate="ddlReligion" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>

                    </div>
                    <%--======================== email passport ========================--%>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">

                            <div class="form-group">
                                <label>Email<span class="spanAsterisk">*</span></label>
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control"
                                    placeholder="Email"
                                    TextMode="Email"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <span style="color: darkorange; font-size: 9pt;">Please provide a valid email address.</span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail"
                                    ValidationGroup="SUBMIT"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Please enter valid email. (Ex: xyz@zyz.com)">   
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator1"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtEmail"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Email Required" />
                            </div>

                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label><strong>Passport Number <span style="color: crimson; font-weight: bold;">*</span></strong></label>


                                <asp:TextBox ID="txtPassportNo" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtPlaceOfBirth_ReqV" runat="server"
                                    ControlToValidate="txtPassportNo"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="SUBMIT">

                                </asp:RequiredFieldValidator>

                            </div>
                        </div>
                    </div>

                    <%--phone--%>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Mobile No.(International)<span class="spanAsterisk">*</span></label>
                                <div>
                                    <%--  <div style="float: left;">
                                    <asp:TextBox ID="txtCountryCodeGuardianMobile" runat="server" CssClass="form-control" Text="+" Style="width: 54px;" ReadOnly="true"></asp:TextBox>
                                </div>--%>
                                    <div>
                                        <asp:TextBox ID="txtIntMobile" runat="server" Width="100%" type="number" CssClass="form-control" placeholder="" MaxLength="11" onkeydown="return (event.keyCode!=13);"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Mobile No.(If you have any bangladeshi Number)<span class="spanAsterisk">*</span></label>
                                <div>
                                    <%--<div style="float: left;">
                                    <asp:TextBox ID="txtSmsPhone" runat="server" CssClass="form-control" Text="+88" Style="width: 54px;" ReadOnly="true"></asp:TextBox>
                                </div>--%>
                                    <div>
                                        <asp:TextBox ID="txtPhone" runat="server" Width="100%" type="number" CssClass="form-control" placeholder="017XXXXXXXX" MaxLength="11" onkeydown="return (event.keyCode!=13);"></asp:TextBox>

                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>



                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label><strong>Corresponding Address<span style="color: crimson; font-weight: bold;">*</span></strong></label>


                                <asp:TextBox ID="txtPresentAddress" runat="server" CssClass="form-control" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtPresentAddress"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="SUBMIT">
                                </asp:RequiredFieldValidator>

                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label><strong>Permanent Address at home country<span style="color: crimson; font-weight: bold;">*</span></strong></label>

                                <asp:TextBox ID="txtParmamentAddress" runat="server" CssClass="form-control" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ControlToValidate="txtParmamentAddress"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="SUBMIT">
                                </asp:RequiredFieldValidator>

                            </div>
                        </div>
                    </div>



                    <div class="row col-lg-12 col-md-12 col-sm-12">
                        <label><strong>Source of Fund<span style="color: crimson; font-weight: bold;">*</span></strong></label>
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" style="margin-left:7px;" AutoPostBack="true">
                            <asp:ListItem Text="Private/Family" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Scholarship/Grant" Value="1"></asp:ListItem>

                        </asp:RadioButtonList>
                    </div>



                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6" id="divSc" runat="server">
                            <div class="form-group">
                                <label><strong>If Scholarship/Grant in (ii) please write the name of the donor (Use separate sheet if needed for details)<span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:TextBox ID="lbldonorname" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <div class="form-group">
                                <label><strong>Reference in Bangladesh (If any)<span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                
                                <asp:TextBox ID="lblreference" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>





                    <div class="row">
                        <div class="col-lg-9 col-md-9 col-sm-9">
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <div style="text-align: right">

                                <asp:Button ID="btnSave" ValidationGroup="SUBMIT" Style="border-radius: 4px; width: 85px;" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



        </div>
    </div>


</asp:Content>
