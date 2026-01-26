<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="academicInfoAppForm.aspx.cs" Inherits="Admission.Admission.Candidate.academicInfoAppForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
    .header-center{
        padding:5px;
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
                    <h4 style="color: #c4bfbf">Academic Information</h4>
                </div>
                <div class="panel-body">

                    
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Session<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionCompare" runat="server"
                                    ControlToValidate="ddlSession" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Name of Desired Faculty<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmissionUnit_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                    ControlToValidate="ddlAdmissionUnit" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Name of Desired Program<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlProgram" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlProgramCompare" runat="server"
                                    ControlToValidate="ddlProgram" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <div class="form-group">

                                <asp:Button ID="btnAddProgram" ValidationGroup="SUBMIT" Style="border-radius: 4px;" runat="server" Text="Add Program" OnClick="btnAddProgram_Click" CssClass="btn btn-primary"></asp:Button>


                            </div>
                        </div>
                    </div>


                    <div class="panel panel-default" style="margin-top: 20px">
                        <div class="panel-body">
                            <asp:GridView runat="server" ID="GridViewProgramList" AutoGenerateColumns="False" AllowPaging="false" GridLines="None"
                                PagerSettings-Mode="NumericFirstLast" Width="100%"
                                PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                                ShowHeader="true">
                                <HeaderStyle BackColor="#91CDE0" ForeColor="Black" />
                                <RowStyle BackColor="#ecf0f0" />
                                <AlternatingRowStyle BackColor="#ffffff" />
                                <Columns>

                                    <asp:TemplateField HeaderText="SL" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:Label runat="server" ID="lblSL" Text='<%#Eval("CreatedBy") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Program Name" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:Label runat="server" ID="lblName" Text='<%#Eval("ProgramName") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Priority" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:Label runat="server" ID="lblPriority" Text='<%#Eval("Priority") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Session" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:Label runat="server" ID="lblSession" Text='<%#Eval("Attribute1") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <%--  <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="padding: 5px; text-align: center">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn-info btn-sm" CommandArgument='<%# Eval("ID") %>' OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="padding: 5px; text-align: center">
                                                <asp:LinkButton ID="lnkRemove" runat="server" CssClass="btn-danger btn-sm" CommandArgument='<%# Eval("ID") %>' OnClick="lnkRemove_Click" OnClientClick="return confirm('Are you sure you want to delete?');">Remove</asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />

                            </asp:GridView>

                        </div>
                    </div>


                    <div class="panel panel-default" style="margin-top: 20px">
                        <div class="panel-body">
                            <asp:GridView runat="server" ID="gvEducationList" AutoGenerateColumns="False" AllowPaging="false" GridLines="None" OnRowDataBound="gvEducationList_RowDataBound"
                                PagerSettings-Mode="NumericFirstLast" Width="100%"
                                PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                                ShowHeader="true">
                                <HeaderStyle BackColor="#91CDE0" ForeColor="Black"/>
                                <RowStyle BackColor="#ecf0f0" />
                                <AlternatingRowStyle BackColor="#ffffff" />
                                <Columns>

                                    <asp:TemplateField HeaderText="SL" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">

                                                <asp:Label runat="server" ID="lblSL" Text='<%#Eval("SL") %>' ForeColor="Black" Width="100%" Font-Bold="true"></asp:Label>
                                                <asp:HiddenField ID="ExamTypeId" runat="server" Value='<%#Eval("ExamId") %>' />

                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name of the Examination" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:TextBox runat="server" ID="lblExam" Text='<%#Eval("ExamName") %>' ForeColor="Black" Font-Bold="false" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Year of Passing" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:HiddenField ID="hdnYear" runat="server" Value='<%#Eval("PassingYear") %>' />

                                                <asp:DropDownList ID="ddlPassingYear" runat="server" Font-Bold="true" CssClass="form-control" Width="90%">
                                                </asp:DropDownList>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name of the Institution" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:TextBox runat="server" ID="txtInst" ForeColor="Black" Text='<%#Eval("InstituteName") %>' Font-Bold="false" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Letter Grade Obtained" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:TextBox runat="server" ID="txtLgD" ForeColor="Black" Text='<%#Eval("Grade") %>' Font-Bold="false" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Division" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">

                                                <asp:HiddenField ID="hdnDivisionId" runat="server" Value='<%#Eval("Division") %>' />

                                                <asp:DropDownList ID="ddlDvision" runat="server" Font-Bold="true" CssClass="form-control" Width="90%">
                                                    <asp:ListItem Text="Select Division" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="1st Division" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="2nd Division" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="3rd Division" Value="4"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="GPA/%Marks" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <div style="padding: 5px">
                                                <asp:TextBox runat="server" ID="txtGpa" Text='<%#Eval("GpaMarks") %>' ForeColor="Black" CssClass="form-control" Font-Bold="false"></asp:TextBox>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>

                                </Columns>
                                <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />

                            </asp:GridView>

                        </div>
                    </div>


                    <div class="row">
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <div class="button" style="margin-top: 0px;">
                                <asp:HyperLink ID="HyperLink1" Style="text-align: left; border-radius: 4px; background-color: #000000a6; color: white;" NavigateUrl="~/Admission/Candidate/PersonalInfoAppForm.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-light">
                                        <span class="glyphicon glyphicon-chevron-left"></span> Back &nbsp;
                                </asp:HyperLink>
                            </div>

                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Button ID="buttonsave" Style="margin-left: 138px; border-radius: 4px; width: 85px;" runat="server" Text="Save" OnClick="buttonsave_Click" CssClass="btn btn-primary"></asp:Button>
                        </div>
                    </div>
                </div>


            </div>
        </div>
    </div>

</asp:Content>
