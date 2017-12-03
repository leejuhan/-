using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace market2
{
    public partial class photo : Form
    {
        public string link;
        public photo(string link)
        {

            InitializeComponent();
            this.link = link;
            try
            {
                var request = WebRequest.Create(this.link);
                Console.WriteLine(link);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    pictureBox1.Image = Bitmap.FromStream(stream);
                }

            }
            catch (UriFormatException e)
            {
                Console.WriteLine("사진박스 예외");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("사진박스 예외");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("사진박스 예외");
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("선택한 항목에 이미지가 없습니다.", "취소", MessageBoxButtons.YesNo);
            }



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }
    }
}
