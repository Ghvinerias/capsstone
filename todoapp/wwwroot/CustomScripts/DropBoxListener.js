$(document).ready(function () {
    $('.ActiveCheck').change(function () {
        var self = $(this);
        var id = self.attr('id');
        var aaa = $("#dropdrop").val();
        var val = $(this).val();
        //var value = self.prop('selected');
        console.log(aaa);
        $.ajax({
            url: '/Todoes/AjaxEdit',
            data: {
                id: id,
                statusid:   val
            },
            type: 'POST',
            success: function (result) {
                $('#tableDiv').html(result)
                window.location.reload();
               
                
             
            }

        });
    });
})



