function validateAdditionalInfo() {

    var isAdmittedBefore, currentStudentId, fatherAnnualIncome;

    isAdmittedBefore = document.getElementById("MainContent_ddlAdmittedBefore").value;
    fatherAnnualIncome = document.getElementById("MainContent_txtFatherAnnualIncome").value;
    currentStudentId = document.getElementById("MainContent_txtCurrentStudentId").value;

    //if (isAdmittedBefore == -1) { document.getElementById("MainContent_ddlAdmittedBefore").style.border = "1px solid rgb(255, 0, 0)" }

    if (document.getElementById("MainContent_ddlAdmittedBefore").disabled == false) {
        if (isAdmittedBefore == -1) { document.getElementById("MainContent_ddlAdmittedBefore").style.border = "1px solid rgb(255, 0, 0)" }
    }


    if (fatherAnnualIncome == null || fatherAnnualIncome.trim() == '') {
        document.getElementById("MainContent_txtFatherAnnualIncome").style.border = "1px solid rgb(255, 0, 0)";
    }
    //&& (document.getElementById("MainContent_txtAwardScholarshipDetails").disabled == false) 

    if (isAdmittedBefore == 'Yes' && (currentStudentId == null || currentStudentId.trim() == "")) {
        document.getElementById("MainContent_txtCurrentStudentId").style.border = "1px solid rgb(255, 0, 0)";
        //alert(currentStudentId);
    }
    //validation for occupation and extracurricular activities not implemented.

    if (
        ((document.getElementById("MainContent_ddlAdmittedBefore").disabled == true) && (fatherAnnualIncome == null || fatherAnnualIncome.trim() == '')) ||
        ((document.getElementById("MainContent_ddlAdmittedBefore").disabled == false) && (
                (isAdmittedBefore == '-1') ||
                (isAdmittedBefore == 'Yes' && (currentStudentId == null || currentStudentId.trim() == '')) ||
                (fatherAnnualIncome == null || fatherAnnualIncome.trim() == ''))
        )
        ) {
        document.getElementById("validationMsg").innerText = "Please provide the required fields";
        return false;
    }
    else {
        return true;
    }

    //if (
    //    (isAdmittedBefore == '-1') ||
    //    (isAdmittedBefore == 'Yes' && (currentStudentId == null || currentStudentId.trim() == '')) ||
    //    (fatherAnnualIncome == null || fatherAnnualIncome.trim() == '')
    //    ) {
    //    document.getElementById("validationMsg").innerText = "Please provide the required fields";
    //    return false;
    //}
    //else {
    //    return true;
    //}
}