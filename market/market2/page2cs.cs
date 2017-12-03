using System;
using System.Collections;
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
    public partial class page2cs : Form
    {
        private ArrayList PRODUCT;
        private List<string> url;
        private List<string> profit;
        
        public page2cs(ArrayList PRODUCT)
        {
            InitializeComponent();
            /////////////////////////////////////////////////////////////////////////////
            string[] data = { "무이자혜택", "카드혜택", "기타혜택" };
            // 각 콤보박스에 데이타를 초기화
            comboBox1.Items.AddRange(data);
            // 처음 선택값 지정. 첫째 아이템 선택
            textBox13.Text = ((string[]) PRODUCT[15]).ElementAt(8);
            ///////////////////////////////////////////////////////////////////////////////
            comboBox1.SelectedIndex = 0;
            this.PRODUCT = PRODUCT;
            this.url=new List<string>();
            this.profit=new List<string>();
            textBox4.Text = ((string[]) PRODUCT[15]).ElementAt(2);
            textBox6.Text = ((string[])PRODUCT[15]).ElementAt(1);
            textBox7.Text = ((string[])PRODUCT[15]).ElementAt(0);
            textBox18.Text = ((string[])PRODUCT[15]).ElementAt(4);
            textBox19.Text = ((string[])PRODUCT[15]).ElementAt(7);
            textBox20.Text = ((string[])PRODUCT[15]).ElementAt(3);
            textBox1.Text = ((string[])PRODUCT[15]).ElementAt(5);
            textBox2.Text = ((int) PRODUCT[5]) + "위";
            textBox3.Text = ((int)PRODUCT[8]) + "개";
            textBox9.Text = ((int)PRODUCT[7]) + "원";
            textBox5.Text = ((int)PRODUCT[6]) + "원";
            textBox8.Text = ((int)PRODUCT[9]) + "원";
            int tmp = ((int) PRODUCT[6]) - ((int) PRODUCT[7]);
            textBox10.Text = tmp+"원";
            textBox11.Text = ((int)PRODUCT[11]) + "개";
            textBox12.Text = ((int)PRODUCT[12]) + "위";
            /////////////그림/////////////////////////////
            string imageurl = "";
            try
            {
                imageurl = ((string[]) PRODUCT[15]).ElementAt(11);
                var request = WebRequest.Create(imageurl);
                Console.WriteLine(imageurl);
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


            ///////////////////////////////////////////// 
            ////////////////////////////////////////////////
            ////14  15   16
          



            /////////////////////////////////////////////////////////


        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 1;
            int tmpcount = 0;
            dataGridView1.Rows.Clear();
            url.Clear();
            profit.Clear();
            dataGridView1.Columns[0].HeaderText = "순위";
            foreach (var VARIABLE in (List<string[]>)PRODUCT[2])
            {
                /*
                 *   datas[0] = realurl.Trim();
                     datas[1] = name.Trim();
                     datas[2] = itemNum.Trim();
                     datas[3] = price.Trim();
                     datas[4] = profit.Trim();
                     datas[5] = delivery.Trim();
                 * 
                 */

                url.Add(VARIABLE[0]);
                profit.Add(VARIABLE[4]);
                string[] data=new string[10];
                data[0]=count + "";
                data[1] = VARIABLE[1];
                data[2] = VARIABLE[2];
                data[3] = ((string[])PRODUCT[15]).ElementAt(2); ;
                data[4] = VARIABLE[3];
                data[5] = cal(VARIABLE[3]) - ((int) PRODUCT[6]) + "";
                data[6] = ((int) PRODUCT[6]) - cal(VARIABLE[3]) + "";
                /*
                foreach (var VARIABLE2 in (List<int>)PRODUCT[13])
                {
                    Console.WriteLine(VARIABLE2);
                }*/
                //data[5] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                //data[6] = ((List<int>)PRODUCT[14]).ElementAt(tmpcount) + "원";
                data[7] = VARIABLE[5];
               // data[8] = ((List<int>) PRODUCT[13]).ElementAt(tmpcount) + "원";
               // data[9] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                dataGridView1.Rows.Add(data);
                count++;
                tmpcount++;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Console.WriteLine("줄맞추기");
                dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void page2cs_Load(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            System.Diagnostics.Process.Start(textBox20.Text);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView) sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                ///이미지 버튼 눌렀을 경우
                if (e.ColumnIndex == 8)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(url.ElementAt(e.RowIndex));
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                        MessageBox.Show("선택한 항목에 이미지가 없습니다.", "취소", MessageBoxButtons.YesNo);

                    }

                }
                if (e.ColumnIndex == 9)
                {
                    try
                    {
                        popup popup=new popup(profit.ElementAt(e.RowIndex));
                        popup.Show();
                    }
                    catch (NullReferenceException)
                    {

                        MessageBox.Show("해당제품의 혜택이 없습니다.", "취소", MessageBoxButtons.YesNo);

                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = 1;
            int tmpcount = 0;
            dataGridView1.Rows.Clear();
            url.Clear();
            profit.Clear();
            dataGridView1.Columns[0].HeaderText = "순위";
            foreach (var VARIABLE in (List<string[]>)PRODUCT[4])
            {
                /*
                 *   datas[0] = realurl.Trim();
                     datas[1] = name.Trim();
                     datas[2] = itemNum.Trim();
                     datas[3] = price.Trim();
                     datas[4] = profit.Trim();
                     datas[5] = delivery.Trim();
                 * 
                 */
                 url.Add(VARIABLE[0]);
                 profit.Add(VARIABLE[4]);
                string[] data = new string[10];
                data[0] = count + "";
                data[1] = VARIABLE[1];
                data[2] = VARIABLE[2];
                data[3] = ((string[])PRODUCT[15]).ElementAt(2); ;
                data[4] = VARIABLE[3];
                data[5] = cal(VARIABLE[3]) - ((int)PRODUCT[6]) + "";
                data[6] = ((int)PRODUCT[6]) - cal(VARIABLE[3]) + "";
                // data[5] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                // data[6] = ((List<int>)PRODUCT[14]).ElementAt(tmpcount) + "원";
                data[7] = VARIABLE[5];
                // data[8] = ((List<int>) PRODUCT[13]).ElementAt(tmpcount) + "원";
                // data[9] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                dataGridView1.Rows.Add(data);
                count++;
                tmpcount++;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Console.WriteLine("줄맞추기");
                dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = 1;
            int tmpcount = ((List<string[]>)PRODUCT[4]).Count;
            dataGridView1.Rows.Clear();
            url.Clear();
            profit.Clear();
            dataGridView1.Columns[0].HeaderText = "순위";
            foreach (var VARIABLE in (List<string[]>)PRODUCT[3])
            {
                /*
                 *   datas[0] = realurl.Trim();
                     datas[1] = name.Trim();
                     datas[2] = itemNum.Trim();
                     datas[3] = price.Trim();
                     datas[4] = profit.Trim();
                     datas[5] = delivery.Trim();
                 * 
                 */
                 url.Add(VARIABLE[0]);
                profit.Add(VARIABLE[4]);
                string[] data = new string[10];
                data[0] = count + "";
                data[1] = VARIABLE[1];
                data[2] = VARIABLE[2];
                data[3] = ((string[])PRODUCT[15]).ElementAt(2); ;
                data[4] = VARIABLE[3];
                data[5] = cal(VARIABLE[3]) - ((int)PRODUCT[6]) + "";
                data[6] = ((int)PRODUCT[6]) - cal(VARIABLE[3]) + "";
                // data[5] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                // data[6] = ((List<int>)PRODUCT[14]).ElementAt(tmpcount) + "원";
                data[7] = VARIABLE[5];
                // data[8] = ((List<int>) PRODUCT[13]).ElementAt(tmpcount) + "원";
                // data[9] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                dataGridView1.Rows.Add(data);
                count++;
                tmpcount++;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Console.WriteLine("줄맞추기");
                dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    textBox13.Text = ((string[])PRODUCT[15]).ElementAt(8);
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    textBox13.Text = ((string[])PRODUCT[15]).ElementAt(9);
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    textBox13.Text = ((string[])PRODUCT[15]).ElementAt(10);
                }
            }
            catch (NullReferenceException)
            {
                
                
            }
         
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        public int cal(string stringprice)
        {
            int price = 0;
            string tmp = "";
            foreach (var variable in stringprice.Split(','))
            {
                tmp += variable;
            }
            try
            {
                price = int.Parse(tmp);
            }
            catch (FormatException)
            {
                
              
            }
          return price;
        }



        /// <summary>
        /// //////네이버리스트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            int count = 1;
            int tmpcount = ((List<string[]>)PRODUCT[4]).Count;
            dataGridView1.Rows.Clear();
            url.Clear();
            profit.Clear();
            dataGridView1.Columns[0].HeaderText = "번호";
            foreach (var VARIABLE in (List<string[]>)PRODUCT[0])
            {
                /*
                 *   datas[0] = realurl.Trim();
                     datas[1] = name.Trim();
                     datas[2] = itemNum.Trim();
                     datas[3] = price.Trim();
                     datas[4] = profit.Trim();
                     datas[5] = delivery.Trim();
                 * 
                 */
                url.Add(VARIABLE[0]);
                profit.Add(VARIABLE[4]);
                string[] data = new string[10];
                data[0] = count + "";
                data[1] = VARIABLE[1];
                data[2] = VARIABLE[2];
                data[3] = ((string[])PRODUCT[15]).ElementAt(2); ;
                data[4] = VARIABLE[3];
                data[5] = cal(VARIABLE[3]) - ((int)PRODUCT[6]) + "";
                data[6] = ((int)PRODUCT[6]) - cal(VARIABLE[3]) + "";
                // data[5] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                // data[6] = ((List<int>)PRODUCT[14]).ElementAt(tmpcount) + "원";
                data[7] = VARIABLE[5];
                // data[8] = ((List<int>) PRODUCT[13]).ElementAt(tmpcount) + "원";
                // data[9] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                dataGridView1.Rows.Add(data);
                count++;
                tmpcount++;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Console.WriteLine("줄맞추기");
                dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }


        /// <summary>
        /// ///////다나와리스트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            int count = 1;
            int tmpcount = ((List<string[]>)PRODUCT[4]).Count;
            dataGridView1.Rows.Clear();
            url.Clear();
            profit.Clear();
            dataGridView1.Columns[0].HeaderText = "번호";
            foreach (var VARIABLE in (List<string[]>)PRODUCT[1])
            {
                /*
                 *   datas[0] = realurl.Trim();
                     datas[1] = name.Trim();
                     datas[2] = itemNum.Trim();
                     datas[3] = price.Trim();
                     datas[4] = profit.Trim();
                     datas[5] = delivery.Trim();
                 * 
                 */
                url.Add(VARIABLE[0]);
                profit.Add(VARIABLE[4]);
                string[] data = new string[10];
                data[0] = count + "";
                data[1] = VARIABLE[1];
                data[2] = VARIABLE[2];
                data[3] = ((string[])PRODUCT[15]).ElementAt(2); ;
                data[4] = VARIABLE[3];
                data[5] = cal(VARIABLE[3]) - ((int)PRODUCT[6]) + "";
                data[6] = ((int)PRODUCT[6]) - cal(VARIABLE[3]) + "";
                // data[5] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                // data[6] = ((List<int>)PRODUCT[14]).ElementAt(tmpcount) + "원";
                data[7] = VARIABLE[5];
                // data[8] = ((List<int>) PRODUCT[13]).ElementAt(tmpcount) + "원";
                // data[9] = ((List<int>)PRODUCT[13]).ElementAt(tmpcount) + "원";
                dataGridView1.Rows.Add(data);
                count++;
                tmpcount++;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Console.WriteLine("줄맞추기");
                dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            popup2 popup2=new popup2(textBox13.Text);
            popup2.Show();
        }
    }
}
