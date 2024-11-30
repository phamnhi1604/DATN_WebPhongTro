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
    //duyệt bài đăng
    $(".accept").on("click", function (e) {
        e.preventDefault();

        var postId = $(this).data("post-id");

        $.ajax({
            url: '/KDVHome/Confirm',
            type: 'POST',
            data: { IdBaiDang: postId },
            success: function (response) {
                if (response.success) {
                    alert(response.message); // Thông báo thành công
                    location.reload();

                } else {
                    alert(response.message); // Thông báo lỗi 
                }
            },
            error: function () {
                alert('Đã xảy ra lỗi trong quá trình duyệt bài.');
            }
        });
    });

    $(".reject").on("click", function (e) {
        e.preventDefault();
        var rejectReason = prompt("Nhập lý do từ chối:");

        // Nếu người dùng không nhập hoặc hủy prompt
        if (rejectReason === null || rejectReason.trim() === "") {
            alert("Lý do từ chối không được để trống!");
            return;
        }
        var postId = $(this).data("post-id");

        $.ajax({
            url: '/KDVHome/Reject',
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
                alert('Đã xảy ra lỗi trong quá trình duyệt bài.');
            }

        });
    });


    $(".delete").on("click", function (e) {
        e.preventDefault();

        var postId = $(this).data('post-id');

        $.ajax({
            url: '/KDVHome/Delete',
            type: 'POST',
            data: { postId: postId },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    location.reload();

                    $(`button[data-post-id="${postId}"]`).closest('.post-item').remove();
                } else {
                    alert(response.message);
                    location.reload();

                }
            },
            error: function () {
                alert('Đã xảy ra lỗi khi xóa bài đăng!');
            }

        });
    });

});

