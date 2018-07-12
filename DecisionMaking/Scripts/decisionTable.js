$(document).ready(function () {
    var UserStatusesTable = $('#statuses_table').dataTable({
        "sAjaxSource": "GetDecisions",
        "sAjaxDataProp": "data",

        //        "ajax": {
        //            "url": "GetDecisions",
        //            "type": "POST"
        //        },                 // string Is2015ObjectiveFilled = feedbackUserForm.Is2015ObjectiveFilled == 1 ? "בוצע" : "למילוי";
        //string IsObjectiveFilledByEmployee = feedbackUserForm.IsObjectiveFilledByEmployee == 2 ? "בוצע" : "למילוי";
        //string IsObjectiveFilledByManager = feedbackUserForm.IsObjectiveFilledByManager == 2 ? "בוצע" : "למילוי";
        //string Is2016ObjectivesSetted = feedbackUserForm.Is2016ObjectivesSetted == 1 ? "בוצע" : "למילוי";

        "aoColumns": [
                    { "mData": "openeddate", "visible": true },
                    { "mData": "Department", "visible": true },
                    { "mData": "Subject", "visible": true },
                    { "mData": "Requireddecision", "visible": true },
                    { "mData": "Currentstatedesc", "visible": true },
                    { "mData": "Decision", "visible": true }
                  


        // { "data": "IsMAnagerAssessment", "visible": true, "mRender": function (obj) { var source = obj; return '<a href ="GetEmsDoc?fileName=' + obj + '"><span>הורד מסמך</span><i class="fa fa-file-text"></i></a>'; } },



           ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            // Bold the grade for all 'A' grade browsers

            debugger;
            $(nRow).attr("data-id", aData["ID"]);

        }

    });

    var selectRowEvents = function () {
        debugger;
        var rowId = $(this).data('id');
        var url = "DisplayDocumentDecision?DecisionID=" + rowId
        window.open(url, '_blank');
    }

    var initEvents = function () {
        $('#statuses_table tbody').on('click', 'tr', selectRowEvents);

    


    };


    initEvents();
});        