function validateEducationForm() {
    var eduCat;
    eduCat = document.getElementById("MainContent_hfEduCat").value;

    var secExamType, secEducationBoard, secInstitute, secRollNo, secGrpSub, secDivClass,
        secCgpa, secPassingYear;

    secExamType = document.getElementById("MainContent_ddlSec_ExamType").value;
    secEducationBoard = document.getElementById("MainContent_ddlSec_EducationBrd").value;
    secInstitute = document.getElementById("MainContent_txtSec_Institute").value;
    secRollNo = document.getElementById("MainContent_txtSec_RollNo").value;
    secGrpSub = document.getElementById("MainContent_ddlSec_GrpOrSub").value;
    secDivClass = document.getElementById("MainContent_ddlSec_DivClass").value;
    //var secDivClassText = secDivClass.options["secDivClass.selectedIndex"].text;
    secCgpa = document.getElementById("MainContent_txtSec_CgpaScore").value;
    secPassingYear = document.getElementById("MainContent_ddlSec_PassingYear").value;

    if (secExamType == -1) {
        //document.getElementById("validationMsg").innerText = "";
        // document.getElementById("validationMsg").innerText = "Please provide the required fields";
        document.getElementById("MainContent_ddlSec_ExamType").style.border = "1px solid rgb(255, 0, 0)";
        //return false;
    }

    if (secEducationBoard == -1) {
        //document.getElementById("validationMsg").innerText = "";
        //document.getElementById("validationMsg").innerText = "Please provide the required fields";
        document.getElementById("MainContent_ddlSec_EducationBrd").style.border = "1px solid rgb(255, 0, 0)";
        //console.log("secEducationBoard false");
        //return false;
    }

    if (secInstitute.toString().trim() == "" || secInstitute == null) {
        //document.getElementById("validationMsg").innerText = "";
        //document.getElementById("validationMsg").innerText = "Please provide the required fields";
        document.getElementById("MainContent_txtSec_Institute").style.border = "1px solid rgb(255, 0, 0)";
        //console.log("secInstitute false");
        //return false;
    }

    if (secRollNo.toString().trim() == "" || secRollNo == null) {
        //document.getElementById("validationMsg").innerText = "";
        //document.getElementById("validationMsg").innerText = "Please provide the required fields";
        document.getElementById("MainContent_txtSec_RollNo").style.border = "1px solid rgb(255, 0, 0)";
        //console.log("secRoll false");
        //return false;
    }

    if (secGrpSub == -1) {
        //document.getElementById("validationMsg").innerText = "";
        //document.getElementById("validationMsg").innerText = "Please provide the required fields";
        document.getElementById("MainContent_ddlSec_GrpOrSub").style.border = "1px solid rgb(255, 0, 0)";
        //return false;
    }

    if (secDivClass == -1 && eduCat == 'masters') {
        //document.getElementById("validationMsg").innerText = "";
        //document.getElementById("validationMsg").innerText = "Please provide the required fields";
        document.getElementById("MainContent_ddlSec_DivClass").style.border = "1px solid rgb(255, 0, 0)";
        //return false;
    }

    if (secDivClass == '5' && eduCat == 'masters' &&(secCgpa.toString().trim() == "" || secCgpa == null)) {
        //document.getElementById("validationMsg").innerText = "";
        //document.getElementById("validationMsg").innerText = "Please provide the required fields";
        document.getElementById("MainContent_txtSec_CgpaScore").style.border = "1px solid rgb(255, 0, 0)";
        //return false;
    }
    else if (eduCat == 'bachelors' && (secCgpa.toString().trim() == "" || secCgpa == null)) {
        document.getElementById("MainContent_txtSec_CgpaScore").style.border = "1px solid rgb(255, 0, 0)";
    }

    if (secPassingYear == -1) {
        //document.getElementById("validationMsg").innerText = "";
        //document.getElementById("validationMsg").innerText = "Please provide the required fields";
        document.getElementById("MainContent_ddlSec_PassingYear").style.border = "1px solid rgb(255, 0, 0)";
        //return false;
    }

    //-------------------------------------------------------------------------------------------------------


    var higherSecExamType, higherSecEducationBoard, higherSecInstitute, higherSecRollNo,
        higherSecGrpSub, higherSecDivClass, higherSecCgpa, higherSecPassingYear;

    higherSecExamType = document.getElementById("MainContent_ddlHigherSec_ExamType").value;
    if (higherSecExamType == '-1') {
        document.getElementById("MainContent_ddlHigherSec_ExamType").style.border = "1px solid rgb(255, 0, 0)";
    }

    higherSecEducationBoard = document.getElementById("MainContent_ddlHigherSec_EducationBrd").value;
    if (higherSecEducationBoard == '-1') {
        document.getElementById("MainContent_ddlHigherSec_EducationBrd").style.border = "1px solid rgb(255, 0, 0)";
    }

    higherSecInstitute = document.getElementById("MainContent_txtHigherSec_Institute").value;
    if (higherSecInstitute.toString().trim() == "" || higherSecInstitute == null) {
        document.getElementById("MainContent_txtHigherSec_Institute").style.border = "1px solid rgb(255, 0, 0)";
    }

    higherSecRollNo = document.getElementById("MainContent_txtHigherSec_RollNo").value;
    if (higherSecRollNo.toString().trim() == "" || higherSecRollNo == null) {
        document.getElementById("MainContent_txtHigherSec_RollNo").style.border = "1px solid rgb(255, 0, 0)";
    }

    higherSecGrpSub = document.getElementById("MainContent_ddlHigherSec_GrpOrSub").value;
    if (higherSecGrpSub == '-1') {
        document.getElementById("MainContent_ddlHigherSec_GrpOrSub").style.border = "1px solid rgb(255, 0, 0)";
    }

    higherSecDivClass = document.getElementById("MainContent_ddlHigherSec_DivClass").value;
    if (higherSecDivClass == "-1" && eduCat == 'masters') {
        document.getElementById("MainContent_ddlHigherSec_DivClass").style.border = "1px solid rgb(255, 0, 0)";
    }

    higherSecCgpa = document.getElementById("MainContent_txtHigherSec_CgpaScore").value;
    if (higherSecDivClass == '5' && eduCat == 'masters' && (higherSecCgpa.toString().trim() == "" || higherSecCgpa == null)) {
        document.getElementById("MainContent_txtHigherSec_CgpaScore").style.border = "1px solid rgb(255, 0, 0)";
    } else if (eduCat == 'bachelors' && (higherSecCgpa.toString().trim() == "" || higherSecCgpa == null)) {
        document.getElementById("MainContent_txtHigherSec_CgpaScore").style.border = "1px solid rgb(255, 0, 0)";
    }

    higherSecPassingYear = document.getElementById("MainContent_ddlHigherSec_PassingYear").value;
    if (higherSecPassingYear == '-1') {
        document.getElementById("MainContent_ddlHigherSec_PassingYear").style.border = "1px solid rgb(255, 0, 0)";
    }

    //-------------------------------------------------------------------------------------------------------

    var undInstitute, undProgramDegreeId, undProgramOther, undGrpSub, undDivClass, undCgpa, undYear;

    //eduCat == 'masters'

    

    if (eduCat == 'masters') {

        undInstitute = document.getElementById("MainContent_txtUndergrad_Institute").value;
        undProgramDegreeId = document.getElementById("MainContent_ddlUndergrad_ProgramDegree").value;
        undProgramOther = document.getElementById("MainContent_txtUndergrad_ProgOthers").value;
        //undGrpSub = document.getElementById("MainContent_ddlUndergrad_GrpOrSub").value;
        undDivClass = document.getElementById("MainContent_ddlUndergrad_DivClass").value;
        undCgpa = document.getElementById("MainContent_txtUndergrad_CgpaScore").value;
        undYear = document.getElementById("MainContent_ddlUndergrad_PassingYear").value;
        
        if (undInstitute.toString().trim() == "" || undInstitute == null)
            document.getElementById("MainContent_txtUndergrad_Institute").style.border = "1px solid rgb(255, 0, 0)";

        if(undProgramDegreeId == '-1')
            document.getElementById("MainContent_ddlUndergrad_ProgramDegree").style.border = "1px solid rgb(255, 0, 0)";

        if(undProgramDegreeId == '55' && (undProgramOther.toString().trim() == "" || undProgramOther == null))
            document.getElementById("MainContent_txtUndergrad_ProgOthers").style.border = "1px solid rgb(255, 0, 0)";

        //if(undGrpSub == '-1')
        //    document.getElementById("MainContent_ddlUndergrad_GrpOrSub").style.border = "1px solid rgb(255, 0, 0)";

        if(undDivClass == '-1')
            document.getElementById("MainContent_ddlUndergrad_DivClass").style.border = "1px solid rgb(255, 0, 0)";

        if(undDivClass == '5' && (undCgpa.toString().trim() == "" || undCgpa == null))
            document.getElementById("MainContent_txtUndergrad_CgpaScore").style.border = "1px solid rgb(255, 0, 0)";

        if (undYear == '-1')
            document.getElementById("MainContent_ddlUndergrad_PassingYear").style.border = "1px solid rgb(255, 0, 0)";
    }

    //-------------------------------------------------------------------------------------------------------



    //-------------------------------------------------------------------------------------------------------

    //if (secExamType == -1 || secEducationBoard == -1 ||
    //    (secInstitute.toString().trim() == "" || secInstitute == null) ||
    //    (secRollNo.toString().trim() == "" || secRollNo == null) ||
    //    secGrpSub == -1 || (secDivClass == -1 && eduCat == 'masters') ||
    //    (secDivClass == '5' && eduCat == 'masters' && (secCgpa.toString().trim() == "" || secCgpa == null)) ||
    //    (eduCat == 'bachelors' && (secCgpa.toString().trim() == "" || secCgpa == null)) ||
    //    secPassingYear == -1 ||

    //    higherSecExamType == -1 || higherSecEducationBoard == -1 ||
    //    (higherSecInstitute.toString.trim() == "" || higherSecInstitute == null) ||
    //    (higherSecRollNo.toString().trim() == "" || higherSecRollNo == null) ||
    //    higherSecGrpSub == '-1' || (higherSecDivClass == "-1" && eduCat == 'masters') ||
    //    (higherSecDivClass == '5' && eduCat == 'masters' && (higherSecCgpa.toString().trim() == "" || higherSecCgpa == null)) ||
    //    (eduCat == 'bachelors' && (higherSecCgpa.toString().trim() == "" || higherSecCgpa == null)) ||
    //    higherSecPassingYear == '-1') {

    //    document.getElementById("validationMsg").innerText = "Please provide the required fields";

    //    return false;
    //}

    if (eduCat == 'bachelors') {
        if (secExamType == -1 || secEducationBoard == -1 ||
        (secInstitute.toString().trim() == "" || secInstitute == null) ||
        (secRollNo.toString().trim() == "" || secRollNo == null) ||
        secGrpSub == -1 || (secDivClass == -1 && eduCat == 'masters') ||
        (secDivClass == '5' && eduCat == 'masters' && (secCgpa.toString().trim() == "" || secCgpa == null)) ||
        (eduCat == 'bachelors' && (secCgpa.toString().trim() == "" || secCgpa == null)) ||
        secPassingYear == -1 ||

        higherSecExamType == -1 || higherSecEducationBoard == -1 ||
        (higherSecInstitute.toString().trim() == "" || higherSecInstitute == null) ||
        (higherSecRollNo.toString().trim() == "" || higherSecRollNo == null) ||
        higherSecGrpSub == '-1' || (higherSecDivClass == "-1" && eduCat == 'masters') ||
        (higherSecDivClass == '5' && eduCat == 'masters' && (higherSecCgpa.toString().trim() == "" || higherSecCgpa == null)) ||
        (eduCat == 'bachelors' && (higherSecCgpa.toString().trim() == "" || higherSecCgpa == null)) ||
        higherSecPassingYear == '-1') {

            document.getElementById("validationMsg").innerText = "Please provide the required fields";

            return false;
        }
    }
    else if (eduCat == 'masters') {
        if (secExamType == -1 || secEducationBoard == -1 ||
        (secInstitute.toString().trim() == "" || secInstitute == null) ||
        (secRollNo.toString().trim() == "" || secRollNo == null) ||
        secGrpSub == -1 || (secDivClass == -1 && eduCat == 'masters') ||
        (secDivClass == '5' && eduCat == 'masters' && (secCgpa.toString().trim() == "" || secCgpa == null)) ||
        (eduCat == 'bachelors' && (secCgpa.toString().trim() == "" || secCgpa == null)) ||
        secPassingYear == -1 ||

        higherSecExamType == -1 || higherSecEducationBoard == -1 ||
        (higherSecInstitute.toString().trim() == "" || higherSecInstitute == null) ||
        (higherSecRollNo.toString().trim() == "" || higherSecRollNo == null) ||
        higherSecGrpSub == '-1' || (higherSecDivClass == "-1" && eduCat == 'masters') ||
        (higherSecDivClass == '5' && eduCat == 'masters' && (higherSecCgpa.toString().trim() == "" || higherSecCgpa == null)) ||
        (eduCat == 'bachelors' && (higherSecCgpa.toString().trim() == "" || higherSecCgpa == null)) ||
        higherSecPassingYear == '-1' ||
        
        (undInstitute.toString().trim() == "" || undInstitute == null) ||
        (undProgramDegreeId == '-1') ||
        (undProgramDegreeId == '55' && (undProgramOther.toString().trim() == "" || undProgramOther == null)) ||
        (undDivClass == '-1') || 
        (undDivClass == '5' && (undCgpa.toString().trim() == "" || undCgpa == null)) ||
        (undYear == '-1')
        ) {

            document.getElementById("validationMsg").innerText = "Please provide the required fields";

            return false;
        }
    }

    return true;
}