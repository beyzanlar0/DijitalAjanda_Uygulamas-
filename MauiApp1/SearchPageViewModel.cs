using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace MauiApp1
{
  public class SearchPageViewModel
    {
        public async Task<bool> SendFriendRequestAsync(string fromUser, string toUser)
        {
            try
            {
                
                string connectionString = "Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=True;TrustServerCertificate=True";

              
                string query = "INSERT INTO FriendRequests (FromUser, ToUser) VALUES (@FromUser, @ToUser)";

                using (var connection = new SqlConnection(connectionString))
                {
                   
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@FromUser", fromUser);
                        command.Parameters.AddWithValue("@ToUser", toUser);

                        
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
