function validateForm() {
    var textArea = document.getElementById("textMessage");
    if (textArea.value.trim() == "") {
        alert("Please enter a comment.");
        return false;
    }
    return true;
}
