let nicks = document.querySelectorAll('.last-stories .story p');
nicks.forEach((elem) => {
    if (elem.innerHTML.length > 7 && document.documentElement.clientWidth >= 767) {
        elem.innerHTML = elem.innerHTML.slice(0, 5) + '..';
    } else if (elem.innerHTML.length > 4 && document.documentElement.clientWidth < 767) {
        elem.innerHTML = elem.innerHTML.slice(0, 3) + '..';
    }
})

let myStory = document.querySelectorAll('.my-story img');
myStory.forEach((elem) => {
    if (elem.width > elem.height) {
        elem.style.height = '100%';
    } else if (elem.height >= elem.width) {
        elem.style.width = '100%';
    }
})

let stories = document.querySelectorAll('.last-stories .story img');
stories.forEach((elem) => {
    if (elem.width > elem.height) {
        elem.style.height = '100%';
    } else if (elem.height >= elem.width) {
        elem.style.width = '100%';
    }
})


let avatars = document.querySelectorAll('.nickname .story img');
avatars.forEach((elem) => {
    if (elem.width > elem.height) {
        elem.style.height = '100%';
    } else if (elem.height >= elem.width) {
        elem.style.width = '100%';
    }
})

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

let storyImg = document.querySelectorAll('.story-img img');
storyImg.forEach((elem) => {
    if (elem.width > elem.height) {
        elem.width = 700;
    } else if (elem.height >= elem.width) {
        elem.height = 700;
    }
})

document.addEventListener("DOMContentLoaded", function () {
    var scrollbar = document.body.clientWidth - window.innerWidth + 'px';
    console.log(scrollbar);
    document.querySelector('[href="#openModal"]').addEventListener('click', function () {
        //document.body.style.overflow = 'hidden';
        //document.querySelector('#openModal').style.marginLeft = scrollbar;
    });
    document.querySelector('[href="#close"]').addEventListener('click', function () {
        document.body.style.overflow = 'visible';
        document.querySelector('#openModal').style.marginLeft = '0px';
    });
});
