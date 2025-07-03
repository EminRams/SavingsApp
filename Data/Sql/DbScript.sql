CREATE DATABASE SavingsApp;

USE SavingsApp;

-- Tabla: Users (operadores bancarios).
CREATE TABLE Users (
    id INT PRIMARY KEY IDENTITY(1, 1),
    username NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    created_at DATETIME2 DEFAULT GETDATE()
);

-- Customers (clientes)
CREATE TABLE Customers (
    id INT PRIMARY KEY IDENTITY(1, 1),
    name NVARCHAR(100) NOT NULL,
    identification NVARCHAR(50) NOT NULL,
    phone_number NVARCHAR(20),
    email NVARCHAR(100),
    created_at DATETIME2 DEFAULT GETDATE()
)

-- Tabla: SavingsAccounts (cuentas de ahorro)
CREATE TABLE SavingsAccounts (
    id INT PRIMARY KEY IDENTITY(1, 1),
    customer_id INT NOT NULL,
    account_number NVARCHAR(20) NOT NULL UNIQUE,
    balance DECIMAL(18, 6) NOT NULL DEFAULT 0,
    description varchar(255) NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (customer_id) REFERENCES Customers(id)
);

-- Tabla: Transactions (depósitos y retiros)
CREATE TABLE Transactions (
    id INT PRIMARY KEY IDENTITY(1, 1),
    savings_account_id INT NOT NULL,
    type NVARCHAR(10) NOT NULL CHECK (type IN ('deposit', 'withdrawal')),
    amount DECIMAL(18,6) NOT NULL CHECK (Amount > 0),
    transaction_date DATETIME2 GETDATE() NOT NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (savings_account_id) REFERENCES SavingsAccounts(id)
);

-- Tabla: AuditLogs (registro de auditoría)
CREATE TABLE AuditLogs (
    id INT PRIMARY KEY IDENTITY(1, 1),
    customer_id INT,
    action NVARCHAR(100) NOT NULL,
    details NVARCHAR(200),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (customer_id) REFERENCES Customers(id)
);