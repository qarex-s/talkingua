let avatar = document.querySelectorAll('.head .nick img');
avatar.forEach((elem) => {
    if (elem.width > elem.height) {
        elem.style.height = '100%';
    } else if (elem.height >= elem.width) {
        elem.style.width = '100%';
    }
})

let posts = document.querySelectorAll('.post img');
posts.forEach((elem) => {
    if (elem.width / elem.height >= 1.162) {
        elem.style.height = '234px';
        elem.style.position = 'relative';
        elem.style.top = '2px';
    } else if (elem.width / elem.height < 1.162) {
        elem.style.width = '272px';
    }
})