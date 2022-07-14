$(document).ready(function (){
    $.ajax({
        url: '/ToDoes/GetToDoTable',
        success: function (result) {
            $('#tableDiv').html(result);
           
          
        }
    })
})