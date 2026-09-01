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
