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
    public partial class popup : Form
    {
      
        public popup(String text)
        {
           
            InitializeComponent();
            textBox4.Text = text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
