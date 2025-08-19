# URL Shortener

A simple yet powerful URL shortener application built with ASP.NET Core. Transform long URLs into short, manageable links with comprehensive tracking and management features.

## 🚀 Features

- **URL Shortening**: Convert long URLs into short, easy-to-share links
- **Custom Short Codes**: Choose your own short codes or let the system generate them automatically
- **Click Tracking**: Monitor how many times each short URL has been accessed
- **Web Admin Interface**: Intuitive web-based management dashboard
- **REST API**: Full API support for programmatic access via CURL or other tools
- **Responsive Design**: Clean, mobile-friendly interface using Bootstrap
- **SQLite Database**: Lightweight, file-based database for easy deployment

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git (for cloning and version control)

## 🛠️ Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/yourusername/UrlShortener.git
   cd UrlShortener
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access the application:**
   - Open your browser and navigate to `http://localhost:5171`
   - The admin interface will be available at the root URL

## 💻 Usage

### Web Interface

1. **Creating Short URLs:**
   - Click "Create New Short URL" on the main page
   - Enter the original URL you want to shorten
   - Optionally specify a custom short code
   - Add a description for easier management
   - Click "Create Short URL"

2. **Managing URLs:**
   - View all your short URLs on the main dashboard
   - Edit existing URLs by clicking the "Edit" button
   - Delete URLs with the "Delete" button
   - Track click statistics for each URL

3. **Using Short URLs:**
   - Access any short URL like: `http://localhost:5171/yourcode`
   - Users will be automatically redirected to the original URL
   - Click counts are tracked automatically

### API Usage

The application provides a full REST API for programmatic access:

#### Create a new short URL:
```bash
curl -X POST http://localhost:5171/api/urls \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://example.com/very/long/url",
    "shortCode": "mycode",
    "description": "My custom short URL"
  }'
```

#### List all URLs:
```bash
curl http://localhost:5171/api/urls
```

#### Get URL information by short code:
```bash
curl http://localhost:5171/api/urls/by-code/mycode
```

#### Update an existing URL:
```bash
curl -X PUT http://localhost:5171/api/urls/1 \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://updated-example.com",
    "shortCode": "mycode",
    "description": "Updated description"
  }'
```

#### Delete a URL:
```bash
curl -X DELETE http://localhost:5171/api/urls/1
```

## 🏗️ Architecture

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQLite with Entity Framework Core
- **Frontend**: Bootstrap 5, FontAwesome icons
- **API**: RESTful API with JSON responses

### Project Structure
```
UrlShortener/
├── Controllers/
│   ├── AdminController.cs      # Web interface controller
│   ├── RedirectController.cs   # Handles short URL redirects
│   └── UrlsController.cs       # REST API controller
├── Data/
│   └── UrlShortenerContext.cs  # Entity Framework context
├── Models/
│   └── UrlMapping.cs           # URL mapping model
├── ViewModels/
│   └── CreateUrlMappingViewModel.cs
├── Views/
│   ├── Admin/                  # Admin interface views
│   └── Shared/                 # Layout templates
└── Program.cs                  # Application entry point
```

## 🔧 Configuration

The application uses SQLite by default with the database file `urlshortener.db` created automatically. You can modify the connection string in `Program.cs` if needed.

## 🚀 Deployment

For production deployment:

1. **Publish the application:**
   ```bash
   dotnet publish -c Release
   ```

2. **Deploy to your preferred hosting platform:**
   - The application can be deployed to IIS, Linux servers, Docker, or cloud platforms
   - Ensure the SQLite database file has proper write permissions
   - Consider using a more robust database (SQL Server, PostgreSQL) for production

## 📝 API Documentation

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/urls` | List all URL mappings |
| GET | `/api/urls/{id}` | Get URL mapping by ID |
| GET | `/api/urls/by-code/{shortCode}` | Get URL mapping by short code |
| POST | `/api/urls` | Create new URL mapping |
| PUT | `/api/urls/{id}` | Update URL mapping |
| DELETE | `/api/urls/{id}` | Delete URL mapping |

### Data Models

**URL Mapping Response:**
```json
{
  "id": 1,
  "shortCode": "abc123",
  "shortUrl": "http://localhost:5171/abc123",
  "originalUrl": "https://example.com",
  "description": "Example URL",
  "createdDate": "2025-08-19T21:00:00Z",
  "clickCount": 5
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🎯 Future Enhancements

- User authentication and authorization
- Bulk URL import/export
- Custom domains support
- Advanced analytics and reporting
- URL expiration dates
- QR code generation
- Rate limiting and abuse prevention

## 📞 Support

If you encounter any issues or have questions, please open an issue on GitHub or contact the maintainer.
