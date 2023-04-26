function active(active) {
    let a = document.querySelectorAll('a');
    a.forEach(element => {
        if (element.innerText == active.innerText) {
            element.style.color = '#ffffffff';
        } else {
            element.style.color = '#ffffff80';
        }
    });
}

$(document).ready(function () {
    $('.burger').click(function (event) {
        $('.burger, .menu').toggleClass('active');
        $('body').toggleClass('lock');
    });
})