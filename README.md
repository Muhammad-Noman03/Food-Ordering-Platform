# FoodieExpress - Food Ordering Web Application

A fully functional, interactive, and responsive web application for food ordering built with HTML, CSS, JavaScript, and ASP.NET Core backend.

## 📋 Project Overview

This project demonstrates the integration of client-side and server-side technologies to build a cohesive and user-friendly food ordering application.

### Features

- **3 Main Pages**: Home, Menu, and Contact
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **Interactive UI**: Form validation, animations, shopping cart functionality
- **Backend API**: ASP.NET Core with CRUD operations
- **Database Integration**: Entity Framework Core with SQL Server

## 🛠️ Technologies Used

### Frontend
- **HTML5** - Page structure and semantic markup
- **CSS3** - Styling with responsive design
- **Bootstrap 5** - CSS framework for responsive layout
- **JavaScript (ES6+)** - Client-side interactivity
- **Font Awesome** - Icons

### Backend
- **ASP.NET Core 8.0** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server LocalDB** - Database storage
- **Swagger/OpenAPI** - API documentation

## 📁 Project Structure

```
FoodOrderingApp/
├── index.html              # Home page
├── menu.html               # Menu/Products page
├── contact.html            # Contact/Information page
├── css/
│   └── style.css          # Main stylesheet (responsive)
├── js/
│   ├── app.js             # Main application JavaScript
│   ├── cart.js            # Shopping cart functionality
│   ├── menu.js            # Menu page functionality
│   └── contact.js         # Contact form validation
└── Backend/
    └── FoodOrderingAPI/
        ├── Controllers/    # API Controllers (CRUD operations)
        ├── Models/         # Data models
        ├── DTOs/           # Data Transfer Objects
        ├── Services/       # Business logic layer
        ├── Data/           # Database context & seeder
        └── Program.cs      # Application entry point
```

## 🚀 Getting Started

### Prerequisites

1. **Visual Studio 2022** or **VS Code** with C# extension
2. **.NET 8.0 SDK**
3. **SQL Server LocalDB** (included with Visual Studio)
4. Modern web browser (Chrome, Firefox, Edge)

### Running the Frontend

1. Navigate to the `FoodOrderingApp` folder
2. Open `index.html` in a web browser
3. Or use VS Code Live Server extension for hot reload

### Running the Backend

1. Open terminal in `Backend/FoodOrderingAPI` folder
2. Restore packages:
   ```bash
   dotnet restore
   ```
3. Run the application:
   ```bash
   dotnet run
   ```
4. API will be available at:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:7001`
5. Swagger documentation: `https://localhost:7001/swagger`

### Database Setup

The database is automatically created and seeded when you run the application for the first time.

**Connection String** (in `appsettings.json`):
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FoodOrderingDB;Trusted_Connection=True"
}
```

## 📡 API Endpoints

### Menu Items (CRUD)
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/menuitems` | Get all menu items |
| GET | `/api/menuitems/{id}` | Get menu item by ID |
| GET | `/api/menuitems/popular` | Get popular items |
| GET | `/api/menuitems/category/{category}` | Get items by category |
| GET | `/api/menuitems/search?term={term}` | Search menu items |
| POST | `/api/menuitems` | Create new menu item |
| PUT | `/api/menuitems/{id}` | Update menu item |
| DELETE | `/api/menuitems/{id}` | Delete menu item |

### Orders (CRUD)
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/orders` | Get all orders |
| GET | `/api/orders/{id}` | Get order by ID |
| GET | `/api/orders/number/{orderNumber}` | Get order by number |
| GET | `/api/orders/status/{status}` | Get orders by status |
| POST | `/api/orders` | Create new order |
| PUT | `/api/orders/{id}/status` | Update order status |
| DELETE | `/api/orders/{id}` | Delete order |

### Contacts (CRUD)
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/contacts` | Get all contacts |
| GET | `/api/contacts/{id}` | Get contact by ID |
| GET | `/api/contacts/unread` | Get unread contacts |
| POST | `/api/contacts` | Submit contact form |
| PUT | `/api/contacts/{id}/read` | Mark as read |
| PUT | `/api/contacts/{id}/resolve` | Mark as resolved |
| DELETE | `/api/contacts/{id}` | Delete contact |

## 🎨 Responsive Design

The application is fully responsive with breakpoints for:
- **Desktop**: 1200px and above
- **Tablet**: 768px - 1199px
- **Mobile**: Below 768px

CSS features used:
- CSS Variables for theming
- Flexbox and Grid layouts
- Media queries for responsiveness
- CSS animations and transitions

## ✨ JavaScript Features

- **Form Validation**: Real-time validation with feedback
- **Shopping Cart**: Add, remove, update quantities
- **Local Storage**: Cart persistence across sessions
- **Dynamic Content**: Menu items loaded dynamically
- **Search & Filter**: Category filtering and text search
- **Toast Notifications**: User feedback messages
- **Modal Dialogs**: Item details and checkout

## 🔒 Security Considerations

- Input validation on both client and server
- CORS configuration for frontend access
- SQL injection prevention via Entity Framework
- XSS prevention through proper encoding

## 📝 Assignment Requirements Met

1. ✅ Three pages: Home, Menu, Contact
2. ✅ Responsive design using Bootstrap
3. ✅ JavaScript for dynamic behavior (validation, animations, interactivity)
4. ✅ ASP.NET backend with CRUD operations
5. ✅ Database integration (SQL Server with Entity Framework)

## 👨‍💻 Development Tools Used

- **VS Code** - Frontend development
- **Visual Studio 2022** - Backend development
- **SQL Server Management Studio** - Database management
- **Browser DevTools** - Testing and debugging
- **Postman** - API testing

## 📄 License

This project is created for educational purposes.

---

**FoodieExpress** - Delivering happiness one meal at a time! 🍔🍕🥗
