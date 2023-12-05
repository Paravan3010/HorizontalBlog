var navBar
document.addEventListener("DOMContentLoaded", function () {
    loadNavbar();
    actualizeTopPosition();
});
// On resize, top position of the top navbar is actualized
addEventListener("resize", function () {
    actualizeTopPosition();
});

function loadNavbar() {
    if (navBar == null)
        navBar = document.querySelector(".main-header .top-navbar-container");
}

// Defines the desired top position of the top nav toolbar in pixes
let navBarTopPosition = 150;
let isNavBarSticky = false;

// Once navbar top position is reached, it changes to sticky
// This cannot be done via CSS, since the site has no given heigth
function onScroll()
{
    // When page is scrolled down and reloaded onScroll is in some instances called BEFORE
    // DOMContentLoaded event is executed. Therefore we must ensure that navBar is loaded.
    loadNavbar();

    var placeholder = document.querySelector(".main-header .top-navbar-container-placeholder");

    if (!isNavBarSticky && window.scrollY >= navBarTopPosition) {
        isNavBarSticky = true;

        // Once the navbar becomes position fixed a placeholder must take its space to
        // prevent content jumps to the no longer occupied space. 
        placeholder.style.height = `${navBar.offsetHeight}px`;

        navBar.style.position = 'fixed';
        navBar.style.top = 0;
    }
    else if (isNavBarSticky && window.scrollY <= navBarTopPosition) {
        isNavBarSticky = false;

        // Once the navbar becomes positon unset and again occupies its space in the
        // layout, then placeholder must be hidden
        placeholder.style.height = 0;

        navBar.style.position = 'unset';
        navBar.style.top = 'unset';
    }
}

// Sets the navBarTopPosition variable.
function actualizeTopPosition() {
    var topImg = document.querySelector(".main-header__img");
    if (topImg == null)
        return;

    var styleObj;
    if (typeof window.getComputedStyle != "undefined") {
        styleObj = window.getComputedStyle(topImg, null);
    } else if (topImg.currentStyle != "undefined") {
        styleObj = topImg.currentStyle;
    }

    // If the top image is not displayed, the top Navbar has no top offset.
    // Otherwise it has an offset of the top image height + 4px gap.
    if (styleObj.display == "none")
        navBarTopPosition = 0;
    else
        navBarTopPosition = topImg.clientHeight + 4;
}