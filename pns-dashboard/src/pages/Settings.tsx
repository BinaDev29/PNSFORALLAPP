import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";
import { Bell, Globe, Mail, Palette, Wifi } from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";
import { motion, AnimatePresence } from "framer-motion";
import { cn } from "@/lib/utils";
import { useTheme } from "@/components/theme-provider";

export default function SettingsPage() {
    const [activeTab, setActiveTab] = useState("general");
    const [isLoading, setIsLoading] = useState(false);
    const { setTheme, theme } = useTheme();

    const [settings, setSettings] = useState({
        appName: "PNS Admin Dashboard",
        supportEmail: "support@pns.com",
        notifications: {
            email: true,
            push: true,
            marketing: false,
            security: true
        },
        appearance: {
            compactMode: false,
            animations: true
        }
    });

    const handleSave = async () => {
        setIsLoading(true);
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 1000));
        setIsLoading(false);
        toast.success("Settings saved successfully");
    };

    const tabs = [
        { id: "general", label: "General", icon: Globe },
        { id: "appearance", label: "Appearance", icon: Palette },
        { id: "notifications", label: "Notifications", icon: Bell },
        { id: "api", label: "API & Integrations", icon: Wifi },
    ];

    return (
        <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
            <div className="flex flex-col gap-2">
                <h2 className="text-3xl font-bold tracking-tight bg-gradient-to-r from-blue-500 to-indigo-600 bg-clip-text text-transparent">
                    Settings
                </h2>
                <p className="text-muted-foreground">
                    Configure your application preferences and system defaults.
                </p>
            </div>

            <div className="flex flex-col md:flex-row gap-8">
                {/* Sidebar Navigation for Settings */}
                <div className="w-full md:w-64 flex-shrink-0 space-y-2">
                    {tabs.map((tab) => (
                        <button
                            key={tab.id}
                            onClick={() => setActiveTab(tab.id)}
                            className={cn(
                                "flex items-center gap-3 w-full px-4 py-3 rounded-xl text-sm font-medium transition-all duration-200",
                                activeTab === tab.id
                                    ? "bg-primary text-primary-foreground shadow-md"
                                    : "bg-card hover:bg-muted text-muted-foreground hover:text-foreground"
                            )}
                        >
                            <tab.icon className="w-4 h-4" />
                            {tab.label}
                        </button>
                    ))}
                </div>

                {/* Content Area */}
                <div className="flex-1">
                    <AnimatePresence mode="wait">
                        <motion.div
                            key={activeTab}
                            initial={{ opacity: 0, x: 20 }}
                            animate={{ opacity: 1, x: 0 }}
                            exit={{ opacity: 0, x: -20 }}
                            transition={{ duration: 0.2 }}
                        >
                            {activeTab === "general" && (
                                <Card className="border-border/50 bg-card/50 backdrop-blur-sm">
                                    <CardHeader>
                                        <CardTitle>General Settings</CardTitle>
                                        <CardDescription>System-wide configurations.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-6">
                                        <div className="space-y-2">
                                            <Label>Application Name</Label>
                                            <Input
                                                value={settings.appName}
                                                onChange={(e) => setSettings({ ...settings, appName: e.target.value })}
                                            />
                                        </div>
                                        <div className="space-y-2">
                                            <Label>Support Email</Label>
                                            <div className="relative">
                                                <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                                <Input
                                                    className="pl-9"
                                                    value={settings.supportEmail}
                                                    onChange={(e) => setSettings({ ...settings, supportEmail: e.target.value })}
                                                />
                                            </div>
                                        </div>
                                    </CardContent>
                                    <CardFooter>
                                        <Button onClick={handleSave} disabled={isLoading}>
                                            {isLoading ? "Saving..." : "Save Changes"}
                                        </Button>
                                    </CardFooter>
                                </Card>
                            )}

                            {activeTab === "appearance" && (
                                <Card className="border-border/50 bg-card/50 backdrop-blur-sm">
                                    <CardHeader>
                                        <CardTitle>Appearance</CardTitle>
                                        <CardDescription>Customize how the dashboard looks and feels.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-6">
                                        <div className="flex items-center justify-between p-4 border rounded-lg bg-card/50">
                                            <div className="space-y-0.5">
                                                <Label className="text-base">Theme Preference</Label>
                                                <p className="text-sm text-muted-foreground">
                                                    Toggle between light and dark modes.
                                                </p>
                                            </div>
                                            <div className="flex items-center gap-2 border p-1 rounded-full">
                                                <Button
                                                    variant={theme === 'light' ? 'default' : 'ghost'}
                                                    size="sm"
                                                    className="rounded-full h-8"
                                                    onClick={() => setTheme("light")}
                                                >
                                                    Light
                                                </Button>
                                                <Button
                                                    variant={theme === 'dark' ? 'default' : 'ghost'}
                                                    size="sm"
                                                    className="rounded-full h-8"
                                                    onClick={() => setTheme("dark")}
                                                >
                                                    Dark
                                                </Button>
                                                <Button
                                                    variant={theme === 'system' ? 'default' : 'ghost'}
                                                    size="sm"
                                                    className="rounded-full h-8"
                                                    onClick={() => setTheme("system")}
                                                >
                                                    System
                                                </Button>
                                            </div>
                                        </div>

                                        <div className="flex items-center justify-between p-4 border rounded-lg bg-card/50">
                                            <div className="space-y-0.5">
                                                <Label className="text-base">Reduce Animations</Label>
                                                <p className="text-sm text-muted-foreground">
                                                    Turn off complex animations for better performance.
                                                </p>
                                            </div>
                                            <Switch
                                                checked={!settings.appearance.animations}
                                                onCheckedChange={(c) => setSettings({
                                                    ...settings,
                                                    appearance: { ...settings.appearance, animations: !c }
                                                })}
                                            />
                                        </div>
                                    </CardContent>
                                </Card>
                            )}

                            {activeTab === "notifications" && (
                                <Card className="border-border/50 bg-card/50 backdrop-blur-sm">
                                    <CardHeader>
                                        <CardTitle>Notification Preferences</CardTitle>
                                        <CardDescription>Control what alerts you receive.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-6">
                                        <div className="flex items-center justify-between p-4 border rounded-lg bg-card/50">
                                            <div className="space-y-0.5">
                                                <Label className="text-base">Email Notifications</Label>
                                                <p className="text-sm text-muted-foreground">
                                                    Receive daily summaries via email.
                                                </p>
                                            </div>
                                            <Switch
                                                checked={settings.notifications.email}
                                                onCheckedChange={(c) => setSettings({
                                                    ...settings,
                                                    notifications: { ...settings.notifications, email: c }
                                                })}
                                            />
                                        </div>
                                        <div className="flex items-center justify-between p-4 border rounded-lg bg-card/50">
                                            <div className="space-y-0.5">
                                                <Label className="text-base">Push Notifications</Label>
                                                <p className="text-sm text-muted-foreground">
                                                    Real-time alerts for critical events.
                                                </p>
                                            </div>
                                            <Switch
                                                checked={settings.notifications.push}
                                                onCheckedChange={(c) => setSettings({
                                                    ...settings,
                                                    notifications: { ...settings.notifications, push: c }
                                                })}
                                            />
                                        </div>
                                        <div className="flex items-center justify-between p-4 border rounded-lg bg-card/50">
                                            <div className="space-y-0.5">
                                                <Label className="text-base">Security Alerts</Label>
                                                <p className="text-sm text-muted-foreground">
                                                    Notify me about suspicious login attempts.
                                                </p>
                                            </div>
                                            <Switch
                                                checked={settings.notifications.security}
                                                disabled
                                            />
                                        </div>
                                    </CardContent>
                                </Card>
                            )}

                            {activeTab === "api" && (
                                <Card className="border-border/50 bg-card/50 backdrop-blur-sm">
                                    <CardHeader>
                                        <CardTitle>API Configuration</CardTitle>
                                        <CardDescription>Manage API keys and access tokens.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-6">
                                        <div className="space-y-2">
                                            <Label>Public API Key</Label>
                                            <div className="relative">
                                                <Input readOnly value="pk_live_51M...xYz" className="font-mono bg-muted/50" />
                                                <Button size="sm" variant="ghost" className="absolute right-1 top-1 h-8">Copy</Button>
                                            </div>
                                        </div>
                                        <div className="space-y-2">
                                            <Label>Secret Key</Label>
                                            <div className="relative">
                                                <Input type="password" readOnly value="sk_live_..." className="font-mono bg-muted/50" />
                                                <Button size="sm" variant="outline" className="absolute right-1 top-1 h-8 bg-background">Reveal</Button>
                                            </div>
                                        </div>
                                        <div className="pt-4">
                                            <Button variant="destructive" className="w-full sm:w-auto">Roll API Keys</Button>
                                        </div>
                                    </CardContent>
                                </Card>
                            )}
                        </motion.div>
                    </AnimatePresence>
                </div>
            </div>
        </div>
    );
}
