<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="finalsubmitPage.aspx.cs" Inherits="Admission.Admission.Candidate.finalsubmitPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
        .container {
            box-sizing: border-box;
            border: 0.1px solid #80808059;
            box-shadow: 2px;
            
        }

        .InputInfo {
            width: 100%;
            height: 100px;
            background-color: whitesmoke;
            text-align: center;
            font-size: 18pt;
            margin-left: 0%;
            margin-bottom: 3.3%;
            margin-top: 10px;
            padding-top: 10px;
        }

        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12">
                <div class="InputInfo">
                    <p style="color: green;" 
                runat="server">
               <b>Congratulations!</b> &nbsp;  </p>               
                    <p style="color: green;" 
                runat="server">
                <b>Your Payment is Successful!</b> &nbsp;  </p>  
                </div>
            </div>
         

            <div class="col-sm-12 col-md-12 col-lg-12">
                  <div class="button" style="margin-top:-14px;margin-left:450px;">
                                <asp:HyperLink ID="HyperLink1" style="border-radius:4px; text-align:center;" NavigateUrl="~/Admission/Candidate/PersonalInfoAppForm.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-lg btn-danger">
                                      Click To Final Submit &nbsp;
                                </asp:HyperLink>
                  </div>
            </div>
        </div>
        
    </div>
</asp:Content>
