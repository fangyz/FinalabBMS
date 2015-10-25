
$(function () {
    $('.am-slider').flexslider({
        animation: "slide",
        direction: "horizontal",
        animationLoop: true,
        slideshow: true,
        touch: true,
        directionNav: true,
        pauseOnAction: false,
        slideshowSpeed: 5000,
        animationSpeed: 600
    });
    $("#btn_next").click(function () {
        $('#my-confirm').modal({
            relatedTarget: this,
            onConfirm: function () {
                window.location = '/JoinUs/BeginTest/RegInfo';
            }
        });
    });
});
