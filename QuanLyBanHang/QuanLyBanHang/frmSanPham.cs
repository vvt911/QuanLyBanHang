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
    public partial class frmSanPham : Form
    {
        DataTable dtSP;

        public frmSanPham()
        {
            InitializeComponent();
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from NhaCungCap";
            txtMaSanPham.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView();
            Functions.FillComboBox(sql, cboMaNhaCungCap, "MaNCC");
            cboMaNhaCungCap.SelectedIndex = -1;
        }

        public void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * FROM SanPham";
            dtSP = Functions.GetDataToTable(sql); //lấy dữ liệu
            dgvSanPham.DataSource = dtSP;
            dgvSanPham.Columns[0].HeaderText = "Mã sản phẩm";
            dgvSanPham.Columns[1].HeaderText = "Mã nhà cung cấp";
            dgvSanPham.Columns[2].HeaderText = "Tên sản phẩm";
            dgvSanPham.Columns[3].HeaderText = "Số lượng";
            dgvSanPham.Columns[4].HeaderText = "Đơn giá nhập";
            dgvSanPham.Columns[5].HeaderText = "Đơn giá bán";
               
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvSanPham_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaSanPham.Focus();
                return;
            }
            if (dtSP.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtMaSanPham.Text = dgvSanPham.CurrentRow.Cells["MaSP"].Value.ToString();
            cboMaNhaCungCap.Text = dgvSanPham.CurrentRow.Cells["MaNCC"].Value.ToString();
            txtTenSanPham.Text = dgvSanPham.CurrentRow.Cells["TenSP"].Value.ToString();
            txtSoLuong.Text = dgvSanPham.CurrentRow.Cells["SoLuong"].Value.ToString();
            txtDonGiaNhap.Text = dgvSanPham.CurrentRow.Cells["DonGiaNhap"].Value.ToString();
            txtDonGiaBan.Text = dgvSanPham.CurrentRow.Cells["DonGiaBan"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnTimKiem.Enabled = false;
            ResetValues();
            txtMaSanPham.Enabled = true;
            txtMaSanPham.Focus();
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            txtDonGiaBan.Enabled = true;
        }

        private void ResetValues()
        {
            txtMaSanPham.Text = "";
            cboMaNhaCungCap.Text = "";
            txtTenSanPham.Text = "";
            txtSoLuong.Text = "";
            txtDonGiaNhap.Text = "";
            txtDonGiaBan.Text = "";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;

            if (txtMaSanPham.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSanPham.Focus();
                return;
            }

            if (cboMaNhaCungCap.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaNhaCungCap.Focus();
                return;
            }

            if (txtTenSanPham.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSanPham.Focus();
                return;
            }

            if (txtSoLuong.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }
            if (!Char.IsDigit(txtSoLuong.Text, txtSoLuong.Text.Length - 1))
            {
                MessageBox.Show("Số lượng không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            if (txtDonGiaNhap.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập đơn giá nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return;
            }
            if (!Char.IsDigit(txtDonGiaNhap.Text, txtDonGiaNhap.Text.Length - 1))
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return;
            }

            if (txtDonGiaBan.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập đơn giá bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return;
            }
            if (!Char.IsDigit(txtDonGiaBan.Text, txtDonGiaBan.Text.Length - 1))
            {
                MessageBox.Show("Đơn giá bán không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return;
            }



            sql = "SELECT MaSP FROM SanPham WHERE MaSP=N'" + txtMaSanPham.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSanPham.Focus();
                txtMaSanPham.Text = "";
                return;
            }

            sql = "SELECT MaNCC FROM NhaCungCap WHERE MaNCC=N'" + cboMaNhaCungCap.Text.Trim() + "'";
            if (!Functions.CheckKey(sql))
            {
                if (MessageBox.Show("Mã nhà cung cấp không tồn tại, bạn có muốn thêm nhà cung cấp", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    frmNhaCungCap ncc = new frmNhaCungCap();
                    ncc.Show();
                }
                return;
            }

            sql = "INSERT INTO SanPham VALUES (N'" + txtMaSanPham.Text.Trim() + "', N'" + cboMaNhaCungCap.Text.Trim() + "', N'" + txtTenSanPham.Text.Trim() + "', N'" + txtSoLuong.Text.Trim() + "', N'" + txtDonGiaNhap.Text.Trim() + "', N'" + txtDonGiaBan.Text.Trim() + "')";
            Class.Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;

            if (dtSP.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtMaSanPham.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboMaNhaCungCap.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaNhaCungCap.Focus();
                return;
            }

            if (txtTenSanPham.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSanPham.Focus();
                return;
            }

            if (txtSoLuong.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }
            if (!Char.IsDigit(txtSoLuong.Text, txtSoLuong.Text.Length - 1))
            {
                MessageBox.Show("Số lượng không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            if (txtDonGiaNhap.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập đơn giá nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return;
            }
            if (!Char.IsDigit(txtDonGiaNhap.Text, txtDonGiaNhap.Text.Length - 1))
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return;
            }

            if (txtDonGiaBan.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập đơn giá bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return;
            }
            if (!Char.IsDigit(txtDonGiaBan.Text, txtDonGiaBan.Text.Length - 1))
            {
                MessageBox.Show("Đơn giá bán không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return;
            }

            sql = "SELECT MaNCC FROM NhaCungCap WHERE MaNCC=N'" + cboMaNhaCungCap.Text.Trim() + "'";
            if (!Functions.CheckKey(sql))
            {
                if (MessageBox.Show("Mã nhà cung cấp không tồn tại, bạn có muốn thêm nhà cung cấp", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    frmNhaCungCap ncc = new frmNhaCungCap();
                    ncc.Show();
                }
                return;
            }

            sql = "UPDATE SanPham SET MaNCC = N'" + cboMaNhaCungCap.Text.Trim() + "', TenSP = N'" + txtTenSanPham.Text.Trim() + "', SoLuong = N'" + txtSoLuong.Text.Trim() + "', DonGiaNhap = N'" + txtDonGiaNhap.Text.Trim() + "', DonGiaBan = N'" + txtDonGiaBan.Text.Trim() + "' WHERE MaSP = N'" + txtMaSanPham.Text.Trim() + "'";
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnBoQua.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (dtSP.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaSanPham.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE SanPham WHERE MaSP=N'" + txtMaSanPham.Text + "'";
                Functions.RunSqlDel(sql);
                LoadDataGridView();
                ResetValues();
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnThem.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnTimKiem.Enabled = true;
            txtMaSanPham.Enabled = false;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaSanPham.Enabled == false)
            {
                txtMaSanPham.Enabled = true;
                txtMaSanPham.Focus();
                return;
            }
            if ((txtMaSanPham.Text == "") && (txtTenSanPham.Text == "") && (cboMaNhaCungCap.Text == ""))
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from SanPham WHERE 1=1";
            if (txtMaSanPham.Text != "")
                sql += " AND MaSP LIKE N'%" + txtMaSanPham.Text + "%'";
            if (cboMaNhaCungCap.Text != "")
                sql += " AND MaNCC LIKE N'%" + cboMaNhaCungCap.Text + "%'";
            if (txtTenSanPham.Text != "")
                sql += " AND TenSP LIKE N'%" + txtTenSanPham.Text + "%'";
            dtSP = Functions.GetDataToTable(sql);
            if (dtSP.Rows.Count == 0)
                MessageBox.Show("Không có sản phẩm nào thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + dtSP.Rows.Count + "  sản phẩm thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvSanPham.DataSource = dtSP;
            ResetValues();
        }

        private void btnHienThiDS_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * FROM SanPham";
            dtSP = Functions.GetDataToTable(sql);
            dgvSanPham.DataSource = dtSP;
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
