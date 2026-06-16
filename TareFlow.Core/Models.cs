namespace TareFlow.Core;

/// <summary>Kantar indikatöründen okunan anlık tartım değeri.</summary>
public sealed class ScaleReading
{
    public int Weight { get; set; }
    public bool Stable { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}

/// <summary>1. tartım bekleyen kayıt (Weight tablosu).</summary>
public sealed class WeightRecord
{
    public long Id { get; set; }
    public string Plate { get; set; } = "";
    public string Date { get; set; } = "";
    public int Weight { get; set; }
    public string? Customer { get; set; }
    public string? Vendor { get; set; }
    public string? Product { get; set; }
}

/// <summary>Tamamlanmış tartım kaydı (SecWeight tablosu).</summary>
public sealed class SecWeightRecord
{
    public long Id { get; set; }
    public string Plate { get; set; } = "";
    public string Date { get; set; } = "";
    public string SecDate { get; set; } = "";
    public int Weight { get; set; }
    public int SecWeight { get; set; }
    public int Total { get; set; }
    public string? Customer { get; set; }
    public string? Vendor { get; set; }
    public string? Product { get; set; }
}

/// <summary>Tahsilat kaydı (Receivable tablosu). Borç müşteri/cari adına tutulur.</summary>
public sealed class ReceivableRecord
{
    public long Id { get; set; }
    public string Customer { get; set; } = "";
    public string Plate { get; set; } = "";
    public string Date { get; set; } = "";
    public int Fee { get; set; }
}

/// <summary>Bir müşterinin (cari) toplam bekleyen borcu — tahsilat ekranında gruplama için.</summary>
public sealed class CustomerDebt
{
    public string Customer { get; set; } = "";
    public int Count { get; set; }
    public int Total { get; set; }
}

/// <summary>Plaka bazlı sabit dara (boş ağırlık) kaydı (Tare tablosu).</summary>
public sealed class TareRecord
{
    public string Plate { get; set; } = "";
    public int Weight { get; set; }
    public string UpdatedDate { get; set; } = "";
}

/// <summary>Araç tipi (kantar ücreti için).</summary>
public enum VehicleType
{
    Truck,
    Van
}
