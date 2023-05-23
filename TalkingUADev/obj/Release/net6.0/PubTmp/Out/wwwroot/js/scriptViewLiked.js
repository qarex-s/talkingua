let posts = document.querySelectorAll('.post img');
posts.forEach((elem) => {
    if (elem.width / elem.height >= 1.162) {
        elem.style.height = '234px';
        elem.style.position = 'relative';
        elem.style.top = '2px';
    } else if (elem.width / elem.height < 1.162) {
        elem.style.width = '272px';
        elem.style.position = 'relative';
        elem.style.top = '2px';
    }
})