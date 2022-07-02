using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyBanHang.Class;

namespace QuanLyBanHang
{
    public partial class frmTimKiem : Form
    {
        DataTable dtTK;

        public frmTimKiem()
        {
            InitializeComponent();
        }

        private void frmTimKiem_Load(object sender, EventArgs e)
        {
            ResetValues();
            dgvTimKiemHoaDon.DataSource = null;
        }

        private void ResetValues()
        {
            foreach (Control Ctl in this.Controls)
                if (Ctl is TextBox)
                    Ctl.Text = "";
            txtMaHoaDon.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql;

            if ((txtMaHoaDon.Text.Trim() == "") && (txtThang.Text.Trim() == "") && (txtNam.Text.Trim() == "") && (txtMaNhanVien.Text.Trim() == "") && (txtMaKhachHang.Text.Trim() == ""))
            {
                MessageBox.Show("Hãy nhập một điều kiện tìm kiếm!!!", "Yêu cầu ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sql = "SELECT * FROM HoaDon WHERE 1=1";
            if (txtMaHoaDon.Text.Trim() != "")
                sql = sql + " AND MaHD Like N'%" + txtMaHoaDon.Text.Trim() + "%'";
            if (txtThang.Text.Trim() != "")
                sql = sql + " AND MONTH(NgayBan) =" + txtThang.Text.Trim();
            if (txtNam.Text.Trim() != "")
                sql = sql + " AND YEAR(NgayBan) =" + txtNam.Text.Trim();
            if (txtMaKhachHang.Text.Trim() != "")
                sql = sql + " AND MaKH Like N'%" + txtMaKhachHang.Text.Trim() + "%'";
            if (txtMaNhanVien.Text.Trim() != "")
                sql = sql + " AND MaNV Like N'%" + txtMaNhanVien.Text.Trim() + "%'";
            
            dtTK = Functions.GetDataToTable(sql);
            if (dtTK.Rows.Count == 0)
            {
                MessageBox.Show("Không có hóa đơn nào thỏa mãn điều kiện!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Có " + dtTK.Rows.Count + " bản ghi thỏa mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dgvTimKiemHoaDon.DataSource = dtTK;
            LoadDataGridView();
        }

        private void LoadDataGridView()
        {
            dgvTimKiemHoaDon.Columns[0].HeaderText = "Mã hóa đơn";
            dgvTimKiemHoaDon.Columns[1].HeaderText = "Mã nhân viên";
            dgvTimKiemHoaDon.Columns[2].HeaderText = "Mã khách hàng";
            dgvTimKiemHoaDon.Columns[3].HeaderText = "Ngày bán";
            dgvTimKiemHoaDon.Columns[4].HeaderText = "Tổng tiền";
            
            dgvTimKiemHoaDon.AllowUserToAddRows = false;
            dgvTimKiemHoaDon.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvTimKiemHoaDon_DoubleClick(object sender, EventArgs e)
        {
            string mahd;
            if (MessageBox.Show("Hiển thị thông tin chi tiết?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                mahd = dgvTimKiemHoaDon.CurrentRow.Cells["MaHD"].Value.ToString();
                frmHoaDon hd = new frmHoaDon();
                hd.txtMaHoaDon.Text = mahd;
                hd.StartPosition = FormStartPosition.CenterParent;
                hd.ShowDialog();
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận đóng?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
