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