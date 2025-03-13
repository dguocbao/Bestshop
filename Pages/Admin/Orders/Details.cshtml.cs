using Bestshop.MyHelpers;
using Bestshop.Pages.Admin.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Bestshop.Pages.Admin.Orders
{
    [RequireAuth(RequiredRole = "admin")]
    public class DetailsModel : PageModel
    {
        public OrderInfo orderInfo = new OrderInfo();
        public UserInfo userInfo = new UserInfo();

        public void OnGet(int id)
        {
            // Kiểm tra id, nếu không hợp lệ sẽ chuyển hướng về trang danh sách đơn hàng.
            if (id < 1)
            {
                Response.Redirect("/Admin/Orders/Index");
                return;
            }

            // Lấy trạng thái thanh toán và trạng thái đơn hàng từ query string.
            string paymentStatus = Request.Query["payment_status"];
            string orderStatus = Request.Query["order_status"];

            try
            {
                // Kết nối với cơ sở dữ liệu.
                string connectionString = "Data Source=HUY;Initial Catalog=ThayHuy;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Cập nhật trạng thái thanh toán nếu có trong query.
                    if (paymentStatus != null)
                    {
                        string sqlUpdate = "UPDATE orders SET payment_status=@payment_status WHERE id=@id";
                        using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                        {
                            command.Parameters.AddWithValue("@payment_status", paymentStatus);
                            command.Parameters.AddWithValue("@id", id);

                            command.ExecuteNonQuery();
                        }
                    }

                    // Cập nhật trạng thái đơn hàng nếu có trong query.
                    if (orderStatus != null)
                    {
                        string sqlUpdate = "UPDATE orders SET order_status=@order_status WHERE id=@id";
                        using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                        {
                            command.Parameters.AddWithValue("@order_status", orderStatus);
                            command.Parameters.AddWithValue("@id", id);

                            command.ExecuteNonQuery();
                        }
                    }

                    // Truy vấn thông tin chi tiết đơn hàng.
                    string sql = "SELECT * FROM orders WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Gán dữ liệu từ cơ sở dữ liệu vào đối tượng orderInfo.
                                orderInfo.id = reader.GetInt32(0);
                                orderInfo.clientId = reader.GetInt32(1);
                                orderInfo.orderDate = reader.GetDateTime(2).ToString("MM/dd/yyyy");
                                orderInfo.shippingFee = reader.GetDecimal(3);
                                orderInfo.deliveryAddress = reader.GetString(4);
                                orderInfo.paymentMethod = reader.GetString(5);
                                orderInfo.paymentStatus = reader.GetString(6);
                                orderInfo.orderStatus = reader.GetString(7);

                                // Lấy danh sách sản phẩm trong đơn hàng.
                                orderInfo.items = OrderInfo.GetOrderItems(orderInfo.id);
                            }
                            else
                            {
                                Response.Redirect("/Admin/Orders/Index");
                                return;
                            }
                        }
                    }

                    // Truy vấn thông tin chi tiết của người dùng.
                    sql = "SELECT * FROM users WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", orderInfo.clientId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Gán dữ liệu từ cơ sở dữ liệu vào đối tượng userInfo.
                                userInfo.id = reader.GetInt32(0);
                                userInfo.firstname = reader.GetString(1);
                                userInfo.lastname = reader.GetString(2);
                                userInfo.email = reader.GetString(3);
                                userInfo.phone = reader.GetString(4);
                                userInfo.address = reader.GetString(5);
                                userInfo.password = reader.GetString(6);
                                userInfo.role = reader.GetString(7);
                                userInfo.createdAt = reader.GetDateTime(8).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                Console.WriteLine("Không tìm thấy client, id=" + orderInfo.clientId);
                                Response.Redirect("/Admin/Orders/Index");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và chuyển hướng nếu xảy ra lỗi.
                Console.WriteLine(ex.Message);
                Response.Redirect("/Admin/Orders/Index");
            }
        }
    }
}
