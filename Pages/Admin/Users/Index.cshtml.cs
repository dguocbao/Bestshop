using Bestshop.MyHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Bestshop.Pages.Admin.Users
{
    [RequireAuth(RequiredRole = "admin")]
    public class IndexModel : PageModel
    {
        public List<UserInfo> listUsers = new List<UserInfo>(); // Danh sách chứa thông tin người dùng.

        public int page = 1; // Biến lưu trữ trang hiện tại, mặc định là trang 1.
        public int totalPages = 0; // Tổng số trang, được tính dựa trên tổng số người dùng.
        private readonly int pageSize = 5; // Số lượng người dùng hiển thị trên mỗi trang.

        public void OnGet()
        {
            page = 1;
            string requestPage = Request.Query["page"];
            if (requestPage != null)
            {
                try
                {
                    page = int.Parse(requestPage); // Đọc số trang từ query string, nếu không hợp lệ thì mặc định là trang 1.
                }
                catch (Exception ex)
                {
                    page = 1;
                }
            }

            try
            {
                // Kết nối đến cơ sở dữ liệu
                string connectionString = "Data Source=HUY;Initial Catalog=ThayHuy;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Đếm tổng số người dùng trong cơ sở dữ liệu để tính tổng số trang.
                    string sqlCount = "SELECT COUNT(*) FROM users";
                    using (SqlCommand command = new SqlCommand(sqlCount, connection))
                    {
                        decimal count = (int)command.ExecuteScalar();
                        totalPages = (int)Math.Ceiling(count / pageSize); // Tính tổng số trang dựa trên pageSize.
                    }

                    // Lấy danh sách người dùng từ cơ sở dữ liệu với phân trang.
                    string sql = "SELECT * FROM users ORDER BY id DESC";
                    sql += " OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY"; // Phân trang SQL.
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@skip", (page - 1) * pageSize); // Bỏ qua số dòng.
                        command.Parameters.AddWithValue("@pageSize", pageSize); // Số dòng cần lấy.

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Đọc dữ liệu của từng người dùng và thêm vào danh sách `listUsers`.
                            while (reader.Read())
                            {
                                UserInfo userInfo = new UserInfo();

                                userInfo.id = reader.GetInt32(0);
                                userInfo.firstname = reader.GetString(1);
                                userInfo.lastname = reader.GetString(2);
                                userInfo.email = reader.GetString(3);
                                userInfo.phone = reader.GetString(4);
                                userInfo.address = reader.GetString(5);
                                userInfo.password = reader.GetString(6);
                                userInfo.role = reader.GetString(7);
                                userInfo.createdAt = reader.GetDateTime(8).ToString("MM/dd/yyyy");

                                listUsers.Add(userInfo); // Thêm người dùng vào danh sách.
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Ghi lỗi vào console nếu có.
            }
        }
    }

    // Lớp lưu trữ thông tin của một người dùng.
    public class UserInfo
    {
        public int id;
        public string firstname;
        public string lastname;
        public string email;
        public string phone;
        public string address;
        public string password;
        public string role;
        public string createdAt;
    }
}
