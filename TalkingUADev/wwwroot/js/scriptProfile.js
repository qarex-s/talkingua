console.log('cool');
let avatar = document.querySelectorAll('.head .nick img');
avatar.forEach((elem) => {
    if (elem.width > elem.height) {
        elem.style.height = '100%';
    } else if (elem.height >= elem.width) {
        elem.style.width = '100%';
    }
})