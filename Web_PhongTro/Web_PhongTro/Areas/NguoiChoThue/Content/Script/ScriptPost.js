$(document).ready(function () {

    $("#editParentpostTypeDropdown").change(function () {
        refreshClothesStyleDropdown($(this).val()).then(function () {
        });
    });

    function getBaseFilename(filename) {
        return filename.replace(/^.*[\\\/]/, '');
    }
    $('.hide-post-btn').click(function () {

        var postId = $(this).data("post-id");

        $.ajax({
            url: '/DangBai/hidePost',
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
                alert('Đã xảy ra lỗi trong quá trình ẩn bài.');
            }
        });
    });

    //$('#btn-add-baidang').click(function () {
    //    // Bước 1: Tải ảnh bìa
    //    uploadCoverImage()
    //        .then(function (coverImage) {
    //            if (!coverImage) {
    //                alert("Không thể tải ảnh bìa, vui lòng thử lại.");
    //                return;
    //            }

    //            // Bước 2: Tải danh sách ảnh
    //            uploadImageList().then(function (imageList) {
    //                if (!imageList) {
    //                    alert("Không thể tải danh sách ảnh, vui lòng thử lại.");
    //                    return;
    //                }

    //                // Bước 3: Tạo bài đăng
    //                createPost(coverImage, imageList);
    //            });
    //        })
    //        .catch(function (error) {
    //            console.error("Lỗi khi tải ảnh:", error);
    //            alert("Đã xảy ra lỗi khi tải ảnh.");
    //        });
    //});

    // Hàm tải ảnh bìa
    function uploadCoverImage() {
        return new Promise(function (resolve, reject) {
            var formData = new FormData();

            // Lấy file ảnh bìa từ input
            var coverFile = $('#imageFile')[0].files[0];
            if (!coverFile) {
                alert("Vui lòng chọn ảnh bìa!");
                resolve(null); // Không có ảnh bìa
                return;
            }

            formData.append('imageFile', coverFile);

            // Gửi yêu cầu AJAX để tải ảnh bìa
            $.ajax({
                url: '/DangBai/UploadImage',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.success) {
                        resolve(response.savedFileName); // Tên ảnh bìa đã lưu
                    } else {
                        alert(response.message);
                        resolve(null);
                    }
                },
                error: function (xhr, status, error) {
                    reject(error);
                }
            });
        });
    }

    // Hàm tải danh sách ảnh
    function uploadImageList() {
        return new Promise(function (resolve, reject) {
            var formData = new FormData();

            // Lấy danh sách file từ input
            var imageFiles = $('#DanhSachAnh')[0].files;
            if (imageFiles.length === 0) {
                alert("Vui lòng chọn ít nhất một ảnh trong danh sách!");
                resolve(null); // Không có danh sách ảnh
                return;
            }

            for (var i = 0; i < imageFiles.length; i++) {
                formData.append('imageFiles', imageFiles[i]);
            }

            // Gửi yêu cầu AJAX để tải danh sách ảnh
            $.ajax({
                url: '/DangBai/UploadListImages',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.success) {
                        resolve(response.namefile); // Danh sách đường dẫn ảnh
                    } else {
                        alert(response.message);
                        resolve(null);
                    }
                },
                error: function (xhr, status, error) {
                    reject(error);
                }
            });
        });
    }

    // Hàm tạo bài đăng
    //function createPost(coverImage, imageList) {
    //    // Chuẩn bị dữ liệu bài đăng
    //    var postData = {
    //        TieuDe: $('#post_title').val(),
    //        NoiDung: $('#post_content').val(),
    //        AnhBaiDang: coverImage, // Ảnh bìa
    //        DanhSachAnh: imageList   // Danh sách ảnh
    //    };

    //    // Gửi yêu cầu AJAX để tạo bài đăng
    //    $.ajax({
    //        url: '/DangBai/ThemBai',
    //        type: 'POST',
    //        contentType: 'application/json',
    //        data: JSON.stringify(postData),
    //        success: function (response) {
    //            if (response.success) {
    //                alert(response.message);
    //                location.reload();
    //                $('#post_form')[0].reset();
    //            } else {
    //                alert(response.message);
    //            }
    //        },
    //        error: function (xhr, status, error) {
    //            console.error("Lỗi khi tạo bài đăng:", error);
    //            alert("Đã xảy ra lỗi khi tạo bài đăng.");
    //        }
    //    });
    //}

    function createPost(coverImage, imageList) {
        // Lấy IdPhongTro từ dropdown
        var idPhongTro = $('#post_cat').val();  // Lấy giá trị IdPhongTro đã chọn

        // Kiểm tra nếu IdPhongTro chưa được chọn
        if (!idPhongTro) {
            alert('Vui lòng chọn phòng trọ!');
            return;  // Dừng lại nếu chưa chọn phòng trọ
        }

        // Chuẩn bị dữ liệu bài đăng
        var postData = {
            TieuDe: $('#post_title').val(),
            NoiDung: $('#post_content').val(),
            AnhBaiDang: coverImage, // Ảnh bìa
            DanhSachAnh: imageList,  // Danh sách ảnh
            IdPhongTro: idPhongTro  // Thêm IdPhongTro vào dữ liệu
        };

        // Gửi yêu cầu AJAX để tạo bài đăng
        $.ajax({
            url: '/DangBai/ThemBai',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(postData),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    location.reload();  // Làm mới trang sau khi tạo bài đăng thành công
                    $('#post_form')[0].reset();
                } else {
                    alert(response.message);  // Thông báo lỗi nếu không thành công
                }
            },
            error: function (xhr, status, error) {
                console.error("Lỗi khi tạo bài đăng:", error);
                alert("Đã xảy ra lỗi khi tạo bài đăng.");
            }
        });
    }


    let btnAddpost = document.querySelector('#btn-add-post');
    let frmPost = document.querySelector('.frm-post');
    let frmUpdtPost = document.querySelector('.frm-updtPost');
    let btnUpdtposts = document.getElementsByClassName('edit-post-btn'); // HTMLCollection
    for (let i = 0; i < btnUpdtposts.length; i++) {
        btnUpdtposts[i].addEventListener('click', function () {
            let frmUpdtPost = document.querySelector('.frm-updtPost');
            if (frmUpdtPost) {
                frmUpdtPost.classList.add('active');
                console.log("Button clicked:", btnUpdtposts[i]);
            }
        });
    }

    let closeBtn = document.querySelector('.close-btn');

    //btnUpdtpost.addEventListener('click', function () {
    //    frmUpdtPost.classList.add('active');
    //});
    btnAddpost.addEventListener('click', function () {  
        frmPost.classList.add('active');
    });

    closeBtn.addEventListener('click', function () {
        frmPost.classList.remove('active');
        frmUpdtPost.classList.remove('active');
    });



    

    document.addEventListener('DOMContentLoaded', function () {
        const uploadInput = document.getElementById('DanhSachAnh');
        const uploadedImagesContainer = document.querySelector('.uploaded-images');

        uploadInput.addEventListener('change', function () {
            const files = Array.from(uploadInput.files);

            files.forEach(file => {
                const reader = new FileReader();

                reader.onload = function (e) {
                    const img = document.createElement('img');
                    img.src = e.target.result;
                    img.classList.add('uploaded-image');

                    const deleteButton = document.createElement('button');
                    deleteButton.innerHTML = 'Xóa';
                    deleteButton.classList.add('delete-button');
                    deleteButton.addEventListener('click', function () {
                        uploadedImagesContainer.removeChild(imageContainer);
                    });

                    const imageContainer = document.createElement('div');
                    imageContainer.classList.add('image-container');
                    imageContainer.appendChild(img);
                    imageContainer.appendChild(deleteButton);

                    uploadedImagesContainer.appendChild(imageContainer);
                };

                reader.readAsDataURL(file);
            });
        });
    });
});
