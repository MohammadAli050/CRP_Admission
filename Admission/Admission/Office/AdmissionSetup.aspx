<%@ Page Title="Admission Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionSetup.aspx.cs" Inherits="Admission.Admission.Office.AdmissionSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
    <style>
        .card {
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 20px;
            border: 1px solid #ddd;
            background: #fff;
        }

        .card-header {
            background-color: #f8f9fa;
            border-bottom: 1px solid #ddd;
            padding: 12px 20px;
        }

            .card-header h4 {
                margin: 0;
                font-size: 1.2rem;
                font-weight: bold;
                color: #333;
            }

        .form-label {
            font-weight: bold;
            margin-bottom: 5px;
            display: block;
            font-size: 13px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .control-style {
            width: 100%;
            padding: 6px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <asp:Panel ID="messagePanel" runat="server">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </asp:Panel>

        <div class="card">
            <div class="card-header">
                <h4>Admission Exam Setup</h4>
            </div>
            <div class="card-body" style="padding: 20px;">
                <div class="row">
                    <div class="col-md-3 form-group">
                        <label class="form-label">Subject</label>
                        <asp:DropDownList ID="ddlAdmissionUnit" runat="server" CssClass="control-style"></asp:DropDownList>
                        <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server" ControlToValidate="ddlAdmissionUnit" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1" Display="Dynamic" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label class="form-label">bKash Number</label>
                        <asp:TextBox ID="txtBkashNumber" runat="server" CssClass="control-style" placeholder="Merchant No."></asp:TextBox>
                        <small class="text-muted">Empty = hide 'Pay By bKash'</small>
                    </div>
                    <div class="col-md-3 form-group">
                        <label class="form-label">Gateway</label>
                        <asp:DropDownList ID="ddlGateway" runat="server" CssClass="control-style" OnSelectedIndexChanged="ddlGateway_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <asp:CompareValidator ID="ddlGatewayComV" runat="server" ControlToValidate="ddlGateway" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1" Display="Dynamic" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label class="form-label">Store</label>
                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="control-style"></asp:DropDownList>
                        <asp:CompareValidator ID="ddlStoreCompare" runat="server" ControlToValidate="ddlStore" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1" Display="Dynamic" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3 form-group">
                        <label class="form-label">Start Date</label>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="control-style"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" Format="dd/MM/yyyy" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="gr1" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label class="form-label">End Date</label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="control-style"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" Format="dd/MM/yyyy" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="gr1" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label class="form-label">Category</label>
                        <asp:DropDownList ID="ddlEducationCategory" runat="server" CssClass="control-style"></asp:DropDownList>
                        <asp:CompareValidator ID="ddlEducationCategoryComV" runat="server" ControlToValidate="ddlEducationCategory" ErrorMessage="*" ForeColor="Red" ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1" Display="Dynamic" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label class="form-label">Session</label>
                        <asp:DropDownList ID="ddlSession" runat="server" CssClass="control-style"></asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2 form-group">
                        <label class="form-label">Fee</label>
                        <asp:TextBox ID="txtFee" runat="server" CssClass="control-style" TextMode="Number"></asp:TextBox>
                    </div>
                    <div class="col-md-2 form-group">
                        <label class="form-label">Test Number</label>
                        <asp:TextBox ID="txtTestNumber" runat="server" CssClass="control-style" TextMode="Number"></asp:TextBox>
                    </div>
                    <div class="col-md-4 form-group">
                        <label class="form-label">Exam Schedule</label>
                        <div style="display: flex; gap: 5px;">
                            <asp:TextBox ID="txtExamDate" runat="server" CssClass="control-style"></asp:TextBox>
                            <asp:DropDownList ID="ddlHour" runat="server" CssClass="control-style" Width="70px"></asp:DropDownList>
                            <asp:DropDownList ID="ddlMinute" runat="server" CssClass="control-style" Width="70px"></asp:DropDownList>
                            <asp:DropDownList ID="ddlAmPm" runat="server" CssClass="control-style" Width="80px"></asp:DropDownList>
                        </div>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtExamDate" Format="dd/MM/yyyy" />
                    </div>
                    <div class="col-md-4 form-group">
                        <label class="form-label">Viva Schedule</label>
                        <div style="display: flex; gap: 5px;">
                            <asp:TextBox ID="txtVivaDate" runat="server" CssClass="control-style"></asp:TextBox>
                            <asp:DropDownList ID="ddlHourViva" runat="server" CssClass="control-style" Width="70px"></asp:DropDownList>
                            <asp:DropDownList ID="ddlMinuteViva" runat="server" CssClass="control-style" Width="70px"></asp:DropDownList>
                            <asp:DropDownList ID="ddlAmPmViva" runat="server" CssClass="control-style" Width="80px"></asp:DropDownList>
                        </div>
                        <ajaxToolkit:CalendarExtender ID="txtVivaDateCalendarExtender" runat="server" TargetControlID="txtVivaDate" Format="dd/MM/yyyy" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:CheckBox ID="ckbxIsActive" runat="server" Text="&nbsp;Is Active" />
                        <hr />
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1" CssClass="btn btn-success" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-default" OnClick="btnClear_Click" Visible="false" />
                        <asp:Button ID="btnInActiveAll" runat="server" Text="In-Active All" CssClass="btn btn-danger" Style="float: right;" OnClientClick="return confirm('Are you sure?')" OnClick="btnInActiveAll_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="card">
            <div class="card-header bg-primary" style="color: white;">
                Opened Admissions. Records:
                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
            </div>
            <div style="overflow-x: auto;">
                <asp:ListView ID="lvAdmSetup" runat="server" OnItemDataBound="lvAdmSetup_ItemDataBound" OnItemCommand="lvAdmSetup_ItemCommand" OnItemDeleting="lvAdmSetup_ItemDeleting" OnItemUpdating="lvAdmSetup_ItemUpdating">
                    <LayoutTemplate>
                        <table class="table table-bordered table-striped" style="margin-bottom: 0; font-size: 12px;">
                            <tr style="background-color: #1387de; color: white;">
                                <th>SL#</th>
                                <th>School</th>
                                <th>Session</th>
                                <th>Dates</th>
                                <th>Store</th>
                                <th>Level</th>
                                <th>Fee</th>
                                <th>Test#</th>
                                <th>Exam</th>
                                <th>Viva</th>
                                <th>Active</th>
                                <th>Actions</th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" />
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lblSerial" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblAdmissionUnit" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblSession" runat="server" /></td>
                            <td>
                                <b>S:</b>
                                <asp:Label ID="lblStartDate" runat="server" /><br />
                                <b>E:</b>
                                <asp:Label ID="lblEndDate" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblStore" runat="server" /><br />
                                <small>bK:
                                    <asp:Label ID="lblBkashStore" runat="server" /></small>
                            </td>
                            <td>
                                <asp:Label ID="lblEducationCategory" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblFee" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblTestNo" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblExamDateTime" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblVivaExamDateTime" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblIsActive" runat="server" /></td>
                            <td>
                                <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                | 
                               
                                <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return confirm('Delete?');">Delete</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>

        <div class="card">
            <div class="card-header bg-danger" style="color: white;">
                Closed Admissions. Records:
                <asp:Label ID="lblRecordInactive" runat="server" CssClass="badge"></asp:Label>
            </div>
            <div style="overflow-x: auto;">
                <asp:ListView ID="lvAdmSetupInactive" runat="server" OnItemDataBound="lvAdmSetupInactive_ItemDataBound" OnItemCommand="lvAdmSetupInactive_ItemCommand" OnItemUpdating="lvAdmSetupInactive_ItemUpdating" >
                    <LayoutTemplate>
                        <table class="table table-condensed table-bordered" style="font-size: 11px;">
                            <tr style="background-color: #eee;">
                                <th>SL#</th>
                                <th>Unit</th>
                                <th>Session</th>
                                <th>Start</th>
                                <th>End</th>
                                <th>Level</th>
                                <th>Fee</th>
                                <th>Test#</th>
                                <th>Exam</th>
                                <th>Time</th>
                                <th>Active</th>
                                <th>Actions</th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" />
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lblSerialIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblAdmissionUnitIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblSessionIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblStartDateIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblEndDateIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblEducationCategoryIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblFeeIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblTestNoIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblExamDateIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblExamTimeIn" runat="server" /></td>
                            <td>
                                <asp:Label ID="lblIsActiveIn" runat="server" /></td>
                            <td>
    <asp:LinkButton ID="lnkEdit" runat="server" 
        CommandName="Update" 
        CommandArgument='<%# Eval("ID") %>'>Edit</asp:LinkButton>
</td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>
</asp:Content>
