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
        $.ajax({
            url: '/AdminHome/ResetPassWord',
            type: 'POST',
            dataType: { idND: USId },
            success: function (result) {
                if (response.success) {
                    alert(response.message);
                    location.reload();

                    //$(`button[data-post-id="${USId}"]`).closest('.post-item').remove();
                } else {
                    alert(response.message);
                    location.reload();

                }
            },
            error: function () {
                alert('An error occurred during reset password.');
            }
        });
    });
    

});

