<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="foreignStudentInputInfo.aspx.cs" Inherits="Admission.Admission.Candidate.foreignStudentInputInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
        .container {
            box-sizing: border-box;
            border: 0.1px solid #80808059;
            box-shadow: 2px;
            
        }

        .InputInfo {
            width: 100%;
            height: 55px;
            background-color: aliceblue;
            text-align: center;
            font-size: 18pt;
            margin-left: 0%;
            margin-bottom: 3.3%;
            margin-top: 10px;
            padding-top: 10px;
        }

        .button {
        }

        .blink {
            animation: blinker 0.6s linear infinite;
            color: #1c87c9;
            font-size: 30px;
            font-weight: bold;
            font-family: sans-serif;
        }
        
        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }

        @media only screen and (max-width: 600px){
            .InputInfo{
                width:100%;
                font-size: 11pt;
            }

        }
        @media only screen and (max-width: 600px){
            #buttonApply{
                margin-top: -54px;
            }

        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default" style="margin-top:10px;">
    <div class="panel-body">
        <div class="form-group">

        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12">
                <div class="InputInfo">
                       <p runat="server">
                            
                           <b>Apply to Undergraduate Program for Foreign Students</b>  &nbsp;                 
                        </p>
            <%--   NavigateUrl="~/Admission/Candidate/MassageAfterAdmissionClosed.aspx" --%>
                </div>
            </div>
         </div>

        <div class="row"">
            <div class="col-sm-4 col-md-4 col-lg-4">
                <label>First Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                <asp:RequiredFieldValidator ID="CompareValidator4" runat="server"
                    ControlToValidate="txtName" ErrorMessage="Required" Font-Size="15px" Font-Bold="true"
                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="First Name"></asp:TextBox>
                
            </div>
            <div class="col-sm-4 col-md-4 col-lg-4">
                
                <label>Middle Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                    ControlToValidate="txtNamem" ErrorMessage="Required" Font-Size="15px" Font-Bold="true"
                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtNamem" runat="server" CssClass="form-control" placeholder="Middle Name"></asp:TextBox>
                
            </div>
           <div class="col-sm-4 col-md-4 col-lg-4">
                
                <label>Last Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                    ControlToValidate="TextBoxl" ErrorMessage="Required" Font-Size="15px" Font-Bold="true"
                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                <asp:TextBox ID="TextBoxl" runat="server" CssClass="form-control" placeholder="Last Name"></asp:TextBox>
                
            </div>

        </div>
        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12">
                
                <label>Email<span class="spanAsterisk">*</span></label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                    ControlToValidate="txtEmail" ErrorMessage="Required" Font-Size="15px" Font-Bold="true"
                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtEmail" Font-Size="15px" runat="server" ForeColor="Red" Display="Dynamic" CssClass="blink" ErrorMessage="Enter valid email" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"></asp:RegularExpressionValidator>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                    
            </div>
        </div>

    </div>
    </div>
    </div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <asp:Label ID="lblmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            </div>
        </div>

        <div class="row">
            
            <div class="col-lg-6 col-md-6 col-sm-6" style="text-align:left;">
                    <div class="button">
                        <asp:HyperLink ID="HyperLink1" style="border-radius:4px; margin-top: 14px; background-color: #000000a6; color: white;" NavigateUrl="~/Admission/Candidate/foreignStudentProgramSelection.aspx?ecat=4"
                                runat="server" CssClass="btn btn-light">
                                <span class="glyphicon glyphicon-chevron-left"></span> Back &nbsp;
                        </asp:HyperLink>
                    </div>
           </div>

            <div class="col-lg-6 col-md-6 col-sm-6" style="text-align:right;">
        
                <div class="button">
                    <asp:Button ID="buttonApply" ValidationGroup="VG1" runat="server" Text="Apply" Style="width: 85px; margin-top: 14px; border-radius:4px;" OnClick="buttonApply_Click" CssClass="btn btn-success"></asp:Button>
                </div>
            </div>

       </div>
    

</asp:Content>
