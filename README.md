# ArvinTabriz

یک وب‌سایت شروعی راست‌به‌چپ با ASP.NET Core 8 و Razor Pages.

## پیش‌نیازها

- .NET 8 SDK

## اجرا

```bash
dotnet restore
dotnet run
```

پس از اجرا، سایت از طریق آدرس‌های تنظیم‌شده در `Properties/launchSettings.json` در دسترس است.

## زیرساخت CMS و پایگاه‌داده

لایهٔ دادهٔ اولیهٔ CMS با Entity Framework Core و SQLite آماده شده است. پایگاه‌داده
به‌صورت پیش‌فرض در `App_Data/arvintabriz.db` ساخته می‌شود و هنگام شروع برنامه،
schema اولیه به‌صورت خودکار ایجاد می‌شود. مسیر پایگاه‌داده از طریق
`ConnectionStrings__DefaultConnection` (یا بخش `ConnectionStrings:DefaultConnection`
در تنظیمات) قابل تغییر است. پوشهٔ `App_Data` و فایل پایگاه‌داده در صورت نیاز
هنگام شروع برنامه ساخته می‌شوند.

این مرحله فقط مدل‌های مستقل `Products`، `Projects`، `Slides` و `ContentPages` و
context پایگاه‌داده را فراهم می‌کند. در این مرحله از `EnsureCreated` استفاده شده
است تا پس از تثبیت مدل‌ها، migrationهای نسخه‌گذاری‌شده اضافه شوند. مرحلهٔ بعد شامل صفحات CRUD امن
در پنل مدیریت، اعتبارسنجی slug و مدیریت تصاویر خواهد بود. پیام‌های فرم تماس
فعلاً مانند قبل در فایل JSON ذخیره می‌شوند.

## پنل مدیریت

فرم تماس سایت درخواست‌ها را بدون نیاز به سرویس خارجی در
`App_Data/contact-messages.json` ذخیره می‌کند. پنل مدیریت در مسیر `/Admin` قرار
دارد و امکان مشاهده و تغییر وضعیت درخواست‌ها را فراهم می‌کند.

برای ورود به پنل، باید مقدارهای `Admin:Username` و `Admin:Password` را در
تنظیمات محیطی تعریف کنید. این مقدارها عمداً در `appsettings.json` قرار ندارند
تا credential پیش‌فرض یا قابل‌انتشار در مخزن وجود نداشته باشد:

```bash
export Admin__Username="your-admin-username"
export Admin__Password="a-long-unique-password"
```

در PowerShell ویندوز:

```powershell
$env:Admin__Username = "your-admin-username"
$env:Admin__Password = "a-long-unique-password"
dotnet run
```

رمز عبور باید یکتا و طولانی باشد و در مخزن یا فایل‌های قابل‌انتشار ذخیره نشود.

پس از ورود، کاربر به داشبورد مدیریت منتقل می‌شود و با انتخاب «خروج از حساب»،
نشست کاربر پایان می‌یابد و به صفحهٔ ورود بازمی‌گردد.

## رفع خطای `NuGet.targets(198,5)` در ویندوز

خطای زیر معمولاً به باقی‌ماندن خروجی restore در `obj` یا اجرای هم‌زمان دو
فرآیند restore/build برای همین پروژه مربوط است:

```text
Cannot create a file when that file already exists.
```

ابتدا Visual Studio و تمام پنجره‌های ترمینالی که ممکن است در حال اجرای
`dotnet` باشند را ببندید. سپس، در PowerShell و از پوشهٔ پروژه، خروجی‌های
بازسازی‌پذیر را پاک کرده و restore و build را فقط یک‌بار اجرا کنید:

```powershell
Remove-Item -Recurse -Force bin, obj -ErrorAction SilentlyContinue
dotnet restore
dotnet build --no-restore
```

پوشه‌های `bin` و `obj` خروجی تولیدشده هستند و در گیت نگهداری نمی‌شوند؛ پاک
کردن آن‌ها فایل‌های منبع پروژه را حذف نمی‌کند. اگر خطا بازگشت، بررسی کنید که
یک IDE، اسکریپت CI، یا پنجرهٔ ترمینال دیگر هم‌زمان `dotnet restore` یا
`dotnet build` را برای همین پوشه اجرا نکند.

## همگام‌سازی تغییرات گیت‌هاب با کامپیوتر

برای اینکه هر تغییری که در گیت‌هاب کامیت و منتشر می‌شود روی نسخه داخل کامپیوتر هم قرار بگیرد، در پوشه پروژه این دستور را اجرا کنید:

```bash
git pull
```

اگر می‌خواهید این کار همیشه خودکار انجام شود، می‌توانید روی سیستم مقصد یک job زمان‌بندی‌شده مثل cron یا Task Scheduler بسازید که همین دستور را در مسیر پروژه اجرا کند.
