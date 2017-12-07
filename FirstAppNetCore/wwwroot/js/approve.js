var wid = $(window).width();
if (wid < 1020) {
    $(window).scroll(function (event) {
        var scroll = $(window).scrollTop();
        var scrollsave = $('#btnSave').scrollTop();
        var div = document.getElementById("fim").scrollTop;
        document.getElementById("btnSave").innerHTML = div;
        if (scroll > 5000) {
            
            $('#guiaL').text('');
        } else if (scroll < 3072) {
            $('#guiaL').text('Latest');
        }

    });
}else {
    $(window).scroll(function (event) {
        var scroll = $(window).scrollTop();
        if (scroll > 3072) {
            $('#guiaL').text('');
        } else if (scroll < 3072) {
            $('#guiaL').text('Latest');
        }

    });

}