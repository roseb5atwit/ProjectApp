using CodeCampApp.Ui.ChildComponents;
using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CodeCampApp
{
    public partial class Session : Form
    {
        private DataTable _dt;
        string connectionString = @"Server=mydb.c6botwup9amq.us-east-2.rds.amazonaws.com;Database=projectconference;Uid=root;Pwd=password123;convert zero datetime=True";
        public Session()
        {
            InitializeComponent();

            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();

                var stm = "SELECT sessions.session_id, sessions.title, time_slots.start_time, time_slots.end_time, speaker.name, rooms.room_name" +
                    "                            FROM sessions" +
                    "	                            INNER JOIN time_slots" +
                    "		                            ON sessions.time_slots_id = time_slots.time_slots_id" +
                    "	                            INNER JOIN speaker" +
                    "		                            ON sessions.speaker_id = speaker.speaker_id" +
                    "	                            INNER JOIN rooms" +
                    "		                            ON sessions.room_id = rooms.room_id";

                var cmd = new MySqlCommand(stm, mysqlCon);

                MySqlDataReader rdr = cmd.ExecuteReader();

                DataTable dt = new DataTable();

                _dt = dt;

                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("title", typeof(string));
                dt.Columns.Add("start_time", typeof(string));
                dt.Columns.Add("end_time", typeof(string));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("room_name", typeof(string));
                dt.Columns.Add("edit", typeof(string));
                dt.Columns.Add("delete", typeof(string));
                dt.Columns.Add("save", typeof(string));
                dt.Columns.Add("cancel", typeof(string));

                while (rdr.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = rdr.GetInt32(0);
                    dr["title"] = rdr.GetString(1);
                    dr["start_time"] = rdr.GetString(2);
                    dr["end_time"] = rdr.GetString(3);
                    dr["name"] = rdr.GetString(4);
                    dr["room_name"] = rdr.GetString(5);
                    dr["edit"] = "1";
                    dr["delete"] = "1";
                    dr["save"] = "1";
                    dr["cancel"] = "1";
                    dt.Rows.Add(dr);
                }

                dataGridView1.Columns.Add("id", "ID");
                dataGridView1.Columns.Add("title", "Title");
                dataGridView1.Columns.Add("time_slots", "Time Slot");
                dataGridView1.Columns.Add("room_name", "Room");
                dataGridView1.Columns.Add("name", "Speaker");
                dataGridView1.Columns.Add("edit", "Edit");
                dataGridView1.Columns.Add("delete", "Delete");
                dataGridView1.Columns.Add("save", "Save");
                dataGridView1.Columns.Add("cancel", "Cancel");
                dataGridView1.Columns["time_slots"].ReadOnly = true;
                dataGridView1.Columns["title"].ReadOnly = true;
                dataGridView1.Columns["room_name"].ReadOnly = true;
                dataGridView1.Columns["name"].ReadOnly = true;
                dataGridView1.Columns["id"].Visible = false;
                dataGridView1.Columns["save"].Visible = false;
                dataGridView1.Columns["cancel"].Visible = false;
                dataGridView1.AllowUserToAddRows = false;

                string[] lst_dados = new string[5];

                foreach (DataRow dr1 in dt.Rows)
                {
                    lst_dados[0] = dr1["id"].ToString();
                    lst_dados[1] = dr1["title"].ToString();
                    var start_time = DateTime.Parse((dr1["start_time"].ToString()));
                    var end_time = DateTime.Parse((dr1["end_time"].ToString()));
                    lst_dados[2] = start_time.ToString("hh:mm tt") + (" - ") + end_time.ToString("hh:mm tt");
                    lst_dados[3] = dr1["name"].ToString();
                    lst_dados[4] = dr1["room_name"].ToString();


                    this.dataGridView1.Rows.Add(lst_dados);

                    if (dr1["edit"].ToString() == "1")
                    {
                        DataGridViewButtonCell btn = new DataGridViewButtonCell();
                        btn.Value = "Edit";
                        btn.UseColumnTextForButtonValue = true;
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells["edit"] = btn;
                    }
                    if (dr1["delete"].ToString() == "1")
                    {
                        DataGridViewButtonCell btn = new DataGridViewButtonCell();
                        btn.Value = "Delete";
                        btn.UseColumnTextForButtonValue = true;
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells["delete"] = btn;
                    }
                    if (dr1["save"].ToString() == "1")
                    {
                        DataGridViewButtonCell btn = new DataGridViewButtonCell();
                        btn.Value = "Save";
                        btn.UseColumnTextForButtonValue = true;
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells["save"] = btn;
                    }
                    if (dr1["cancel"].ToString() == "1")
                    {
                        DataGridViewButtonCell btn = new DataGridViewButtonCell();
                        btn.Value = "Cancel";
                        btn.UseColumnTextForButtonValue = true;
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells["cancel"] = btn;
                    }
                }
                mysqlCon.Close();
            }
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
        

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            handleButtons(sender, e, "edit");
            handleButtons(sender, e, "delete");
            handleButtons(sender, e, "save");
            handleButtons(sender, e, "cancel");

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void handleButtons(object sender, DataGridViewCellEventArgs e, string buttonName)
        {
            if (e.ColumnIndex == this.dataGridView1.Columns[buttonName].Index)
            {
                if (buttonName == "edit")
                {
                    dataGridView1.Columns[5].HeaderCell.Value = "Save";
                    dataGridView1.Columns[6].HeaderCell.Value = "Cancel";

                    dataGridView1.Columns["title"].ReadOnly = false;
                    dataGridView1.Columns["time_slots"].ReadOnly = false;
                    dataGridView1.Columns["room_name"].ReadOnly = false;
                    dataGridView1.Columns["name"].ReadOnly = false;
                    dataGridView1.Columns["edit"].Visible = false;
                    dataGridView1.Columns["delete"].Visible = false;
                    dataGridView1.Columns["save"].Visible = true;
                    dataGridView1.Columns["cancel"].Visible = true;
                }
                if (buttonName == "delete")
                {
                    DeleteModal popup = new DeleteModal(this.dataGridView1, e);
                    popup.ShowDialog(this);
                }
                if (buttonName == "cancel")
                {
                    dataGridView1.Columns[5].HeaderCell.Value = "Edit";
                    dataGridView1.Columns[6].HeaderCell.Value = "Delete";

                    dataGridView1.Columns["title"].ReadOnly = false;
                    dataGridView1.Columns["time_slots"].ReadOnly = false;
                    dataGridView1.Columns["room_name"].ReadOnly = false;
                    dataGridView1.Columns["name"].ReadOnly = false;
                    dataGridView1.Columns["edit"].Visible = true;
                    dataGridView1.Columns["delete"].Visible = true;
                    dataGridView1.Columns["save"].Visible = false;
                    dataGridView1.Columns["cancel"].Visible = false;
                }
                if (buttonName == "edit")
                {

                    DataGridViewComboBoxColumn speakerDropDown = new DataGridViewComboBoxColumn();
                    
                        DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[3];
                    cell.Value = speakerDropDown;


                }

            }
        }

       /* private void editDropDownFill(object sender, EventArgs e, string tableName)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(""))
            {
                mysqlCon.Open();

                var cmd = new MySqlCommand(mysqlCon);
                MySqlDataReader rdr = cmd.ExecuteReader();
            }
        }*/
        // ADDED BUTTON
        private void iconButton1_Click(object sender, EventArgs e)
        {
            // BOX NAME IS SEARCHTEXTBOX
            // string roomName = searchTextBox.Text;

            //  GridFind("find_room",roomName);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
