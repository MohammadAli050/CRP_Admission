function validateRelationInfo() {
    //variable starting with g means guardian.
    var fatherName, fatherOccupation, fatherMobile, fatherNationalId, fatherNationality, fatherIsLate, fatherOccupationType;
    var motherName, motherOccupation, motherMobile, motherNationalId, motherNationality, motherIsLate, motherOccupationType;
    var gName, gRelation, gRelationOther, gOccupation, gMobile, gOccupationType, guardianMailingAddress, guardianNIDBCPN;
    var fatherServiceType, fatherOrganisation, fatherDesignation, motherServiceType, motherOrganisation, motherDesignation;

    fatherName = document.getElementById("MainContent_txtFatherName").value;
    //fatherOccupation = document.getElementById("MainContent_txtFatherOccupation").value;
    fatherMobile = document.getElementById("MainContent_txtFatherMobile").value;
    fatherNationalId = document.getElementById("MainContent_txtFatherNationalId").value;
    fatherNationality = document.getElementById("MainContent_ddlFatherNationality").value;
    fatherIsLate = document.getElementById("MainContent_ddlIsLateFather").value; //ddlIsLateFather
    fatherOccupationType = document.getElementById("MainContent_ddlFatherOccupationType").value;

    motherName = document.getElementById("MainContent_txtMotherName").value;
    //motherOccupation = document.getElementById("MainContent_txtMotherOccupation").value;
    motherMobile = document.getElementById("MainContent_txtMotherMobile").value;
    motherNationalId = document.getElementById("MainContent_txtMotherNationalId").value;
    motherNationality = document.getElementById("MainContent_ddlMotherNationality").value;
    motherIsLate = document.getElementById("MainContent_ddlIsLateMother").value; // ddlIsLateMother
    motherOccupationType = document.getElementById("MainContent_ddlMotherOccupationType").value; //ddlMotherOccupationType

    gName = document.getElementById("MainContent_txtGuardian_Name").value;
    gRelation = document.getElementById("MainContent_ddlGuardianRelation").value;
    gRelationOther = document.getElementById("MainContent_txtGuardianOtherRelation").value;
    //gOccupation = document.getElementById("MainContent_txtGuardianOccupation").value;
    gMobile = document.getElementById("MainContent_txtGuardianMobile").value;
    gOccupationType = document.getElementById("MainContent_ddlGuardianOccupationType").value; //ddlGuardianOccupationType

    guardianMailingAddress = document.getElementById("MainContent_txtGuardianMailingAddress").value;
    guardianNIDBCPN = document.getElementById("MainContent_txtGuardianNationalId").value;


    //change by rafi
    //if (fatherIsLate == -1) {
    //    document.getElementById("MainContent_ddlIsLateFather").style.border = "1px solid rgb(0, 0, 0)";
    //}
    //else if (fatherIsLate == 1) {
    //    if (fatherName == null || fatherName.toString().trim() == "") {
    //        document.getElementById("MainContent_txtFatherName").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    if (fatherNationality == -1) {
    //        document.getElementById("MainContent_ddlFatherNationality").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //}
    //change by rafi
    //else if (fatherIsLate == 0) {
    //    if (fatherName == null || fatherName.toString().trim() == "") {
    //        document.getElementById("MainContent_txtFatherName").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    //if (fatherOccupation == null || fatherOccupation.toString().trim() == "") {
    //    //    document.getElementById("MainContent_txtFatherOccupation").style.border = "1px solid rgb(255, 0, 0)";
    //    //}
    //    if (fatherMobile == null || fatherMobile.toString().trim() == "") {
    //        document.getElementById("MainContent_txtFatherMobile").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    if (fatherNationalId == null || fatherNationalId.toString().trim() == "") {
    //        document.getElementById("MainContent_txtFatherNationalId").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    if (fatherOccupationType == -1) {
    //        document.getElementById("MainContent_ddlFatherOccupationType").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    if (fatherNationality == -1) {
    //        document.getElementById("MainContent_ddlFatherNationality").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //}
    ////////// Mother
    //if (motherIsLate == -1) {
    //    document.getElementById("MainContent_ddlIsLateMother").style.border = "1px solid rgb(0, 0, 0)";
    //}
    //else if (motherIsLate == 1) {
    //    if (motherName == null || motherName.toString().trim() == "") {
    //        document.getElementById("MainContent_txtMotherName").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    if (motherNationality == -1) {
    //        document.getElementById("MainContent_ddlMotherNationality").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //}
    //else if (motherIsLate == 0) {
    //    if (motherName == null || motherName.toString().trim() == "") {
    //        document.getElementById("MainContent_txtMotherName").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    if (motherMobile == null || motherMobile.toString().trim() == "") {
    //        document.getElementById("MainContent_txtMotherMobile").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    //if (motherOccupation == null || motherOccupation.toString().trim() == "") {
    //    //    document.getElementById("MainContent_txtMotherOccupation").style.border = "1px solid rgb(255, 0, 0)";
    //    //}
    //    if (motherNationalId == null || motherNationalId.toString().trim() == "") {
    //        document.getElementById("MainContent_txtMotherNationalId").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    if (motherNationality == -1) {
    //        document.getElementById("MainContent_ddlMotherNationality").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //    if (motherOccupationType == -1) {
    //        document.getElementById("MainContent_ddlMotherOccupationType").style.border = "1px solid rgb(0, 0, 0)";
    //    }
    //}
    ////////// Guardian
    if (gName == null || gName.toString().trim() == "") {
        document.getElementById("MainContent_txtGuardian_Name").style.border = "1px solid rgb(255, 0, 0)";
    }
    if (gRelation == -1) {
        document.getElementById("MainContent_ddlGuardianRelation").style.border = "1px solid rgb(255, 0, 0)";
    }
    if (gRelation == '10' && (gRelationOther == null || gName.toString().trim() == "")) {
        document.getElementById("MainContent_txtGuardianOtherRelation").style.border = "1px solid rgb(255, 0, 0)";
    }
    //if (gOccupation == null || gName.toString().trim() == "") {
    //    document.getElementById("MainContent_txtGuardianOccupation").style.border = "1px solid rgb(255, 0, 0)";
    //}
    if (gMobile == null || gName.toString().trim() == "") {
        document.getElementById("MainContent_txtGuardianMobile").style.border = "1px solid rgb(255, 0, 0)";
    }

    //change by rafi
    //if (gOccupationType == -1) {
    //    document.getElementById("MainContent_ddlGuardianOccupationType").style.border = "1px solid rgb(0, 0, 0)";
    //}


    if (guardianMailingAddress == null || guardianMailingAddress.toString().trim() == "") {
        document.getElementById("MainContent_txtGuardianMailingAddress").style.border = "1px solid rgb(255, 0, 0)";
    }

    //change by rafi
    //if (guardianNIDBCPN == null || guardianNIDBCPN.toString().trim() == "") {
    //    document.getElementById("MainContent_txtGuardianNationalId").style.border = "1px solid rgb(0, 0, 0)";
    //}



    //no occupation...
    //if (
    //    ((fatherIsLate == -1) ||
    //        ((fatherIsLate == 1) && (fatherName == null || fatherName.toString().trim() == "") &&
    //            (fatherNationality == -1)) ||
    //        ((fatherIsLate == 0) && ((fatherName == null || fatherName.toString().trim() == "") ||
    //                (fatherMobile == null || fatherMobile.toString().trim() == "") ||
    //                (fatherNationalId == null || fatherNationalId.toString().trim() == "") ||
    //                (fatherOccupationType == -1) ||
    //                (fatherNationality == -1))))//end father
    //    ||
    //    ((motherIsLate == -1) ||
    //        ((motherIsLate == 1) && (motherName == null || motherName.toString().trim() == "") &&
    //            (motherNationality == -1)) ||
    //        ((motherIsLate == 0) && ((motherName == null || motherName.toString().trim() == "") ||
    //                (motherMobile == null || motherMobile.toString().trim() == "") ||
    //                (motherNationalId == null || motherNationalId.toString().trim() == "") ||
    //                (motherOccupationType == -1) ||
    //                (motherNationality == -1))))//end mother
    //    ||
    //    ((gName == null || gName.toString().trim() == "") ||
    //        gRelation == -1 ||
    //        (gRelation == '10' && (gRelationOther == null || gName.toString().trim() == "")) ||
    //        (gName.toString().trim() == "") || (gOccupationType == -1) ||
    //        (gMobile == null || gName.toString().trim() == "") ||
    //    (guardianMailingAddress == null || guardianMailingAddress.toString().trim() == "") ||
    //    (guardianNIDBCPN == null || guardianNIDBCPN.toString().trim() == ""))//end guardian
    //    )
    if ((fatherMobile == null || fatherMobile.toString().trim() == "" && fatherIsLate != 1) || (motherMobile == null || motherMobile.toString().trim() == "" && motherIsLate != 1)
        ((gName == null || gName.toString().trim() == "") ||
                gRelation == -1 ||
                (gRelation == '10' && (gRelationOther == null || gName.toString().trim() == "")) ||
                (gName.toString().trim() == "") || //(gOccupationType == -1) ||
                (gMobile == null || gName.toString().trim() == "") ||
            (guardianMailingAddress == null || guardianMailingAddress.toString().trim() == "")
        //||(guardianNIDBCPN == null || guardianNIDBCPN.toString().trim() == "")
        )//end guardian
        )
    {
        document.getElementById("validationMsg").innerText = "Please provide the required fields";
        return false;

    //old one...not needed now.
    //if (
    //    (fatherName == null || fatherName.toString().trim() == "") ||
    //    (fatherOccupation == null || fatherOccupation.toString().trim() == "") ||
    //    (fatherMobile == null || fatherMobile.toString().trim() == "") ||
    //    (fatherNationalId == null || fatherNationalId.toString().trim() == "") ||
    //    (fatherNationality == -1) ||
    //    (motherName == null || motherName.toString().trim() == "") ||
    //    (motherMobile == null || motherMobile.toString().trim() == "") ||
    //    (motherOccupation == null || motherOccupation.toString().trim() == "") ||
    //    (motherNationalId == null || motherNationalId.toString().trim() == "") ||
    //    (motherNationality == -1) ||
    //    (gName == null || gName.toString().trim() == "") ||
    //    gRelation == -1 ||
    //    (gRelation == '10' && (gRelationOther == null || gName.toString().trim() == "")) ||
    //    (gOccupation == null || gName.toString().trim() == "") ||
    //    (gMobile == null || gName.toString().trim() == "")
    //    ) {
    //    document.getElementById("validationMsg").innerText = "Please provide the required fields";
    //    return false;
    //}

    //with occupation...
    //if (
    //    ( (fatherIsLate == -1) ||
    //        ( (fatherIsLate == 1) && (fatherName == null || fatherName.toString().trim() == "") && 
    //            (fatherNationality == -1) ) ||
    //        ( (fatherIsLate == 0) && ( (fatherName == null || fatherName.toString().trim() == "") ||
    //                (fatherOccupation == null || fatherOccupation.toString().trim() == "") ||
    //                (fatherMobile == null || fatherMobile.toString().trim() == "") ||
    //                (fatherNationalId == null || fatherNationalId.toString().trim() == "") ||
    //                (fatherOccupationType == -1) ||
    //                (fatherNationality == -1) ) ) )//end father
    //    ||
    //    ( (motherIsLate == -1) ||
    //        ( (motherIsLate == 1) && (motherName == null || motherName.toString().trim() == "") &&
    //            (motherNationality == -1) ) ||
    //        ( (motherIsLate == 0) && ( (motherName == null || motherName.toString().trim() == "") ||
    //                (motherMobile == null || motherMobile.toString().trim() == "") ||
    //                (motherOccupation == null || motherOccupation.toString().trim() == "") ||
    //                (motherNationalId == null || motherNationalId.toString().trim() == "") ||
    //                (motherOccupationType == -1) ||
    //                (motherNationality == -1) ) ) )//end mother
    //    ||
    //    ( (gName == null || gName.toString().trim() == "") ||
    //        gRelation == -1 ||
    //        (gRelation == '10' && (gRelationOther == null || gName.toString().trim() == "")) ||
    //        (gOccupation == null || gName.toString().trim() == "") || (gOccupationType == -1) ||
    //        (gMobile == null || gName.toString().trim() == "") )//end guardian
    //    ){
    //    document.getElementById("validationMsg").innerText = "Please provide the required fields";
    //    return false;

    
    }

    if (fatherOccupationType == "8" || motherOccupationType == "8")
    {
        fatherOrganisation = document.getElementById("MainContent_txtFatherOrganization").value;
        fatherDesignation = document.getElementById("MainContent_txtFatherDesignation").value;
        fatherServiceType = document.getElementById("MainContent_ddlFatherServiceType").value;

        motherOrganisation = document.getElementById("MainContent_txtMotherOrganization").value;
        motherDesignation = document.getElementById("MainContent_txtMotherDesignation").value;
        motherServiceType = document.getElementById("MainContent_ddlMotherServiceType").value;

        if (fatherOrganisation == null || fatherOrganisation.toString().trim() == "") {
            document.getElementById("MainContent_txtFatherOrganization").style.border = "1px solid rgb(255, 0, 0)";
        }
        if (fatherServiceType == -1) {
            document.getElementById("MainContent_ddlFatherServiceType").style.border = "1px solid rgb(255, 0, 0)";
        }
        if (fatherDesignation == null || fatherDesignation.toString().trim() == "") {
            document.getElementById("MainContent_txtFatherDesignation").style.border = "1px solid rgb(255, 0, 0)";
        }

        if (motherOrganisation == null || motherOrganisation.toString().trim() == "") {
            document.getElementById("MainContent_txtMotherOrganization").style.border = "1px solid rgb(255, 0, 0)";
        }
        if (motherServiceType == -1) {
            document.getElementById("MainContent_ddlMotherServiceType").style.border = "1px solid rgb(255, 0, 0)";
        }
        if (motherDesignation == null || motherDesignation.toString().trim() == "") {
            document.getElementById("MainContent_txtMotherDesignation").style.border = "1px solid rgb(255, 0, 0)";
        }

        if ((fatherOrganisation == null || fatherOrganisation.toString().trim() == "") || (fatherDesignation == null || fatherDesignation.toString().trim() == "")
            (fatherServiceType == "-1") || (motherOrganisation == null || motherOrganisation.toString().trim() == "")
            || (motherDesignation == null || motherDesignation.toString().trim() == "") || (motherServiceType == "-1"))
        {
            document.getElementById("validationMsg").innerText = "Please provide the required fields";
            return false;
        }
    }

    return true;
}