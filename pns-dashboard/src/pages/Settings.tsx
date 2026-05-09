import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";
import { Bell, Globe, Mail, Palette, Wifi, Code, Terminal, Copy, Shield, ChevronRight, Loader2, ArrowRight } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { useState } from "react";
import { toast } from "sonner";
import { motion, AnimatePresence } from "framer-motion";
import { cn } from "@/lib/utils";
import { useTheme } from "@/components/theme-provider";

const container = {
    hidden: { opacity: 0 },
    show: {
        opacity: 1,
        transition: { staggerChildren: 0.1 }
    }
};

const item = {
    hidden: { x: -20, opacity: 0 },
    show: { x: 0, opacity: 1 }
};

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
        await new Promise(resolve => setTimeout(resolve, 1000));
        setIsLoading(false);
        toast.success("Settings saved successfully");
    };

    const tabs = [
        { id: "general", label: "General Settings", icon: Globe, description: "System name & support" },
        { id: "appearance", label: "Appearance", icon: Palette, description: "Themes & animations" },
        { id: "notifications", label: "Notifications", icon: Bell, description: "Alert preferences" },
        { id: "api", label: "API & Integrations", icon: Wifi, description: "Keys & authentication" },
        { id: "docs", label: "Documentation", icon: Code, description: "Developer guide" },
    ];

    return (
        <div className="space-y-8 pb-10">
            <div className="flex flex-col gap-2">
                <h2 className="text-4xl font-black tracking-tight bg-gradient-to-r from-blue-500 via-indigo-500 to-purple-500 bg-clip-text text-transparent uppercase">
                    System Configuration
                </h2>
                <p className="text-muted-foreground font-medium">
                    Manage your application ecosystem and developer credentials.
                </p>
            </div>

            <div className="flex flex-col lg:flex-row gap-8">
                {/* Modern Sidebar */}
                <motion.div 
                    variants={container}
                    initial="hidden"
                    animate="show"
                    className="w-full lg:w-80 flex-shrink-0 space-y-3"
                >
                    {tabs.map((tab) => (
                        <motion.button
                            key={tab.id}
                            variants={item}
                            onClick={() => setActiveTab(tab.id)}
                            className={cn(
                                "flex items-center justify-between w-full p-4 rounded-2xl text-left transition-all duration-300 border-2 group shadow-lg",
                                activeTab === tab.id
                                    ? "bg-primary text-primary-foreground border-primary shadow-primary/20 scale-[1.02]"
                                    : "bg-card text-muted-foreground border-border/40 hover:border-primary/40 hover:bg-muted"
                            )}
                        >
                            <div className="flex items-center gap-4">
                                <div className={cn(
                                    "p-2.5 rounded-xl ring-1",
                                    activeTab === tab.id ? "bg-white/20 ring-white/30" : "bg-slate-950 ring-white/5 group-hover:ring-primary/30"
                                )}>
                                    <tab.icon className="w-5 h-5" />
                                </div>
                                <div>
                                    <p className="font-black text-xs uppercase tracking-widest">{tab.label}</p>
                                    <p className={cn(
                                        "text-[10px] font-medium opacity-60",
                                        activeTab === tab.id ? "text-primary-foreground" : "text-muted-foreground"
                                    )}>{tab.description}</p>
                                </div>
                            </div>
                            <ChevronRight className={cn(
                                "w-4 h-4 transition-transform",
                                activeTab === tab.id ? "rotate-90 translate-x-1" : "opacity-0 group-hover:opacity-100"
                            )} />
                        </motion.button>
                    ))}
                </motion.div>

                {/* Content Box */}
                <div className="flex-1">
                    <AnimatePresence mode="wait">
                        <motion.div
                            key={activeTab}
                            initial={{ opacity: 0, y: 10 }}
                            animate={{ opacity: 1, y: 0 }}
                            exit={{ opacity: 0, y: -10 }}
                            transition={{ duration: 0.3 }}
                        >
                            {activeTab === "general" && (
                                <Card className="border-2 border-border bg-card shadow-2xl overflow-hidden">
                                    <div className="absolute top-0 left-0 w-1 h-full bg-blue-500" />
                                    <CardHeader className="border-b border-border/40 bg-muted/10">
                                        <CardTitle className="text-xl font-black text-foreground uppercase tracking-tight">General System Settings</CardTitle>
                                        <CardDescription className="text-muted-foreground font-medium">Core configuration for your PNS instance.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-6 pt-8">
                                        <div className="space-y-3 p-4 rounded-xl bg-muted/30 border-2 border-border/50">
                                            <Label className="text-[10px] font-black uppercase tracking-widest text-primary">Application Identification</Label>
                                            <Input
                                                value={settings.appName}
                                                onChange={(e) => setSettings({ ...settings, appName: e.target.value })}
                                                className="bg-muted/20 border-border/40 font-bold h-11 text-foreground"
                                            />
                                        </div>
                                        <div className="space-y-3 p-4 rounded-xl bg-muted/30 border-2 border-border/50">
                                            <Label className="text-[10px] font-black uppercase tracking-widest text-primary">System Support Channel</Label>
                                            <div className="relative">
                                                <Mail className="absolute left-3 top-3.5 h-4 w-4 text-muted-foreground" />
                                                <Input
                                                    className="pl-10 bg-muted/20 border-border/40 font-bold h-11 text-foreground"
                                                    value={settings.supportEmail}
                                                    onChange={(e) => setSettings({ ...settings, supportEmail: e.target.value })}
                                                />
                                            </div>
                                        </div>
                                    </CardContent>
                                    <CardFooter className="bg-muted/5 border-t border-border/40 p-6">
                                        <Button onClick={handleSave} disabled={isLoading} className="bg-primary hover:bg-primary/90 font-black uppercase tracking-widest px-8 shadow-lg shadow-primary/20">
                                            {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                            Save All Changes
                                        </Button>
                                    </CardFooter>
                                </Card>
                            )}

                            {activeTab === "appearance" && (
                                <Card className="border-2 border-border bg-card shadow-2xl overflow-hidden">
                                    <div className="absolute top-0 left-0 w-1 h-full bg-purple-500" />
                                    <CardHeader className="border-b border-border/40 bg-muted/10">
                                        <CardTitle className="text-xl font-black text-foreground uppercase tracking-tight text-purple-500">Visual Experience</CardTitle>
                                        <CardDescription className="text-muted-foreground font-medium">Control the interface look and performance.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-6 pt-8">
                                        <div className="flex flex-col sm:flex-row sm:items-center justify-between p-6 border-2 border-border/50 rounded-2xl bg-muted/30 gap-4">
                                            <div className="space-y-1">
                                                <Label className="text-sm font-black text-foreground">System Color Mode</Label>
                                                <p className="text-[10px] text-muted-foreground font-bold uppercase tracking-tighter">Choose your preferred visual atmosphere</p>
                                            </div>
                                            <div className="flex items-center gap-1.5 bg-muted/30 p-1.5 rounded-full border-2 border-border/40 shadow-inner">
                                                {['light', 'dark', 'system'].map((m) => (
                                                    <Button
                                                        key={m}
                                                        variant={theme === m ? 'default' : 'ghost'}
                                                        size="sm"
                                                        className={cn(
                                                            "rounded-full h-8 px-4 text-[10px] font-black uppercase",
                                                            theme === m && "shadow-lg"
                                                        )}
                                                        onClick={() => setTheme(m as any)}
                                                    >
                                                        {m}
                                                    </Button>
                                                ))}
                                            </div>
                                        </div>

                                        <div className="flex items-center justify-between p-6 border-2 border-border/50 rounded-2xl bg-muted/30">
                                            <div className="space-y-1">
                                                <Label className="text-sm font-black text-foreground">Performance Mode</Label>
                                                <p className="text-[10px] text-muted-foreground font-bold uppercase tracking-tighter">Reduce UI animations for faster rendering</p>
                                            </div>
                                            <Switch
                                                checked={!settings.appearance.animations}
                                                onCheckedChange={(c) => setSettings({
                                                    ...settings,
                                                    appearance: { ...settings.appearance, animations: !c }
                                                })}
                                                className="data-[state=checked]:bg-emerald-500"
                                            />
                                        </div>
                                    </CardContent>
                                </Card>
                            )}

                            {activeTab === "notifications" && (
                                <Card className="border-2 border-border bg-card shadow-2xl overflow-hidden">
                                    <div className="absolute top-0 left-0 w-1 h-full bg-rose-500" />
                                    <CardHeader className="border-b border-border/40 bg-muted/10">
                                        <CardTitle className="text-xl font-black text-foreground uppercase tracking-tight text-rose-500">Communication Prefs</CardTitle>
                                        <CardDescription className="text-muted-foreground font-medium">Set how the system talks back to you.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-4 pt-8">
                                        {[
                                            { key: 'email', label: 'Email Summaries', desc: 'Daily activity reports via email', icon: Mail },
                                            { key: 'push', label: 'Push Alerts', desc: 'Instant browser notifications', icon: Bell },
                                            { key: 'security', label: 'Security Locks', desc: 'Locked critical security alerts', icon: Shield, locked: true },
                                        ].map((item) => (
                                            <div key={item.key} className="flex items-center justify-between p-5 border-2 border-border/50 rounded-2xl bg-muted/30 group hover:border-rose-500/30 transition-all">
                                                <div className="flex items-center gap-4">
                                                    <div className="p-2 rounded-xl bg-card border border-border group-hover:scale-110 transition-transform">
                                                        <item.icon className="w-5 h-5 text-rose-500" />
                                                    </div>
                                                    <div>
                                                        <p className="text-[10px] text-muted-foreground font-bold uppercase">{item.desc}</p>
                                                    </div>
                                                </div>
                                                <Switch
                                                    checked={(settings.notifications as any)[item.key]}
                                                    disabled={item.locked}
                                                    onCheckedChange={(c) => setSettings({
                                                        ...settings,
                                                        notifications: { ...settings.notifications, [item.key]: c }
                                                    })}
                                                />
                                            </div>
                                        ))}
                                    </CardContent>
                                </Card>
                            )}

                            {activeTab === "api" && (
                                <Card className="border-2 border-border bg-card shadow-2xl overflow-hidden">
                                    <div className="absolute top-0 left-0 w-1 h-full bg-emerald-500" />
                                    <CardHeader className="border-b border-border/40 bg-muted/10">
                                        <CardTitle className="text-xl font-black text-foreground uppercase tracking-tight text-emerald-500">Access & Integration</CardTitle>
                                        <CardDescription className="text-muted-foreground font-medium">Manage your secret keys and authentication.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-6 pt-8">
                                        <div className="space-y-3 p-5 rounded-2xl bg-muted/30 border-2 border-border/40">
                                            <div className="flex items-center justify-between">
                                                <Label className="text-[10px] font-black uppercase tracking-widest text-emerald-500">Master API Key</Label>
                                                <Badge variant="outline" className="text-[8px] bg-emerald-500/10 text-emerald-500 border-emerald-500/20">LIVE</Badge>
                                            </div>
                                            <div className="relative group">
                                                <Input readOnly value="pk_live_51M...xYz" className="font-mono bg-muted/20 border-border/40 h-11 pr-12 text-foreground" />
                                                <Button 
                                                    size="icon" 
                                                    variant="ghost" 
                                                    className="absolute right-1 top-1.5 h-8 w-8 hover:bg-muted" 
                                                    onClick={() => {
                                                        navigator.clipboard.writeText("pk_live_51M...xYz");
                                                        toast.success("Public key copied");
                                                    }}
                                                >
                                                    <Copy className="h-4 w-4" />
                                                </Button>
                                            </div>
                                        </div>
                                        <div className="p-4 rounded-xl bg-emerald-500/5 border-2 border-emerald-500/20 flex gap-4">
                                            <div className="p-3 rounded-full bg-emerald-500/10 h-fit">
                                                <Terminal className="w-5 h-5 text-emerald-500" />
                                            </div>
                                            <div className="space-y-1">
                                                <h4 className="text-sm font-black text-foreground uppercase tracking-tight">Security Protocol</h4>
                                                <p className="text-[10px] text-muted-foreground leading-relaxed font-medium">
                                                    Store your secret keys in a secure server-side environment. Never expose keys in client-side codebases or public repositories.
                                                </p>
                                            </div>
                                        </div>
                                        <Button variant="destructive" className="w-full font-black uppercase tracking-widest shadow-xl shadow-destructive/10">Rotate Credentials</Button>
                                    </CardContent>
                                </Card>
                            )}

                            {activeTab === "docs" && (
                                <Card className="border-2 border-border bg-card shadow-2xl overflow-hidden">
                                    <div className="absolute top-0 left-0 w-1 h-full bg-blue-500" />
                                    <CardHeader className="border-b border-border/40 bg-muted/10">
                                        <CardTitle className="text-xl font-black text-foreground uppercase tracking-tight text-blue-500">Developer Handbook</CardTitle>
                                        <CardDescription className="text-muted-foreground font-medium">How to connect and scale with PNS.</CardDescription>
                                    </CardHeader>
                                    <CardContent className="space-y-8 pt-8">
                                        <div className="space-y-6">
                                            <div className="space-y-3">
                                                <h3 className="text-sm font-black text-foreground flex items-center gap-2 uppercase tracking-tight">
                                                    <div className="w-2 h-2 rounded-full bg-blue-500" /> Standard Endpoint
                                                </h3>
                                                <div className="flex items-center gap-3 p-4 rounded-xl bg-muted/30 border-2 border-border/50">
                                                    <span className="px-3 py-1 rounded-full bg-blue-500/10 text-blue-500 text-[10px] font-black border border-blue-500/30">POST</span>
                                                    <code className="text-xs font-mono text-foreground">/api/Notification/send</code>
                                                </div>
                                            </div>

                                            <div className="space-y-3">
                                                <Label className="text-[10px] font-black uppercase tracking-widest text-muted-foreground">Payload Architecture</Label>
                                                <div className="relative group">
                                                    <pre className="p-5 rounded-2xl bg-muted border-2 border-border/50 text-[11px] overflow-x-auto font-mono text-foreground leading-relaxed shadow-inner">
                                                        {`{
  "title": "Alert Title",
  "message": "Notification message body",
  "to": ["user@example.com"],
  "appId": "CLIENT_APP_ID"
}`}
                                                    </pre>
                                                    <Button size="icon" variant="ghost" className="absolute right-3 top-3 h-8 w-8 opacity-0 group-hover:opacity-100 transition-opacity">
                                                        <Copy className="w-4 h-4" />
                                                    </Button>
                                                </div>
                                            </div>
                                            <div className="p-4 rounded-2xl bg-slate-900 border-2 border-border/40 flex items-center justify-between group cursor-pointer hover:border-blue-500/30 transition-all">
                                                <div className="flex items-center gap-3">
                                                    <div className="p-2 rounded-lg bg-blue-500/10">
                                                        <Code className="w-4 h-4 text-blue-500" />
                                                    </div>
                                                    <span className="text-xs font-black uppercase text-white tracking-tight">Full SDK Documentation</span>
                                                </div>
                                                <ArrowRight className="w-4 h-4 text-muted-foreground group-hover:translate-x-1 transition-transform" />
                                            </div>
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
