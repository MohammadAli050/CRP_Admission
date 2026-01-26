function validateBasicInfoForm() {
    var firstName, dob, placeOfBirth, nationality, motherTongue,
        gender, maritalStatus, nationalId, bloodGroup, email,
        mobile, religion, quota;

    /// Comment out because visible false from interface

    //firstName = document.getElementById("MainContent_txtFirstName").value;
    //dob = document.getElementById("MainContent_txtDateOfBirth").value;
    //placeOfBirth = document.getElementById("MainContent_txtPlaceOfBirth").value;
    //nationality = document.getElementById("MainContent_ddlNationality").value;
    //motherTongue = document.getElementById("MainContent_ddlLanguage").value;
    //gender = document.getElementById("MainContent_ddlGender").value;
    //maritalStatus = document.getElementById("MainContent_ddlMaritalStatus").value;
    //email = document.getElementById("MainContent_txtEmail").value;
    //mobile = document.getElementById("MainContent_txtMobile").value;

    try {
        nationalId = document.getElementById("MainContent_txtNationalIdOrBirthRegistration").value;
    } catch (ex) {
        nationalId = null;
    }
    bloodGroup = document.getElementById("MainContent_ddlBloodGroup").value;
    religion = document.getElementById("MainContent_ddlReligion").value;
    quota = document.getElementById("MainContent_ddlQuota").value;

    //if (firstName == null || firstName.toString().trim() == '') {
    //    document.getElementById("MainContent_txtFirstName").style.border = "1px solid rgb(255, 0, 0)";
    //}
    //if (dob == null || dob.toString().trim() == '') {
    //    document.getElementById("MainContent_txtDateOfBirth").style.border = "1px solid rgb(255, 0, 0)";
    //}
    //if (placeOfBirth == null || placeOfBirth.toString().trim() == '') {
    //    document.getElementById("MainContent_txtPlaceOfBirth").style.border = "1px solid rgb(255, 0, 0)";
    //}
    //if (nationality == -1) { document.getElementById("MainContent_ddlNationality").style.border = "1px solid rgb(255, 0, 0)"; }
    //if (motherTongue == -1) { document.getElementById("MainContent_ddlLanguage").style.border = "1px solid rgb(255, 0, 0)"; }
    //if (gender == -1) { document.getElementById("MainContent_ddlGender").style.border = "1px solid rgb(255, 0, 0)"; }
    //if (maritalStatus == -1) { document.getElementById("MainContent_ddlMaritalStatus").style.border = "1px solid rgb(255, 0, 0)"; }

    //if (email == null || email.toString().trim() == '') {
    //    document.getElementById("MainContent_txtEmail").style.border = "1px solid rgb(255, 0, 0)";
    //}
    //if (mobile == null || mobile.toString().trim() == '') {
    //    document.getElementById("MainContent_txtMobile").style.border = "1px solid rgb(255, 0, 0)";
    //}
    try {
        if (nationalId == null || nationalId.toString().trim() == '') {
            document.getElementById("MainContent_txtNationalIdOrBirthRegistration").style.border = "1px solid rgb(255, 0, 0)";
        }
    } catch (ex) {
        document.getElementById("MainContent_ddlNationalIdOrBirthRegistration").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (bloodGroup == -1) { document.getElementById("MainContent_ddlBloodGroup").style.border = "1px solid rgb(255, 0, 0)"; }

    if (religion == -1) { document.getElementById("MainContent_ddlReligion").style.border = "1px solid rgb(255, 0, 0)"; }
    if (quota == -1) { document.getElementById("MainContent_ddlQuota").style.border = "1px solid rgb(255, 0, 0)"; }

    if (
        //(firstName == null || firstName.toString().trim() == '') ||
        //(dob == null || dob.toString().trim() == '') ||
        //(placeOfBirth == null || placeOfBirth.toString().trim() == '') ||
        (nationalId == null || nationalId.toString().trim() == '') ||
        //motherTongue == -1  || maritalStatus == -1 ||
        //gender == -1
        //||
        bloodGroup == -1
        //|| (email == null || email.toString().trim() == '') || 
        //(mobile == null || mobile.toString().trim() == '')
        || religion == -1 || quota == -1) {
        document.getElementById("validationMsg").innerText = "Please provide the required fields";
        return false;
    }
    else {
        return true;
    }
}

function validateBasicInfoFormOnNext() {
    var firstName, dob, email, mobile;

    return true;

    //firstName = document.getElementById("MainContent_txtFirstName").value;
    //dob = document.getElementById("MainContent_txtDateOfBirth").value;
    ////gender = document.getElementById("MainContent_ddlGender").value;
    //email = document.getElementById("MainContent_txtEmail").value;
    //mobile = document.getElementById("MainContent_txtMobile").value;

    //if (firstName == null || firstName.toString().trim() == '') {
    //    document.getElementById("MainContent_txtFirstName").style.border = "1px solid rgb(255, 0, 0)";
    //    console.log("first name validation");
    //}
    //if (dob == null || dob.toString().trim() == '') {
    //    document.getElementById("MainContent_txtDateOfBirth").style.border = "1px solid rgb(255, 0, 0)";
    //}
    //if (email == null || email.toString().trim() == '') {
    //    document.getElementById("MainContent_txtEmail").style.border = "1px solid rgb(255, 0, 0)";
    //}
    //if (mobile == null || mobile.toString().trim() == '') {
    //    document.getElementById("MainContent_txtMobile").style.border = "1px solid rgb(255, 0, 0)";
    //}

    //if ((firstName == null || firstName.toString().trim() == '') ||
    //    (dob == null || dob.toString().trim() == '') ||
    //    (email == null || email.toString().trim() == '') ||
    //    (mobile == null || mobile.toString().trim() == '')) {
    //    document.getElementById("validationMsg").innerText = "Please provide the required fields";
    //    return false;
    //}
    //else {
    //    return true;
    //}
}
