using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Responsi2
{



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connstring = "Host=localhost;Port=2022;Username=postgres;Password=informatika;Database=responsiDika";
        
        private NpgsqlConnection conn;
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from st_insert_karyawan(:_nama, :_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", tb_Nama.Text);
                cmd.Parameters.AddWithValue("_id_dep", tb_Dep.Text);
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Berhasil Memasukkan Data!","Well Done",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    conn.Close();
                    btn_LoadData.PerformClick();
                    tb_Nama.Text = tb_Dep.Text = null;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Gagal Memasukkan Data!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon Pilih Data!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn.Open();
                sql = @"select * from st_delete_karyawan(:_id_karyawan)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());

                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Berhasil Menghapus Data!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btn_LoadData.PerformClick();
                    tb_Nama.Text = tb_Dep.Text = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Menghapus Data!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon Pilih Data!","", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn.Open();
                sql = @"select * from st_update_karyawan(:_id_karyawan,:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", tb_Nama.Text);
                cmd.Parameters.AddWithValue("_id_dep", tb_Dep.Text);
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Berhasil Mengupdate Data!", "Well Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btn_LoadData.PerformClick();
                    tb_Nama.Text = tb_Dep.Text = null;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Gagal Mengupdate Data!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                tb_Nama.Text = r.Cells["_nama"].Value.ToString();
                tb_Dep.Text = r.Cells["_id_dep"].Value.ToString();
            }
        }



        private void btn_LoadData_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = "select * from st_select_karyawan()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Menampilkan Data!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
