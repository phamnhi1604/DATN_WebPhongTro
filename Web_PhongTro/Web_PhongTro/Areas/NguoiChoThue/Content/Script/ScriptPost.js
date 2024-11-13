$(document).ready(function () {

    let closeBtnAddPro = document.querySelector('.add-post .close-btn');
    let addpost = document.querySelector('.add-post');

    closeBtnAddPro.addEventListener('click', function () {
        addpost.classList.remove('active');
    });
    let formAddpost = document.getElementById('btn-add-post');
    formAddpost.addEventListener('click', function () {
        addpost.classList.add('active');
    });

    //Render loại sản phẩm khi chọn loại sản phẩm cha
    $("#parentpostTypeDropdown").change(function () {
        var selectedParentTypeId = $(this).val();
        if (selectedParentTypeId === "") {
            $("#clothes-style").empty();
            $("#clothes-style").append('<option value="">Select an option</option>');
        } else {
            $.ajax({
                url: "/Categories/GetLoaiSanPham",
                type: "GET",
                data: { parentId: selectedParentTypeId },
                success: function (data) {
                    $("#clothes-style").empty();

                    $("#clothes-style").append('<option value="">Select an option</option>');

                    $.each(data, function (index, item) {
                        $("#clothes-style").append('<option value="' + item.Value + '">' + item.Text + '</option>');
                    });
                },
                error: function (error) {
                    console.log("Error: " + error);
                }
            });
        }
    });



    // Thêm sản phẩm
    $('#add-post-form').submit(function (e) {
        e.preventDefault();

        var formData = $(this).serialize();

        // Make an AJAX request
        $.ajax({
            type: 'POST',
            url: '/posts/Add',
            data: formData,
            success: function (response) {
                if (response.success) {
                    alert('Thêm sản phầm thành công!');
                    $('#add-post-form')[0].reset();
                    addpost.classList.remove('active');
                } else {
                    alert('Error: ' + response.message);
                }
            },
            error: function (error) {
                console.log('Error: ', error);
            }
        });
    });


    //  Xóa sản phẩm
    $('.delete-post-btn').on('click', function () {
        var postId = $(this).data('post-id');

        // Make an AJAX request to delete the post
        $.ajax({
            type: 'POST',
            url: '/posts/Delete',
            data: { idSanPham: postId },
            success: function (response) {
                if (response.success) {
                    alert('Xóa sản phẩm thành công!');                                     
                    window.location.reload();
                } else {
                    alert('Error: ' + response.message);
                }
            },
            error: function (error) {
                console.log('Error: ', error);
            }
        });
    });


    let closeBtnEditPro = document.querySelector('.edit-post .close-btn');
    let editpost = document.querySelector('.edit-post');

    closeBtnEditPro.addEventListener('click', function () {
        editpost.classList.remove('active');
    });
    $("#editParentpostTypeDropdown").change(function () {
        refreshClothesStyleDropdown($(this).val()).then(function () {
        });
    });

    $('.edit-post-btn').click(function () {
        var postId = $(this).data('post-id');

        $.ajax({
            url: '/posts/GetDetails',
            method: 'GET',
            data: { postId: postId },
            success: function (data) {
                $('#edit-post-id-header').text('ID: ' + data.postData.IdSanPham);
                $('#edit-post-id').val(data.postData.IdSanPham);
                $('#edit-post-name').val(data.postData.TenSanPham);
                var selectedParentTypeId = data.idLoaiCha;
                $('#editParentpostTypeDropdown').val(selectedParentTypeId);
                refreshClothesStyleDropdown(selectedParentTypeId, function () {
                    $('#edit-clothes-style').val(data.postData.IdLoaiSP);
                });
                $('#edit-post-price').val(data.postData.GiaBan);
                $('#edit-post-discount').val(data.postData.GiamGia);
                $('#edit-post-status').val(data.postData.TonTai.toString());
                $('#preview-image').attr('src', '/Content/Images/' + data.postData.AnhSP);
                $('#preview-image-compact-1').attr('src', '/Content/Images/' + data.postData.AnhSPChiTiet1);
                $('#preview-image-compact-2').attr('src', '/Content/Images/' + data.postData.AnhSPChiTiet2);
                $('#edit-post-content').text(data.postData.NoiDungSanPham);
                $('#edit-post-review').text(data.postData.DanhGiaSanPham);
                $('#edit-post-payment-shipping').text(data.postData.ThanhToanVanChuyen)
                document.querySelector('.edit-post').classList.add('active');
            },
            error: function (error) {
                console.error('Error fetching post details:', error);
            }
        });
    });
    $('#edit-image-url').change(function (event) {
        if (event.target.files.length > 0) {
            var imageUrl = URL.createObjectURL(event.target.files[0]);
            $('#preview-image').attr('src', imageUrl);
        } else {
            console.log('Không có tệp nào được chọn');
        }
    });
    $('#edit-image-url-compact-1').change(function (event) {
        if (event.target.files.length > 0) {
            var imageUrl = URL.createObjectURL(event.target.files[0]);
            $('#preview-image-compact-1').attr('src', imageUrl);
        } else {
            console.log('Không có tệp nào được chọn');
        }
    });
    $('#edit-image-url-compact-2').change(function (event) {
        if (event.target.files.length > 0) {
            var imageUrl = URL.createObjectURL(event.target.files[0]);
            $('#preview-image-compact-2').attr('src', imageUrl);
        } else {
            console.log('Không có tệp nào được chọn');
        }
    });
    function refreshClothesStyleDropdown(selectedParentTypeId, callback) {
        if (selectedParentTypeId === "") {
            $("#edit-clothes-style").empty();
            $("#edit-clothes-style").append('<option value="">Select an option</option>');
            if (callback) {
                callback();
            }
        } else {
            $.ajax({
                url: "/Categories/GetLoaiSanPham",
                type: "GET",
                data: { parentId: selectedParentTypeId },
                success: function (data) {
                    $("#edit-clothes-style").empty();
                    $("#edit-clothes-style").append('<option value="">Select an option</option>');

                    $.each(data, function (index, item) {
                        $("#edit-clothes-style").append('<option value="' + item.Value + '">' + item.Text + '</option>');
                    });

                    if (callback) {
                        callback();
                    }
                },
                error: function (error) {
                    console.log("Error: " + error);
                }
            });
        }
    }

    //Cập nhật sản phẩm
    $('#edit-post-form').submit(function (e) {
        e.preventDefault();

        var confirmation = confirm("Bạn có chắc muốn cập nhật thông tin?");

        if (confirmation) {
            $('#image-source').val(getBaseFilename($('#preview-image').attr('src')));
            $('#image-compact-source-1').val(getBaseFilename($('#preview-image-compact-1').attr('src')));
            $('#image-compact-source-2').val(getBaseFilename($('#preview-image-compact-2').attr('src')));

            var formData = new FormData($(this)[0]);
            console.log();

            $.ajax({
                url: '/posts/Update',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data.success) {
                        alert('Cập nhật thông tin thành công!');
                        document.querySelector('.edit-post').classList.remove('active');
                        location.reload();
                    } else {
                        alert('Update failed. ' + data.message);
                    }
                },
                error: function (error) {
                    console.log('Error: ' + error);
                }
            });
        } else {
            alert('Update cancelled.');
        }
    });

    function getBaseFilename(filename) {
        return filename.replace(/^.*[\\\/]/, '');
    }
});

