document.addEventListener("DOMContentLoaded", function () {
    var goTopButton = document.getElementById("goTopButton");
    window.addEventListener("scroll", function () {
        if (window.pageYOffset > 100) { /* Sayfanın 100 piksel altına inildiğinde butonu göster */
            goTopButton.style.display = "block";
        } else {
            goTopButton.style.display = "none";
        }
    });

    goTopButton.addEventListener("click", function () {
        window.scrollTo(0, 0); /* Butona tıklandığında sayfanın başına kaydır */
    });
});
