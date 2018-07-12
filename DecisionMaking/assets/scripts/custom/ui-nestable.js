var UINestable = function () {


   



    var updateOutput = function (e) {
        
        var list = e.length ? e : $(e.target),
            output = list.data('output');
        if (window.JSON) {
            output.val(window.JSON.stringify(list.nestable('serialize'))); //, null, 2));
        } else {
            output.val('JSON browser support required for this demo.');
        }
    };

    var updateOutputNotActive = function (e) {
        
        var list = e.length ? e : $(e.target),
            output = list.data('output');
        if (window.JSON) {
            output.val(window.JSON.stringify(list.nestable('serialize'))); //, null, 2));
        } else {
            output.val('JSON browser support required for this demo.');
        }
    };
    return {
        //main function to initiate the module
        init: function () {

            $(".ManageContentBoxes").each(function (index, value) {
                var contentType = $(this).data("contentType");


                $(this).find('.dd.ActiveList').nestable({
                    group: 1,
                    maxDepth: 1
                })
                .on('change', updateOutput);

                // activate Nestable for list 2
                $(this).find('.dd.NotActiveList').nestable({
                    group: 1,
                    maxDepth: 1
                })
                .on('change', updateOutputNotActive);

                // output initial serialised data
                updateOutput($(this).find('.ActiveList').data('output', $(this).find('.ActiveList_output')));
                updateOutput($(this).find('.NotActiveList').data('output', $(this).find('.NotActiveList_output')));



            });

            // activate Nestable for list 1


            //            $('#nestable_list_menu').on('click', function (e) {
            //                var target = $(e.target),
            //                    action = target.data('action');
            //                if (action === 'expand-all') {
            //                    $('.dd').nestable('expandAll');
            //                }
            //                if (action === 'collapse-all') {
            //                    $('.dd').nestable('collapseAll');
            //                }
            //            });

            //            $('#nestable_list_3').nestable();

        }

    };

} ();