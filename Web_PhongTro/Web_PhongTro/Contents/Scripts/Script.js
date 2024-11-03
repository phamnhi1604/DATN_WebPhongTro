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
                        window.location.href = "../Home/Index"
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
                    //window.location.reload();
                    window.location.href = '/account/loginV';
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
                        //window.location.href = result.redirectUrlAdmin;
                        //alert('Đã đăng nhập');
                    } else if (result.isInRoleNCT)
                    {
                        window.location.href = result.redirectUrlNCT;
                    }
                    $('.user-box').css('display', 'unset');
                    alert('Đã đăng nhập');

                } else {
                    alert('Chưa đăng nhập');
                    console.log(result.isAuthenticated);

                    window.location.href = "/Account/LoginV";
                }
            },
            error: function () {
                console.log('Error checking authentication status.');
            }
        });
        if ($('.user-box').css('display', 'unset')) {
            $('.user-box').css('display', 'none');
        }
    });
    //close account form
    $('.close-btn').on('click', function () {
        $('.account').css('display', 'none')
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