let posts = document.querySelectorAll('.photo img');
posts.forEach((elem) => {
    console.log('acaca');
    if (elem.height / elem.width > 1.1465) {
        elem.style.width = '100%';
    } else if (elem.height / elem.width < 1.1465) {
        elem.style.width = '100%';
    } else {
        elem.style.width = '100%';
    }
})