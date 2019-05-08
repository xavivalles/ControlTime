using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlTime
{
    class DatabaseConnection
    {
        public SqlConnection GetConnection()
        {
            SqlConnection cn = new SqlConnection(@"Server=tp-ae-develop.database.windows.net;Database=tp-ae-develop;User Id=aeadmin;Password=Travelport2017;");
            cn.Open();
            return cn;
        }

        //SELECT Name FROM Triggers
        public Dictionary<int, string> GetTriggerNames()
        {
            var events = new Dictionary<int,string>();
            var cont = 0;
            SqlConnection cn = GetConnection();
            string commandText = "SELECT Name FROM Triggers";
            SqlCommand command = new SqlCommand(commandText, cn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    events.Add(cont, reader.GetString(reader.GetOrdinal("Name")));
                    //Console.WriteLine(String.Format(cont + ": {0}", reader.GetString(reader.GetOrdinal("Name"))));
                    cont++;
                }
            }
            cn.Close();
            return events;
        }
    }
}
