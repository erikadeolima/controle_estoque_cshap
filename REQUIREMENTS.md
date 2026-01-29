# Module: Inventory Management – Snack Bar (Food Items)

## 1. Objective

The **Inventory Control** module aims to manage food products available at the snack bar, allowing tracking of quantities, availability status, history, and specific rules for the item lifecycle.

## 2. Scope

The system will cover **only food products**, excluding any other type of supply or non-edible item.

## 3. Features

### 3.1 Listings

- **List active products:** Display all items available in stock.
- **Search specific item:** Return the current quantity of a product based on search by name, SKU, or equivalent identifier.
- **List inactive products:** Display discontinued items, with specific filter by "inactive" status.

### 3.2 Creation

- **Item registration:** Allow the registration of new products with the following mandatory data:
  - Product name.
  - SKU (or equivalent identifier).
  - Initial quantity.
  - Expiration date (when applicable).
  - Category (e.g., cold cuts, beverages, ingredients, etc.).
  - Initial status (active/available).

### 3.3 Update

- **Update quantity:** Allow modification of stock quantity (input or output).
- **Update status:**
  - **Active:** Product available for use/consumption.
  - **Inactive:** Discontinued product, not displayed in the main listing.
  - **Available:** Stock above minimum level.
  - **Out of Stock:** Zero stock.
  - **Alert:** Stock below configured minimum threshold.

### 3.4 Removal

- **Automatic deletion:** Products stored for more than _X years_ can be permanently removed from the system, as long as they are not marked as inactive.

## 4. Business Rules

1. **Mandatory identification:** Every product must have a unique SKU or equivalent code.
2. **Product deactivation:**
   - A deactivated product cannot be manually removed.
   - Its data remains registered only as "inactive" and does not undergo additional changes.
   - Inactive items do not appear in available product listings.
3. **Transaction history:**
   - The system must maintain the complete history of transactions (inputs, outputs, status changes) for a minimum period of _X years_.
4. **Low stock notifications:**
   - When a product quantity reaches the configured "alert" level, the system must generate an automatic warning.

## 5. Technical Considerations (Optional)

- The system should allow future integration with **sales** and **purchasing** modules for automatic stock updates.
- Audit fields should record: responsible user, date and time of changes.
