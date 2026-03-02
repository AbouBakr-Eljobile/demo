# منظومة العقود الحكومية (Backend + Frontend)

مشروع Full-Stack مبني بأسلوب Monorepo ويحتوي على:

- **Backend:** ASP.NET Core Web API (C#)
- **Frontend:** Blazor WebAssembly + Tailwind CSS
- **الواجهات:** RTL عربية حديثة مناسبة لبيئات مؤسسات الدولة
- **الصفحات:** تسجيل الدخول، لوحة التحكم، إضافة عقد
- **الحقول المطلوبة في إضافة العقد:**
  - `contractName`
  - `contractDate`
  - `contractAttachment`

## هيكل المشروع

- `backend/GovContracts.Api`
- `frontend/GovContracts.Web`
- `GovContracts.sln`

## معمارية Backend (SOLID-Oriented)

داخل مشروع API تم فصل الطبقات إلى:

- `Domain/Entities`
- `Application/DTOs`
- `Application/Interfaces`
- `Application/Services`
- `Infrastructure/Repositories`
- `Infrastructure/Services`
- `Controllers`

## المتطلبات

- .NET SDK 9.0+

## التشغيل

### 1) تشغيل Backend

```powershell
cd backend/GovContracts.Api
dotnet run
```

- API Base URL الافتراضي: `http://localhost:5025`
- OpenAPI JSON (في وضع التطوير): `http://localhost:5025/openapi/v1.json`

### 2) تشغيل Frontend

```powershell
cd frontend/GovContracts.Web
dotnet run
```

- Frontend URL الافتراضي: `http://localhost:5092`

## بيانات الدخول التجريبية

- اسم المستخدم: `admin`
- كلمة المرور: `Admin@123`

## API Endpoints

### Auth

- `POST /api/auth/login`

### Contracts

- `GET /api/contracts`
- `GET /api/contracts/{id}`
- `POST /api/contracts` (multipart/form-data)
- `PUT /api/contracts/{id}` (multipart/form-data)
- `DELETE /api/contracts/{id}`

## المرفقات

- يتم حفظ ملفات العقود داخل:
  - `backend/GovContracts.Api/uploads/contracts`
- ويتم إتاحتها عبر مسار:
  - `/uploads/contracts/{fileName}`

## ضبط عنوان الـAPI في Frontend

الملف:

- `frontend/GovContracts.Web/wwwroot/appsettings.json`

القيمة:

```json
{
  "ApiBaseUrl": "http://localhost:5025"
}
```
