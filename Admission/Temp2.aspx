<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Temp2.aspx.cs" Inherits="Admission.Temp2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading"><strong>Application Form</strong></div>
                <div class="panel-body">
                    <div>

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li role="presentation" class="active"><a href="#basicInfo" aria-controls="home" role="tab" data-toggle="tab"><strong>1. </strong>Basic Info</a></li>
                            <li role="presentation"><a href="#educationInfo" aria-controls="profile" role="tab" data-toggle="tab"><strong>2. </strong>Education Info</a></li>
                            <li role="presentation"><a href="#parentInfo" aria-controls="messages" role="tab" data-toggle="tab"><strong>3. </strong>Parent/Guardian Info</a></li>
                            <li role="presentation"><a href="#extraWorkInfo" aria-controls="settings" role="tab" data-toggle="tab"><strong>4. </strong>Extracurricular/Work Experience Info </a></li>
                            <li role="presentation"><a href="#photoSigUpload" aria-controls="settings" role="tab" data-toggle="tab"><strong>5. </strong>Photo/Signature Upload</a></li>
                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div role="tabpanel" class="tab-pane active" id="basicInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        basic info
                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV BASIC INFO TAB-CONTENT************************************************************************************ --%>
                            <div role="tabpanel" class="tab-pane" id="educationInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        Education
                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV EDUCATION INFO TAB-CONTENT******************************************************************************** --%>
                            <div role="tabpanel" class="tab-pane" id="parentInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        Parent
                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV PARENT/GUARDIAN INFO TAB-CONTENT************************************************************************** --%>
                            <div role="tabpanel" class="tab-pane" id="extraWorkInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        Extra
                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV EXTRACURRICULAR/WORKEXPERIENCE INFO TAB-CONTENT*********************************************************** --%>
                            <div role="tabpanel" class="tab-pane" id="photoSigUpload">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        Photo
                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV PHOTO/SIGNATURE INFO TAB-CONTENT************************************************************************** --%>
                        </div>

                    </div>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD- --%>
    </div>
    <%-- END ROW --%>
    </div>
    </form>
</body>
</html>
