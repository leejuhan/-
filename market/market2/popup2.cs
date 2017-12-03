using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace market2
{
    public partial class popup2 : Form
    {
        public popup2(string text)
        {
            InitializeComponent();
            textBox18.Text = text;
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
