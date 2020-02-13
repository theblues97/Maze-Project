using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResearchFinal
{
    public partial class SizePick : Form
    {
        private int width;
        private int height;
        public SizePick()
        {
            InitializeComponent();           
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                width = Int32.Parse(txtWidth.Text);
                height = Int32.Parse(txtHeight.Text);

                if (width > 5  && width < 41 && height > 3 && height < 31)
                {

                    Content._width = Int32.Parse(txtWidth.Text) / 2;
                    Content._height = Int32.Parse(txtHeight.Text) / 2;
                    Content._ePoint = new Point(Int32.Parse(txtWidth.Text) - 1, Int32.Parse(txtHeight.Text) - 1);

                    this.Hide();
                    Home frmHome = new Home();
                    frmHome.ShowDialog();

                    this.Close();
                }
                else
                {
                    DialogResult answer = MessageBox.Show("Thông số nhập quá lớn, bạn có muốn để mặc định?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        this.Hide();
                        Home frmHome = new Home();
                        frmHome.ShowDialog();

                        this.Close();

                    }
                    else
                    {
                        txtWidth.Text = "";
                        txtHeight.Text = "";
                    }
                }
            }
            catch
            {
                DialogResult answer = MessageBox.Show("Thông số nhập bị sai, bạn có muốn để thông số mặc định?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    this.Hide();
                    Home frmHome = new Home();
                    frmHome.ShowDialog();

                    this.Close();
                }
                else
                {
                    txtWidth.Text = "";
                    txtHeight.Text = "";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtWidth_Leave(object sender, EventArgs e)
        {
            int value;

            try { value = Int32.Parse(txtWidth.Text); }
            catch { value = 0; }
            value = value % 2 == 0 ? value : value - 1;
            txtHeight.Text =  (value * 3 / 4) % 2 == 0 ? (value * 3 / 4).ToString() : ((value * 3 / 4) - 1).ToString();
        }
    }
}
