document.documentElement.classList.add(localStorage.getItem("theme") || "light-theme");

document.addEventListener("DOMContentLoaded", () => {
    var lightModeButton = document.getElementById("light-mode-button");
    var darkModeButton = document.getElementById("dark-mode-button");

    var theme = localStorage.getItem("theme") || "light-theme";
    if (lightModeButton != null && theme == "light-theme")
        lightModeButton.parentElement.style.display = "none";
    else if (darkModeButton != null && theme == "dark-theme")
        darkModeButton.parentElement.style.display = "none";

    function setTheme(theme) {
        document.documentElement.classList.remove("light-theme", "dark-theme");
        document.documentElement.classList.add(theme);
        localStorage.setItem("theme", theme);

        if (lightModeButton != null && darkModeButton != null) {
            if (lightModeButton != null && theme == "light-theme") {
                lightModeButton.parentElement.style.display = "none";
                darkModeButton.parentElement.style.display = "block";
            }
            else if (darkModeButton != null && theme == "dark-theme") {
                darkModeButton.parentElement.style.display = "none";
                lightModeButton.parentElement.style.display = "block";
            }
        }
    }

    if (lightModeButton != null)
        lightModeButton.addEventListener("click", () => setTheme("light-theme"));

    if (darkModeButton != null)
        darkModeButton.addEventListener("click", () => setTheme("dark-theme"));
});