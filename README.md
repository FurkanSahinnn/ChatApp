# 💬 ChatApp - Real-Time Chat Application

Modern ve güvenli bir gerçek zamanlı chat uygulaması. ASP.NET Core Web API backend ve MVC frontend ile geliştirilmiştir.

## 🌟 Özellikler

### 🔐 Güvenlik
- **JWT Authentication**: Güvenli kullanıcı kimlik doğrulama
- **İki Faktörlü Kimlik Doğrulama (2FA)**: SendGrid ile email doğrulama
- **Rol Tabanlı Yetkilendirme**: Admin ve Member rolleri
- **Güvenli Şifreleme**: BCrypt ile şifre hashleme

### 💬 Chat Özellikleri
- **Gerçek Zamanlı Mesajlaşma**: SignalR ile anlık mesaj iletimi
- **Kullanıcı Listesi**: Online kullanıcıları görüntüleme
- **Mesaj Geçmişi**: Tüm konuşma geçmişini saklar
- **Modern UI**: Responsive ve kullanıcı dostu arayüz

### 🛠️ Teknik Özellikler
- **Clean Architecture**: CQRS pattern ve MediatR
- **Entity Framework Core**: Code-First yaklaşım
- **SQL Server**: Veritabanı yönetimi
- **SignalR**: Real-time communication
- **SendGrid**: Email servisi

## 🏗️ Proje Yapısı

```
ChatApp/
├── ChatApp.API/                 # Backend Web API
│   ├── Controllers/             # API Controllers
│   │   ├── Core/
│   │   │   ├── Application/         # Business Logic
│   │   │   │   ├── Features/        # CQRS Commands/Queries
│   │   │   │   ├── Interfaces/      # Service Interfaces
│   │   │   │   ├── Dtos/           # Data Transfer Objects
│   │   │   │   └── Options/        # Configuration Options
│   │   │   ├── Domain/             # Domain Entities
│   │   │   ├── Entities/           # Database Entities
│   │   │   └── Hubs/              # SignalR Hubs
│   │   ├── Persistence/            # Data Access Layer
│   │   │   ├── Context/           # Database Context
│   │   │   ├── Repositories/      # Repository Pattern
│   │   │   ├── Services/          # Data Services
│   │   │   └── Migrations/        # EF Migrations
│   │   └── JwtFeatures/           # JWT Implementation
│   ├── ChatApp.Front/              # Frontend MVC
│   │   ├── Controllers/           # MVC Controllers
│   │   ├── Models/               # View Models
│   │   ├── Views/               # Razor Views
│   │   ├── Services/            # Frontend Services
│   │   ├── TwoFactorService/    # 2FA Implementation
│   │   ├── Utils/               # Utility Classes
│   │   └── wwwroot/            # Static Files
│   └── README.md
```

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8.0 SDK
- SQL Server (LocalDB veya Express)
- Visual Studio 2022 veya VS Code
- SendGrid Account (2FA için)

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/your-username/ChatApp.git
cd ChatApp
```

### 2. Konfigürasyon Ayarları

#### ChatApp.API konfigürasyonu:
`ChatApp.API/appsettings.json` dosyasını düzenleyin:

```json
{
  "TokenOptions": {
    "Audience": "https://localhost",
    "Issuer": "https://localhost",
    "AccessTokenExpiration": 5,
    "SecurityKey": "YOUR_JWT_SECRET_KEY_HERE_MINIMUM_32_CHARACTERS_LONG"
  },
  "ConnectionStrings": {
    "SqlServer": "Data Source=YOUR_SERVER_NAME;Initial Catalog=YOUR_DATABASE_NAME;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
  },
  "AllowedHosts": "*"
}
```

#### ChatApp.Front konfigürasyonu:
`ChatApp.Front/appsettings.json` dosyasını düzenleyin:

```json
{
  "TwoFactorOptions": {
    "SendGrid_ApiKey": "YOUR_SENDGRID_API_KEY_HERE",
    "CodeTimeExpire": 180
  },
  "TokenOptions": {
    "Audience": "https://localhost",
    "Issuer": "https://localhost",
    "AccessTokenExpiration": 5,
    "SecurityKey": "YOUR_JWT_SECRET_KEY_HERE_MINIMUM_32_CHARACTERS_LONG"
  },
  "AllowedHosts": "*"
}
```

### 3. Veritabanı Kurulumu

API projesinde migration'ları çalıştırın:

```bash
cd ChatApp.API
dotnet ef database update
```

### 4. Bağımlılıkları Yükleyin

Her iki proje için:
```bash
cd ChatApp.API
dotnet restore

cd ../ChatApp.Front
dotnet restore
```

### 5. Projeleri Çalıştırın

**Terminal 1 - API:**
```bash
cd ChatApp.API
dotnet run
```

**Terminal 2 - Frontend:**
```bash
cd ChatApp.Front
dotnet run
```

API: `https://localhost:5221`
Frontend: `https://localhost:7041`

## 📚 Kullanım

### Kayıt Olma
1. Ana sayfadan "Sign Up" linkine tıklayın
2. Kullanıcı bilgilerinizi girin
3. Email adresinize gelen 5 haneli doğrulama kodunu girin
4. Hesabınız aktif hale gelecektir

### Giriş Yapma
1. Email ve şifrenizi girin
2. Başarılı girişten sonra chat sayfasına yönlendirileceksiniz

### Chat Kullanımı
1. Sol taraftaki kullanıcı listesinden mesaj göndermek istediğiniz kişiyi seçin
2. Alt kısımdaki metin kutusuna mesajınızı yazın
3. "Send" butonuna tıklayın veya Enter tuşuna basın
4. Mesajlar gerçek zamanlı olarak iletilir

## 🔧 API Endpoints

### Authentication
- `POST /api/auth/register` - Kullanıcı kaydı
- `POST /api/auth/login` - Kullanıcı girişi

### Chat
- `GET /api/chats/GetChats` - Mesaj geçmişini getir
- `POST /api/chats/SendMessage` - Mesaj gönder
- `GET /api/chats/GetUsers` - Kullanıcı listesini getir

### Admin
- `GET /api/admin/GetUsers` - Tüm kullanıcıları listele (Admin only)
- `DELETE /api/admin/DeleteUser/{id}` - Kullanıcı sil (Admin only)

## 🔐 Güvenlik Önlemleri

- **JWT Token**: Her API isteği için gerekli
- **CORS**: Belirli origin'lere izin verilir
- **HTTPS**: Production'da zorunlu
- **Şifre Hashleme**: BCrypt algoritması
- **2FA**: Email doğrulama sistemi

## 🗄️ Veritabanı Şeması

### UserApps Tablosu
- Id (int, Primary Key)
- Name (string)
- Email (string)
- Password (string, hashed)
- RoleId (int, Foreign Key)

### RoleApps Tablosu
- Id (int, Primary Key)
- Name (string)

### Chats Tablosu
- Id (int, Primary Key)
- UserId (int)
- ToUserId (int)
- Message (string)
- Date (DateTime)

## 🛠️ Teknoloji Stack

### Backend (ChatApp.API)
- **ASP.NET Core 8.0** - Web API Framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **SignalR** - Real-time communication
- **MediatR** - CQRS pattern
- **JWT** - Authentication
- **BCrypt** - Password hashing
- **Swagger** - API documentation

### Frontend (ChatApp.Front)
- **ASP.NET Core MVC** - Web Framework
- **Bootstrap** - CSS Framework
- **SignalR Client** - Real-time client
- **jQuery** - JavaScript library
- **SendGrid** - Email service

## 📋 Geliştirici Notları

### CQRS Pattern
Proje CQRS (Command Query Responsibility Segregation) pattern kullanır:
- **Commands**: Veri değiştirme işlemleri
- **Queries**: Veri okuma işlemleri
- **MediatR**: Command/Query dispatching

### SignalR Hub
`ChatHub` sınıfı gerçek zamanlı mesajlaşma için kullanılır:
- Kullanıcı bağlantı yönetimi
- Mesaj broadcasting
- Online kullanıcı takibi

### Clean Architecture
Proje katmanları:
1. **Core**: Business logic ve domain entities
2. **Persistence**: Data access layer
3. **API**: Presentation layer

## 🤝 Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit edin (`git commit -m 'Add some amazing feature'`)
4. Push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE.txt` dosyasına bakın.

## 📞 İletişim

Proje hakkında sorularınız için GitHub Issues kullanabilirsiniz.

---

**⚠️ Önemli**: Production ortamında kullanmadan önce tüm güvenlik ayarlarını gözden geçirin ve güncel tutun.