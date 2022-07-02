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
    public partial class frmHoaDon : Form
    {
        DataTable dtHD;

        public frmHoaDon()
        {
            InitializeComponent();
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            txtMaHoaDon.ReadOnly = true;
            txtTenNhanVien.ReadOnly = true;
            txtTenKhachHang.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtSDT.ReadOnly = true;
            txtTenSanPham.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtGiamGia.Text = "0";
            txtTongTien.ReadOnly = true;
            Functions.FillComboBox("SELECT MaKH FROM KhachHang", cboMaKhachHang, "MaKH");
            cboMaKhachHang.SelectedIndex = -1;
            Functions.FillComboBox("SELECT MaNV FROM NhanVien", cboMaNhanVien, "MaNV");
            cboMaNhanVien.SelectedIndex = -1;
            Functions.FillComboBox("SELECT MaSP FROM SanPham", cboMaSanPham, "MaSP");
            cboMaSanPham.SelectedIndex = -1;

            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaHoaDon.Text != "")
            {
                LoadInfoHoaDon();
                btnXoa.Enabled = true;
            }
            LoadDataGridView();
        }

        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.MaSP, b.TenSP, a.SoLuong, b.DonGiaBan, a.GiamGia, a.ThanhTien FROM ChiTietHoaDon AS a, SanPham AS b WHERE a.MaHD = N'" + txtMaHoaDon.Text.Trim() + "' AND a.MaSP = b.MaSP";
            dtHD = Functions.GetDataToTable(sql);
            dgvHoaDon.DataSource = dtHD;
            dgvHoaDon.Columns[0].HeaderText = "Mã sản phẩm";
            dgvHoaDon.Columns[1].HeaderText = "Tên sản phẩm";
            dgvHoaDon.Columns[2].HeaderText = "Số lượng";
            dgvHoaDon.Columns[3].HeaderText = "Đơn giá";
            dgvHoaDon.Columns[4].HeaderText = "Giảm giá %";
            dgvHoaDon.Columns[5].HeaderText = "Thành tiền";

            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NgayBan FROM HoaDon WHERE MaHD = N'" + txtMaHoaDon.Text.Trim() + "'";
            dtpNgayBan.Value = DateTime.Parse(Functions.GetFieldValues(str));

            str = "SELECT MaNV FROM HoaDon WHERE MaHD = N'" + txtMaHoaDon.Text.Trim() + "'";
            cboMaNhanVien.Text = Functions.GetFieldValues(str);

            str = "SELECT MaKH FROM HoaDon WHERE MaHD = N'" + txtMaHoaDon.Text.Trim() + "'";
            cboMaKhachHang.Text = Functions.GetFieldValues(str);

            str = "SELECT TongTien FROM HoaDon WHERE MaHD = N'" + txtMaHoaDon.Text.Trim() + "'";
            txtTongTien.Text = Functions.GetFieldValues(str);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
            txtMaHoaDon.Text = Functions.CreateKey("HD");
            LoadDataGridView();
        }

        private void ResetValues()
        {
            txtMaHoaDon.Text = "";
            dtpNgayBan.Text = DateTime.Now.ToShortDateString();
            cboMaNhanVien.Text = "";
            cboMaKhachHang.Text = "";
            txtTongTien.Text = "0";
            cboMaSanPham.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, tong;

            sql = "SELECT MaHD FROM HoaDon WHERE MaHD=N'" + txtMaHoaDon.Text.Trim() + "'";
            if (!Functions.CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HD được tạo tự động do đó không có trường hợp trùng khóa
                if (cboMaNhanVien.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaNhanVien.Focus();
                    return;
                }
                if (cboMaKhachHang.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập mã khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaKhachHang.Focus();
                    return;
                }
                sql = "INSERT INTO HoaDon VALUES (N'" + txtMaHoaDon.Text.Trim() + "',N'" + cboMaNhanVien.SelectedValue + "', N'" + cboMaKhachHang.SelectedValue + "', '" + dtpNgayBan.Value.ToString("yyyy/MM/dd") + "', " + txtTongTien.Text.Trim() + ")";
                Functions.RunSQL(sql);
            }

            // Lưu thông tin của các mặt hàng
            if (cboMaSanPham.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaSanPham.Focus();
                return;
            }
            if ((txtSoLuong.Text.Trim().Length == 0) || (txtSoLuong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            if (txtGiamGia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGiamGia.Focus();
                return;
            }

            //Kiểm tra xem sản phẩm đã có trong hóa đơn chưa
            sql = "SELECT MaSP FROM ChiTietHoaDon WHERE MaSP=N'" + cboMaSanPham.SelectedValue + "' AND MaHD = N'" + txtMaHoaDon.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Sản phầm này đã có, bạn phải chọn sản phẩm khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMaSanPham.Focus();
                return;
            }

            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(Functions.GetFieldValues("SELECT SoLuong FROM SanPham WHERE MaSP = N'" + cboMaSanPham.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoLuong.Text.Trim()) > sl)
            {
                MessageBox.Show("Số lượng sản phẩm này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }

            //Thêm vào bảng chi tiết hóa đơn
            sql = "INSERT INTO ChiTietHoaDon VALUES (N'" + txtMaHoaDon.Text.Trim() + "', N'" + cboMaSanPham.SelectedValue + "'," + txtSoLuong.Text.Trim() + ", " + txtDonGia.Text.Trim() + ", " + txtGiamGia.Text.Trim() + ", " + txtThanhTien.Text.Trim() + ")";
            Functions.RunSQL(sql);
            LoadDataGridView();

            tong = Convert.ToDouble(Functions.GetFieldValues("SELECT TongTien FROM HoaDon WHERE MaHD = N'" + txtMaHoaDon.Text.Trim() + "'"));
            txtTongTien.Text = tong.ToString(); 
            ResetValuesHang();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
        }

        private void ResetValuesHang()
        {
            cboMaSanPham.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void dgvHoaDon_DoubleClick(object sender, EventArgs e)
        {
            string MaHangxoa, sql;
            Double tong;
            if (dtHD.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa chi tiết hóa đơn
                MaHangxoa = dgvHoaDon.CurrentRow.Cells["MaSP"].Value.ToString();
                sql = "DELETE ChiTietHoaDon WHERE MaHD = N'" + txtMaHoaDon.Text + "' AND MaSP = N'" + MaHangxoa + "'";
                Functions.RunSQL(sql);

                tong = Convert.ToDouble(Functions.GetFieldValues("SELECT TongTien FROM HoaDon WHERE MaHD = N'" + txtMaHoaDon.Text + "'"));
                txtTongTien.Text = tong.ToString();
                LoadDataGridView();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql;

                //Xóa chi tiết hóa đơn
                sql = "SELECT * FROM ChiTietHoaDon WHERE MaHD = N'" + txtMaHoaDon.Text + "'";
                DataTable dtRows = Functions.GetDataToTable(sql);
                for (int row = 0; row <= dtRows.Rows.Count - 1; row++)
                {
                    // Duyệt từng hàng để xóa
                    sql = "DELETE ChiTietHoaDon WHERE MaHD = N'" + txtMaHoaDon.Text + "' AND MaSP = N'"+ dtRows.Rows[row]["MaSP"] +"'";
                    Functions.RunSqlDel(sql);
                }

                //Xóa hóa đơn
                sql = "DELETE HoaDon WHERE MaHD = N'" + txtMaHoaDon.Text + "'";
                Functions.RunSqlDel(sql);

                ResetValues();
                LoadDataGridView();
                btnXoa.Enabled = false;
            }
        }

        private void cboMaNhanVien_TextChanged(object sender, EventArgs e)
        {
            string sql;
            if (cboMaNhanVien.Text == "")
                txtTenNhanVien.Text = "";
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            sql = "Select TenNV from NhanVien where MaNV = N'" + cboMaNhanVien.SelectedValue + "'";
            txtTenNhanVien.Text = Functions.GetFieldValues(sql);
        }

        private void cboMaKhachHang_TextChanged(object sender, EventArgs e)
        {
            string sql;
            if (cboMaKhachHang.Text == "")
            {
                txtTenKhachHang.Text = "";
                txtDiaChi.Text = "";
                txtSDT.Text = "";
            }

            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            sql = "Select TenKH from KhachHang where MaKH = N'" + cboMaKhachHang.SelectedValue + "'";
            txtTenKhachHang.Text = Functions.GetFieldValues(sql);
            sql = "Select DiaChi from KhachHang where MaKH = N'" + cboMaKhachHang.SelectedValue + "'";
            txtDiaChi.Text = Functions.GetFieldValues(sql);
            sql = "Select SDT from KhachHang where MaKH= N'" + cboMaKhachHang.SelectedValue + "'";
            txtSDT.Text = Functions.GetFieldValues(sql);
        }

        private void cboMaSanPham_TextChanged(object sender, EventArgs e)
        {
            string sql;
            if (cboMaSanPham.Text == "")
            {
                txtTenSanPham.Text = "";
                txtDonGia.Text = "";
            }
            // Khi chọn mã sản phẩm thì các thông tin về sản phẩm hiện ra
            sql = "SELECT TenSP FROM SanPham WHERE MaSP = N'" + cboMaSanPham.SelectedValue + "'";
            txtTenSanPham.Text = Functions.GetFieldValues(sql);
            sql = "SELECT DonGiaBan FROM SanPham WHERE MaSP =N'" + cboMaSanPham.SelectedValue + "'";
            txtDonGia.Text = Functions.GetFieldValues(sql);
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGia.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGia.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi giảm giá thì tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGia.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGia.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboMaHoaDon.Text == "")
            {
                MessageBox.Show("Bạn phải chọn mã hóa đơn để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHoaDon.Focus();
                return;
            }
            txtMaHoaDon.Text = cboMaHoaDon.Text;
            LoadInfoHoaDon();
            LoadDataGridView();
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            cboMaHoaDon.SelectedIndex = -1;
        }

        private void cboMaHoaDon_DropDown(object sender, EventArgs e)
        {
            Functions.FillComboBox("SELECT MaHD FROM HoaDon", cboMaHoaDon, "MaHD");
            cboMaHoaDon.SelectedIndex = -1;
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
