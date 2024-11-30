$(document).ready(function () {
    //change image
    window.changeImage = function (imageUrl) {
        console.log(imageUrl)
        var productImage = document.querySelector("#post-image");
        productImage.src = imageUrl;
    };




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
                    alert('Đăng nhập thành công');
                    window.location.href = "../Home/Index"
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
                        //window.location.href = result.redirectUrlNCT;
                    }
                    if ($('.user-box').css('display') === 'none') {
                        $('.user-box').css('display', 'unset');
                    } else {
                        $('.user-box').css('display', 'none');
                    }

                } else {
                    //alert('Chưa đăng nhập');
                    console.log(result.isAuthenticated);

                    window.location.href = "/Account/LoginV";
                }
                
            },
            error: function () {
                console.log('Error checking authentication status.');
            }
        });
        
    });

    $('#user-information').on('click', function () {
        $.ajax({
            url: '/Account/CheckAuthentication',
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                if (result.isAuthenticated) {
                    if (result.isInRoleAdmin) {
                        window.location.href = result.redirectUrlAdmin;
                    }
                    else if (result.isInRoleNCT) {
                        window.location.href = result.redirectUrlNCT;
                    }
                    else if (result.isInRoleKDV) {
                        window.location.href = result.redirectUrlKDV;
                    } else {
                        window.location.href = result.redirectUrlKT;

                    }

                }
                else {
                    alert('Chưa đăng nhập');
                }
            },
            error: function () {
                console.log('Error checking authentication status.');
            }
        });
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


    $(".report-post").on("click", function (e) {
        e.preventDefault();
        var rejectReason = prompt("Nhập nội dung:");

        // Nếu người dùng không nhập hoặc hủy prompt
        if (rejectReason === null || rejectReason.trim() === "") {
            alert("Nội dung không được để trống!");
            return;
        }
        var postId = $(this).data("post-id");

        $.ajax({
            url: '/Home/AddPhanHoi',
            type: 'POST',
            data: { IdBaiDang: postId, NoiDung: rejectReason },
            success: function (response) {
                if (response.success) {
                    alert(response.message); // Thông báo thành công
                    location.reload();

                } else {
                    alert(response.message); // Thông báo lỗi 
                }
            },
            error: function () {
                alert('Đã xảy ra lỗi trong quá trình báo cáo bài viết.');
            }

        });
    });



});