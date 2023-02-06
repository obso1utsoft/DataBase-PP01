using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataGrid.Connection
{
    public partial class ConnectionToDb
    {
        public static MySqlConnection con = new MySqlConnection(App.connectToDB);
    }
}
