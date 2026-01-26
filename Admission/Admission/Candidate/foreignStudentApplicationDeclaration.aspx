<%@ Page Title="Application Form - Declaration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="foreignStudentApplicationDeclaration.aspx.cs" Inherits="Admission.Admission.Candidate.foreignStudentApplicationDeclaration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/ApplicationForm.css" rel="stylesheet" />
    <style>
        input[type="checkbox"] {
            width: 20px !important;
            height: 20px !important;
        }

        .navio {
            padding: 10px 10px !important;
        }

        @media screen and (min-width: 768px) {
            .navi {
                height: 41px !important;
                margin-bottom: 0px !important;
                min-height: auto;
            }
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" id="divFinalSubmit" runat="server" style="text-align: center; font-size: 30px">
        <b style="color: red; font-weight: bold">You have already final submitted your application</b>
    </div>

    <div class="row" id="divMain" runat="server" style="margin-top: 10px">

        <div class="col-md-12">
            <div class="panel panel-default">

                <%-- end panel heading --%>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td><strong>Declaration by the Candidate:</strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    I, do hereby 
                                    declare that all the details listed here are true and comply with the terms of the applicable announcement. I must abide by the rules and regulations of this University and refrain from any activities that tarnish the image and reputation of this University. I will not take part in any activity subversive of Bangladesh or of discipline. I understand that the submission of false or misleading information may be sufficient cause for the cancellation of studentship and legal action thereafter. 
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">
                                <div class="row">
                                    <div class="col-lg-1">
                                        <asp:CheckBox ID="chbxAgreed" runat="server" AutoPostBack="true" OnCheckedChanged="chbxAgreed_CheckedChanged" />

                                    </div>
                                    <div class="col-lg-11">
                                        <label for="MainContent_chbxAgreed" style="margin-top:7px;margin-left:-60px;font-weight:bold">I agree To the above terms and conditions.</label>

                                    </div>
                                </div>
                            </td>
                        </tr>
                        <%-- <tr>
                            <td>
                                <span style="color: crimson;font-weight: bold;font-size: 20px;">Note: After final submit, you will not be able to edit your application form. If you do not submit, you will not receive your admit card.</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br /><asp:Label ID="lblMessage" runat="server" ForeColor="Crimson"></asp:Label>
                            </td>
                        </tr>--%>
                    </table>


                    <div class="row">
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button ID="btnBack" runat="server" Text="Back"
                                CssClass="btn btn-primary form-control" OnClick="btnBack_Click" />

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="text-align: center">
                            <asp:Button ID="btnPreview" runat="server" Text="Preview All Information"
                                CssClass="btn btn-info form-control" OnClick="btnPreview_Click" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnPayOnline" runat="server" Text="Payment Information"
                                CssClass="btn btn-warning form-control" BackColor="#ff5400" OnClick="btnPayOnline_Click" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button ID="btnFinalSubmit" runat="server" Text="Final Submit"
                                CssClass="btn btn-success form-control" OnClick="btnFinalSubmit_Click1" />
                        </div>
                    </div>



                </div>

            </div>

        </div>

    </div>



    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="Button1" PopupControlID="Panel2"
                    BackgroundCssClass="modalBackground" CancelControlID="Button2">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel2" Style="display: none; padding: 5px;" BackColor="White" Width="30%">


                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="row col-lg-12 col-md-12 col-sm-12" style="text-align: center; color: blue; font-weight: bold">
                                <b>Are you sure you want to submit your Application? After the Final Submission, You can not make any changes anymore!</b>
                            </div>
                            <hr />


                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <asp:Button runat="server" ID="btnRequestConfirm" Text="Confirm" OnClick="btnFinalSubmitConfirm_Click" CssClass="btn-info btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />

                        </div>
                        <div class="col-lg-1 col-md-1 col-sm-1">
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5">
                            <asp:Button runat="server" ID="Button2" Text="Cancel" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>


                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button3" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button3" PopupControlID="Panel3"
                    BackgroundCssClass="modalBackground" CancelControlID="Button5">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel3" Style="display: none; padding: 5px;" BackColor="White" Width="40%">


                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="row col-lg-12 col-md-12 col-sm-12" style="text-align: center; font-weight: bold">
                                <asp:Label ID="Label1" runat="server" Text="Final Submission Failed! Before the Final Submission, You Need To Update Below Information" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:Label ID="lblNotGivenField" runat="server" Text="" ForeColor="Blue"></asp:Label>
                            </div>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" ID="Button5" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        </div>
                    </div>


                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
