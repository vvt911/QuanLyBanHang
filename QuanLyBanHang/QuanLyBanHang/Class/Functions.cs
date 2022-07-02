using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyBanHang.Class
{
    internal class Functions
    {
        public static SqlConnection conn; //Khai báo đối tượng kết nối

        public static void Connect()
        {
            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=DESKTOP-R08TGCI\SQLEXPRESS;Initial Catalog=QuanLyBanHang;Integrated Security=True";
            conn.Open();
        }

        public static void Disconnect()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();       
                conn.Dispose();     //Giải phóng tài nguyên
                conn = null;
            }
        }

        public static DataTable GetDataToTable (string sql)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        //Hàm kiểm tra khóa trùng
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }

        //Hàm thực hiện câu lệnh sql
        public static void RunSQL(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn); 
            try
            {
                cmd.ExecuteNonQuery();  //Thực hiện câu lệnh sql
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }

        //Xóa dữ liệu
        public static void RunSqlDel(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi, không thể xoá...\n" + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            cmd.Dispose();
            cmd = null;
        }

        //Lấy dữ liệu từ SQL đổ vào ComboBox
        public static void FillComboBox(string sql, ComboBox cbo, string ma)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            cbo.DataSource = table;
            cbo.ValueMember = ma;
            cbo.DisplayMember = ma;
        }

        //Lấy dữ liệu từ 1 câu lệnh SQL
        public static string GetFieldValues(string sql)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            return ma;
        }

        //Tạo mã hóa đơn tự động
        public static string CreateKey(string tiento)
        {
            string key = tiento;
            int count = Convert.ToInt32(GetFieldValues("Select count(MaHD) from HoaDon")) + 1 ;
            return tiento + count.ToString();
        }
    }
}
