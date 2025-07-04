#  Regivit SavingsApp

Aplicación web para la gestión de clientes y cuentas de ahorro. Permite a operadores bancarios realizar acciones como crear cuentas, registrar transacciones (depósitos y retiros), y consultar el historial de operaciones, todo desde una interfaz amigable y segura.

---

## 📝 Descripción General

SavingsApp es un sistema de gestion de cuentas de ahorro que permite:

- Administrar las cuentas de ahorro para clientes
- Realizar depósitos y retiros con límites diarios
- Consultar el historial de transacciones
- Autenticación de usuarios con login seguro
- Registro de errores y notificaciones visuales

## 🧱 Arquitecturas y Patrones de Diseño

### Arquitectura Monolítica
- Ideal para proyectos que no requieren escalabilidad distribuida.
- Agiliza la compilación y despliegue.
- Posee una menor complejidad inicial.
- Facilita la integración entre modulos.

### Patron Modelo Vista Controlador
- Separa las responsabilidades del sistema.
- Es escalable y de facil mantenimiento.

## 🛠️ Tecnologías y Patrones Utilizados

| Categoría        | Herramientas / Tecnologías                          |
|------------------|-----------------------------------------------------|
| Backend          | ASP.NET Core MVC 8, C#, Entity Framework Core       |
| Base de Datos    | SQL Server                                          |
| Seguridad        | Autenticación con cookies y hash (BCrypt)           |
| UI               | Bootstrap 5, HTML,                    |
| Logging          | ILogger, NToastNotify para notificaciones           |

---

## ⚙️ Instrucciones de Configuración y Ejecución

### 1. Clonar el repositorio.

```bash
git clone https://github.com/EminRams/SavingsApp.git
cd SavingsApp
```

### 2. Renombar el archivos appsettings.example.json a appsettings.json y declarar la cadena de conexión.
```bash
"DefaultConnection": "Server=SERVER_NAME;Database=DATABASE_NAME;User Id=USER_NAME;Password=PASSWORD;TrustServerCertificate=True"
```

### 3. Ejecutar el Script de la base de datos en SqlServer.

### 4. Ejecutar el programa.

### 5. Automaticamente se crearan los usuarios y clientes en la base de datos.

### 6. Puedes iniciar sesion con los siguientes datos:
- Correo: emin@example.com
- Contraseña: secret123
