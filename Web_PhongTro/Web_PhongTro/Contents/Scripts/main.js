﻿//Search from
let searchForm = document.querySelector(".search-form");
let searchBox = document.getElementById("search-box");
let searchLabel = document.querySelector("label[for='search-box']");

document.querySelector("#search-btn").onclick = () => {
    searchForm.classList.toggle("active");
    navbar.classList.remove("active");

    if (searchForm.classList.contains("active")) {
        searchBox.focus();
    }
};

//Click product cart box
let cartBox = document.querySelector(".product-cart-box");
let shoppingCartBtn = document.querySelector("#shopping-cart-btn");

let heartBox = document.querySelector(".product-heart-box");
let heartBtn = document.querySelector("#heart-btn");

if (cartBox && shoppingCartBtn && heartBox && heartBtn) {
    shoppingCartBtn.onclick = () => {
        if (cartBox.style.display === "none") {
            cartBox.style.display = "flex";
            heartBox.style.display = "none";
        } else {
            cartBox.style.display = "none";
        }
    };

    heartBtn.onclick = () => {
        if (heartBox.style.display === "none") {
            heartBox.style.display = "flex";
            cartBox.style.display = "none";
        } else {
            heartBox.style.display = "none";
        }
    };
}
//active nav in reponsive
let navbar = document.querySelector(".navbar");
let isNavbarActive = false;

document.querySelector("#menu-btn").onclick = () => {
    isNavbarActive = !isNavbarActive;
    if (isNavbarActive) {
        navbar.classList.add("active");
    } else {
        navbar.classList.remove("active");
    }
    searchForm.classList.remove("active");
};

window.onscroll = () => {
    searchForm.classList.remove("active");
    navbar.classList.remove("active");
};

//change sign up and sign in
const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');
