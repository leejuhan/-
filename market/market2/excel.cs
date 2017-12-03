using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
namespace market2
{
    public partial class excel : Form
    {
        private string filefullname="";
        private string[] data = { "GMARKET", "GSSHOP", "11MARKET", "CJMALL", "AUCTION" };
        public List<string[]> exceldata;
        public Screen screen;
        public excel(Screen screen)
        {
            InitializeComponent();
            ///////////////////////////combobox////////////////////////
         
            comboBox1.Items.AddRange(data);
            comboBox1.SelectedIndex = 0;
            /////////////////////////////////////////////////////
            exceldata=new List<string[]>();
            this.screen = screen;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(ShowFileOpenDialog());
            String filepath= ShowFileOpenDialog();
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;
            try
            {
                excelApp = new Excel.Application();

                // 엑셀 파일 열기
                wb = excelApp.Workbooks.Open(filepath);

                // 첫번째 Worksheet
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                // 현재 Worksheet에서 사용된 Range 전체를 선택
                Excel.Range rng = ws.UsedRange;

                // 현재 Worksheet에서 일부 범위만 선택
                // Excel.Range rng = ws.Range[ws.Cells[2, 1], ws.Cells[5, 3]];

                // Range 데이타를 배열 (One-based array)로
                object[,] data = rng.Value;

                for (int r = 1; r <= data.GetLength(0); r++)
                {

                    string[] tmp = new string[4];
                    int count = 1;
                    for (int c = 1; c <= data.GetLength(1); c++)
                    {
                        try
                        {
                            Debug.Write(data[r, c].ToString() + " ");
                            Console.WriteLine(data[r, c].ToString());
                            tmp[count] = data[r, c].ToString();
                            count++;
                        }
                       catch(NullReferenceException)
                        {
                            
                        }
                    
                    }

                    tmp[count] = DateTime.Now.ToShortDateString();
                    exceldata.Add(tmp);
                    dataGridView2.Rows.Add(tmp);

                    Console.WriteLine("////////////////////////");
                }

                wb.Close(true);
                excelApp.Quit();
            }catch (COMException)
            {
                
            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                Console.WriteLine("줄맞추기");
                dataGridView2.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                dataGridView2.Columns[i].HeaderCell.Style.Alignment= DataGridViewContentAlignment.MiddleCenter;
            }


        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            
        }




        /// <summary>
        /// ///////////////엑셀파일 업로드해주는 함수 
        /// </summary>
        /// <returns></returns>
        public string ShowFileOpenDialog()
        {
            //파일오픈창 생성 및 설정
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = false;
            ofd.Title = "파일 오픈 예제창";
            ofd.FileName = "test";
            ofd.Filter = "그림 파일 (*.jpg, *.gif, *.bmp) | *.jpg; *.gif; *.bmp; | 모든 파일 (*.*) | *.*";

            //파일 오픈창 로드
            DialogResult dr = ofd.ShowDialog();

            //OK버튼 클릭시
            if (dr == DialogResult.OK)
            {
                //File명과 확장자를 가지고 온다.
                string fileName = ofd.SafeFileName;
                //File경로와 File명을 모두 가지고 온다.
                string fileFullName = ofd.FileName;
                //File경로만 가지고 온다.
                string filePath = fileFullName.Replace(fileName, "");
                //출력 예제용 로직
                
                //File경로 + 파일명 리턴
                return fileFullName;
            }
            //취소버튼 클릭시 또는 ESC키로 파일창을 종료 했을경우
            else if (dr == DialogResult.Cancel)
            {
                return "";
            }

            return "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }


        private static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string[]> data =new List<string[]>();
            int count = 0;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {

                if (Convert.ToBoolean(dataGridView2.Rows[i].Cells[0].Value))

                {
                   data.Add(exceldata.ElementAt(i));
                    count = 1;
                }

            }
            if (count == 0)
            {
                screen.Marketengin.exceladd(exceldata);
            }
            else
            {
                screen.Marketengin.exceladd(data);
            }



            //screen.Marketengin.MainscreenList();
            screen.Marketengin.MainscreenListstartexcel();
            screen.screenshow();
            this.Close();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int chk = checkBox1.Checked ? 1 : 0;
            if (chk == 1)
            {
                for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                {


                    dataGridView2.Rows[i].Cells[0].Value = true;



                }

            }
            if (chk == 0)
            {
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {


                    dataGridView2.Rows[i].Cells[0].Value = false;



                }

            }
        }
    }
}
