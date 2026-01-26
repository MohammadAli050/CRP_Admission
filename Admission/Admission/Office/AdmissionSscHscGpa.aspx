<%@ Page Title="Admission SSC HSC GPA" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionSscHscGpa.aspx.cs" Inherits="Admission.Admission.Office.AdmissionSscHscGpa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        .header-center {
            text-align: center;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdatePanel ID="updatePanel4" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4>Faculty wise SSC and HSC Setup</h4>
                        </div>
                        <div class="panel-body" style="margin-bottom: -30px;">


                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <table style="width: 100%" class="table table-hover table-condensed">
                                <tr>
                                    <td style="text-align: left; width: 10%; font-weight: bold">Faculty</td>
                                    <td style="text-align: left; width: 45%">

                                        <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="100%"></asp:DropDownList>


                                    </td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                            ControlToValidate="ddlAdmissionUnit"
                                            Display="Dynamic"
                                            ErrorMessage="School/Faculty is required"
                                            ForeColor="Crimson"
                                            ValueToCompare="-1"
                                            Font-Size="9pt"
                                            Operator="NotEqual"
                                            ValidationGroup="gr1">
                                        </asp:CompareValidator>
                                    </td>
                                </tr>


                                <tr>
                                    <td style="text-align: left; font-weight: bold">Exam Type</td>
                                    <td style="text-align: left;">

                                        <asp:DropDownList ID="ddlExamType" runat="server" Width="100%">
                                            <asp:ListItem Enabled="true" Text="--Select Exam Type--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <td style="text-align: left;">
                                        <asp:CompareValidator ID="ddlExamType_CV" runat="server"
                                            ControlToValidate="ddlExamType"
                                            Display="Dynamic"
                                            ErrorMessage="Exam Type is required"
                                            ForeColor="Crimson"
                                            ValueToCompare="-1"
                                            Font-Size="9pt"
                                            Operator="NotEqual"
                                            ValidationGroup="gr1">
                                        </asp:CompareValidator>
                                    </td>
                                </tr>



                                <tr>
                                    <td style="text-align: left; font-weight: bold">Group</td>
                                    <td style="text-align: left;">

                                        <asp:DropDownList ID="ddlGroup" runat="server" Width="100%">
                                            <asp:ListItem Enabled="true" Text="--Select Group--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Science" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Business Studies" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Humanities" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="General" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="71-Dress Making" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Accounting" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="Computer Operation" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="68-Computer & Inform" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="Commerce" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="90-General Electrica" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="62-General Electroni" Value="13"></asp:ListItem>
                                            <asp:ListItem Text="Computer Operation a" Value="14"></asp:ListItem>
                                            <asp:ListItem Text="96-Welding and Fabr" Value="15"></asp:ListItem>
                                            <asp:ListItem Text="MUZABBID" Value="16"></asp:ListItem>
                                            <asp:ListItem Text="64-Building Maintena" Value="17"></asp:ListItem>
                                            <asp:ListItem Text="Banking" Value="18"></asp:ListItem>
                                            <asp:ListItem Text="67-Civil Constructio" Value="19"></asp:ListItem>
                                            <asp:ListItem Text="Human Resource Manag" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="Agro Based Food" Value="21"></asp:ListItem>
                                            <asp:ListItem Text="Agro Machinery" Value="22"></asp:ListItem>
                                            <asp:ListItem Text="Architectural Drafting with AutoCAD" Value="23"></asp:ListItem>
                                            <asp:ListItem Text="Automobile" Value="24"></asp:ListItem>
                                            <asp:ListItem Text="Automotive" Value="25"></asp:ListItem>
                                            <asp:ListItem Text="Building Maintenance" Value="26"></asp:ListItem>
                                            <asp:ListItem Text="Building Maintenance and Construction" Value="27"></asp:ListItem>
                                            <asp:ListItem Text="Civil Construction" Value="28"></asp:ListItem>
                                            <asp:ListItem Text="Civil Drafting with CAD" Value="29"></asp:ListItem>
                                            <asp:ListItem Text="Clothing and Garments Finishing" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="Computer & Information Technology" Value="31"></asp:ListItem>
                                            <asp:ListItem Text="Computer Operation and Maintenance" Value="32"></asp:ListItem>
                                            <asp:ListItem Text="Computerized Accounting System" Value="33"></asp:ListItem>
                                            <asp:ListItem Text="Digital Technology in Business" Value="34"></asp:ListItem>
                                            <asp:ListItem Text="Drafting Civil" Value="35"></asp:ListItem>
                                            <asp:ListItem Text="Dress Making" Value="36"></asp:ListItem>
                                            <asp:ListItem Text="Dying, Printing and Finishing" Value="37"></asp:ListItem>
                                            <asp:ListItem Text="E-Business" Value="38"></asp:ListItem>
                                            <asp:ListItem Text="Electrical Maintenance Works" Value="39"></asp:ListItem>
                                            <asp:ListItem Text="Electrical Works and Maintenance" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="Electronic Control and Communication" Value="41"></asp:ListItem>
                                            <asp:ListItem Text="Entrepreneurship Development" Value="42"></asp:ListItem>
                                            <asp:ListItem Text="Farm Machinery" Value="43"></asp:ListItem>
                                            <asp:ListItem Text="Financial Practices" Value="44"></asp:ListItem>
                                            <asp:ListItem Text="Fish Culture and Breeding" Value="45"></asp:ListItem>
                                            <asp:ListItem Text="Food Processing and Preservation" Value="46"></asp:ListItem>
                                            <asp:ListItem Text="Fruit and Vegetable Cultivation" Value="47"></asp:ListItem>
                                            <asp:ListItem Text="General Electrical Works" Value="48"></asp:ListItem>
                                            <asp:ListItem Text="General Electronics" Value="49"></asp:ListItem>
                                            <asp:ListItem Text="General Mechanics" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="Human Resource Development" Value="51"></asp:ListItem>
                                            <asp:ListItem Text="Human Resource Management" Value="52"></asp:ListItem>
                                            <asp:ListItem Text="Industrial Wood Working" Value="53"></asp:ListItem>
                                            <asp:ListItem Text="Knitting" Value="54"></asp:ListItem>
                                            <asp:ListItem Text="Livestock Rearing and Farming" Value="55"></asp:ListItem>
                                            <asp:ListItem Text="Machine Tools Operation" Value="56"></asp:ListItem>
                                            <asp:ListItem Text="Machine Tools Operation and Maintenance" Value="57"></asp:ListItem>
                                            <asp:ListItem Text="Mechanical Drafting with CAD" Value="58"></asp:ListItem>
                                            <asp:ListItem Text="Patient Care Technique" Value="59"></asp:ListItem>
                                            <asp:ListItem Text="Plumbing and Pipe Fitting" Value="60"></asp:ListItem>
                                            <asp:ListItem Text="Poultry Rearing and Farming" Value="61"></asp:ListItem>
                                            <asp:ListItem Text="Refrigeration and Air Conditioning" Value="62"></asp:ListItem>
                                            <asp:ListItem Text="Refrigeration and Air-Conditioning" Value="63"></asp:ListItem>
                                            <asp:ListItem  Text="Secretarial Science" Value="64"></asp:ListItem>
                                            <asp:ListItem  Text="Shorthand" Value="65"></asp:ListItem>
                                            <asp:ListItem  Text="Shrimp Culture and Breeding" Value="66"></asp:ListItem>
                                            <asp:ListItem  Text="Weaving" Value="67"></asp:ListItem>
                                            <asp:ListItem  Text="Welding  and Fabrication" Value="68"></asp:ListItem>
                                            <asp:ListItem  Text="Welding and Fabrication" Value="69"></asp:ListItem>
                                            <asp:ListItem  Text="Wood Working" Value="70"></asp:ListItem>

                                        </asp:DropDownList>

                                    </td>
                                    <td style="text-align: left;">
                                        <asp:CompareValidator ID="ddlGroupCompare" runat="server"
                                            ControlToValidate="ddlGroup"
                                            Display="Dynamic"
                                            ErrorMessage="Group is required"
                                            ForeColor="Crimson"
                                            ValueToCompare="-1"
                                            Font-Size="9pt"
                                            Operator="NotEqual"
                                            ValidationGroup="gr1">
                                        </asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold">GPA</td>
                                    <td style="text-align: left;">

                                        <asp:TextBox ID="txtGPA" runat="server" Width="100%"></asp:TextBox>

                                    </td>
                                    <td style="text-align: left;">
                                        <asp:RequiredFieldValidator ID="txtGPA_ReqV" runat="server"
                                            ControlToValidate="txtGPA"
                                            ErrorMessage="GPA required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="true"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold">Total GPA Point</td>
                                    <td style="text-align: left;">

                                        <asp:TextBox ID="txtTotalGPAPoint" runat="server" Width="100%"></asp:TextBox>

                                    </td>
                                    <td style="text-align: left;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtTotalGPAPoint"
                                            ErrorMessage="Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="true"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">

                                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1"
                                            OnClick="btnSave_Click" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btnClearAdmin"
                                            OnClick="btnClear_Click" />

                                    </td>
                                </tr>
                            </table>
                        </div>
                       
                    </div>
        
                </div>
             
            </div>
        
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-body" style="margin-bottom: -25px;">

                            <div class="row" style="margin-bottom: 5%;">
                                Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                            </div>
                            <div class="row" style="overflow-x:scroll;overflow-y:scroll">
                            <asp:ListView ID="lvAdmissionUnitProgram" runat="server"
                                OnItemDataBound="lvAdmissionUnitProgram_ItemDataBound"
                                OnItemCommand="lvAdmissionUnitProgram_ItemCommand"
                                OnItemDeleting="lvAdmissionUnitProgram_ItemDeleting"
                                OnItemUpdating="lvAdmissionUnitProgram_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Faculty Name</th>
                                            <th runat="server">Exam Type</th>
                                            <th runat="server">Group</th>
                                            <th runat="server">GPA</th>
                                            <th runat="server">Total GPA Point</th>
                                            <th runat="server" style="text-align: center;">Action</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblFacultyName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblExamType" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblGroupName" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblGPA" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblTotalGPAPoint" runat="server" />
                                        </td>
                                        <td valign="middle" align="right" class="" style="text-align: center;">

                                            <asp:LinkButton class="btn btn-info" ID="lnkEdit" runat="server" Style="border-radius: 8px; padding: 2% 15%;">Edit</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            </div>

                        </div>
          
                    </div>
        
                </div>
      
            </div>
     
       </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>






<%--============ New Code for Setup -- Work On Later ===============--%>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script src="../../Scripts/jquery-3.1.1.min.js"></script>

    <script type="text/javascript">

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });

        function tableSearchFunction() {

            var input, filter, table, tr, tdList, td, i, j, txtValue, rowCount;

            input = document.getElementById("tableSearch");
            //console.log('input');
            //console.log(input);

            filter = input.value.toUpperCase();
            //console.log('filter');
            //console.log(filter);

            table = document.getElementById("MainContent_gvGPASetup");
            //console.log('table');
            //console.log(table);

            tr = table.getElementsByTagName("tr");
            //console.log('tr');
            //console.log(tr);

            //console.log('tr.length');
            //console.log(tr.length);

            //console.log('==== Start Loop 1 ====');
            for (i = 0; i < tr.length; i++) {
                if (i !== 0) {

                    rowCount = 0;

                    //td = tr[i].getElementsByTagName("td")[4];
                    tdList = tr[i].getElementsByTagName("td");
                    //console.log('tdList');
                    //console.log(tdList);

                    //console.log('tdList.length');
                    //console.log(tdList.length);

                    //console.log('==== Start Loop 2 -> ' + i + ' ====');
                    for (j = 0; j < tdList.length; j++) {

                        //console.log('==== tr-' + i + ' td-' + j + ' ====');

                        td = tdList[j]
                        //console.log('td');
                        //console.log(td);

                        txtValue = td.textContent.trim() || td.innerText.trim();
                        //console.log('tdList => txtValue');
                        //console.log(txtValue);

                        if (txtValue != "" && txtValue != null) {

                            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                                console.log('IF');
                                //tr[i].style.display = "";

                                rowCount++;

                            } else {
                                console.log('ELSE');
                                //tr[i].style.display = "none";
                            }

                        }


                    } //END : for (j = 0; j < tdList.length; j++)

                    //console.log('==== End Loop 2 ====');


                    //console.log('rowCount');
                    //console.log(rowCount);

                    if (rowCount > 0) {
                        //console.log('rowCount => IF');
                        tr[i].style.display = "";
                    }
                    else {
                        //console.log('rowCount => ELSE');
                        tr[i].style.display = "none";
                    }

                }

            }
            //console.log('==== End Loop 1 ====');
        }

        function clearSearchTable() {
            $('#tableSearch').val('');
            tableSearchFunction();
        }


    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="updatePanel4" runat="server">
        <ContentTemplate>

            <h3><strong>GPA Setup</strong></h3>
            <div class="panel panel-default">
                <div class="panel-body">



                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Faculty <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator0" runat="server"
                                    ControlToValidate="ddlAdmissionUnit"
                                    ErrorMessage="Required"
                                    Font-Bold="true"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValueToCompare="-1"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Education Category <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="100%" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlEducationCategory_SelectedIndexChanged">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    <asp:ListItem Value="4">Bachelor</asp:ListItem>
                                    <asp:ListItem Value="6">Masters</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                    ControlToValidate="ddlEducationCategory"
                                    ErrorMessage="Required"
                                    Font-Bold="true"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValueToCompare="-1"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Program </strong></label>
                                <asp:DropDownList ID="ddlProgram" runat="server" Width="100%" CssClass="form-control"
                                    Enabled="false">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator2" runat="server"
                                    ControlToValidate="ddlProgram"
                                    ErrorMessage="Required"
                                    Font-Bold="true"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValueToCompare="-1"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1"
                                    Enabled="false"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Exam Type <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlExamType" runat="server" Width="100%" CssClass="form-control"
                                    Enabled="false">
                                    <asp:ListItem Text="--Select Exam Type--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator3" runat="server"
                                    ControlToValidate="ddlExamType"
                                    ErrorMessage="Required"
                                    Font-Bold="true"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValueToCompare="-1"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1"
                                    Enabled="false"></asp:CompareValidator>
                            </div>
                        </div>

                    </div>




                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Group <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlGroup" runat="server" Width="100%" CssClass="form-control"
                                    Enabled="false">
                                    <asp:ListItem Text="--Select Group--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Science" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Business" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Humanities" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator4" runat="server"
                                    ControlToValidate="ddlGroup"
                                    ErrorMessage="Required"
                                    Font-Bold="true"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValueToCompare="-1"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1"
                                    Enabled="false"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Result Division </strong></label>
                                <asp:DropDownList ID="ddlResultDivision" runat="server" Width="100%" CssClass="form-control">
                                    <asp:ListItem Text="--Select Result Division--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="1st Division" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="2nd Division" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="3rd Division" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="GPA / CGPA" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator5" runat="server"
                                    ControlToValidate="ddlResultDivision"
                                    ErrorMessage="Required"
                                    Font-Bold="true"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValueToCompare="-1"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1">
                                </asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>GPA From <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:TextBox ID="txtGPAFrom" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server"
                                    ControlToValidate="txtGPAFrom"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="gr1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>GPA To <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:TextBox ID="txtGPATo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtGPATo"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="gr1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Total GPA <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:TextBox ID="txtTotalGPA" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtTotalGPA"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="gr1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>GPA Point </strong></label>
                                <asp:TextBox ID="txtGPAPoint" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success"
                                Width="25%"
                                ValidationGroup="gr1" OnClick="btnSave_Click" />
                            &nbsp;
                            &nbsp;
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-default"
                                Width="25%"
                                OnClick="btnClear_Click" />
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <asp:Panel ID="messagePanel" runat="server" Visible="false">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>



                </div>
            </div>


            <div class="panel panel-default">
                <div class="panel-body">


                    <div class="row">
                        <div class="col-sm-8 col-md-8 col-lg-8">
                            <p style="margin-top: 8px;">
                                <strong>Total:</strong>
                                <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label>
                            </p>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <div class="input-group">
                                    <input type="text" id="tableSearch" placeholder="search"
                                        onkeyup="tableSearchFunction()" class="form-control" />
                                    <div class="input-group-addon" style="padding: 0;">
                                        <input type="button" value="×" onclick="clearSearchTable()"
                                            data-toggle="tooltip" data-placement="left" title="Clear Search"
                                            style="border: 0; padding: 8px 17px;" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>




                    <asp:GridView ID="gvGPASetup" runat="server" CssClass="table table-responsive table-hover"
                        AutoGenerateColumns="false" GridLines="none" Width="100%">
                        <HeaderStyle BackColor="#1387de" ForeColor="White" />
                        <Columns>

                            <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAdmissionSscHscGpaSetupId"
                                        Text='<%#Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Faculty Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFacultyName"
                                        Text='<%#Eval("FacultyName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Education Category">
                                <ItemTemplate>
                                    
                                    <asp:Label runat="server" ID="lblEducationCategoryName"
                                        Text='<%#Eval("EducationCategoryName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Program">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProgramShortName"
                                        Text='<%#Eval("ProgramShortName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Group">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblGroupName"
                                        Text='<%#Eval("GroupName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Exam Type">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblExamTypeName"
                                        Text='<%#Eval("ExamTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>




                            <asp:TemplateField HeaderText="GPA Range">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblGPAFrom"
                                        Text='<%#Eval("GPAFrom") %>'></asp:Label>
                                    -
                                    <asp:Label runat="server" ID="lblGPATo"
                                        Text='<%#Eval("GPATo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total GPA">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTotalGPA"
                                        Text='<%#Eval("TotalGPA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="GPA Point">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblGPAPoint"
                                        Text='<%#Eval("GPAPoint") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>

                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-info"
                                        OnClick="btnEdit_Click" />

                                </ItemTemplate>
                            </asp:TemplateField>



                        </Columns>
                    </asp:GridView>
                </div>
            </div>--%>





            <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>
            <%--<div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4>Faculty wise SSC and HSC Setup</h4>
                        </div>
                        <div class="panel-body" style="margin-bottom: -30px;">


                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <table style="width: 100%" class="table table-hover table-condensed">
                                <tr>
                                    <td style="text-align: left; width: 10%; font-weight: bold">Faculty</td>
                                    <td style="text-align: left; width: 45%">

                                        <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="100%"></asp:DropDownList>


                                    </td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                            ControlToValidate="ddlAdmissionUnit"
                                            Display="Dynamic"
                                            ErrorMessage="School/Faculty is required"
                                            ForeColor="Crimson"
                                            ValueToCompare="-1"
                                            Font-Size="9pt"
                                            Operator="NotEqual"
                                            ValidationGroup="gr1">
                                        </asp:CompareValidator>
                                    </td>
                                </tr>


                                <tr>
                                    <td style="text-align: left; font-weight: bold">Exam Type</td>
                                    <td style="text-align: left;">

                                        <asp:DropDownList ID="ddlExamType" runat="server" Width="100%">
                                            <asp:ListItem Enabled="true" Text="--Select Exam Type--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <td style="text-align: left;">
                                        <asp:CompareValidator ID="ddlExamType_CV" runat="server"
                                            ControlToValidate="ddlExamType"
                                            Display="Dynamic"
                                            ErrorMessage="Exam Type is required"
                                            ForeColor="Crimson"
                                            ValueToCompare="-1"
                                            Font-Size="9pt"
                                            Operator="NotEqual"
                                            ValidationGroup="gr1">
                                        </asp:CompareValidator>
                                    </td>
                                </tr>



                                <tr>
                                    <td style="text-align: left; font-weight: bold">Group</td>
                                    <td style="text-align: left;">

                                        <asp:DropDownList ID="ddlGroup" runat="server" Width="100%">
                                            <asp:ListItem Enabled="true" Text="--Select Group--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Science" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Business" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Humanities" Value="4"></asp:ListItem>

                                        </asp:DropDownList>

                                    </td>
                                    <td style="text-align: left;">
                                        <asp:CompareValidator ID="ddlGroupCompare" runat="server"
                                            ControlToValidate="ddlGroup"
                                            Display="Dynamic"
                                            ErrorMessage="Group is required"
                                            ForeColor="Crimson"
                                            ValueToCompare="-1"
                                            Font-Size="9pt"
                                            Operator="NotEqual"
                                            ValidationGroup="gr1">
                                        </asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold">GPA</td>
                                    <td style="text-align: left;">

                                        <asp:TextBox ID="txtGPA" runat="server" Width="100%"></asp:TextBox>

                                    </td>
                                    <td style="text-align: left;">
                                        <asp:RequiredFieldValidator ID="txtGPA_ReqV" runat="server"
                                            ControlToValidate="txtGPA"
                                            ErrorMessage="GPA required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="true"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold">Total GPA Point</td>
                                    <td style="text-align: left;">

                                        <asp:TextBox ID="txtTotalGPAPoint" runat="server" Width="100%"></asp:TextBox>

                                    </td>
                                    <td style="text-align: left;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtTotalGPAPoint"
                                            ErrorMessage="Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="true"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">

                                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1"
                                            OnClick="btnSave_Click" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btnClearAdmin"
                                            OnClick="btnClear_Click" />

                                    </td>
                                  
                                </tr>
                            </table>
                        </div>
            
                    </div>
          
                </div>
              
            </div>--%>
            <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>
            <%--<div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-body" style="margin-bottom: -25px;">

                            <div class="row" style="margin-bottom: 1%;">
                                Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                            </div>
                            <asp:ListView ID="lvAdmissionUnitProgram" runat="server"
                                OnItemDataBound="lvAdmissionUnitProgram_ItemDataBound"
                                OnItemCommand="lvAdmissionUnitProgram_ItemCommand"
                                OnItemDeleting="lvAdmissionUnitProgram_ItemDeleting"
                                OnItemUpdating="lvAdmissionUnitProgram_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Faculty Name</th>
                                            <th runat="server">Exam Type</th>
                                            <th runat="server">Group</th>
                                            <th runat="server">GPA</th>
                                            <th runat="server">Total GPA Point</th>
                                            <th runat="server" style="text-align: center;">Action</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblFacultyName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblExamType" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblGroupName" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblGPA" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblTotalGPAPoint" runat="server" />
                                        </td>
                                        <td valign="middle" align="right" class="" style="text-align: center;">

                                            <asp:LinkButton class="btn btn-info" ID="lnkEdit" runat="server" Style="border-radius: 8px; padding: 2% 15%;">Edit</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                </EmptyDataTemplate>
                            </asp:ListView>

                        </div>
        
                    </div>
        
                </div>
             
            </div>--%>


<%--        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>--%>

















