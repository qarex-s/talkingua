$(document).ready(function () {
    $('.user').click(function (event) {
        $('.user.active').toggleClass('active');
        $(this).toggleClass('active');
    });
    let myAvatar = document.querySelectorAll('.active-avatar img, .nickname img');
    myAvatar.forEach((elem) => {
        if (elem.width > elem.height) {
            elem.style.height = '100%';
        } else if (elem.height >= elem.width) {
            elem.style.width = '100%';
        }
    });
        let myFriend = document.querySelectorAll('.our-message img, .their-message img, .input>div img');
        myFriend.forEach((elem) => {
            if (elem.width > elem.height) {
                elem.style.height = '100%';
            } else if (elem.height >= elem.width) {
                elem.style.width = '100%';
            }
        });


    document.addEventListener("DOMContentLoaded", function () {
        var scrollbar = document.body.clientWidth - window.innerWidth + 'px';
        console.log(scrollbar);
        document.querySelector('[href="#openModal"]').addEventListener('click', function () {
            document.body.style.overflow = 'hidden';
            document.querySelector('#openModal').style.marginLeft = scrollbar;
        });
        document.querySelector('[href="#close"]').addEventListener('click', function () {
            document.body.style.overflow = 'visible';
            document.querySelector('#openModal').style.marginLeft = '0px';
        });
    });
})

