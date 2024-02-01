// checkboxy
document.addEventListener("DOMContentLoaded", function () {
    var checkboxes = document.querySelectorAll(".form-check-input");
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener("click", (e) => {
            if (checkbox.checked)
                checkbox.value = true;
            else
                checkbox.value = false;
        });
    });
});

// pocitadlo znaku
function updateCharacterCount(textarea, counterId) {
    var charCountSpan = document.getElementById(counterId);
    var currentLength = textarea.value.length;
    charCountSpan.innerText = 'Znaků: ' + currentLength;
}