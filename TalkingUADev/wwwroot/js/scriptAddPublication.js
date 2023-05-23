var dt = new DataTransfer();

$('.input-file input[type=file]').on('change', function () {
    let $files_list = $(this).closest('.input-file').next();
    $files_list.empty();

    for (var i = 0; i < this.files.length; i++) {
        let file = this.files.item(i);
        dt.items.add(file);

        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onloadend = async function () {
            let new_file_input = '<img class="input-file-list-img" src="' + reader.result + '">';
            await $files_list.append(new_file_input);
            let photo = document.querySelectorAll('.view-photo img');
            photo.forEach((elem) => {
                console.log(elem.height);
                console.log(elem.width);
                if (elem.height / elem.width > 1.026) {
                    elem.height = 553;
                    console.log(1);
                } else if (elem.height / elem.width < 1.026) {
                    elem.width = 539;
                    console.log(2);
                } else {
                    elem.width = 539;
                    console.log(3);
                }
            });
        }
    };
    this.files = dt.files;
});