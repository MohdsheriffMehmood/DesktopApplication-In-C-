

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.VisualBasic;
using System.Drawing.Imaging;
using System.IO;


namespace StudentInfoWindowsForm
{
    public partial class Form1 : Form
    {
        SqlConnection Con;
        SqlCommand Cmd;
        string Query;

        SqlDataAdapter adapter;
      
        DataSet ds = new DataSet();
        SqlDataReader SetUpdate;
        OpenFileDialog open;
         int rno = 0;
        int n;
        MemoryStream ms;
        byte[] photo_aray;
        public Form1()
        {
            Thread t = new Thread(new ThreadStart(StartForm));
            t.Start();
            Thread.Sleep(10000);
             InitializeComponent();
            t.Abort();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                Con = new SqlConnection("Server = BLT200\\SQLEXPRESS; Database = Practice1; Integrated Security = true");
                Con.Open();
                MessageBox.Show("connected");

               /* adapter = new SqlDataAdapter("Select * From Student", Con);
                DS = new DataSet();

                adapter.Fill(DS, "Student");*/
                comboBox1.Items.Add("ALL");
                comboBox1.Items.Add("Courses");
                comboBox1.Items.Add("Date");
               loaddata();
                showdata();



                
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    MessageBox.Show("Not Connected");

            }
        }
        public void StartForm()
        {
           
            Application.Run(new Form2() );

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Insert();
        }
        public void Insert()
        {

           
               // DateTime d = Convert.ToDateTime(dateTimePicker1.Value);
                Query = "Insert into Student values('" + textBox1.Text +"', '"+textBox2.Text + "','" + textBox3.Text + "','" + dateTimePicker1.Value + "',@photo)";
            Cmd = new SqlCommand(Query, Con);
                conv_photo();
         
          int i=   Cmd.ExecuteNonQuery();
            n = 1;
            Con.Close();
              
                if (n > 0)
                {
                    MessageBox.Show("record inserted");
                    loaddata();
                }
                else
                    MessageBox.Show("insertion failed");
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Delete();
        }
        public void Delete()
        {
            try
            {

                Query = "Delete from Student where ID='" + textBox1.Text + "'";
                Cmd = new SqlCommand(Query, Con);
                /* 
                 textBox1.Text = " ";
                 textBox2.Text = " ";
                 textBox3.Text = " ";
                 textBox4.Text = " ";*/
                Cmd.ExecuteNonQuery();
                MessageBox.Show("Deleted");
                loaddata();
                Con.Close();
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                { MessageBox.Show("Not Connected"); }
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {

            UpdateData();
        }
        public void UpdateData()
        {
            int newsno = Convert.ToInt32(textBox1.Text);
            String newEmp = textBox2.Text;
            DateTime d = Convert.ToDateTime(dateTimePicker1.Value);

            if (newEmp.Length > 0)
            {

                Query = "Update Student Set Name = '" + textBox2.Text + "' ";
                Cmd = new SqlCommand(Query, Con);
                SetUpdate = Cmd.ExecuteReader();

                SetUpdate.Close();

            }


           else if (textBox3.Text.Length > 0)
            {

                Query = "Update Student Set Course = '" + textBox3.Text + "' ";
                Cmd = new SqlCommand(Query, Con);
                SetUpdate = Cmd.ExecuteReader();
                SetUpdate.Close();


            }
            else if(pictureBox1.Image!=null)
            {
                Query = "Update Student Set Pic =@photo";
                conv_photo();
                Con.Open();
                int n = Cmd.ExecuteNonQuery();
                Con.Close();
                if (n > 0)
                {
                    MessageBox.Show("Record Updated");
                    loaddata();
                }
                else
                    MessageBox.Show("Updation Failed");

            }


            /*if (dateTimePicker1.Value)
            { }*/

            /* Query = "Update Student Set Name = '" + textBox2.Text + "' Where Employee_ID = '" + TxtEmpID.Text + "'";
             Cmd = new SqlCommand(Query, Con);
             SqlDataReader SetUpdate = Cmd.ExecuteReader();*/
            /* try
             {


                 int newsno = Convert.ToInt32(textBox1.Text);
                 String newEmp = textBox3.Text;

                 if (newEmp.Length > 0)
                 {

                     MessageBox.Show(newEmp);
                     Query = "Update NewEmployee set Name='" + newEmp + "' where EmpID='" + newsno + "'";
                     Cmd = new SqlCommand(Query, Con);

                     cmd.ExecuteNonQuery();
                     MessageBox.Show("Update");
                     Con.Close();
                 }
                 if (textBox4.Text.Length > 0)
                 {


                     MessageBox.Show(textBox4.Text);
                     int Sal = Convert.ToInt32(textBox4.Text);
                     query = "Update NewEmployee set Salary='" + Sal + "' where EmpID='" + newsno + "'";
                     cmd = new SqlCommand(query, con);

                     cmd.ExecuteNonQuery();
                     MessageBox.Show("Update");
                     con.Close();

                 }



             }
             catch (Exception e)
             {
                 if (e is SqlException)
                     MessageBox.Show("Not update");
             }*/

        }

        private void button4_Click(object sender, EventArgs e)
        {
            PictureBox();
        }
        public void PictureBox()
        {
            open = new OpenFileDialog();
          open.Filter = "jpeg|*.jpg|bmp|*.bmp|all files|*.*";
            DialogResult res = open.ShowDialog();
            if (res == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(open.FileName);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem.ToString();

            int selectedIndex = comboBox1.SelectedIndex;
         
            switch (selectedIndex)
            {
                case 0:
                    adapter = new SqlDataAdapter("Select * from Student",Con);


                   
                   adapter.Fill(ds, "Student");
                    dataGridView1.DataSource = ds.Tables["Student"].DefaultView;
                    break;
                case 1:
                    MessageBox.Show("Courses");

                    textBox4.Visible = true;
                    String str = textBox4.Text;
                    

                    break;
                         case 2:
                    MessageBox.Show("Date");
                    DateTime d = Convert.ToDateTime(dateTimePicker1.Value);
                 adapter = new SqlDataAdapter("Select * from Student where DateOfJoin ='"+d+"'", Con);


                 
                    adapter.Fill(ds, "Student");
                    dataGridView1.DataSource = ds.Tables["Student"].DefaultView;
                    break;


                default:
                    break;

            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            String str = textBox4.Text;
            if (textBox4.Text.Length > 0)
            {
               adapter= new SqlDataAdapter("Select * from Student where Course = '" + str + "'", Con);



              adapter.Fill(ds, "Student");
                dataGridView1.DataSource = ds.Tables["Student"].DefaultView;


            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            rno = 0; showdata();
            MessageBox.Show("First record");
        }
        void loaddata()
        {
            adapter = new SqlDataAdapter("select ID,Name,Course,DateOfJoin,Pic from Student", Con);
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            ds = new DataSet(); adapter.Fill(ds, "student");
        }
        void showdata()
        {
            if (ds.Tables["Student"].Rows.Count >= 0)
            {
                textBox1.Text = ds.Tables["Student"].Rows[rno][0].ToString();
                textBox2.Text = ds.Tables["Student"].Rows[rno][1].ToString();
                textBox3.Text = ds.Tables["Student"].Rows[rno][2].ToString();
                textBox4.Text = ds.Tables["Student"].Rows[rno][3].ToString();
                pictureBox1.Image = null;
                if (ds.Tables[0].Rows[rno][4] != System.DBNull.Value)
                {
                    photo_aray = (byte[])ds.Tables[0].Rows[rno][4];
                    MemoryStream ms = new MemoryStream(photo_aray);
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
                MessageBox.Show("No Records");
        }
        void conv_photo()
        {
            //converting photo to binary data  
            if (pictureBox1.Image != null)
            {
                //using FileStream:(will not work while updating, if image is not changed)  
                //FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);  
                //byte[] photo_aray = new byte[fs.Length];  
                //fs.Read(photo_aray, 0, photo_aray.Length);    

                //using MemoryStream:  
                ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                byte[] photo_aray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo_aray, 0, photo_aray.Length);
                Cmd.Parameters.AddWithValue("@photo", photo_aray);
            }
        }

        private void SearchData_Click(object sender, EventArgs e)
        {
            try
            {
                int n = Convert.ToInt32(Interaction.InputBox("Enter sno:", "Search", "20", 100, 100));
                DataRow drow;
                drow = ds.Tables[0].Rows.Find(n);
                if (drow != null)
                {
                    rno = ds.Tables[0].Rows.IndexOf(drow);
                    textBox1.Text = drow[0].ToString();
                    textBox2.Text = drow[1].ToString();
                    textBox3.Text = drow[2].ToString();
                    textBox4.Text = drow[3].ToString();
                    pictureBox1.Image = null;
                    if (drow[4] != System.DBNull.Value)
                    {
                        photo_aray = (byte[])drow[4];
                        MemoryStream ms = new MemoryStream(photo_aray);
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                else
                    MessageBox.Show("Record Not Found");
            }
            catch
            {
                MessageBox.Show("Invalid Input");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (rno < ds.Tables[0].Rows.Count - 1)
            {
                rno++; showdata();
            }
            else
                MessageBox.Show("Last record");
        }

        private void button8_Click(object sender, EventArgs e)
        {

            rno = ds.Tables[0].Rows.Count - 1;
            showdata(); MessageBox.Show("Last record");
        }

        private void PreBtn_Click(object sender, EventArgs e)
        {
            if (rno > 0)
            {
                rno--; showdata();
            }
            else
                MessageBox.Show("First record");
        }
    }


    }



