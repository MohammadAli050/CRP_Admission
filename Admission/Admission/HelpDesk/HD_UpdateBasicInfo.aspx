<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="HD_UpdateBasicInfo.aspx.cs" Inherits="Admission.Admission.HelpDesk.HD_UpdateBasicInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://pro.fontawesome.com/releases/v5.10.0/css/all.css" integrity="sha384-AYmEC3Yw5cVb3ZcuHtOA93w35dYTsvhLPVnYs9eStHfGJvOvKxVfELGroGkvsg+p" crossorigin="anonymous" />


    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



    <div id="divProgress" style="display: none; z-index: 1000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <b>Payment Id</b>
                        <asp:TextBox ID="txtPaymentId" runat="server" CssClass="form-control" Width="100%" TextMode="Number" AutoPostBack="true" OnTextChanged="txtPaymentId_TextChanged"></asp:TextBox>
                    </div>

                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <br />
                        <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn btn-info form-control" Width="100%" Style="text-align: center" OnClick="btnLoad_Click" />
                    </div>

                </div>
            </div>


            <div class="panel panel-default" style="margin-top: 10px">

                <div class="panel-body">

                    <asp:GridView runat="server" ID="gvCandidateInfo" AutoGenerateColumns="False" AllowPaging="false"
                        PagerSettings-Mode="NumericFirstLast" Width="100%" Font-Size="12px"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                        ShowHeader="true" CssClass="table table-bordered table-responsive">
                        <HeaderStyle BackColor="#2d712a" ForeColor="White" />
                        <RowStyle BackColor="#ecf0f0" />
                        <AlternatingRowStyle BackColor="#ffffff" />
                        <Columns>

                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <div style="margin-top: 10px">
                                        <asp:Label ID="txtName" runat="server" Text='<%#Eval("FirstName") %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtEmail" runat="server" Text='<%#Eval("Email") %>' placeholder="Test@gmail.com" ForeColor="Black" Font-Bold="true" CssClass="form-control"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Mobile">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMobile" runat="server" Text='<%#Eval("Mobile") %>' placeholder="+8801XXXXXXXXX" ForeColor="Black" Font-Bold="true" CssClass="form-control"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:LinkButton ID="Edit" ToolTip="Update" CssClass="btn btn-primary" Width="100%" CommandArgument='<%#Eval("ID")%>' runat="server" OnClick="Edit_Click">
                                                                             Click to Update
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel1" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSave_Education" Enabled="false" />
                    <EnableAction AnimationTarget="btnNext" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnSave_Education" Enabled="true" />
                    <EnableAction   AnimationTarget="btnNext" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
