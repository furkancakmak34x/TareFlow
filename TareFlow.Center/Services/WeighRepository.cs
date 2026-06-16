using System.IO;
using Microsoft.Data.Sqlite;
using TareFlow.Core;

namespace TareFlow.Center.Services;

/// <summary>
/// SQLite veri erişimi. Şema eski WinForms uygulamasıyla birebir aynı:
/// Weight, SecWeight, Receivable, Fee tabloları.
/// </summary>
public sealed class WeighRepository
{
    private readonly string _connectionString;

    public WeighRepository()
    {
        Directory.CreateDirectory(CenterSettings.AppDataDir);
        string dbPath = Path.Combine(CenterSettings.AppDataDir, "Database.db");

        // İlk çalıştırmada uygulamayla gelen tohum veritabanını kopyala.
        if (!File.Exists(dbPath))
        {
            string seed = Path.Combine(AppContext.BaseDirectory, "Database.db");
            if (File.Exists(seed))
                File.Copy(seed, dbPath);
        }

        _connectionString = new SqliteConnectionStringBuilder { DataSource = dbPath }.ToString();
        EnsureSchema();
    }

    private SqliteConnection Open()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        return conn;
    }

    private void EnsureSchema()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Weight (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                Plate TEXT NOT NULL, Date TEXT NOT NULL, Weight INTEGER NOT NULL,
                Customer TEXT, Vendor TEXT, Product TEXT);
            CREATE TABLE IF NOT EXISTS SecWeight (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                Plate TEXT NOT NULL, Date TEXT NOT NULL, SecDate TEXT NOT NULL,
                Weight INTEGER NOT NULL, SecWeight INTEGER NOT NULL, Total INTEGER NOT NULL,
                Customer TEXT, Vendor TEXT, Product TEXT);
            CREATE TABLE IF NOT EXISTS Receivable (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                Plate TEXT NOT NULL, Date TEXT NOT NULL, Fee INTEGER NOT NULL);
            CREATE TABLE IF NOT EXISTS Fee (
                Type TEXT NOT NULL, WeightFee INTEGER NOT NULL);
            CREATE TABLE IF NOT EXISTS Tare (
                Plate TEXT NOT NULL PRIMARY KEY,
                Weight INTEGER NOT NULL, UpdatedDate TEXT NOT NULL);
            """;
        cmd.ExecuteNonQuery();

        // Migrasyon: Receivable tablosuna Customer kolonu yoksa ekle.
        if (!ColumnExists(conn, "Receivable", "Customer"))
        {
            using var alter = conn.CreateCommand();
            alter.CommandText = "ALTER TABLE Receivable ADD COLUMN Customer TEXT";
            alter.ExecuteNonQuery();

            // Eski kayıtlarda müşteri yoktu; cari olarak plakayı kullan.
            using var fill = conn.CreateCommand();
            fill.CommandText = "UPDATE Receivable SET Customer = Plate WHERE Customer IS NULL OR Customer = ''";
            fill.ExecuteNonQuery();
        }
    }

    private static bool ColumnExists(SqliteConnection conn, string table, string column)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"PRAGMA table_info({table})";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            if (string.Equals(r.GetString(1), column, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    // --- Weight (1. tartım) ---

    public List<WeightRecord> ListWeight()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Weight ORDER BY Date DESC";
        using var r = cmd.ExecuteReader();
        var list = new List<WeightRecord>();
        while (r.Read())
        {
            list.Add(new WeightRecord
            {
                Id = r.GetInt64(r.GetOrdinal("Id")),
                Plate = Str(r, "Plate"),
                Date = Str(r, "Date"),
                Weight = r.GetInt32(r.GetOrdinal("Weight")),
                Customer = Str(r, "Customer"),
                Vendor = Str(r, "Vendor"),
                Product = Str(r, "Product")
            });
        }
        return list;
    }

    public void AddWeight(WeightRecord w)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO Weight (Plate, Date, Weight, Customer, Vendor, Product)
            VALUES ($plate, $date, $weight, $customer, $vendor, $product)
            """;
        cmd.Parameters.AddWithValue("$plate", w.Plate);
        cmd.Parameters.AddWithValue("$date", w.Date);
        cmd.Parameters.AddWithValue("$weight", w.Weight);
        cmd.Parameters.AddWithValue("$customer", (object?)w.Customer ?? "");
        cmd.Parameters.AddWithValue("$vendor", (object?)w.Vendor ?? "");
        cmd.Parameters.AddWithValue("$product", (object?)w.Product ?? "");
        cmd.ExecuteNonQuery();
    }

    public void DeleteWeight(string plate, string date)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Weight WHERE Plate = $plate AND Date = $date";
        cmd.Parameters.AddWithValue("$plate", plate);
        cmd.Parameters.AddWithValue("$date", date);
        cmd.ExecuteNonQuery();
    }

    // --- SecWeight (tamamlanmış tartım) ---

    public List<SecWeightRecord> ListSecWeight()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM SecWeight ORDER BY Date DESC";
        using var r = cmd.ExecuteReader();
        var list = new List<SecWeightRecord>();
        while (r.Read())
        {
            list.Add(new SecWeightRecord
            {
                Id = r.GetInt64(r.GetOrdinal("Id")),
                Plate = Str(r, "Plate"),
                Date = Str(r, "Date"),
                SecDate = Str(r, "SecDate"),
                Weight = r.GetInt32(r.GetOrdinal("Weight")),
                SecWeight = r.GetInt32(r.GetOrdinal("SecWeight")),
                Total = r.GetInt32(r.GetOrdinal("Total")),
                Customer = Str(r, "Customer"),
                Vendor = Str(r, "Vendor"),
                Product = Str(r, "Product")
            });
        }
        return list;
    }

    public void AddSecWeight(SecWeightRecord s)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO SecWeight (Plate, Date, SecDate, Customer, Vendor, Product, Weight, SecWeight, Total)
            VALUES ($plate, $date, $secdate, $customer, $vendor, $product, $weight, $secweight, $total)
            """;
        cmd.Parameters.AddWithValue("$plate", s.Plate);
        cmd.Parameters.AddWithValue("$date", s.Date);
        cmd.Parameters.AddWithValue("$secdate", s.SecDate);
        cmd.Parameters.AddWithValue("$customer", (object?)s.Customer ?? "");
        cmd.Parameters.AddWithValue("$vendor", (object?)s.Vendor ?? "");
        cmd.Parameters.AddWithValue("$product", (object?)s.Product ?? "");
        cmd.Parameters.AddWithValue("$weight", s.Weight);
        cmd.Parameters.AddWithValue("$secweight", s.SecWeight);
        cmd.Parameters.AddWithValue("$total", s.Total);
        cmd.ExecuteNonQuery();
    }

    public void DeleteSecWeight(long id)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM SecWeight WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // --- Receivable (tahsilat) ---

    public List<ReceivableRecord> ListReceivable()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Receivable ORDER BY Date DESC";
        using var r = cmd.ExecuteReader();
        var list = new List<ReceivableRecord>();
        while (r.Read())
        {
            list.Add(new ReceivableRecord
            {
                Id = r.GetInt64(r.GetOrdinal("Id")),
                Customer = Str(r, "Customer"),
                Plate = Str(r, "Plate"),
                Date = Str(r, "Date"),
                Fee = r.GetInt32(r.GetOrdinal("Fee"))
            });
        }
        return list;
    }

    /// <summary>Müşteri (cari) bazında toplam bekleyen borçları döner.</summary>
    public List<CustomerDebt> ListCustomerDebts()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            SELECT COALESCE(NULLIF(Customer, ''), Plate) AS Acc,
                   COUNT(*) AS Cnt, SUM(Fee) AS Tot
            FROM Receivable
            GROUP BY Acc
            ORDER BY Tot DESC
            """;
        using var r = cmd.ExecuteReader();
        var list = new List<CustomerDebt>();
        while (r.Read())
        {
            list.Add(new CustomerDebt
            {
                Customer = r.IsDBNull(0) ? "" : r.GetString(0),
                Count = r.GetInt32(1),
                Total = r.IsDBNull(2) ? 0 : r.GetInt32(2)
            });
        }
        return list;
    }

    /// <summary>Belirli bir müşterinin (cari) bekleyen borç kalemlerini döner.</summary>
    public List<ReceivableRecord> ListReceivableByCustomer(string customer)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            SELECT * FROM Receivable
            WHERE COALESCE(NULLIF(Customer, ''), Plate) = $cust
            ORDER BY Date DESC
            """;
        cmd.Parameters.AddWithValue("$cust", customer);
        using var r = cmd.ExecuteReader();
        var list = new List<ReceivableRecord>();
        while (r.Read())
        {
            list.Add(new ReceivableRecord
            {
                Id = r.GetInt64(r.GetOrdinal("Id")),
                Customer = Str(r, "Customer"),
                Plate = Str(r, "Plate"),
                Date = Str(r, "Date"),
                Fee = r.GetInt32(r.GetOrdinal("Fee"))
            });
        }
        return list;
    }

    public void AddReceivable(string customer, string plate, string date, int fee)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Receivable (Customer, Plate, Date, Fee) VALUES ($customer, $plate, $date, $fee)";
        cmd.Parameters.AddWithValue("$customer", string.IsNullOrWhiteSpace(customer) ? plate : customer);
        cmd.Parameters.AddWithValue("$plate", plate);
        cmd.Parameters.AddWithValue("$date", date);
        cmd.Parameters.AddWithValue("$fee", fee);
        cmd.ExecuteNonQuery();
    }

    public void DeleteReceivable(long id)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Receivable WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Bir müşterinin (cari) tüm bekleyen borçlarını tahsil edildi olarak siler.</summary>
    public void DeleteReceivablesByCustomer(string customer)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Receivable WHERE COALESCE(NULLIF(Customer, ''), Plate) = $cust";
        cmd.Parameters.AddWithValue("$cust", customer);
        cmd.ExecuteNonQuery();
    }

    // --- Tare (sabit dara / plaka bazlı boş ağırlık) ---

    public List<TareRecord> ListTare()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Tare ORDER BY Plate";
        using var r = cmd.ExecuteReader();
        var list = new List<TareRecord>();
        while (r.Read())
        {
            list.Add(new TareRecord
            {
                Plate = Str(r, "Plate"),
                Weight = r.GetInt32(r.GetOrdinal("Weight")),
                UpdatedDate = Str(r, "UpdatedDate")
            });
        }
        return list;
    }

    /// <summary>Plakanın kayıtlı darasını döner; yoksa null.</summary>
    public TareRecord? GetTare(string plate)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Tare WHERE Plate = $plate LIMIT 1";
        cmd.Parameters.AddWithValue("$plate", plate);
        using var r = cmd.ExecuteReader();
        if (!r.Read())
            return null;
        return new TareRecord
        {
            Plate = Str(r, "Plate"),
            Weight = r.GetInt32(r.GetOrdinal("Weight")),
            UpdatedDate = Str(r, "UpdatedDate")
        };
    }

    /// <summary>Plakanın darasını ekler/günceller (upsert).</summary>
    public void UpsertTare(string plate, int weight, string updatedDate)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO Tare (Plate, Weight, UpdatedDate) VALUES ($plate, $weight, $date)
            ON CONFLICT(Plate) DO UPDATE SET Weight = $weight, UpdatedDate = $date
            """;
        cmd.Parameters.AddWithValue("$plate", plate);
        cmd.Parameters.AddWithValue("$weight", weight);
        cmd.Parameters.AddWithValue("$date", updatedDate);
        cmd.ExecuteNonQuery();
    }

    public void DeleteTare(string plate)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Tare WHERE Plate = $plate";
        cmd.Parameters.AddWithValue("$plate", plate);
        cmd.ExecuteNonQuery();
    }

    // --- Fee (araç tipi ücreti) ---

    public int GetFee(VehicleType type)
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT WeightFee FROM Fee WHERE Type = $type LIMIT 1";
        cmd.Parameters.AddWithValue("$type", type == VehicleType.Van ? "Van" : "Truck");
        var result = cmd.ExecuteScalar();
        return result is null || result is DBNull ? 0 : Convert.ToInt32(result);
    }

    private static string Str(SqliteDataReader r, string column)
    {
        int i = r.GetOrdinal(column);
        return r.IsDBNull(i) ? "" : r.GetString(i);
    }
}
