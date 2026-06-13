# 🍦 ShopManager — EREN Dondurma & Waffle

> Manuel stok ve günlük ciro takip sistemi.  
> ASP.NET Core 8 MVC · PostgreSQL · Bootstrap 5 · SweetAlert2

---

## Özellikler

| Özellik | Açıklama |
|---|---|
| 📊 Dashboard | Bugünkü Nakit / Kart / E-Ticaret özetleri + Kritik stok uyarıları |
| 📦 Stok Yönetimi | Tüm ürünleri listele, miktar ekle/çıkar (AJAX + SweetAlert2 toast) |
| 💵 Günlük Ciro | Günü kapat formu (tarih otomatik), geçmiş kayıtlar tablosu |
| 📱 Mobil Uyumlu | Bootstrap 5 ile tam responsive tasarım |

---

## Kurulum

### 1. Gereksinimler
- .NET 8 SDK
- PostgreSQL 14+

### 2. Veritabanı Bağlantısı
`appsettings.json` dosyasını düzenleyin:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ShopManagerDb;Username=postgres;Password=ŞIFRENIZ"
}
```

### 3. Migration & Başlat
```bash
dotnet-ef database update
dotnet run
```
> Uygulama ilk açılışta `database update` işlemini otomatik çalıştırır. Seed verileri (örnek stok kalemleri) otomatik eklenir.

---

## Proje Yapısı

```
ShopManager/
├── Controllers/
│   ├── HomeController.cs          ← Dashboard
│   ├── StockController.cs         ← Stok CRUD + AJAX /Stock/AdjustQuantity
│   └── DailyTurnoverController.cs ← Ciro CRUD
├── Models/
│   ├── Stock.cs
│   ├── DailyTurnover.cs
│   └── AppDbContext.cs
├── Views/
│   ├── Home/Index.cshtml          ← Dashboard
│   ├── Stock/Index.cshtml         ← Stok grid + AJAX modal
│   ├── Stock/Create.cshtml
│   ├── Stock/Edit.cshtml
│   ├── DailyTurnover/Index.cshtml
│   └── DailyTurnover/Create.cshtml
└── wwwroot/css/site.css           ← Dark tema tasarım sistemi
```

---

## Teknoloji Yığını

- **Backend**: ASP.NET Core 8 MVC, Entity Framework Core 8
- **Veritabanı**: PostgreSQL (Npgsql provider)
- **Frontend**: Bootstrap 5, Bootstrap Icons
- **Toast/Modal**: SweetAlert2 (CDN)
- **AJAX**: Vanilla JS Fetch API

---

## AJAX Endpoint

`POST /Stock/AdjustQuantity`

```json
Request Body:  { "id": 1, "delta": 5.0 }
Response:      { "id": 1, "itemName": "Çilek", "currentQuantity": 15.0, "unit": "Kg", "isCritical": false }
```
