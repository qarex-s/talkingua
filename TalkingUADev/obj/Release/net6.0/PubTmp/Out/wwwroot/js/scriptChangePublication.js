$(document).ready(function () {
    let photo = document.querySelectorAll('.view-photo img');
    photo.forEach((elem) => {
        if (elem.height / elem.width > 1.026) {
            elem.height = 549;
            console.log(1);
        } else if (elem.height / elem.width < 1.026) {
            elem.width = 539;
            console.log(2);
        } else {
            elem.width = 539;
            console.log(3);
        }
    });
}) 