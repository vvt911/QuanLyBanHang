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
    public partial class frmNhaCungCap : Form
    {
        DataTable dtNCC; //Chữa dữ liệu bảng Nhà cung cấp

        public frmNhaCungCap()
        {
            InitializeComponent();
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            txtMaNhaCungCap.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView(); //Hiển thị bảng Nhà Cung Cấp
        }

        private void LoadDataGridView()
        {
            string sql;
            sql = "Select * from NhaCungCap";

            //Đọc dữ liệu từ bảng
            dtNCC = Class.Functions.GetDataToTable(sql);
            dgvNhaCungCap.DataSource = dtNCC; //Nguồn dữ liệu
            dgvNhaCungCap.Columns[0].HeaderText = "Mã nhà cung cấp";
            dgvNhaCungCap.Columns[1].HeaderText = "Tên nhà cung cấp";

            dgvNhaCungCap.AllowUserToAddRows = false; //Không cho người dùng thêm dữ liệu trực tiếp
            dgvNhaCungCap.EditMode = DataGridViewEditMode.EditProgrammatically; //Không cho sửa dữ liệu trực tiếp
        }

        private void dgvNhaCungCap_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNhaCungCap.Focus();
                return;
            }
            if (dgvNhaCungCap.Rows.Count == 0) //Nếu không có dữ liệu
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtMaNhaCungCap.Text = dgvNhaCungCap.CurrentRow.Cells["MaNCC"].Value.ToString();
            txtTenNhaCungCap.Text = dgvNhaCungCap.CurrentRow.Cells["TenNCC"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
            btnThem.Enabled = false;
            txtMaNhaCungCap.Text = "";
            txtTenNhaCungCap.Text = "";
            txtMaNhaCungCap.Enabled = true;     //Cho phép nhập mới
            txtMaNhaCungCap.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaNhaCungCap.Text.Trim().Length == 0) //Chưa nhập mã chất liệu
            {
                MessageBox.Show("Bạn phải nhập mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNhaCungCap.Focus();
                return;
            }

            if (txtTenNhaCungCap.Text.Trim().Length == 0) //Chưa nhập mã chất liệu
            {
                MessageBox.Show("Bạn phải nhập mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNhaCungCap.Focus();
                return;
            }

            sql = "Select MaNCC from NhaCungCap where MaNCC = N'" + txtMaNhaCungCap.Text.Trim() + "'";
            if (Class.Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã nhà cung cấp đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhaCungCap.Focus();
                return ;
            }

            sql = "INSERT INTO NhaCungCap VALUES(N'" + txtMaNhaCungCap.Text + "', N'"+ txtTenNhaCungCap.Text +"')";
            Class.Functions.RunSQL(sql);
            txtMaNhaCungCap.Text = "";
            txtTenNhaCungCap.Text = "";
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            txtMaNhaCungCap.Enabled = false;
            LoadDataGridView();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql; //Lưu câu lệnh sql
            if (dtNCC.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNhaCungCap.Text == "") //nếu chưa chọn Nhà cung cấp nào
            {
                MessageBox.Show("Bạn chưa chọn Nhà cung cấp nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenNhaCungCap.Text.Trim().Length == 0) //nếu chưa nhập tên nhà cung cấp
            {
                MessageBox.Show("Bạn chưa nhập tên chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sql = "UPDATE NhaCungCap SET TenNCC=N'"+ txtTenNhaCungCap.Text.ToString() +"' WHERE MaNCC=N'" + txtMaNhaCungCap.Text +"'";
            Class.Functions.RunSQL(sql);
            LoadDataGridView();
            txtMaNhaCungCap.Text = "";
            txtTenNhaCungCap.Text = "";

            btnBoQua.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (dtNCC.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNhaCungCap.Text == "") //nếu chưa chọn Nhà cung cấp nào
            {
                MessageBox.Show("Bạn chưa chọn Nhà cung cấp nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE NhaCungCap WHERE MaNCC=N'" + txtMaNhaCungCap.Text + "'";
                Class.Functions.RunSqlDel(sql);
                LoadDataGridView();
                txtMaNhaCungCap.Text = "";
                txtTenNhaCungCap.Text = "";
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            txtMaNhaCungCap.Text = "";
            txtTenNhaCungCap.Text = "";

            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaNhaCungCap.Enabled = false;
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
