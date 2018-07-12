$(function () {

    App.init();
    ComponentsPickers.init();

    var availableTags = userDetails;

    $(".autocomplete").autocomplete({
        source: availableTags
    });

    var notifysettings = {

        horizontalEdge: "bottom",
        life: "2000",
        sticky: false,
        theme: "teal",
        verticalEdge: "left"
    };

    jQuery.validator.addMethod("AdantageRequired", function (value, element) {

        return true;
    }, "* Amount must be greater than zero");
    jQuery.validator.addMethod("DisAdantageRequired", function (value, element) {

        return true;
    }, "* Amount must be greater than zero");
    jQuery.validator.addMethod("TwoOptionsRequired", function (value, element) {
        //chech if there are at least two options in decision as requiered
        var result = $(element).closest("#CreateDecisionForm").find(".decision-option-input:not(.delete)").length > 1;
        return result;
    }, "יש להכניס שתי אפשרויות לפחות");



    varlidationSettings = {
        errorElement: 'span', //default input error message container
        errorClass: 'help-block', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            Department: {

                required: true

            },
            Requireddecision: {

                required: true

            },
            DecisionElement: {
                TwoOptionsRequired: true
            },

            Subject: {
                required: true
            },
            Currentstatedesc: {
                required: true
            },
            openeddate: {
                required: true
            }



        },
        errorPlacement: function (error, element) { // render error placement for each input type
            debugger;
            if (element.attr('type') == "radio") {
                errorPlace = $(element).closest("div");
                error.insertAfter(errorPlace);
            }

            else {
                error.insertAfter(element); // for other inputs, just perform default behavior
            }
        },
        invalidHandler: function (event, validator) { //display error alert on form submit       
        },
        highlight: function (element) { // hightlight error inputs
            $(element)
                                    .closest('.form-group').addClass('has-error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change done by hightlight
            $(element)
                                    .closest('.form-group').removeClass('has-error'); // set error class to the control group
        },

        success: function (label) {
            label
                                    .closest('.form-group').removeClass('has-error'); // set success class to the control group
        },

        submitHandler: function (form) {

            var $button = $(form).find("button[type=submit]");

            var PostModel = { "DecisionDocument": { "DetailsType": "advantages", "DecisionOptions": [], "NextSteps": [] } };


            $(form).find("input:not([type='radio']):not(.no-serialized), textarea:not(.no-serialized), select:not(.no-serialized)").each(function () {
                PostModel.DecisionDocument[$(this).attr('name')] = $(this).val();
            });
            var options = $(form).find(".DecisionOptions .decision-option-input");
            var nextSteps = $(form).find(".NextSteps .next-step-input");

            options.each(function (index, value) {

                var status = $(this).hasClass("delete") ? 0 : 1;
                var DecisionOption = { "status": status, "OptionAdvantages": [], "OptionRisks": [] }; //PostModel.DecisionDocument.DecisionOptions[index]

                var index = index;
                var optionDescription = $(this).find('input[name="Description"]').val();
                var optionID = $(this).data("optionId");
                DecisionOption["Description"] = optionDescription;
                DecisionOption["ID"] = optionID;

                $(this).find(".option-advantages-inputs input:not(.not-submit),.option-advantages-inputs textarea:not(.not-submit)").each(function (index, value) {
                    var advantage = {};
                    var status = $(this).closest(".advantage-wrapper").hasClass("delete") ? 0 : 1;
                    advantage[$(this).attr('name')] = $(this).val().trim();
                    advantage["ID"] = $(this).data("advandateId");
                    advantage["STATUS"] = status;
                    DecisionOption["OptionAdvantages"][index] = advantage;
                });



                $(this).find(".option-risks-inputs input:not(.not-submit),.option-risks-inputs textarea:not(.not-submit)").each(function (index, value) {
                    var risk = {};
                    var status = $(this).closest(".risk-wrapper").hasClass("delete") ? 0 : 1;
                    risk["ID"] = $(this).data("riskId");
                    risk["STATUS"] = status;
                    risk[$(this).attr('name')] = $(this).val().trim();
                    DecisionOption["OptionRisks"][index] = risk;
                });

                PostModel.DecisionDocument.DecisionOptions[index] = DecisionOption;
            });


            nextSteps.each(function (index, value) {

                var nextStepID = $(this).data('nextStepId');
                var status = $(this).hasClass("delete") ? 0 : 1;
                var NextStep = { ID: nextStepID }
                NextStep["status"] = status;
                $(value).find("input").each(function () {
                    NextStep[$(this).attr('name')] = $(this).val();
                });


                PostModel.DecisionDocument.NextSteps[index] = NextStep;
            });



            var jsonData = JSON.stringify({ 'PostModel': PostModel });




            var action = $(form).attr("action");
            $.ajax({
                type: "POST",
                url: action,
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                data: jsonData,
                success: function (data, textStatus, jqXHR) {

                    $.notific8('zindex', 11500);
                    $.notific8("השינויים נשמרו בהצלחה", notifysettings);
                    $button.removeAttr('disabled');
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    $button.removeAttr('disabled');
                }
            });
            return false;
        }
    };

    var addNewAdvantage = function () {
        var lastInputElement = $(this).closest(".option-advantages-inputs").find(".advantage-wrapper").last();

        $.ajax({
            url: baseUrl + "home/GetNewAdvantage",
            dataType: 'html',
            success: function (newElement) {
                lastInputElement.after(newElement);
            },
            error: function (data) {
            }
        });
        return false;

    };

    var addNewRisk = function () {
        var lastInputElement = $(this).closest(".option-risks-inputs").find(".risk-wrapper").last();

        $.ajax({
            url: baseUrl + "home/GetNewRisk",
            dataType: 'html',
            success: function (newElement) {
                lastInputElement.after(newElement);
            },
            error: function (data) {
            }
        });
        return false;
    };

    var AddNewOption = function () {


        var decisionOptions = $(this).closest(".DecisionOptions").find(".options");
        var OptionNumber = decisionOptions.find(".option").last().data('optionNumber') + 1;

        $.ajax({
            url: baseUrl + 'home/GetOption',
            data: { lastOptionNumber: OptionNumber },
            dataType: 'html',
            success: function (data) {

                decisionOptions.append(data);
            },
            error: function (data) {

            }
        });
        return false;
    };

    var AddNextStep = function () {

        nextStepsTable = $(this).closest(".NextSteps").find(".next-steps-table");
        //  var OptionNumber = decisionOptions.find(".option").last().data('optionNumber') + 1;

        $.ajax({
            url: baseUrl + "home/GetNextStep",
            data: { nextStepNumber: 1 },
            dataType: 'html',
            success: function (data) {


                nextStepsTable.append(data);
                ComponentsPickers.init();
                //  $(".date-picker").datepicker();
            },
            error: function (data) {

            }
        });
        return false;
    };

    var DeleteNextStep = function () {

        var elementToDelete = $(this);
        elementToDelete.closest(".next-step-input").addClass("delete");

    };

    var CancelDeleteNextStep = function () {

        var elementToDelete = $(this);
        elementToDelete.closest(".next-step-input").removeClass("delete");

    };

    var DeleteAdvantage = function () {
        $(this).closest(".advantage-wrapper").addClass("delete");
    }
    var DeleteRisk = function () {
        $(this).closest(".risk-wrapper").addClass("delete");
    }
    var CancleDeleteRisk = function () {
        $(this).closest(".risk-wrapper").removeClass("delete");
    };
    var CancleDeleteAdvantage = function () {
        $(this).closest(".advantage-wrapper").removeClass("delete");
    };
    var DeleteOption = function () {
        $(this).closest(".decision-option-input").addClass("delete");
    }
    var CancleDeleteOption = function () {
        $(this).closest(".decision-option-input").removeClass("delete");
    }


    var addRulesForDecisionOptions = function () {
        $('.option-advantages-inputs-element').each(function () {
            $(this).rules('add', {

                AdantageRequired: true

            });
        });
        debugger;
        $('.option-risks-inputs-element').each(function () {
            $(this).rules('add', {


                DisAdantageRequired: true
            });
        });
    }






    $("#CreateDecisionForm").validate(varlidationSettings);



    addRulesForDecisionOptions();


    $(".DecisionOptions").on("click", ".add-adventage", addNewAdvantage);
    $(".DecisionOptions").on("click", ".add-risk", addNewRisk);
    $(".DecisionOptions").on("click", ".delete-advantage", DeleteAdvantage);
    $(".DecisionOptions").on("click", ".delete-option", DeleteOption);
    $(".DecisionOptions").on("click", ".delete-option-cancle", CancleDeleteOption);

    $(".DecisionOptions").on("click", ".delete-risk-cancle", CancleDeleteRisk);
    $(".DecisionOptions").on("click", ".delete-advantage-cancle", CancleDeleteAdvantage);
    $(".DecisionOptions").on("click", ".delete-risk", DeleteRisk);
    debugger;
    $(".addNewOption").click(AddNewOption);
    $(".DecisionOptions").on("click", ".add-venture", addNewRisk);
    $(".addNextStep").click(AddNextStep);
    $(".next-steps-table").on("click", ".delete-next-step", DeleteNextStep);
    $(".next-steps-table").on("click", ".delete-next-step-cancel", CancelDeleteNextStep);

});