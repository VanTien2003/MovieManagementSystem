Chức năng đăng ký, gửi mã hoặc link xác nhận về email, xác thực đăng ký tài khoản
Chức năng đăng nhập, sử dụng phương thức xác thực là JwtBearer
Chức năng đổi mật khẩu: người đổi mật khẩu phải là người đang trong phiên đăng nhập, chứ không truyền vào userid trong controller (sử dụng jwt)
Chức năng quên mật khẩu - gửi mã xác nhận về email, chức năng tạo mật khẩu mới
Hiển thị các bộ phim nổi bật(sắp xếp theo số lượng đặt vé)
Thêm, sửa, xóa Cinema(Admin)
Thêm, sửa, xóa Room(Admin)
Thêm, sửa, xóa Seat(Admin)
Thêm, sửa, xóa Food(Admin)
Hiển thị phim theo rạp, phòng, trạng thái ghế trong phòng
Thêm, sửa, xóa Movie
Xử lý luồng: 
"Chọn phim => Chọn rạp => Chọn phòng => Chọn suất chiếu, đồ ăn => Tạo hóa đơn => thanh toán VNPAY(dựa vào Promotion để tính giá tiền sau cùng) "
CRUD Schedule - kiểm tra và đảm bảo là trong một phòng không chiếu đồng thời 2 phim trong một khoảng thời gian giao nhau" Sau khi thanh toán thì thông báo được gửi về email của người dùng
Thống kê doanh số của từng rạp theo khoảng thời gian(Admin)
Thống kê đồ ăn bán chạy trong 7 ngày gần nhất(Admin)
Quản lý thông tin của người dùng(Admin)
-- Chia tách các package: controller - service - entity - dto - exception - helper - config - payload và các class theo từng chức năng riêng 
-- Phân tách quyền sử dụng API theo user và admin 
-- Xử lí đầy đủ các ngoại lệ 
