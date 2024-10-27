// Lấy tham chiếu đến hình ảnh và phần tử content-box
const avatarImage = document.getElementById('avatar-chat-bot');
const contentBox = document.getElementById('content-box');

// Thêm sự kiện click cho hình ảnh
avatarImage.addEventListener('click', toggleContentBox);

// Hàm chuyển đổi trạng thái display của content-box
function toggleContentBox() {
    // Kiểm tra trạng thái hiện tại của content-box
    const isContentBoxVisible = contentBox.style.display === 'flex' || getComputedStyle(contentBox).display === 'flex';

    // Nếu content-box hiện đang ẩn, hiển thị nó
    if (!isContentBoxVisible) {
        contentBox.style.display = 'flex';
    } else {
        // Ngược lại, ẩn nó
        contentBox.style.display = 'none';
    }
}

// Tạo biến cờ để kiểm tra trạng thái của việc nhận dạng giọng nói
let isRecognizing = false;

// Hàm bắt đầu hoặc kết thúc nhận dạng giọng nói
function toggleSpeechRecognition() {
    if (!isRecognizing) {
        // Bắt đầu nhận dạng giọng nói
        startSpeechRecognition();
    } else {
        // Kết thúc nhận dạng giọng nói
        stopSpeechRecognition();
    }
}

// Hàm gửi dữ liệu lên server sử dụng jQuery AJAX
function sendDataToServer(transcript) {
    $.ajax({
        type: 'POST',
        url: '/ChatBot/VoiceProcessing',
        data: { transcript: transcript },
        success: function (response) {

            // Kiểm tra giá trị của typeAction
            switch (response.typeAction) {
                case 'Them':
                    // Nếu là 'Them', thực hiện hành động thêm sản phẩm vào giỏ hàng
                    var productId = response.idProduct;
                    $.ajax({
                        url: "/ShoppingCart/Them",
                        type: "POST",
                        data: { idSanPham: productId },
                        success: function (result) {
                            if (result.status) {
                                alert('Thêm sản phẩm thành công')
                                location.reload();
                            } else {
                                alert("Đã có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.");
                            }
                        },
                        error: function () {
                            alert("Đã có lỗi xảy ra khi thực hiện yêu cầu.");
                        }
                    });
                    break;
                case 'Xoa':
                    // Nếu là 'Xoa', thực hiện hành động xóa sản phẩm khỏi giỏ hàng
                    var productId = response.idProduct;
                    $.ajax({
                        url: "/ShoppingCart/Xoa",
                        type: "POST",
                        data: { idSanPham: productId },
                        success: function (result) {
                            if (result.status) {
                                alert('Xóa sản phẩm thành công')
                                location.reload();
                            } else {
                                alert("Đã có lỗi xảy ra khi xóa sản phẩm khỏi giỏ hàng.");
                            }
                        },
                        error: function () {
                            alert("Đã có lỗi xảy ra khi thực hiện yêu cầu.");
                        }
                    });
                    break;
                case 'XoaGioHang':
                    // Nếu là 'XoaGioHang', thực hiện hành động xóa  giỏ hàng
                    $.ajax({
                        url: "/ShoppingCart/XoaTatCa",
                        type: "POST",
                        dataType: "json",
                        success: function (result) {
                            if (result.status) {
                                alert('Xóa toàn bộ giỏ hàng thành công');
                                location.reload();
                            } else {
                                alert("Đã có lỗi xảy ra khi xóa toàn bộ giỏ hàng.");
                            }
                        },
                        error: function () {
                            alert("Đã có lỗi xảy ra khi thực hiện yêu cầu.");
                        }
                    });
                    break;
                case 'ThanhToan':
                    $.ajax({
                        url: "/Account/KiemTraDangNhap", // Đường dẫn tới hành động kiểm tra đăng nhập, thay thế bằng đường dẫn thực tế
                        type: "POST",
                        dataType: "json",
                        success: function (result) {
                            if (result.status) {
                                window.location.href = "/ShoppingCart/ThanhToan";
                                $(".checkout-box").addClass("active");
                            } else {
                                alert("Bạn cần đăng nhập trước khi đặt hàng.");
                                $(".account").css("display", "flex");
                            }

                        },
                        error: function () {
                            alert("Đã có lỗi xảy ra khi kiểm tra đăng nhập.");
                        }
                    });                 
                    break;
                default:
                    alert("Hành động không được xác định.");
                    break;
            }
        },
        error: function (error) {
            console.error('Yêu cầu thất bại.', error);
        }
    });
}

// Hàm bắt đầu nhận dạng giọng nói
function startSpeechRecognition() {
    if ('SpeechRecognition' in window || 'webkitSpeechRecognition' in window) {
        const recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
        recognition.lang = 'vi-VN';
        recognition.onresult = function (event) {
            const transcript = event.results[0][0].transcript;
            // Gửi dữ liệu về server
            sendDataToServer(transcript);
        };

        recognition.start();

        isRecognizing = true;
    } else {
        alert('Trình duyệt của bạn không hỗ trợ chuyển đổi giọng nói thành văn bản.');
    }
}


function stopSpeechRecognition() {
    recognition.stop();
    isRecognizing = false;
}

document.querySelector('button').addEventListener('click', toggleSpeechRecognition);