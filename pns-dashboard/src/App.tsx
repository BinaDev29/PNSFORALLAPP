import { BrowserRouter, Routes, Route, Navigate, useLocation } from "react-router-dom";
import DashboardLayout from "@/layouts/DashboardLayout";
import Dashboard from "@/pages/Dashboard";
import NotificationsPage from "@/pages/Notifications";
import ClientsPage from "@/pages/Clients";
import TemplatesPage from "@/pages/Templates";
import HistoryPage from "@/pages/History";
import ProfilePage from "@/pages/Profile";
import SystemHealthPage from "@/pages/SystemHealth";
import SettingsPage from "@/pages/Settings";
import { Toaster } from "@/components/ui/sonner"
import { AuthProvider, useAuth } from "@/contexts/AuthContext";
import LoginPage from "@/pages/Login";

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const { isAuthenticated, checkAuth } = useAuth();
  const location = useLocation();

  // Check both state and direct storage to prevent flickering
  if (!isAuthenticated && !checkAuth()) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <>{children}</>;
}

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />

          <Route path="/" element={
            <ProtectedRoute>
              <DashboardLayout />
            </ProtectedRoute>
          }>
            <Route index element={<Dashboard />} />
            <Route path="notifications" element={<NotificationsPage />} />
            <Route path="history" element={<HistoryPage />} />
            <Route path="templates" element={<TemplatesPage />} />
            <Route path="clients" element={<ClientsPage />} />
            <Route path="profile" element={<ProfilePage />} />
            <Route path="settings" element={<SettingsPage />} />
            <Route path="system-health" element={<SystemHealthPage />} />
            <Route path="*" element={<div className="h-[60vh] flex items-center justify-center text-muted-foreground">404 - Page Not Found</div>} />
          </Route>
        </Routes>
        <Toaster richColors />
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
