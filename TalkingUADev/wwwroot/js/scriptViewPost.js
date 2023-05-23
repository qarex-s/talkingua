let posts = document.querySelectorAll('.photo img');
posts.forEach((elem) => {
    if (elem.height / elem.width > 1.007) {
        elem.height = 553;
    } else if (elem.height / elem.width < 1.007) {
        elem.width = 549;
    } else {
        elem.width = 549;
    }
})

let avatars = document.querySelectorAll('.nickname img');
console.log(avatars.length);
avatars.forEach((elem) => {
    if (elem.width > elem.height) {
        elem.style.height = '100%';
    } else if (elem.height >= elem.width) {
        elem.style.width = '100%';
    }
})