// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const navTabs = document.querySelectorAll("#nav-tabs > a");
navTabs.forEach((tab) => {
    tab.addEventListener("click", () => {
        navTabs.forEach((tab) => {
            tab.classList.remove("active");
        });
        tab.classList.add("active");
    });
});

