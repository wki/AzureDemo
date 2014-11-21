$(document).ready(function () {
    $('._chart')
        .mouseenter(function () {
            var $img = $(this).find('img._overlay');
            $img.attr('src', $img.data('src'));
        });
})
