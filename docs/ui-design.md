---
layout: default
title: UI/UX Design
---

# UI/UX Design

[Back to Home](./)

---

## Design Philosophy

The interface uses an earthy, agricultural color palette that reflects the farming domain. The design prioritizes clarity and usability, with consistent iconography and card-based layouts.

## Color Palette

| Color              | Hex       | Usage                            |
|-------------------|-----------|----------------------------------|
| Agri Green        | `#5b7a2f` | Primary headings, navbar, links  |
| Agri Brown        | `#6b5b3a` | Secondary headings, labels       |
| Agri Yellow       | `#c5a834` | Accent buttons, action buttons   |
| Card Border       | `#c5d89a` | Card borders                     |
| Card Top          | `#b8cc45` | Card top border accent           |
| Header Background | `#e8efd5` | Table headers, section headers   |
| Page Background   | `#f5f3ee` | Overall page background          |
| Muted Text        | `#7a7a6d` | Subtitles, descriptions          |

## Component Library

### Navigation Bar

- Olive green background (`#5b7a2f`)
- White text with hover effects
- Role-conditional links:
  - **Farmer**: Home, Privacy, Products
  - **Employee**: Home, Privacy, Products, Farmers
- User greeting and Logout on the right

### Cards (`.agri-card`)

- White background with rounded corners
- Yellow-green top border (`3px solid #b8cc45`)
- Light card border (`#c5d89a`)
- Used for all content sections

### Tables (`.agri-table`)

- Olive-tinted header row (`#e8efd5`)
- Uppercase header text with icon prefixes
- Alternating row hover effects
- Green-bordered action buttons

### Buttons

| Style             | Class              | Usage                    |
|------------------|--------------------|--------------------------|
| Green (Primary)  | `.btn-agri-green`  | Submit, Register, Save   |
| Yellow (Accent)  | `.btn-agri-yellow` | Add Another, Secondary   |
| Outline          | `.btn-agri-outline`| Back, Clear, Reset       |

### Badges (`.badge-agri`)

Green pill badges for product categories (Fruits, Vegetables, Grains, Dairy, Livestock, Other).

### Form Inputs (`.agri-input`)

- Rounded borders with olive-green focus ring
- Icon-prefixed labels (e.g., droplet for Product Name, envelope for Email)
- Help text below email fields
- Privacy notice at the bottom of all forms

## Page Layouts

### Home Page
- Hero section with leaf icon and app title
- Three feature cards: For Farmers, For Employees, For the Industry
- Platform Features and Technical Highlights cards

### Products Index
- Page title with leaf icon
- Card containing: search bar, product table, count info
- Category badges in table cells
- Details action buttons

### Products Create
- Back navigation button
- Card with "Product Information" section
- Icon-labeled form fields: Name, Category (dropdown), Production Date (date picker), Price, Quantity, Description (textarea)
- Save Product (green) + Reset (outline) buttons
- Privacy notice footer

### Farmers Directory
- "Register New Farmer" green button at top
- Card with search bar and farmer table
- Eye and Info action buttons per row
- "Add Another Farmer" yellow button at bottom

### Farmer Details
- Profile card with olive header bar
- Tractor SVG illustration
- Icon-labeled detail fields
- Products sub-table (if any products exist)

### Farmer Registration
- Back navigation button
- Card with "Farmer Details" section
- Icon-labeled form fields
- Register Farmer (green) + Clear Form (outline) buttons
- Privacy notice footer

## Responsive Design

The layout uses Bootstrap 5's grid system with responsive breakpoints. All cards and tables adapt to smaller screens for field use on mobile devices.

## Iconography

UI elements use HTML entity icons throughout for a consistent visual language:

| Icon | Entity      | Usage                  |
|------|-------------|------------------------|
| Leaf | `&#x1F33F;` | App branding           |
| Wheat| `&#x1F33E;` | Products section       |
| People| `&#x1F465;`| Employees section      |
| Lock | `&#x1F512;` | Security features      |
| Search| `&#x1F50D;`| Search functionality   |
| Tractor| SVG       | Farmer profile details |
