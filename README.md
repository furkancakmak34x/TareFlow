# 🧾 TareFlow — Uzaktan Tır Kantarı Otomasyonu

**TareFlow**, tır/araç tartım işlemlerini **uzaktan** yapıp kaydeden bir kantar otomasyon sistemidir. Kantar tarafındaki COM portlu indikatör ile IP kamera, merkez tarafındaki operatöre ağ üzerinden taşınır; tüm tartım, kayıt ve fiş işlemleri merkezden yürütülür.

> Eski sürüm (.NET Framework 4.7.2 / WinForms, tek makine) `legacy/` klasöründe arşivlenmiştir.

## 🏗️ Mimari

İki fiziksel taraf, aynı ağa (PTP menzil genişletici) bağlıdır:

```
KANTAR TARAFI                                  MERKEZ TARAFI
┌───────────────────────────┐                 ┌──────────────────────────────┐
│ COM indikatör → Agent ─────┼── TCP 9100 ────▶│ TareFlow.Center (WPF)        │
│ IP kamera (192.168.1.7) ───┼── RTSP ────────▶│  • canlı ağırlık + kamera     │
└───────────────────────────┘                 │  • SQLite kayıt               │
                                               │  • 80mm USB termal fiş        │
                                               └──────────────────────────────┘
```

| Proje | Hedef | Açıklama |
|-------|-------|----------|
| `TareFlow.Core` | net10.0 | Paylaşılan modeller, seri ayrıştırma (`ScaleParser`), TCP protokolü (`ScaleProtocol`), metin yardımcıları |
| `TareFlow.Agent` | net10.0-windows (WPF) | **Kantar PC'sinde** çalışır: COM portunu okuyup TCP ile merkeze yayınlar |
| `TareFlow.Center` | net10.0-windows (WPF, MVVM) | **Merkez** ana uygulaması: tartım, kamera, kayıt, tahsilat, fiş |

## 🚀 Özellikler

- ✅ COM indikatör verisinin TCP ile **uzaktan** dinlenmesi (otomatik yeniden bağlanma)
- ✅ TTEC/RTSP **IP kamera** canlı görüntüsü (LibVLCSharp, düşük gecikme) + tartım anı snapshot
- ✅ İki aşamalı tartım (1. giriş / 2. çıkış), `Net = |1.tartım − 2.tartım|`
- ✅ SQLite kayıt, filtreleme, silme; araç tipine göre kantar ücreti ve tahsilat takibi
- ✅ **80mm USB termal fiş** (ESC/POS) — LPT desteği kaldırıldı
- ✅ CSV içe aktarma

## ⚙️ Gereksinimler

- Windows 10/11
- .NET 10 SDK
- Kantar tarafı: COM portlu indikatör + IP kamera, bir Windows PC
- Merkez tarafı: 80mm USB ESC/POS termal yazıcı (opsiyonel)

## ▶️ Çalıştırma

```powershell
# Kantar PC'sinde
dotnet run --project src/TareFlow.Agent

# Merkez PC'sinde
dotnet run --project src/TareFlow.Center
```

Merkezde **Ayarlar**'dan kantar IP'si, port, kamera RTSP adresi/şifresi ve termal yazıcı seçilir.

## 👨‍💻 Geliştirici

**furkan (@furkancakmak34x)** — [Mail](mailto:furkancakmak34x@gmail.com) · [GitHub](https://www.github.com/furkancakmak34x) · [Telegram](https://t.me/furkancakmak34x)
