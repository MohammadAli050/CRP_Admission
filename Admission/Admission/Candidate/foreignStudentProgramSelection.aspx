<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="foreignStudentProgramSelection.aspx.cs" Inherits="Admission.Admission.Candidate.foreignStudentProgramSelection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
       
    <style>

        .container {
            box-sizing: border-box;
            border: 0.1px solid #80808059;
            box-shadow: 2px;
        }


        /*@media only screen and (max-width: 600px){
            .customSelectDiv {
                width: 100%;
            }

        }*/
        .customSelectDiv{
            width: 100%;
            height: auto;
            text-align: center;
            margin-bottom: 5%;
            
            padding-bottom: 10px;
        }
     .customApplyDiv {
            width: 38%;
            height: auto;
            background-color: cornflowerblue;
            text-align: center;
            padding: 2.8%;
            font-size: 18pt;
            margin:auto;
            margin-bottom: 3.3%;
            padding-top: 10px;
            padding-bottom: 10px;
        }

        .customApplyMastersDiv {
            width: 38%;
            height: auto;
            background-color: #ad49bb;
            text-align: center;
            padding: 2.8%;
            font-size: 18pt;
            margin:auto;
            margin-bottom: 3.3%;
            padding-top: 10px;
            padding-bottom: 10px;
        }

        @media only screen and (max-width: 600px) {
            .customApplyDiv {
                width: 100%;
                height:auto;
            }
        }
        @media only screen and (max-width: 600px) {
            .customApplyMastersDiv {
                width: 100%;
                height:auto;
            }
        }

      </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default" style="margin-top:10px;">
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="customSelectDiv">
                        <h2 style="color: darkblue; font-size: 30pt; font-weight: bold; margin-top:2px; text-align: center;">International Students</h2>


                        <%--   NavigateUrl="~/Admission/Candidate/MassageAfterAdmissionClosed.aspx" --%>


                        <p style="color: crimson; font-size: 12pt; text-align: center;">Last date of online application submission is 15 April, 2023 </p>

                        <p style="color: black; font-size: 20pt; text-align: center;" navigateurl="~/Admission/Candidate/SelectProgramV3.aspx?ecat=4"
                            runat="server">
                            <b>Select the program you want to apply to</b> &nbsp;
                        </p>

                    </div>
                </div>

            </div>
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="customApplyDiv">
                        <asp:HyperLink ID="hrefApplyUndergrad" Style="color: white;" NavigateUrl="~/Admission/Candidate/foreignStudentInputInfo.aspx?ecat=4"
                            runat="server">
                            Apply for Bachelors Program &nbsp;<span class="glyphicon glyphicon-chevron-right"></span>
                        </asp:HyperLink>
                        <%--   NavigateUrl="~/Admission/Candidate/MassageAfterAdmissionClosed.aspx" --%>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="customApplyMastersDiv">
                        <asp:HyperLink ID="hrefApplyGrad" Style="color: white;" NavigateUrl="~/Admission/Candidate/foreignStudentInputInfo.aspx?ecat=6"
                            runat="server">
                            Apply for Masters Program &nbsp;<span class="glyphicon glyphicon-chevron-right"></span>
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>

        <div class="button">
        <asp:HyperLink ID="hrefApplyForeign" style="text-align:left; border-radius:4px; background-color: #000000a6; color: white;" NavigateUrl="~/Admission/Home.aspx?ecat=4"
                    runat="server" CssClass="btn btn-light">
                          <span class="glyphicon glyphicon-chevron-left"></span>  Back &nbsp;
                </asp:HyperLink>
            </div>

</asp:Content>
