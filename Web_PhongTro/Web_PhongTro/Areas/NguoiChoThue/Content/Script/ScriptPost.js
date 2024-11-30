$(document).ready(function () {

    //let closeBtnAdd = document.querySelector('.add-post .close-btn');
    //let btnAddpost = document.querySelector('#btn-add-post');
    //let frmPost = document.querySelector('.frm-post');

    //closeBtnAdd.addEventListener('click', function () {
    //    addpost.classList.remove('active');
    //});
    //let formAddpost = document.getElementById('btn-add-post');
    //btnAddpost.addEventListener('click', function () {
    //    frmPost.classList.add('active');
    //});

     


    //let closeBtnEditPro = document.querySelector('.edit-post .close-btn');
    //let editpost = document.querySelector('.edit-post');

    //closeBtnEditPro.addEventListener('click', function () {
    //    editpost.classList.remove('active');
    //});
    $("#editParentpostTypeDropdown").change(function () {
        refreshClothesStyleDropdown($(this).val()).then(function () {
        });
    });

    function getBaseFilename(filename) {
        return filename.replace(/^.*[\\\/]/, '');
    }    $('#btn-tieptuc').click(function () {

        var formData = new FormData();

        // Append the selected image file to the form data
        var imageFile = $('#imageFile')[0].files[0];
        if (imageFile) {
            formData.append('imageFile', imageFile);
        } else {
            alert("Vui lòng chọn ảnh!");
            return;
        }

        // Upload the image
        $.ajax({
            url: '/DangBai/UploadImage',  // Replace with your actual controller action URL
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    // On successful image upload, create the post
                    var imageFileName = response.savedFileName; // Get the uploaded file's name

                    // Prepare the post data
                    var postData = {
                        IdNguoiChoThue: $('#nguoiChoThue').val(),
                        TieuDe: $('#post_title').val(),
                        NoiDung: $('#post_content').val(),
                        AnhBaiDang: imageFileName // Use the uploaded image file name
                    };

                    // Create the post
                    $.ajax({
                        url: '/DangBai/ThemBai',  // Replace with your actual controller action URL
                        type: 'POST',
                        data: postData,
                        success: function (postResponse) {
                            if (postResponse.success) {
                                alert(postResponse.message);  // Show success message
                                // Optionally, you can redirect to another page or reset the form
                            } else {
                                alert(postResponse.message);  // Show error message from post creation
                            }
                        },
                        error: function () {
                            alert("Đã xảy ra lỗi khi tạo bài đăng.");
                        }
                    });
                } else {
                    alert(response.message);  // Show image upload error message
                }
            },
            error: function () {
                alert("Đã xảy ra lỗi khi tải lên ảnh.");
            }
        });
    });




    $('#DanhSachAnh').on('change', function () {
        var files = this.files;
        var formData = new FormData();

        $.each(files, function (i, file) {
            formData.append('DanhSachAnh', file);
        });

        // Gửi yêu cầu Ajax để tải ảnh lên
        $.ajax({
            url: '/BaiDang/UploadImage', // Cập nhật với đường dẫn đúng
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    // Hiển thị ảnh vừa tải lên
                    var imgTag = $('<img>').attr('src', response.filePath).addClass('uploaded-image');
                    $('.uploaded-images').append(imgTag);
                } else {
                    alert('Tải ảnh lên thất bại: ' + response.message);
                }
            },
            error: function (error) {
                alert('Lỗi khi tải ảnh: ' + error.statusText);
            }
        });
    });


    let btnAddpost = document.querySelector('#btn-add-post');
    let frmPost = document.querySelector('.frm-post');
    let closeBtn = document.querySelector('.frm-post .close-btn');

    btnAddpost.addEventListener('click', function () {
        frmPost.classList.add('active');
    });

    closeBtn.addEventListener('click', function () {
        frmPost.classList.remove('active');
    });



    $(document).ready(function () {


        $('#district_id').prop('disabled', true);
        $('#phuongxa').prop('disabled', true);

        // When a province is selected
        $('#province_id').change(function () {
            var selectedProvince = $(this).val();

            if (selectedProvince !== "") {
                $('#district_id').prop('disabled', false);

                // TODO: Make an AJAX call to load districts for the selected province
                // Example:
                // $.ajax({
            } else {
                $('#district_id').prop('disabled', true).val('');
                $('#phuongxa').prop('disabled', true).val('');
            }

            // Disable the ward dropdown since no district is selected yet
            $('#phuongxa').prop('disabled', true);
        });

        // When a district is selected
        $('#district_id').change(function () {
            var selectedDistrict = $(this).val();

            if (selectedDistrict !== "") {
                // Enable the ward dropdown
                $('#phuongxa').prop('disabled', false);
                $.ajax({
                    url: '@Url.Action("GetWardsByDistrict", "DangBai")',
                    type: 'GET',
                    data: { district: selectedDistrict },
                    success: function (data) {
                        $('#phuongxa').prop('disabled', false);
                        $('#phuongxa').html(''); // Clear current options
                        $('#phuongxa').append('<option value="">-- Chọn Phường/Xã --</option>'); // Default option

                        // Populate ward dropdown with the new data
                        $.each(data, function (index, ward) {
                            $('#phuongxa').append('<option value="' + ward + '">' + ward + '</option>');
                        });
                    }
                });


            } else {
                // If no district is selected, disable the ward dropdown
                $('#phuongxa').prop('disabled', true).val('');
            }
        });
        $('#btn-add-imgtest').click(function () {
            $('#DanhSachAnh').click();
        });



        // Khi người dùng chọn ảnh
        //$('#DanhSachAnh').change(function () {
        //    const files = Array.from(this.files); // Lấy danh sách file
        //    const uploadedImagesContainer = $('#uploaded-images-container');

        //    // Duyệt qua từng file và xử lý
        //    files.forEach(file => {
        //        const reader = new FileReader();

        //        // Khi file đọc xong
        //        reader.onload = function (e) {
        //            const img = $('<img>')
        //                .attr('src', e.target.result) // Gán đường dẫn ảnh
        //                .css({
        //                    width: '100px',
        //                    height: '100px',
        //                    margin: '5px',
        //                    border: '1px solid #ccc',
        //                    borderRadius: '5px'
        //                });

        //            const deleteButton = $('<button>')
        //                .text('Xóa')
        //                .css({
        //                    display: 'block',
        //                    marginTop: '5px',
        //                    background: '#ff0000',
        //                    color: '#fff',
        //                    border: 'none',
        //                    borderRadius: '3px',
        //                    padding: '5px',
        //                    cursor: 'pointer'
        //                })
        //                .click(function () {
        //                    img.remove(); // Xóa ảnh
        //                    $(this).remove(); // Xóa nút xóa
        //                });

        //            const imageContainer = $('<div>')
        //                .css({
        //                    display: 'inline-block',
        //                    textAlign: 'center',
        //                    margin: '10px'
        //                })
        //                .append(img)
        //                .append(deleteButton);

        //            uploadedImagesContainer.append(imageContainer);
        //        };

        //        // Đọc file dưới dạng URL
        //        reader.readAsDataURL(file);
        //    });
        //});

        $('#uploadForm').submit(function (e) {
            e.preventDefault(); // Ngừng việc form gửi dữ liệu theo kiểu mặc định

            var fileInput = $('#imageFile')[0]; // Lấy input file từ DOM
            var file = fileInput.files[0]; // Lấy file đầu tiên

            // Kiểm tra xem có ảnh nào được chọn không
            if (!file) {
                $('#uploadResult').html('<span style="color: red;">Vui lòng chọn một ảnh để tải lên.</span>');
                return; // Dừng lại nếu không có file
            }

            var formData = new FormData(this); // Lấy form data

            // Gửi yêu cầu AJAX đến controller
            $.ajax({
                url: '/DangBai/UploadImage', // Đảm bảo URL đúng
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.success) {
                        $('#uploadResult').html('<span style="color: green;">' + response.message + '</span>');
                    } else {
                        $('#uploadResult').html('<span style="color: red;">' + response.message + '</span>');
                    }
                },
                error: function (xhr, status, error) {
                    $('#uploadResult').html('<span style="color: red;">Lỗi khi kết nối đến server: ' + error + '</span>');
                }
            });
        });
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
