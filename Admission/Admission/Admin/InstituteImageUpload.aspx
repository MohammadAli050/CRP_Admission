<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="InstituteImageUpload.aspx.cs" Inherits="Admission.Admission.Admin.InstituteImageUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />


    <script type="text/javascript">
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnUploadBanner.ClientID %>").click();
            }
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Banner
                </div>
                <div class="panel-body">
                    <table class="table_form" style="width: 100%;">
                        <tr>
                            <td>

                                <div>
                                    <asp:Image ID="imageBanner" runat="server"
                                        ImageUrl="~/Images/AppImg/img1.png"
                                        Width="750" Height="75" />
                                </div>
                                <asp:FileUpload ID="FileUploadBanner" runat="server" AllowMultiple="false" accept="image/*" />
                                <asp:RegularExpressionValidator ID="rexp" runat="server" ControlToValidate="FileUploadBanner"
                                    ErrorMessage="Only .jpg, .png, and .jpeg" Display="Dynamic" ForeColor="Crimson"
                                    ValidationExpression="(.*\.([Gg][Ii][Ff])|.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])|.*\.([Jj][Pp][Ee][Gg])$)"></asp:RegularExpressionValidator>
                                <asp:Button ID="btnUploadBanner" runat="server"
                                    Text="Upload Photo"
                                    CssClass="btn btn-success"
                                    Style="display: none"
                                    OnClick="btnUploadBanner_Click" />
                                <asp:Label ID="lblMessageBanner" runat="server" Text=""></asp:Label>

                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
