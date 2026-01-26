<%@ Page Title="Office Edit - Application Priority" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationPriority.aspx.cs" Inherits="Admission.Admission.PostgraduateDiploma.PDCandidateInfo.CandApplicationPriority" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../../Content/ApplicationForm.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li><asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <li class="active"><asp:HyperLink ID="hrefAppPriority" runat="server" Enabled="false">Program Priority</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppEducation" runat="server">Education</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAddress" runat="server">Address</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <%--<li><asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>--%>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">
                    
                    <asp:UpdatePanel ID="UpdatePanel_GridView" runat="server">
                        <ContentTemplate>

                            <asp:Panel ID="Panel1" runat="server" CssClass="alert alert-info">
                                <ul>
                                    <li>First select <strong>ALL</strong> the priorities/choices. Make sure that there is no duplicate priority/choice.</li>
                                    <li>Then click <strong>ALL</strong> 'Save' buttons on the left.</li>
                                </ul>
                            </asp:Panel>


                            <asp:Panel ID="Panel_Master" runat="server" >
                                <asp:Label ID="lblMessage_Masters" runat="server"></asp:Label>
                            </asp:Panel>
                            <asp:Panel ID="Panel_Message" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>


                            <asp:Panel ID="Panel_GridView" runat="server">

                                <asp:GridView ID="gvProgramPriority" runat="server" CssClass="table table-responsive table-hover"
                                    AutoGenerateColumns="false" GridLines="none" Width="100%"
                                    OnRowDataBound="gvProgramPriority_RowDataBound"
                                    OnRowCommand="gvProgramPriority_RowCommand">
                                    <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Program Name" HeaderStyle-Width="40%">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblProgramName"
                                                    Text='<%#Eval("admUnitProgramName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblProgramNameShort"
                                                    Text='<%#Eval("admUnitProgShortCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Priority" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlPriority" runat="server" Width="50%"></asp:DropDownList>
                                                <%--<asp:CompareValidator ID="ddlPriorityComV" runat="server"
                                                    ControlToValidate="ddlPriority" ErrorMessage="Required" ForeColor="Crimson"
                                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>--%>
                                                <asp:Label ID="lblDdlPriorityMsg" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                
                                                <asp:Label ID="lblProgPriorId" runat="server" Text='<%#Eval("progPriorID") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorCandidateId" runat="server" Text='<%#Eval("progPriorCandidateID") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorAdmUnitID" runat="server" Text='<%#Eval("progPriorAdmUnitID") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorAdmUnitProgID" runat="server" Text='<%#Eval("progPriorAdmUnitProgID") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorAdmSetupID" runat="server" Text='<%#Eval("progPriorAdmSetupID") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorBatchID" runat="server" Text='<%#Eval("progPriorBatchID") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorAcaCalID" runat="server" Text='<%#Eval("progPriorAcaCalID") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorProgramID" runat="server" Text='<%#Eval("progPriorProgramID") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorProgramName" runat="server" Text='<%#Eval("progPriorProgramName") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorShortName" runat="server" Text='<%#Eval("progPriogShortName") %>'></asp:Label>
                                                <asp:Label ID="lblProgPriorPriority" runat="server" Text='<%#Eval("progPriorPriority") %>'></asp:Label>

                                                <asp:Label ID="lblAdmSetupID" runat="server" Text='<%#Eval("admSetupID") %>'></asp:Label>
                                                <asp:Label ID="lblAdmSetupAcaCalID" runat="server" Text='<%#Eval("admSetupAcaCalID") %>'></asp:Label>

                                                <asp:Label ID="lblAdmUnitID" runat="server" Text='<%#Eval("admUnitID") %>'></asp:Label>

                                                <asp:Label ID="lblAdmUnitProgID" runat="server" Text='<%#Eval("admUnitProgID") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgAcaCalID" runat="server" Text='<%#Eval("admUnitProgAcaCalID") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgAdmUnitID" runat="server" Text='<%#Eval("admUnitProgAdmUnitID") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgProgramID" runat="server" Text='<%#Eval("admUnitProgProgramID") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgEduCatId" runat="server" Text='<%#Eval("admUnitProgEduCatId") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgBatchID" runat="server" Text='<%#Eval("admUnitProgBatchID") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgBatchName" runat="server" Text='<%#Eval("admUnitProgBatchName") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgSemesterCode" runat="server" Text='<%#Eval("admUnitProgSemesterCode") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgBatchCode" runat="server" Text='<%#Eval("admUnitProgBatchCode") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgProgramCode" runat="server" Text='<%#Eval("admUnitProgramCode") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgProgramName" runat="server" Text='<%#Eval("admUnitProgramName") %>'></asp:Label>
                                                <asp:Label ID="lblAdmUnitProgShortCode" runat="server" Text='<%#Eval("admUnitProgShortCode") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Button ID="btnSaveIndividual" runat="server" CssClass="btn btn-default" Text="Save" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:Button ID="btnSave_Priority" runat="server" Text="Save Priority Info"
                        CssClass="btn btn-primary" OnClick="btnSave_Priority_Click" Visible="false" />

                    <%--<asp:Button ID="btnNext" runat="server" Text="Next"
                        CssClass="btn btn-primary" OnClick="btnNext_Click" />--%>
                    

                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>


</asp:Content>
