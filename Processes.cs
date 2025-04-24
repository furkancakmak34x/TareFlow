using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TareFlow
{
    public class Processes
    {
        public static SQLiteConnection _conn = new SQLiteConnection(@"Data Source=|DataDirectory|\Database.db;");
        public class Database
        {
            public string Plate { get; set; }
            public string Date { get; set; }
            public string SecDate { get; set; }
            public string Customer { get; set; }
            public string Vendor { get; set; }
            public string Product { get; set; }
            public string Weight { get; set; }
            public string SecWeight { get; set; }
            public string Total { get; set; }
        }

        public class ReceivableDB
        {
            public string Plate { get; set; }
            public string Date { get; set; }
            public string Fee { get; set; }
        }
        public static void Connect()
        {
            try
            {
                _conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public static void Disconnect()
        {
            try
            {
                _conn.Close();
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static string ConvertToASCII(string input)
        {
            string output = input
                            .Replace("Ç", "C")
                            .Replace("ç", "C")
                            .Replace("Ğ", "G")
                            .Replace("ğ", "G")
                            .Replace("İ", "I")
                            .Replace("ı", "I")
                            .Replace("i", "I")
                            .Replace("Ö", "O")
                            .Replace("ö", "O")
                            .Replace("Ş", "S")
                            .Replace("ş", "S")
                            .Replace("Ü", "U")
                            .Replace("ü", "U");
            return output.ToUpper();
        }

        public static List<Database> ListWeight()
        {
            try
            {
                Connect();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Weight ORDER BY Date DESC", _conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                List<Database> list = new List<Database>();


                while (reader.Read())
                {
                    Database db = new Database()
                    {
                        Plate = reader["Plate"].ToString(),
                        Date = reader["Date"].ToString(),
                        Weight = reader["Weight"].ToString(),
                        Customer = reader["Customer"].ToString(),
                        Vendor = reader["Vendor"].ToString(),
                        Product = reader["Product"].ToString()
                    };

                    list.Add(db);
                }

                return list;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            finally
            {
                Disconnect();
            }
        }
        public static List<Database> ListSecWeight()
        {
            try
            {
                Connect();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM SecWeight ORDER BY Date DESC", _conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                List<Database> list = new List<Database>();


                while (reader.Read())
                {
                    Database db = new Database()
                    {
                        Plate = reader["Plate"].ToString(),
                        Date = reader["Date"].ToString(),
                        SecDate = reader["SecDate"].ToString(),
                        Weight = reader["Weight"].ToString(),
                        SecWeight = reader["SecWeight"].ToString(),
                        Total = reader["Total"].ToString(),
                        Customer = reader["Customer"].ToString(),
                        Vendor = reader["Vendor"].ToString(),
                        Product = reader["Product"].ToString()
                    };

                    list.Add(db);
                }

                return list;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            finally
            {
                Disconnect();
            }
        }

        public static void Weight(string plate, string date, int weight, string customer, string vendor, string product)
        {
            try
            {
                Connect();
                string query = "INSERT INTO Weight (Plate, Date, Weight, Customer, Vendor, Product) VALUES (@plate, @date, @weight, @customer, @vendor, @product)";
                SQLiteCommand cmd = new SQLiteCommand(query, _conn);
                cmd.Parameters.AddWithValue("@plate", plate);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@weight", weight);
                cmd.Parameters.AddWithValue("@customer", customer);
                cmd.Parameters.AddWithValue("@vendor", vendor);
                cmd.Parameters.AddWithValue("@product", product);
                cmd.ExecuteNonQuery();
                MessageBox.Show("İlk tartım işlemi başarılı.", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                Disconnect();
            }
        }
        public static void SecWeight(string plate, string date, string secdate, string customer, string vendor, string product, int weight, int secweight, int total)
        {
            try
            {
                Connect();
                string query = "INSERT INTO SecWeight (Plate, Date, SecDate, Customer, Vendor, Product, Weight, SecWeight, Total) VALUES (@plate, @date, @secdate, @customer, @vendor, @product, @weight, @secweight, @total)";
                SQLiteCommand cmd = new SQLiteCommand(query, _conn);
                cmd.Parameters.AddWithValue("@plate", plate);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@secdate", secdate);
                cmd.Parameters.AddWithValue("@customer", customer);
                cmd.Parameters.AddWithValue("@vendor", vendor);
                cmd.Parameters.AddWithValue("@product", product);
                cmd.Parameters.AddWithValue("@weight", weight);
                cmd.Parameters.AddWithValue("@secweight", secweight);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.ExecuteNonQuery();
                MessageBox.Show("İkinci tartım işlemi başarılı.", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                Disconnect();
            }
        }
        public static void DeleteWeight(string plate, string date)
        {
            try
            {
                Connect();
                string query = "DELETE FROM Weight WHERE Plate = @plate AND Date = @date;";
                SQLiteCommand cmd = new SQLiteCommand(query, _conn);
                cmd.Parameters.AddWithValue("@plate", plate);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                Disconnect();
            }
        }
        public static void DeleteReceivable(string plate, string date)
        {
            try
            {
                Connect();
                string query = "DELETE FROM Receivable WHERE Plate = @plate AND Date = @date;";
                SQLiteCommand cmd = new SQLiteCommand(query, _conn);
                cmd.Parameters.AddWithValue("@plate", plate);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.ExecuteNonQuery();
                MessageBox.Show("İşlem başarılı.", "Kayıt Silme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                Disconnect();
            }
        }

        public static bool TableCheck(string type)
        {
            try
            {
                Connect();
                SQLiteCommand cmd = new SQLiteCommand($"SELECT EXISTS (SELECT 1 FROM {type})", _conn);
                bool check = Convert.ToInt32(cmd.ExecuteScalar()) == 1;

                if (check) { return true; }
                else { return false; }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            finally
            {
                Disconnect();
            }
        }

        public static void EnableDoubleBuffering(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null, control, new object[] { true });

                if (control.HasChildren)
                    EnableDoubleBuffering(control);
            }
        }

    }
}
