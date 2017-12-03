using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MARKETINFOCOMBINE;


namespace market2
{
    
    public partial class Screen : Form
    {
        
        public marketengin Marketengin;
        public List<string[]> exceldata;
        private string[] data = { "GMARKET", "GSSHOP", "11MARKET", "CJMALL", "AUCTION" };
        public Screen()
        {
            
            InitializeComponent();
            exceldata = new List<string[]>();
            Marketengin =new marketengin();
            //dataGridView1=new DataGridView();
            /*
            var request = WebRequest.Create("http://i.011st.com/t/300_0/an/7/5/1/2/3/6/789751236_B_V128.jpg");

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                pictureBox1.Image = Bitmap.FromStream(stream);
            }
            */
            
            comboBox1.Items.AddRange(data);
            comboBox1.SelectedIndex = 0;

        }

        private void Screen_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

       

        private void button5_Click(object sender, EventArgs e)
        {
            excel excel = new excel(this);
            excel.Show();

        }

        public void screenshow()
        {
            dataGridView1.Rows.Clear();
            foreach (var VARIABLE in Marketengin.getMallinfo())
            {
                /*  datas[0] = mall;
                    datas[1] = itemnum;
                   datas[2] = name;
                   datas[3] = url;
            datas[4] = category;
            datas[5] = price;
            datas[6] = discountprice;
            datas[7] = delivery;
            datas[8] = timeprofit;
            datas[9] = cardprofit;
            datas[10] = etc;
            datas[11] = image;
                 * 
                 */
                string[] data=new string[13];
                data[1] = VARIABLE[0];
                data[2] = VARIABLE[1];
                data[3] = VARIABLE[2];
                data[4] = VARIABLE[5];
                data[7] = VARIABLE[7];
                data[8] = "대기";
                data[10] = "-";
                data[11] = "-";
                data[12] = "-";
                dataGridView1.Rows.Add(data);
               

            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
               
                dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }




        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                ///이미지 버튼 눌렀을 경우
                if (e.ColumnIndex == 5)
                {
                    try
                    {
                        photo photo = new photo(Marketengin.getMallinfo().ElementAt(e.RowIndex).ElementAt(11));
                        photo.Show();
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                        MessageBox.Show("선택한 항목에 이미지가 없습니다.", "취소", MessageBoxButtons.YesNo);

                    }
                
                }
                if (e.ColumnIndex == 14)
                {
                   
                    try
                    {
                        page2cs page2Cs = new page2cs(Marketengin.getproduct(Marketengin.getMallinfo().ElementAt(e.RowIndex).ElementAt(1)));
                        page2Cs.Show();
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                        MessageBox.Show("다시 선택해 주십시오", "취소", MessageBoxButtons.YesNo);
                    }
                    catch (KeyNotFoundException)
                    {

                        MessageBox.Show("해당 상품상세데이터가 없습니다.", "취소", MessageBoxButtons.YesNo);
                    }

                }
                if (e.ColumnIndex == 13)
                {
                   
                    try
                    {
                        System.Diagnostics.Process.Start(Marketengin.getMallinfo().ElementAt(e.RowIndex).ElementAt(3));
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                        MessageBox.Show("다시 선택해 주십시오", "취소", MessageBoxButtons.YesNo);
                    }

                }


            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox4.Text=="")
            {
                MessageBox.Show("다시 입력해주십쇼", "취소", MessageBoxButtons.YesNo);
            }
            else
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    Marketengin.add("gmarket", textBox4.Text);
                    Marketengin.MainscreenList();
                    screenshow();
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    Marketengin.add("gsshop", textBox4.Text);
                    Marketengin.MainscreenList();
                    screenshow();
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    Marketengin.add("eleven", textBox4.Text);
                    Marketengin.MainscreenList();
                    screenshow();
                }
                else if (comboBox1.SelectedIndex == 3)
                {
                    Marketengin.add("cjmall", textBox4.Text);
                    Marketengin.MainscreenList();
                    screenshow();
                }
                else if (comboBox1.SelectedIndex == 4)
                {
                    Marketengin.add("auction", textBox4.Text);
                    Marketengin.MainscreenList();
                    screenshow();
                }
            }

          
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (var VARIABLE in Marketengin.getMallinfo())
            {
                Console.WriteLine(VARIABLE[1]);
            }
          
        }
        /*
        private void button7_Click(object sender, EventArgs e)
        {
            foreach (var VARIABLE in Marketengin.getlist())
            {
                Console.WriteLine(VARIABLE[1]);
            }

        }
        */
        /// <summary>
        /// ///조회버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            
            Thread worker = new Thread(new ThreadStart(MyThreadRun));
            worker.Start();
            Thread worker2 = new Thread(new ThreadStart(MyThreadRun2));
            worker2.Start();
        }

        public void MyThreadRun()
        {
            int totalFiles = 1;
            int counter = 0;
            int pct = 0;
            List<int> inqurylist = new List<int>();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {

                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))

                {
                    inqurylist.Add(i);
                    dataGridView1.Rows[i].Cells[9].Value = DateTime.Now.ToString();
                }

            }
          
            
            Marketengin.inqurystart(inqurylist);

            textBox2.Text = (Marketengin.ellapsed.ToString());

        }



        public void MyThreadRun2()
        {
            int counter = 0;
            int pct = 0;
            bool stop = true;
            DateTime dtPreTime;
            DateTime dtNowTime;
            TimeSpan timeDiff;
            int count5 = 0;
            while (stop)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    try
                    {
                        if (dataGridView1.Rows[i].Cells[8].Value == "완료")
                        {
                            count5++;
                        }

                        if (Marketengin.PRODUCTDATAS.ContainsKey((string)(dataGridView1.Rows[i].Cells[2].Value)))
                        {
                            dataGridView1.Rows[i].Cells[8].Value = "완료";
                            dataGridView1.Rows[i].Cells[10].Style.BackColor = Color.Green;
                           
                          
                            
                            if (dataGridView1.Rows[i].Cells[10].Value=="-")
                            {
                               
                                dataGridView1.Rows[i].Cells[10].Value = DateTime.Now.ToString();
                                DateTime.TryParse(dataGridView1.Rows[i].Cells[9].Value.ToString(), out dtNowTime);
                                DateTime.TryParse(dataGridView1.Rows[i].Cells[10].Value.ToString(), out dtPreTime);
                                timeDiff = dtNowTime - dtPreTime;
                                dataGridView1.Rows[i].Cells[11].Value = timeDiff.ToString();
                                dataGridView1.Rows[i].Cells[12].Value = "이상없음";
                                
                            }



                        
                            

                        }
                    }
                    catch (NullReferenceException)
                    {
                        
                    }
                    catch (KeyNotFoundException)
                    {

                    }
                    catch (ArgumentException)
                    {

                    }

                }

                textBox1.Text =count5 +"/" + dataGridView1.Rows.Count.ToString() + "";
                count5 = 0;
                if (Marketengin.stop==1)
                {
                    break;
                }
                Thread.Sleep(5000);
            }

        }


        delegate void ShowDelegate(int percent);

        private void ShowProgress(int pct)
        {
            if (InvokeRequired)
            {
                ShowDelegate del = new ShowDelegate(ShowProgress);
                //또는 ShowDelegate del = p => ShowProgress(p);
                Invoke(del, pct);
            }
            else
            {
                progressBar1.Value = pct;
            }
        }






        /// <summary>
        /// //모두선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int chk = checkBox1.Checked ? 1 : 0;
            if (chk == 1)
            {
                for (int i = 0; i < dataGridView1.RowCount-1; i++)
                {


                    dataGridView1.Rows[i].Cells[0].Value = true;



                }

            }
            if (chk == 0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {


                    dataGridView1.Rows[i].Cells[0].Value = false;



                }

            }
        }
        
        /// <summary>
        /// //열개선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int chk = checkBox3.Checked ? 1 : 0;
                if (chk == 1)
                {
                    for (int i = 0; i < 10; i++)
                    {


                        dataGridView1.Rows[i].Cells[0].Value = true;



                    }

                }
                if (chk == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {


                        dataGridView1.Rows[i].Cells[0].Value = false;



                    }

                }
            }
            catch (ArgumentOutOfRangeException)
            {
                
          
            }
           

        }
        /// <summary>
        /// //20개선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int chk = checkBox2.Checked ? 1 : 0;
                if (chk == 1)
                {
                    for (int i = 0; i < 20; i++)
                    {


                        dataGridView1.Rows[i].Cells[0].Value = true;



                    }

                }
                if (chk == 0)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = false;
                    }

                }
            }
            catch (ArgumentOutOfRangeException)
            {
                
               
            }

        

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
           
                for (int i = 0; i < dataGridView1.RowCount ; i++)
                {
                    try
                    {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        Marketengin.remove(i, dataGridView1.Rows[i].Cells[2].Value.ToString());
                    }

                    }
                    catch (NullReferenceException)
                    {
                        
                       
                    }

                }
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            Marketengin.removeall();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
