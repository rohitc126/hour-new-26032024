$(document).ready(function () {

   var hdnHMR_ID = $('#hdnHMR_ID').val();
        var hdnIS_LOCKED = $('#hdnIS_LOCKED').val();

        if (parseInt(hdnHMR_ID) > 0) {
            if (hdnIS_LOCKED == "0") {
                $('#btnSubmit').hide();
                $('#btnUpdate').show();
                $('#btnLock').show();
            }
            else {
                $('#btnReset').hide();
                $('#btnSubmit').hide();
                $('#btnUpdate').hide();
                $('#btnLock').hide();
                $('#tbodyList').attr("disabled", "disabled");
            }
        }
        else {

            $('#btnSubmit').show();
            $('#btnUpdate').hide();
            $('#btnLock').hide();
        }
    });
   
