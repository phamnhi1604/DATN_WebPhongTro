$(document).ready(function () {
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
    $('.rs-password-btn').on('click', function () {
        var USId = $(this).data('post-id');

        // Hiển thị hộp thoại xác nhận
        var isConfirmed = confirm("Bạn có chắc muốn đặt lại mật khẩu của tài khoản này?");

        if (isConfirmed) {
            $.ajax({
                url: '/AdminHome/ResetPassWord',
                type: 'POST',
                data: { idND: USId },
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        alert(response.message);
                        location.reload();
                    } else {
                        alert(response.message);
                    }
                },
                error: function (xhr, status, error) {
                    alert('An error occurred: ' + xhr.responseText);
                }
            });
        } else {
            // Nếu người dùng chọn "Hủy", không làm gì cả
            console.log('Reset password action canceled.');
        }
    });

    

});

