$(document).ready(function () {

    //Thêm bài đăng yêu thích
    $(".favorites-post").on("click", function (e) {
        e.preventDefault();

        var postId = $(this).data("post-id");

        $.ajax({
            url: '/FavoritePost/Add',  
            type: 'POST',
            data: { IdBaiDang: postId },
            success: function (response) {
                if (response.success) {
                    alert(response.message); // Thông báo thành công
                } else {
                    alert(response.message); // Thông báo lỗi (VD: Đã thích sản phẩm này)
                }
            },
            error: function () {
                alert('Đã xảy ra lỗi trong quá trình thêm yêu thích.');
            }
        });
    });
    //Xóa bài đăng khỏi yêu thích
    $(".delete-post-btn").on("click", function (e) {
        e.preventDefault();

        var postId = $(this).data('post-id');

        $.ajax({
            url: '/FavoritePost/Delete',  
            type: 'POST',
            data: { postId: postId },  
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    location.reload();

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

    //update thong tin ca nhan
    $(".btn-update-info").on("click", function (e) {
        e.preventDefault();

        const data = {
            TenKhachHang: document.getElementById("fullName").value,
            SoDienThoai: document.getElementById("phone").value,
            Email: document.getElementById("email").value,
            DiaChi: document.getElementById("address").value,
            Password: document.getElementById("password").value, // đổi từ MatKhau thành Password
            ConfirmPassword: document.getElementById("confirmPassword").value
        };

        $.ajax({
            url: '/Home/UpdateInfo',
            type: 'POST',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Có lỗi xảy ra!");
            }
        });
    });
});