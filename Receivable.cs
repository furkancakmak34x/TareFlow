using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TareFlow.Processes;

namespace TareFlow
{
    public partial class Receivable : Form
    {
        public Receivable()
        {
            InitializeComponent();
            EnableDoubleBuffering(this);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seçili kaydı silmek istediğinizden emin misiniz?", "Kayıt Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                string plate = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string date = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                DeleteReceivable(plate, date);
                Receivable_Load(this, EventArgs.Empty);

                Connect();
                SQLiteCommand cmd = new SQLiteCommand("SELECT EXISTS (SELECT 1 FROM Receivable)", _conn);
                bool check = Convert.ToInt32(cmd.ExecuteScalar()) == 1;

                if (check)
                {
                    Disconnect();
                    return;
                }

                else
                {
                    Disconnect();
                    MessageBox.Show("Tahsilat defteri boş. Anasayfaya yönlendiriliyorsunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }

            else
            {
                return;
            }
        }

        private void Receivable_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;

            Connect();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Receivable ORDER BY Date DESC", _conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            List<ReceivableDB> list = new List<ReceivableDB>();

            while (reader.Read())
            {
                ReceivableDB db = new ReceivableDB()
                {
                    Plate = reader["Plate"].ToString(),
                    Date = reader["Date"].ToString(),
                    Fee = reader["Fee"].ToString()
                };

                list.Add(db);
            }

            dataGridView1.DataSource = list;

            Disconnect();
        }
    }
}
