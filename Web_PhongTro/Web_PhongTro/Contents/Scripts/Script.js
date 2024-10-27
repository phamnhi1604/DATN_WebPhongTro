$(document).ready(function () {
    //change image
    window.changeImage = function (imageUrl) {
        console.log(imageUrl)
        var productImage = document.querySelector("#post-image");
        productImage.src = imageUrl;
    };

    //Change tap
    $("#tab-1").show();
    $(".tab-link[data-tab='tab-1']").addClass("current");

    $(".tab-link").click(function () {
        var tabId = $(this).attr("data-tab");

        $(".tab-content").hide();
        $(".tab-link").removeClass("current");

        $("#" + tabId).show();
        $(this).addClass("current");
    });

    //Login form
    $.validator.unobtrusive.parse("#loginForm");

    $('#btnLoginSubmit').on('click', function () {
        var formData = $('#loginForm').serialize();
        console.log('hello');
        $.ajax({
            url: '/Account/Login',
            type: 'POST',
            data: formData,

            success: function (result) {
                if (result.success) {
                    if (result.isInRoleAdmin) {
                        alert('Chào mừng tới trang quản lý hệ thống');
                        window.location.href = result.redirectUrl;
                    } else {
                        alert('Đăng nhập thành công');
                        window.location.reload();
                    }
                } else {
                    alert(result.message);
                    if (result.validationErrors) {
                        $.each(result.validationErrors, function (key, value) {
                            var errorElement = $('[name="' + key + '"]').next('.field-validation-valid');
                            errorElement.text(value);
                        });
                    }
                }
            },
            error: function () {
                alert('An error occurred during login.');
            }
        });
    });



    $.validator.unobtrusive.parse("#registerForm");

    $('#btnRegisterSubmit').on('click', function () {
        var formData = $('#registerForm').serialize();
        $.ajax({
            url: '/Account/Register',
            type: 'POST',
            data: formData,

            success: function (result) {
                if (result.success) {
                    alert('Đăng ký thành công. Vui lòng đăng nhập');
                    window.location.reload();
                } else {
                    // Handle failure
                    alert(result.message);

                    if (result.validationErrors) {
                        $.each(result.validationErrors, function (key, value) {
                            var errorElement = $('[name="' + key + '"]').next('.field-validation-valid');
                            errorElement.text(value);
                        });
                    }
                }
            },
            error: function () {
                alert('An error occurred during registration.');
            }
        });
    });

    $('#user-btn').on('click', function () {
        $.ajax({
            url: '/Account/CheckAuthentication',
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                if (result.isAuthenticated) {
                    if (result.isInRoleAdmin) {
                        window.location.href = result.redirectUrl;
                    } else {
                        $('.user-box').css('display', 'unset');
                    }
                } else {
                    $('.account').css('display', 'flex');
                }
            },
            error: function () {
                console.log('Error checking authentication status.');
            }
        });
    });

    //close account form
    $('.close-btn').on('click', function () {
        $('.account').css('display','none')
    });
    $('#logoutButton').on('click', function () {
        $.ajax({
            url: '/Account/Logout',
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    alert('Đăng xuất thành công');
                    window.location.href = result.redirectUrl;
                } else {
                    alert('Đăng xuất thất bại');
                }
            },
            error: function () {
                alert('An error occurred during logout.');
            }
        });
    });


});