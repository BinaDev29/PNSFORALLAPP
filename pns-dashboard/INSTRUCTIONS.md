# PNS Admin Dashboard - Setup Instructions

## 1. Install Dependencies
This project uses `i18next` for internationalization and `lucide-react` for icons.
Please ensure all dependencies are installed by running:

```bash
npm install
# OR if you are on Windows and have issues:
npm install --no-bin-links
```

Required packages:
- `react-i18next`
- `i18next`
- `i18next-browser-languagedetector`
- `lucide-react`
- `clsx`
- `tailwind-merge`
- `@radix-ui/react-dropdown-menu` (and other radix primitives if used)

## 2. Backend Integration
The frontend is configured to proxy requests to `http://localhost:5217` (see `vite.config.ts`).
Ensure your backend API is running on this port.

## 3. Features
- **Internationalization**: Dashboard supports English, Spanish, and French.
- **Notifications**: Manage and view notifications (connected to `NotificationController`).
- **Premium UI**: Glassmorphism, Gradients, and responsive design.

## 4. Troubleshooting
If you see "Module not found" errors, it means `npm install` failed or hasn't completed.
Try running the install command manually in your terminal.
