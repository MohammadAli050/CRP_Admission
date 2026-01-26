function onLoad() {
    $(document).ready(function () {
        $('#<%=lblMessage.UniqueID%>').hide();
    });
}

function showAlert() {
    $(document).ready(function(){
        $('#<%=lblMessage.UniqueID%>').show();
        $('#<%=lblMessage.UniqueID%>').text('Key name is required');
    });
}

function validForm() {
    var keyName = document.getElementById('#<%=txtKeyName.ClientID%>').value;
    if (keyName == null || keyName == "") {
        //$('#<%=lblMessage.ClientID%>').show();
        //$('#<%=lblMessage.ClientID%>').html("Key Name is required");
        //$('#<%=lblMessage.ClientID%>').addClass("alert alert-danger");
        showAlert();
        return false;
    } else {
        return true;
    }
}