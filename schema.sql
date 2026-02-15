-- ============================================
-- CREATE TABLES
-- ============================================

-- USERS Table
CREATE TABLE users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    email TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    role TEXT NOT NULL CHECK (role IN ('Admin', 'User')),
    is_active INTEGER NOT NULL DEFAULT 1,
    created_at TEXT NOT NULL DEFAULT (datetime('now'))
);

-- GROCERY_ITEMS Table
CREATE TABLE grocery_items (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    price REAL NOT NULL CHECK (price >= 0),
    description TEXT,
    category TEXT,
    image_url TEXT,
    stock INTEGER NOT NULL DEFAULT 0 CHECK (stock >= 0),
    is_available INTEGER NOT NULL DEFAULT 1,
    admin_id INTEGER NOT NULL,
    created_at TEXT NOT NULL DEFAULT (datetime('now')),
    updated_at TEXT NOT NULL DEFAULT (datetime('now'))
);

-- SHOPPING_CARTS Table
CREATE TABLE shopping_carts (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    status TEXT NOT NULL DEFAULT 'Active'
    CHECK (status IN ('Active', 'Completed', 'Abandoned')),
    total_amount REAL NOT NULL DEFAULT 0.00,
    created_at TEXT NOT NULL DEFAULT (datetime('now')),
    updated_at TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

-- CART_ITEMS Table
CREATE TABLE cart_items (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    shopping_cart_id INTEGER NOT NULL,
    grocery_item_id INTEGER NOT NULL,
    quantity INTEGER NOT NULL CHECK (quantity > 0),
    unit_price REAL NOT NULL CHECK (unit_price >= 0),
    subtotal REAL NOT NULL CHECK (subtotal >= 0),
    added_at TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (shopping_cart_id) REFERENCES shopping_carts(id) ON DELETE CASCADE,
    FOREIGN KEY (grocery_item_id) REFERENCES grocery_items(id) ON DELETE RESTRICT,
    UNIQUE (shopping_cart_id, grocery_item_id)
);

-- ============================================
-- CREATE INDEXES
-- ============================================

CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_role ON users(role);
CREATE INDEX idx_users_is_active ON users(is_active);

CREATE INDEX idx_grocery_category ON grocery_items(category);
CREATE INDEX idx_grocery_admin ON grocery_items(admin_id);
CREATE INDEX idx_grocery_available ON grocery_items(is_available);
CREATE INDEX idx_grocery_name ON grocery_items(name);

CREATE INDEX idx_cart_user ON shopping_carts(user_id);
CREATE INDEX idx_cart_status ON shopping_carts(status);
CREATE INDEX idx_cart_user_status ON shopping_carts(user_id, status);

CREATE INDEX idx_cartitem_cart ON cart_items(shopping_cart_id);
CREATE INDEX idx_cartitem_grocery ON cart_items(grocery_item_id);

-- ============================================
-- CREATE TRIGGERS FOR AUTO-UPDATE
-- ============================================

CREATE TRIGGER trigger_grocery_items_updated_at
    AFTER UPDATE ON grocery_items
    FOR EACH ROW
BEGIN
    UPDATE grocery_items SET updated_at = datetime('now')
    WHERE id = NEW.id;
END;

CREATE TRIGGER trigger_shopping_carts_updated_at
    AFTER UPDATE ON shopping_carts
    FOR EACH ROW
BEGIN
    UPDATE shopping_carts SET updated_at = datetime('now')
    WHERE id = NEW.id;
END;

-- ============================================
-- INSERT TEST DATA
-- ============================================
