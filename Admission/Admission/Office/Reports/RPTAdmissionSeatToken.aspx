<%@ Page Title="Admission Seat Token" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTAdmissionSeatToken.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTAdmissionSeatToken" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Admission Seat Token</h3>
                    <hr style="margin-top: 0px; margin-bottom: 10px;" />
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-5 col-md-5 col-lg-5">
                            <div class="form-group">
                                <label><strong>Faculty <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="100%" CssClass="form-control"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlAdmUnit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <br />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100%" CssClass="btn btn-default"
                                OnClick="btnClear_Click" />
                        </div>
                    </div>

                    <asp:Panel ID="panelFilterLoad" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Venue</strong></label>
                                    <asp:DropDownList ID="ddlVenue" runat="server" Width="100%" CssClass="form-control"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlVenue_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Campus</strong></label>
                                    <asp:DropDownList ID="ddlCampus" runat="server" Width="100%" CssClass="form-control"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Building</strong></label>
                                    <asp:DropDownList ID="ddlBuilding" runat="server" Width="100%" CssClass="form-control"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlBuilding_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Room</strong></label>
                                    <asp:DropDownList ID="ddlRoom" runat="server" Width="100%" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Start Roll</strong></label>
                                    <asp:TextBox ID="txtStartRoll" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>End Roll</strong></label>
                                    <asp:TextBox ID="txtEndRoll" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <br />
                                <asp:Button ID="btnLoad" runat="server" Text="Load Report" Width="100%" CssClass="btn btn-info"
                                    OnClick="btnLoad_Click" />
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                            </div>
                        </div>

                    </asp:Panel>

                </div>
            </div>


            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>

            <hr />


            <div>
                <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana"
                    font-size="8pt" waitmessagefont-names="Verdana" waitmessagefont-size="14pt"
                    asyncrendering="true" width="100%" sizetoreportcontent="true" visible="true">
                    <LocalReport ReportPath="Admission/Office/Reports/RPTAdmissionSeatToken.rdlc" EnableExternalImages="true">
                    </LocalReport>
                </rsweb:reportviewer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
