# PNS Dashboard

The administrative frontend for the **Push Notification System (PNS)**. This dashboard allows system administrators to manage client applications, monitor notification delivery, and view engagement metrics like email open rates.

## ✨ Features
- **Client Management:** Register and manage external applications authorized to use the PNS.
- **Real-time Analytics:** View delivery success rates and open event logs.
- **Notification History:** Search and filter through past notifications.
- **Template Management:** Configure and preview email templates.
- **Modern UI:** Built with React 19, Tailwind CSS, and Shadcn UI for a premium, high-performance experience.

## 🛠️ Tech Stack
- **Framework:** React 19 (Vite)
- **Styling:** Tailwind CSS
- **Components:** Shadcn UI / Radix UI
- **State Management:** React Context API / TanStack Query (optional)
- **Icons:** Lucide React

## 🏁 Getting Started

### Prerequisites
- Node.js (v20+)
- npm or pnpm

### Installation
1. Install dependencies:
   ```bash
   npm install
   ```
2. Configure Environment Variables:
   Create a `.env` file in the root directory:
   ```env
   VITE_API_URL=https://localhost:7198/api
   ```

### Development
Start the development server with HMR:
```bash
npm run dev
```

### Production
Build the optimized bundle for production:
```bash
npm run build
```

## 📂 Project Structure
- `src/components`: Reusable UI components.
- `src/pages`: Main application views (Dashboard, Login, Clients, History).
- `src/services`: API client and data fetching logic.
- `src/hooks`: Custom React hooks for shared logic.
- `src/context`: Authentication and global state.
