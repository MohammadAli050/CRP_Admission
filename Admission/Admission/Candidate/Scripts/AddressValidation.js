function validateAddress() {
    var presMailingAddress, presCountry, presDistrict, presDivision, presPostCode;
    var permMailingAddress, permCountry, permDistrict, permDivision, permPostCode;

    presMailingAddress = document.getElementById("MainContent_txtPresentAddress").value;
    presCountry = document.getElementById("MainContent_ddlPresentCountry").value;
    presDistrict = document.getElementById("MainContent_ddlPresentDistrict").value;
    presDivision = document.getElementById("MainContent_ddlPresentDivision").value;
    presPostCode = document.getElementById("MainContent_txtPresentPostCode").value;

    permMailingAddress = document.getElementById("MainContent_txtPermanentAddress").value;
    permCountry = document.getElementById("MainContent_ddlPermanentCountry").value;
    permDistrict = document.getElementById("MainContent_ddlPermanentDistrict").value;
    permDivision = document.getElementById("MainContent_ddlPermanentDivision").value;
    permPostCode = document.getElementById("MainContent_txtPermanentPostCode").value;


    if(presMailingAddress == null || presMailingAddress.toString().trim() == ''){
        document.getElementById("MainContent_txtPresentAddress").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (presCountry == -1) {
        document.getElementById("MainContent_ddlPresentCountry").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (presDivision == -1) {
        document.getElementById("MainContent_ddlPresentDivision").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (presDivision > 0 && presDistrict == -1) {
        document.getElementById("MainContent_ddlPresentDistrict").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (presPostCode == null || presPostCode == '') {
        document.getElementById("MainContent_txtPresentPostCode").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (permMailingAddress == null || permMailingAddress == '') {
        document.getElementById("MainContent_txtPermanentAddress").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (permCountry == -1) {
        document.getElementById("MainContent_ddlPermanentCountry").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (permDivision == -1) {
        document.getElementById("MainContent_ddlPermanentDivision").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (permDivision > 0 && permDistrict == -1) {
        document.getElementById("MainContent_ddlPermanentDistrict").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (permPostCode == null || permPostCode == '') {
        document.getElementById("MainContent_txtPermanentPostCode").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (
        (presMailingAddress == null || presMailingAddress == '') ||
        presCountry == -1 || presDivision == -1 ||
        (presDivision > 0 && presDistrict == -1) ||
        (presPostCode == null || presPostCode == '') ||
        (permMailingAddress == null || permMailingAddress == '') ||
        permCountry == -1 || permDivision == -1 ||
        (permDivision > 0 && permDistrict == -1) ||
        (permPostCode == null || permPostCode == '')
        ) {
        document.getElementById("validationMsg").innerText = "Please provide the required fields";
        return false;
    }
    else {
        return true;
    }
}