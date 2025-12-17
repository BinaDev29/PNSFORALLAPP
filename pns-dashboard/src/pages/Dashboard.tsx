import { Bell, CheckCircle, AlertTriangle, Smartphone, TrendingUp, Activity, ArrowRight, Loader2, Download } from "lucide-react";
import { useTranslation } from '@/i18n';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { motion } from "framer-motion";
import { useQuery } from "@tanstack/react-query";
import { DashboardService, NotificationStatistics, ClientApplication, NotificationHistory } from "@/services/api";
import { formatDate } from "@/lib/utils";
import { CreateNotificationDialog } from "@/components/shared/CreateNotificationDialog";
import { useMemo } from "react";
import { Link } from "react-router-dom";

const container = {
    hidden: { opacity: 0 },
    show: {
        opacity: 1,
        transition: {
            staggerChildren: 0.1
        }
    }
};

const item = {
    hidden: { y: 20, opacity: 0 },
    show: { y: 0, opacity: 1 }
};

export default function Dashboard() {
    const { t } = useTranslation();

    const { data: statsData, isLoading: statsLoading } = useQuery<NotificationStatistics>({
        queryKey: ['dashboardStats'],
        queryFn: () => DashboardService.getStatistics()
    });

    const { data: clientsData, isLoading: clientsLoading } = useQuery<ClientApplication[]>({
        queryKey: ['activeClients'],
        queryFn: DashboardService.getClientApplications
    });

    const { data: recentActivity, isLoading: activityLoading } = useQuery<NotificationHistory[]>({
        queryKey: ['recentActivity'],
        queryFn: DashboardService.getRecentActivity
    });

    const { data: allHistory } = useQuery<NotificationHistory[]>({
        queryKey: ['chartData'],
        queryFn: DashboardService.getNotificationHistory
    });

    const chartData = useMemo(() => {
        if (!allHistory) return [];

        const last7Days = Array.from({ length: 7 }, (_, i) => {
            const d = new Date();
            d.setDate(d.getDate() - i);
            return d.toISOString().split('T')[0];
        }).reverse();

        return last7Days.map(date => {
            const dayData = allHistory.filter(h => h.sentDate.startsWith(date));
            return {
                name: new Date(date).toLocaleDateString('en-US', { weekday: 'short' }),
                total: dayData.length,
                success: dayData.filter(h => h.status === 'Sent' || h.status === 'Delivered').length,
                failed: dayData.filter(h => h.status === 'Failed').length
            };
        });
    }, [allHistory]);


    const isLoading = statsLoading || clientsLoading || activityLoading;

    const stats = [
        {
            title: t('dashboard.totalNotifications', "Total Notifications"),
            value: statsData?.totalRequests.toLocaleString() || "0",
            change: statsData && statsData.totalRequests > 0 ? Math.round((statsData.sent / statsData.totalRequests) * 100) + "%" : "0%",
            changeLabel: "delivery rate",
            icon: Bell,
            color: "text-purple-500",
            bg: "bg-purple-500/10",
            gradient: "from-purple-500/20 to-transparent"
        },
        {
            title: t('dashboard.successRate', "Success Rate"),
            value: statsData?.successRate ? `${statsData.successRate.toFixed(1)}%` : "0%",
            change: "+0.0%",
            changeLabel: "stable",
            icon: CheckCircle,
            color: "text-emerald-500",
            bg: "bg-emerald-500/10",
            gradient: "from-emerald-500/20 to-transparent"
        },
        {
            title: t('dashboard.failedMessages', "Failed Messages"),
            value: statsData?.failed.toLocaleString() || "0",
            change: statsData?.failed && statsData.totalRequests ? Math.round((statsData.failed / statsData.totalRequests) * 100) + "%" : "0%",
            changeLabel: "failure rate",
            icon: AlertTriangle,
            color: "text-rose-500",
            bg: "bg-rose-500/10",
            gradient: "from-rose-500/20 to-transparent"
        },
        {
            title: t('dashboard.activeClients', "Active Clients"),
            value: clientsData?.length.toString() || "0",
            change: "+",
            changeLabel: "active apps",
            icon: Smartphone,
            color: "text-blue-500",
            bg: "bg-blue-500/10",
            gradient: "from-blue-500/20 to-transparent"
        },
    ];

    if (isLoading) {
        return (
            <div className="flex h-screen items-center justify-center bg-background/50 backdrop-blur-sm">
                <Loader2 className="w-10 h-10 animate-spin text-primary" />
            </div>
        );
    }

    const handleDownloadReport = () => {
        if (!allHistory) return;

        const headers = ["ID", "Recipient", "Subject", "Status", "Sent Date", "Message"];
        const csvContent = [
            headers.join(","),
            ...allHistory.map(row => [
                row.id,
                `"${row.recipient}"`,
                `"${row.subject}"`,
                row.status,
                row.sentDate,
                `"${row.message?.replace(/"/g, '""') || ''}"` // Escape quotes
            ].join(","))
        ].join("\n");

        const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
        const link = document.createElement("a");
        const url = URL.createObjectURL(blob);
        link.setAttribute("href", url);
        link.setAttribute("download", `notification_report_${new Date().toISOString().split('T')[0]}.csv`);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    };

    return (
        <motion.div
            variants={container}
            initial="hidden"
            animate="show"
            className="p-8 space-y-8 max-w-7xl mx-auto"
        >
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-bold tracking-tight bg-gradient-to-r from-primary to-primary/60 bg-clip-text text-transparent">
                        {t('dashboard.overview', "Dashboard Overview")}
                    </h2>
                    <p className="text-muted-foreground">Welcome back to your notification control center.</p>
                </div>
                <div className="flex items-center gap-2">
                    <Button variant="outline" className="gap-2 hidden sm:flex" onClick={handleDownloadReport} disabled={!allHistory || allHistory.length === 0}>
                        <Download className="w-4 h-4" />
                        Download Report
                    </Button>
                    <CreateNotificationDialog />
                </div>
            </div>

            <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
                {stats.map((stat, index) => (
                    <motion.div key={index} variants={item}>
                        <Card className="border-border/50 bg-card/40 backdrop-blur-sm hover:bg-card/60 transition-colors duration-300 overflow-hidden relative group">
                            <div className={`absolute inset-0 bg-gradient-to-br ${stat.gradient} opacity-0 group-hover:opacity-100 transition-opacity duration-500`} />
                            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2 relative z-10">
                                <CardTitle className="text-sm font-medium text-muted-foreground">
                                    {stat.title}
                                </CardTitle>
                                <div className={`p-2 rounded-xl ${stat.bg} ${stat.color} ring-1 ring-black/5 dark:ring-white/10`}>
                                    <stat.icon className="h-4 w-4" />
                                </div>
                            </CardHeader>
                            <CardContent className="relative z-10">
                                <div className="text-2xl font-bold tracking-tight">{stat.value}</div>
                                <p className="text-xs text-muted-foreground mt-1 flex items-center gap-1">
                                    <span className={stat.change.includes('+') ? "text-emerald-500 font-medium" : "text-rose-500 font-medium"}>
                                        {stat.change}
                                    </span>
                                    <span className="opacity-80">from last month</span>
                                </p>
                            </CardContent>
                        </Card>
                    </motion.div>
                ))}
            </div>

            <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-7">
                <motion.div variants={item} className="col-span-4">
                    <Card className="h-full border-border/50 bg-card/40 backdrop-blur-sm shadow-sm">
                        <CardHeader>
                            <div className="flex items-center justify-between">
                                <div>
                                    <CardTitle className="flex items-center gap-2">
                                        <Activity className="w-5 h-5 text-indigo-500" />
                                        <span>Message Volume</span>
                                    </CardTitle>
                                    <CardDescription>
                                        Daily notification traffic over the last 7 days
                                    </CardDescription>
                                </div>
                            </div>
                        </CardHeader>
                        <CardContent className="pl-0">
                            <div className="h-[300px] w-full mt-4">
                                <ResponsiveContainer width="100%" height="100%">
                                    <AreaChart data={chartData}>
                                        <defs>
                                            <linearGradient id="colorTotal" x1="0" y1="0" x2="0" y2="1">
                                                <stop offset="5%" stopColor="#8884d8" stopOpacity={0.3} />
                                                <stop offset="95%" stopColor="#8884d8" stopOpacity={0} />
                                            </linearGradient>
                                            <linearGradient id="colorSuccess" x1="0" y1="0" x2="0" y2="1">
                                                <stop offset="5%" stopColor="#10b981" stopOpacity={0.3} />
                                                <stop offset="95%" stopColor="#10b981" stopOpacity={0} />
                                            </linearGradient>
                                        </defs>
                                        <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="hsl(var(--border))" opacity={0.5} />
                                        <XAxis
                                            dataKey="name"
                                            stroke="#888888"
                                            fontSize={12}
                                            tickLine={false}
                                            axisLine={false}
                                            tickMargin={10}
                                        />
                                        <YAxis
                                            stroke="#888888"
                                            fontSize={12}
                                            tickLine={false}
                                            axisLine={false}
                                            tickFormatter={(value) => `${value}`}
                                        />
                                        <Tooltip
                                            contentStyle={{
                                                backgroundColor: 'hsl(var(--card))',
                                                borderColor: 'hsl(var(--border))',
                                                borderRadius: '8px',
                                                boxShadow: '0 4px 12px rgba(0,0,0,0.1)'
                                            }}
                                            itemStyle={{ color: 'hsl(var(--foreground))' }}
                                        />
                                        <Area
                                            type="monotone"
                                            dataKey="total"
                                            stroke="#8884d8"
                                            strokeWidth={2}
                                            fillOpacity={1}
                                            fill="url(#colorTotal)"
                                            name="Total"
                                        />
                                        <Area
                                            type="monotone"
                                            dataKey="success"
                                            stroke="#10b981"
                                            strokeWidth={2}
                                            fillOpacity={1}
                                            fill="url(#colorSuccess)"
                                            name="Success"
                                        />
                                    </AreaChart>
                                </ResponsiveContainer>
                            </div>
                        </CardContent>
                    </Card>
                </motion.div>

                <motion.div variants={item} className="col-span-3">
                    <Card className="h-full border-border/50 bg-card/40 backdrop-blur-sm shadow-sm flex flex-col">
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <TrendingUp className="w-5 h-5 text-blue-500" />
                                <span>Recent Activity</span>
                            </CardTitle>
                            <CardDescription>
                                Real-time updates from your system
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="flex-1">
                            <div className="space-y-8 relative before:absolute before:inset-y-0 before:left-2.5 before:w-0.5 before:bg-border/50 pl-2">
                                {recentActivity?.map((item) => (
                                    <div key={item.id} className="flex gap-4 relative group">
                                        <span className={`absolute left-0 top-1.5 w-5 h-5 rounded-full border-4 border-background shadow-sm z-10 group-hover:scale-110 transition-transform duration-200 ${item.status === 'Sent' || item.status === 'Delivered' ? 'bg-emerald-500' : 'bg-amber-500'}`} />
                                        <div className="flex-1 -mt-1 p-3 rounded-xl hover:bg-muted/50 transition-colors duration-200 cursor-pointer border border-transparent hover:border-border/50">
                                            <div className="flex justify-between items-start mb-1">
                                                <p className="text-sm font-semibold leading-none">{item.status}</p>
                                                <span className="text-[10px] text-muted-foreground font-medium bg-secondary px-2 py-0.5 rounded-full">{formatDate(item.sentDate)}</span>
                                            </div>
                                            <p className="text-xs text-foreground/80 mb-1 truncate max-w-[200px]" title={item.recipient}>
                                                To: {item.recipient || <span className="text-muted-foreground italic">Unknown</span>}
                                            </p>
                                            <p className="text-xs text-muted-foreground mb-1 line-clamp-2" title={item.message}>
                                                {item.message || <span className="italic">No message</span>}
                                            </p>
                                            <p className="text-[10px] text-muted-foreground/60">
                                                ID: <span className="text-foreground/80 font-mono">{item.id.slice(0, 8)}...</span>
                                            </p>
                                        </div>
                                    </div>
                                ))}
                                {(!recentActivity || recentActivity.length === 0) && (
                                    <div className="text-center text-muted-foreground text-sm py-8">
                                        No recent activity found.
                                    </div>
                                )}
                            </div>
                        </CardContent>
                        <div className="p-4 border-t border-border/50 mt-auto">
                            <Link to="/history">
                                <Button variant="ghost" className="w-full text-xs hover:bg-primary/5 text-primary group">
                                    View Full History <ArrowRight className="w-3 h-3 ml-1 transition-transform group-hover:translate-x-1" />
                                </Button>
                            </Link>
                        </div>
                    </Card>
                </motion.div>
            </div>
        </motion.div>
    );
}
