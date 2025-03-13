﻿using Bestshop.MyHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace BestShop.Pages.Auth
{
	//[RequireNoAuth]
	public class ForgotPasswordModel : PageModel
	{
        private readonly IEmailSender _emailSender;
        public ForgotPasswordModel(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        [BindProperty, Required(ErrorMessage = "Vui lòng điền Email"), EmailAddress]
		public string Email { get; set; } = "";

		public string errorMessage = "";
		public string successMessage = "";

		public void OnGet()
		{
		}

		public void OnPost()
		{
			if (!ModelState.IsValid)
			{
				errorMessage = "Data validation failed";
				return;
			}

			// 1) create token, 2) save token in the database, 3) send token by email to the user
			try
			{
				string connectionString = "Data Source=HUY;Initial Catalog=ThayHuy;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "SELECT * FROM users WHERE email=@email";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@email", Email);

						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								string firstname = reader.GetString(1);
								string lastname = reader.GetString(2);

								string token = Guid.NewGuid().ToString();

								// save the token in the database
								SaveToken(Email, token);

								// send the token by email to the user
								string resetUrl = Url.PageLink("/Auth/ResetPassword") + "?token=" + token;
								string username = firstname + " " + lastname;
								string subject = "Password Reset";
								string message = "Dear " + username + ",\n\n" +
                                    "Bạn có thể đặt lại mật khẩu của mình bằng liên kết sau:\n\n" +
									resetUrl + "\n\n" +
									"Best Regards";

								//EmailSender.SendEmail(Email, username, subject, message).Wait();
                                _emailSender.SendEmail(Email, username, subject, message).Wait();
                            }
							else
							{
								errorMessage = "Chúng tôi không có người dùng nào có địa chỉ email này";
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			successMessage = "Vui lòng kiểm tra Email của bạn và nhấp vào link để cài lại mật khẩu";
		}

		private void SaveToken(string email, string token)
		{
			try
			{
				string connectionString = "Data Source=HUY;Initial Catalog=ThayHuy;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					// delete any old token for this email address from the database
					string sql = "DELETE FROM password_resets WHERE email=@email";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@email", email);

						command.ExecuteNonQuery();
					}

					// add token to database
					sql = "INSERT INTO password_resets (email, token) VALUES (@email, @token)";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@email", email);
						command.Parameters.AddWithValue("@token", token);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
