using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class beginForm : Form
    {
        
        public beginForm()
        {
            
            InitializeComponent();
            var pos = this.PointToScreen(label1.Location);
            pos = pictureBox1.PointToClient(pos);
            label1.Parent = pictureBox1;
            label1.Location = pos;
            label1.BackColor = Color.Transparent;

            var pos1 = this.PointToScreen(label2.Location);
            pos1 = pictureBox1.PointToClient(pos1);
            label2.Parent = pictureBox1;
            label2.Location = pos1;
            label2.BackColor = Color.Transparent;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1(false);
            fm1.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1(true);
            fm1.Show();
        }
    }
}
