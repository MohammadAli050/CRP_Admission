<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MultipleApplication.aspx.cs" Inherits="Admission.Admission.Candidate.MultipleApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">

            <asp:Panel ID="panelIfAllowed" runat="server">

                <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

                <asp:UpdatePanel ID="updatePanel" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>

                        <asp:GridView ID="gvMultipleApplications" runat="server" CssClass="table table-responsive table-condensed table-hover"
                            AutoGenerateColumns="false" GridLines="None" Width="100%"
                            OnRowDataBound="gvMultipleApplications_RowDataBound">

                            <HeaderStyle BackColor="#1387de" ForeColor="White" />

                            <Columns>

                                <asp:TemplateField HeaderText="SL#" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                    <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckbxSelectedSchool" runat="server"
                                            AutoPostBack="true"
                                            OnCheckedChanged="ckbxSelectedSchool_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="School/Faculty Name" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSchoolName" runat="server" Text='<%#Eval("admUnitName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("admSetupEducationCategory") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exam Date" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblExamDate" runat="server" Text='<%#Eval("admSetupExamDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ExamTime" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblExamTime" runat="server" Text='<%#Eval("admSetupExamTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Viva Date" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblVivaDate" runat="server" Text='<%#Eval("admSetupVivaDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Viva Time" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblVivaTime" runat="server" Text='<%#Eval("admSetupVivaTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fee" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFee" runat="server" Text='<%#Eval("admSetupFee") %>'></asp:Label> (+ Charge)
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="hfasi" runat="server" Text='<%#Eval("admSetupID") %>' />
                                        <asp:Label ID="hfaui" runat="server" Text='<%#Eval("admSetupAdmUnitID") %>' />
                                        <asp:Label ID="hfaci" runat="server" Text='<%#Eval("admSetupAcaCalId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>

                        <div class="panel panel-default">
                            <div class="panel-body">
                                <div>
                                    <strong> Number of Selected Faculty(or Faculties):</strong>
                                    <asp:Label ID="lblNoOfSelSchool" runat="server" Text="0" style="color: crimson;font-size: 16px;"></asp:Label>
                                </div>
                                <div>
                                    <strong>Total:</strong>
                                    <asp:Label ID="lblTotal" runat="server" Text="0" style="color: crimson;font-size: 16px;"></asp:Label>
                                </div>
                                <div>
                                    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary"
                                        OnClick="btnNext_Click" style="margin-top: 20px;padding: 5px 40px;"/>
                                </div>
                            </div>
                        </div>

                        <div>
                            <asp:Panel ID="messagePanel" runat="server" Visible="false">
                                <asp:Label ID="lblMessage1" runat="server"></asp:Label>
                            </asp:Panel>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </asp:Panel>
        </div>
    </div>


</asp:Content>
