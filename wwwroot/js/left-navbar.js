// Handling arrow click event in left nevbar
document.addEventListener("DOMContentLoaded", () => {
    const expandableCategoryArrows = document.querySelectorAll(".side-navbar__category i.fa-chevron-right");

    expandableCategoryArrows.forEach(arrow => {
        arrow.addEventListener("click", () => {
            arrow.classList.toggle("side-navbar__arrow-expanded");
            var ul = arrow.nextElementSibling.nextElementSibling;
            if (ul != null) {
                ul.classList.toggle("side-navbar__category_expanded");
                ul.classList.toggle("side-navbar__category_collapsed");
            }
        });
    });
});

// Fucntion for hiding/showing left category navigation
function toggleLeftNavbar() {
    var toggleButton = document.querySelector("#left-navbar-button > i");
    if (toggleButton == null)
        return;
    var aside = document.querySelector("main > aside");
    var leftNavbar = aside.querySelector("nav.side-navbar");
    if (leftNavbar == null)
        return;
    // Hide
    if (toggleButton.classList.contains("left-navbar-button_on")) {
        toggleButton.classList.remove("left-navbar-button_on");
        toggleButton.classList.add("left-navbar-button_off");
        aside.classList.remove("lef-navbar-shadow");
        leftNavbar.style.display = "none";

        localStorage.setItem("sideNavbar", "false");
    }
    // Show
    else {
        toggleButton.classList.remove("left-navbar-button_off");
        toggleButton.classList.add("left-navbar-button_on");
        aside.classList.add("lef-navbar-shadow");
        leftNavbar.style.display = "block";

        localStorage.setItem("sideNavbar", "true");
    }
}
document.addEventListener("DOMContentLoaded", () => {
    const isLeftNavbarOpen = localStorage.getItem("sideNavbar");
    if (isLeftNavbarOpen == "false" || screen.width <= 767)
        toggleLeftNavbar();
});

// Functions for expanding left category navigation to currently loaded category or article
document.addEventListener("DOMContentLoaded", () => {
    if (typeof categoryId !== 'undefined' && categoryId) {
        expandToCateogry(categoryId);
    }
    else if (typeof articleId !== 'undefined' && articleId) {
        expandToArticle(articleId);
    }
});
function expandToCateogry(categoryId) {
    var currentCategory = document.querySelector(`[data-categoryid="${categoryId}"]`);
    if (!currentCategory)
        return;

    // Expand all parent categories in the navigation tree
    let parentLi = currentCategory;
    while (parentLi.tagName == 'LI') {
        var currentCategoryArrow = parentLi.querySelector("i.fa-chevron-right");
        if (!currentCategoryArrow.classList.contains("side-navbar__arrow-expanded"))
            currentCategoryArrow.classList.add("side-navbar__arrow-expanded");
        // Expand selected category subcategories and articles list
        var ul = currentCategoryArrow.nextElementSibling.nextElementSibling;
        if (ul.classList.contains("side-navbar__category_collapsed"))
            ul.classList.remove("side-navbar__category_collapsed");
        if (!ul.classList.contains("side-navbar__category_expanded"))
            ul.classList.add("side-navbar__category_expanded");
        parentLi = parentLi.parentElement.parentElement;
    }
}
function expandToArticle(articleId) {
    var currentArticle = document.querySelector(`[data-articleid="${articleId}"]`);
    if (!currentArticle)
        return;

    // highlight selected article
    if (!currentArticle.classList.contains("side-navbar__article_selected"))
        currentArticle.classList.add("side-navbar__article_selected");

    // expand article category
    var articleCategoryId = currentArticle.parentElement.parentElement.getAttribute('data-categoryid');
    expandToCateogry(articleCategoryId);
}