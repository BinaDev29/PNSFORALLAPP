import { motion } from "framer-motion";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import {
    CheckCircle2,
    ShieldCheck,
    Users,
    BarChart3,
    Webhook,
    Bell,
    Lock,
    Layers,
    Globe,
    KeyRound,
    DatabaseZap,
    FileSearch,
    BellRing,
    Rocket,
    CalendarClock,
    Mail,
    MessageSquare,
    Clock,
    Zap,
    History,
    LayoutDashboard,
    FileText,
    Smartphone,
    Activity,
    ArrowRight,
    AlertCircle,
} from "lucide-react";

const container = {
    hidden: { opacity: 0 },
    show: { opacity: 1, transition: { staggerChildren: 0.07 } },
};
const item = {
    hidden: { y: 20, opacity: 0 },
    show: { y: 0, opacity: 1 },
};

// ---- Already Built Features ----
const builtFeatures = [
    { icon: Mail, label: "Email Notifications via SMTP", desc: "Sends HTML emails using client app Gmail credentials through a background queue." },
    { icon: MessageSquare, label: "SMS Notifications via Twilio", desc: "Sends SMS messages to verified phone numbers using Twilio API integration." },
    { icon: FileText, label: "Email Template Engine", desc: "Create HTML templates with dynamic variables ({{UserName}}) and use them when sending notifications." },
    { icon: Smartphone, label: "Client Application Management", desc: "Register and manage multiple client applications with their SMTP credentials." },
    { icon: History, label: "Notification History", desc: "Every sent notification is logged with recipient, status (Sent/Failed/Queued), and timestamp." },
    { icon: LayoutDashboard, label: "Real-time Dashboard Statistics", desc: "Live stats for total notifications, success rate, pending, and failed — powered by SignalR." },
    { icon: Clock, label: "Background Queue Processing", desc: "Emails and SMS messages are queued and processed asynchronously so the UI is never blocked." },
    { icon: Zap, label: "JWT Authentication & Role-based Access", desc: "Secure login with JWT tokens. Admin role has exclusive access to System Health and Product Roadmap." },
    { icon: Activity, label: "System Health Monitoring", desc: "Admin-only real-time view of API, database, and email service health with latency metrics." },
    { icon: Layers, label: "Clean Architecture + CQRS", desc: "Built with Clean Architecture, MediatR CQRS pattern, and Repository + Unit of Work patterns." },
];

// ---- Future Work (Privacy-focused first, then others) ----
interface FutureItem {
    icon: React.ElementType;
    title: string;
    description: string;
    priority: "Critical" | "High" | "Medium" | "Low";
    status: "Planned" | "In Design" | "Research";
    eta: string;
    category: string;
    isPrivacy?: boolean;
}

const futureFeatures: FutureItem[] = [
    // Privacy & Security (highlighted)
    {
        icon: ShieldCheck,
        title: "Business Verification (KYB)",
        description: "Companies must submit a TIN number and business license during registration. Admin reviews and approves the documents before granting system access.",
        priority: "Critical",
        status: "Planned",
        eta: "Q3 2026",
        category: "Security & Privacy",
        isPrivacy: true,
    },
    {
        icon: Lock,
        title: "Admin Approval Workflow",
        description: "New registrations are placed in a 'Pending' state. A super-admin manually reviews and activates accounts after verifying business credentials.",
        priority: "Critical",
        status: "In Design",
        eta: "Q3 2026",
        category: "Security & Privacy",
        isPrivacy: true,
    },
    {
        icon: KeyRound,
        title: "API-level Ownership Enforcement",
        description: "Update and delete operations on the backend will validate that the requesting user is the resource owner. Currently only enforced on the frontend.",
        priority: "Critical",
        status: "In Design",
        eta: "Q3 2026",
        category: "Security & Privacy",
        isPrivacy: true,
    },
    {
        icon: FileSearch,
        title: "Recipient Data Masking",
        description: "In the admin history view, sensitive fields like recipient emails and phone numbers will be partially masked (e.g. k***@gmail.com, +251911****89).",
        priority: "High",
        status: "Planned",
        eta: "Q4 2026",
        category: "Security & Privacy",
        isPrivacy: true,
    },
    // User & Tenant
    {
        icon: Users,
        title: "Multi-Tenant Role Hierarchy",
        description: "Introduce Tenant Admin and Super Admin roles. Tenant Admins can only view and manage data within their own organization.",
        priority: "High",
        status: "In Design",
        eta: "Q3 2026",
        category: "Tenant Management",
        isPrivacy: false,
    },
    {
        icon: Globe,
        title: "Business Email Domain Restriction",
        description: "Prevent personal emails (Gmail, Yahoo, Outlook) from registering. Only verified business-domain emails will be accepted.",
        priority: "Medium",
        status: "Research",
        eta: "Q4 2026",
        category: "Tenant Management",
        isPrivacy: false,
    },
    {
        icon: BellRing,
        title: "OTP Email Verification at Registration",
        description: "A one-time code is sent to the registrant's business email to confirm ownership before completing account setup.",
        priority: "Medium",
        status: "Planned",
        eta: "Q4 2026",
        category: "Tenant Management",
        isPrivacy: false,
    },
    // Analytics
    {
        icon: BarChart3,
        title: "Per-Tenant Analytics Dashboard",
        description: "Each organization will have an isolated analytics view showing their own delivery rates, bounce rates, and engagement metrics.",
        priority: "Medium",
        status: "Planned",
        eta: "Q4 2026",
        category: "Analytics & Reporting",
        isPrivacy: false,
    },
    {
        icon: DatabaseZap,
        title: "Billing & Usage Reports",
        description: "Track monthly notification quota per organization. Generate downloadable PDF/CSV reports for billing and compliance.",
        priority: "Low",
        status: "Research",
        eta: "Q1 2027",
        category: "Analytics & Reporting",
        isPrivacy: false,
    },
    // Platform
    {
        icon: Webhook,
        title: "Webhook Delivery Confirmation",
        description: "After a notification is dispatched, the platform will call the client's configured webhook URL with the delivery status in real time.",
        priority: "Medium",
        status: "In Design",
        eta: "Q4 2026",
        category: "Platform",
        isPrivacy: false,
    },
    {
        icon: Bell,
        title: "Push Notifications via Firebase (FCM)",
        description: "Send push notifications to Android and iOS devices registered via device tokens using Firebase Cloud Messaging.",
        priority: "Medium",
        status: "Research",
        eta: "Q1 2027",
        category: "Platform",
        isPrivacy: false,
    },
    {
        icon: Layers,
        title: "SMS Template Engine",
        description: "Extend the current HTML email template system to support dynamic SMS templates with {{variable}} substitution.",
        priority: "Low",
        status: "Planned",
        eta: "Q1 2027",
        category: "Platform",
        isPrivacy: false,
    },
];

const priorityConfig = {
    Critical: { label: "Critical", className: "bg-red-500/15 text-red-500 border-red-500/30" },
    High:     { label: "High",     className: "bg-rose-500/15 text-rose-400 border-rose-500/30" },
    Medium:   { label: "Medium",   className: "bg-amber-500/15 text-amber-500 border-amber-500/30" },
    Low:      { label: "Low",      className: "bg-sky-500/15 text-sky-500 border-sky-500/30" },
};

const statusConfig = {
    "Planned":   { className: "bg-slate-500/15 text-slate-400 border-slate-500/30" },
    "In Design": { className: "bg-violet-500/15 text-violet-400 border-violet-500/30" },
    "Research":  { className: "bg-teal-500/15 text-teal-400 border-teal-500/30" },
};

const privacyFeatures = futureFeatures.filter(f => f.isPrivacy);
const otherFeatures   = futureFeatures.filter(f => !f.isPrivacy);

export default function FutureWorkPage() {
    return (
        <div className="space-y-12 pb-16">

            {/* ── Header ── */}
            <motion.div initial={{ opacity: 0, y: -16 }} animate={{ opacity: 1, y: 0 }} className="space-y-2">
                <div className="flex items-center gap-3">
                    <div className="p-2.5 rounded-xl bg-primary/15 border border-primary/25">
                        <Rocket className="w-6 h-6 text-primary" />
                    </div>
                    <div>
                        <h2 className="text-3xl font-black tracking-tight text-foreground uppercase">Product Roadmap</h2>
                        <p className="text-muted-foreground text-sm font-medium">
                            What's already built — and what comes next. <span className="text-primary font-bold">Admin View</span>
                        </p>
                    </div>
                </div>

                {/* counters */}
                <div className="grid grid-cols-3 gap-3 pt-2">
                    {[
                        { label: "Already Built",   value: builtFeatures.length,   color: "text-emerald-500", bg: "bg-emerald-500/10", icon: CheckCircle2 },
                        { label: "Future Features",  value: futureFeatures.length,  color: "text-primary",     bg: "bg-primary/10",     icon: Rocket },
                        { label: "Privacy Critical", value: privacyFeatures.length, color: "text-red-500",     bg: "bg-red-500/10",     icon: ShieldCheck },
                    ].map((s, i) => (
                        <motion.div key={i} initial={{ opacity: 0, scale: 0.95 }} animate={{ opacity: 1, scale: 1 }} transition={{ delay: i * 0.1 }}>
                            <Card className="border border-border bg-card/80 shadow-sm">
                                <CardContent className="p-4 flex items-center gap-3">
                                    <div className={`p-2 rounded-lg ${s.bg}`}><s.icon className={`w-5 h-5 ${s.color}`} /></div>
                                    <div>
                                        <p className={`text-2xl font-black ${s.color}`}>{s.value}</p>
                                        <p className="text-[10px] font-bold uppercase text-muted-foreground tracking-widest">{s.label}</p>
                                    </div>
                                </CardContent>
                            </Card>
                        </motion.div>
                    ))}
                </div>
            </motion.div>

            {/* ══ SECTION 1: Already Built ══ */}
            <section className="space-y-4">
                <div className="flex items-center gap-2 border-b border-border pb-2">
                    <CheckCircle2 className="w-4 h-4 text-emerald-500" />
                    <h3 className="text-xs font-black uppercase tracking-widest text-emerald-500">Already Implemented</h3>
                    <Badge className="ml-auto bg-emerald-500/15 text-emerald-500 border border-emerald-500/30 text-[10px] font-black">
                        {builtFeatures.length} features live
                    </Badge>
                </div>

                <motion.div variants={container} initial="hidden" animate="show" className="grid gap-3 md:grid-cols-2">
                    {builtFeatures.map((f, i) => (
                        <motion.div key={i} variants={item}>
                            <div className="flex items-start gap-3 p-3 rounded-xl border border-emerald-500/20 bg-emerald-500/5 hover:bg-emerald-500/10 transition-colors">
                                <div className="p-1.5 rounded-lg bg-emerald-500/15 shrink-0 mt-0.5">
                                    <f.icon className="w-4 h-4 text-emerald-500" />
                                </div>
                                <div>
                                    <div className="flex items-center gap-2">
                                        <p className="text-sm font-bold text-foreground">{f.label}</p>
                                        <CheckCircle2 className="w-3.5 h-3.5 text-emerald-500 shrink-0" />
                                    </div>
                                    <p className="text-xs text-muted-foreground mt-0.5 leading-relaxed">{f.desc}</p>
                                </div>
                            </div>
                        </motion.div>
                    ))}
                </motion.div>
            </section>

            {/* ══ SECTION 2: Privacy & Security (Critical Future Work) ══ */}
            <section className="space-y-4">
                <div className="flex items-center gap-2 border-b border-border pb-2">
                    <AlertCircle className="w-4 h-4 text-red-500" />
                    <h3 className="text-xs font-black uppercase tracking-widest text-red-500">Future Work — Privacy & Security (Priority)</h3>
                    <Badge className="ml-auto bg-red-500/15 text-red-500 border border-red-500/30 text-[10px] font-black">
                        Required for Production
                    </Badge>
                </div>

                <motion.div variants={container} initial="hidden" animate="show" className="grid gap-4 md:grid-cols-2">
                    {privacyFeatures.map((f, i) => (
                        <motion.div key={i} variants={item}>
                            <Card className="border border-red-500/25 bg-gradient-to-br from-red-500/10 to-rose-500/5 shadow-md hover:shadow-lg hover:-translate-y-0.5 transition-all h-full">
                                <CardHeader className="pb-3">
                                    <CardTitle className="flex items-start gap-3">
                                        <div className="p-2 rounded-lg bg-background/70 border border-border/60 shrink-0 mt-0.5">
                                            <f.icon className="w-4 h-4 text-red-400" />
                                        </div>
                                        <div className="flex-1">
                                            <span className="text-sm font-bold text-foreground block">{f.title}</span>
                                            <div className="flex flex-wrap gap-1.5 mt-2">
                                                <Badge variant="outline" className={`text-[9px] font-black uppercase px-2 py-0.5 border ${priorityConfig[f.priority].className}`}>
                                                    {f.priority}
                                                </Badge>
                                                <Badge variant="outline" className={`text-[9px] font-black uppercase px-2 py-0.5 border ${statusConfig[f.status].className}`}>
                                                    {f.status}
                                                </Badge>
                                                <Badge variant="outline" className="text-[9px] font-black px-2 py-0.5 border bg-background/50 text-muted-foreground border-border/50 flex items-center gap-1">
                                                    <CalendarClock className="w-2.5 h-2.5" />{f.eta}
                                                </Badge>
                                            </div>
                                        </div>
                                    </CardTitle>
                                </CardHeader>
                                <CardContent className="pt-0">
                                    <p className="text-xs text-muted-foreground leading-relaxed">{f.description}</p>
                                </CardContent>
                            </Card>
                        </motion.div>
                    ))}
                </motion.div>
            </section>

            {/* ══ SECTION 3: Other Future Features ══ */}
            <section className="space-y-4">
                <div className="flex items-center gap-2 border-b border-border pb-2">
                    <ArrowRight className="w-4 h-4 text-primary" />
                    <h3 className="text-xs font-black uppercase tracking-widest text-primary">Future Work — Platform Enhancements</h3>
                    <Badge className="ml-auto bg-primary/15 text-primary border border-primary/30 text-[10px] font-black">
                        {otherFeatures.length} planned
                    </Badge>
                </div>

                <motion.div variants={container} initial="hidden" animate="show" className="grid gap-4 md:grid-cols-2">
                    {otherFeatures.map((f, i) => (
                        <motion.div key={i} variants={item}>
                            <Card className="border border-border bg-card/80 hover:bg-muted/20 shadow-sm hover:shadow-md hover:-translate-y-0.5 transition-all h-full">
                                <CardHeader className="pb-3">
                                    <CardTitle className="flex items-start gap-3">
                                        <div className="p-2 rounded-lg bg-muted/60 border border-border/60 shrink-0 mt-0.5">
                                            <f.icon className="w-4 h-4 text-muted-foreground" />
                                        </div>
                                        <div className="flex-1">
                                            <span className="text-sm font-bold text-foreground block">{f.title}</span>
                                            <div className="flex flex-wrap gap-1.5 mt-2">
                                                <Badge variant="outline" className={`text-[9px] font-black uppercase px-2 py-0.5 border ${priorityConfig[f.priority].className}`}>
                                                    {f.priority}
                                                </Badge>
                                                <Badge variant="outline" className={`text-[9px] font-black uppercase px-2 py-0.5 border ${statusConfig[f.status].className}`}>
                                                    {f.status}
                                                </Badge>
                                                <Badge variant="outline" className="text-[9px] font-black px-2 py-0.5 border bg-background/50 text-muted-foreground border-border/50 flex items-center gap-1">
                                                    <CalendarClock className="w-2.5 h-2.5" />{f.eta}
                                                </Badge>
                                                <Badge variant="outline" className="text-[9px] font-black uppercase px-2 py-0.5 border bg-muted/30 text-muted-foreground border-border/40">
                                                    {f.category}
                                                </Badge>
                                            </div>
                                        </div>
                                    </CardTitle>
                                </CardHeader>
                                <CardContent className="pt-0">
                                    <p className="text-xs text-muted-foreground leading-relaxed">{f.description}</p>
                                </CardContent>
                            </Card>
                        </motion.div>
                    ))}
                </motion.div>
            </section>

        </div>
    );
}
