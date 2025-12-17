import { NavLink } from "react-router-dom";
import { LayoutDashboard, Bell, History, FileText, Smartphone, Settings, LogOut, Activity } from "lucide-react";
import { LanguageSwitcher } from "@/components/layout/LanguageSwitcher";
import { ModeToggle } from "@/components/mode-toggle";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";

import { useTranslation } from "@/i18n";

import { useAuth } from "@/contexts/AuthContext";

export function Sidebar() {
    const { t } = useTranslation();
    const { logout } = useAuth();

    const navItems = [
        { icon: LayoutDashboard, label: t('sidebar.dashboard', "Dashboard"), href: "/" },
        { icon: Bell, label: t('sidebar.notifications', "Notifications"), href: "/notifications" },
        { icon: History, label: t('sidebar.history', "History"), href: "/history" },
        { icon: FileText, label: t('sidebar.templates', "Templates"), href: "/templates" },
        { icon: Smartphone, label: t('sidebar.clients', "Client Apps"), href: "/clients" },
        { icon: Activity, label: "System Health", href: "/system-health" },
        { icon: Settings, label: t('sidebar.settings', "Settings"), href: "/settings" },
    ];

    return (
        <aside className="hidden md:flex w-64 h-screen bg-card/30 backdrop-blur-xl border-r border-border/40 fixed left-0 top-0 flex-col z-40 transition-all duration-300">
            <div className="p-6 flex items-center justify-between border-b border-border/40">
                <h1 className="text-2xl font-bold bg-gradient-to-br from-primary via-violet-500 to-indigo-500 bg-clip-text text-transparent drop-shadow-sm">
                    PNS Admin
                </h1>
                <div className="flex items-center gap-2">
                    <ModeToggle />
                    <LanguageSwitcher />
                </div>
            </div>

            <nav className="flex-1 px-4 py-6 space-y-2 overflow-y-auto">
                {navItems.map((item) => (
                    <NavLink
                        key={item.href}
                        to={item.href}
                        className={({ isActive }) =>
                            cn(
                                "flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 group text-sm font-medium",
                                isActive
                                    ? "bg-primary text-primary-foreground shadow-md shadow-primary/25 translate-x-1"
                                    : "text-muted-foreground hover:bg-muted/50 hover:text-foreground hover:translate-x-1"
                            )
                        }
                    >
                        {({ isActive }) => (
                            <>
                                <item.icon className={cn("w-5 h-5 transition-colors", isActive ? "text-primary-foreground" : "text-muted-foreground group-hover:text-primary")} />
                                <span>{item.label}</span>
                            </>
                        )}
                    </NavLink>
                ))}
            </nav>

            <div className="p-4 border-t border-border/40">
                <NavLink to="/profile" className="block">
                    <div className="flex items-center gap-3 px-3 py-3 rounded-xl hover:bg-muted/50 transition-colors cursor-pointer mb-2">
                        <div className="w-9 h-9 rounded-full bg-gradient-to-tr from-primary to-indigo-400 flex items-center justify-center text-white text-xs font-bold shadow-sm">
                            AD
                        </div>
                        <div className="flex flex-col">
                            <span className="text-sm font-semibold">{t('sidebar.profile', 'Admin User')}</span>
                            <span className="text-xs text-muted-foreground">admin@pns.com</span>
                        </div>
                    </div>
                </NavLink>
                <Button
                    variant="ghost"
                    className="w-full justify-start text-destructive hover:text-destructive hover:bg-destructive/10 gap-2 pl-3"
                    onClick={logout}
                >
                    <LogOut className="w-4 h-4" />
                    {t('sidebar.signOut', 'Sign Out')}
                </Button>
            </div>
        </aside>
    );
}
