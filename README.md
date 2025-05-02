# AppointmentApp
ASP.NET Core MVC tabanlı basit bir randevu yönetim sistemi.

## Projeyi Çalıştırmak İçin Gerekli Adımlar

1. **Repo’yu klonlayın**  
   git clone https://github.com/UserFoundNot/AppointmentApp.git
   cd AppointmentApp

2. Bağımlılıkları yükleyin

	dotnet restore

3. Veritabanını oluşturun ve güncelleyin

	dotnet tool install --global dotnet-ef    # (ilk seferde)
	dotnet ef database update \
	  --project src/AppointmentApp.Infrastructure \
	  --startup-project src/AppointmentApp.WebUI

4. Uygulamayı çalıştırın

    dotnet run --project src/AppointmentApp.WebUI

    Tarayıcıda https://localhost:5001 (veya konsolda görünen URL) adresini açın.

    Admin Paneli için rol atayın
	AppointmentApp.WebUI altında program.cs içinde 
	"var adminEmail = "2200001590@stu.iku.edu.tr";" admin hesabının atamasını yapan satırdır.
	Dilerseniz bu kullanıcıyla devam edebilirsiniz (password:Grand18.) ya da kod tabanında kullanıcı değiştirebilirsiniz.

# Kullanılan Teknolojiler
    .NET 7 & ASP.NET Core MVC

    Entity Framework Core (Code-First + SQLite)

    ASP.NET Core Identity (cookie-based authentication)

    Razor View Engine

    MailKit & MimeKit (e-posta gönderimi)

    xUnit (unit testler)

    Git (versiyon kontrol)

# Varsayımlar ve Yaklaşımlar

    Katmanlı Mimari

        Core (Entity ve servis arabirimleri)

        Infrastructure (EF Core DbContext, repository, e-posta servisi)

        WebUI (MVC, Controller, View, DI)

        Tests (Unit testler, InMemory EF)

    Authentication

        Cookie tabanlı, ASP.NET Identity ile

        External login yok, basit e-posta+şifre

    E-posta Bildirimi

        Mock: ConsoleEmailSender (konsola yazar)

        Gerçek SMTP: SmtpEmailSender (MailKit/MimeKit)
		**Gerçek mail gönderimi için bu adımları takip edin**
		Gmail’de “App Password” Oluştur
		Google hesabına web’den https://myaccount.google.com adresinden giriş yap.
		Security sekmesine git.
		App passwords bölümünü (“Uygulama Parolaları”) aç.
		Eğer İki Faktörlü Doğrulama (2FA) etkin değilse, önce 2FA’yı aktifleştirmen gerekir.
		“Select app” kısmından Other (Custom name) seç, “AppointmentApp” yaz ve Generate tuşuna bas.
		Verilen 16 karakterli app password’u appsettings.json içinde kullan.
		Ardından AppointmentApp.WebUI altında appsetting.json içinde
		"EmailSettings": {
		  "Host": "smtp.gmail.com",
		  "Port": 587,
		  "Username": "admin@domain.com",
		  "Password": "password",
		  "From": "AppointmentApp"
		}, kısmını kendi bilgilerinize göre düzenleyin 
		*****************************************
        Hata durumunda akışı kesmeyen try/catch(appsettings.json mail kısmı mock olsa bile çalışır)

    Validasyon

        Geçmiş tarihe randevu engeli

        Sadece gelecekteki randevular düzenlenebilir/silinir

        Kullanıcı yalnızca kendi randevularını görebilir

    Admin Paneli (Opsiyonel)

        Areas/Admin altında, [Authorize(Roles="Admin")]

        Admin rolündeki kullanıcılar tüm kullanıcıların randevularını yönetebilir

    Migration & Veritabanı

        EF Core Code-First, migration’lar projede saklı

        SQLite kullanıldı (kurulumsuz, dosya-tabanlı)

    Test

        xUnit + InMemory EF ile birim testler yazıldı
