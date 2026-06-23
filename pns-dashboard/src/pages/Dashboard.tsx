import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { 
    Download, 
    Bell, 
    ArrowDownRight, 
    Clock, 
    TrendingUp,
    Users,
    Zap,
    Mail
} from "lucide-react";
import { 
    AreaChart, 
    Area, 
    XAxis, 
    YAxis, 
    CartesianGrid, 
    Tooltip, 
    ResponsiveContainer
} from 'recharts';
import { DashboardService, NotificationStatistics, NotificationHistory } from "@/services/api";
import { useQuery } from "@tanstack/react-query";
import { formatDistanceToNow, subDays, format, isSameDay } from "date-fns";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";
import { Skeleton } from "@/components/ui/skeleton";
import { useMemo } from "react";

const formatDate = (dateString: string) => {
    try {
        return formatDistanceToNow(new Date(dateString), { addSuffix: true });
    } catch {
        return dateString;
    }
};

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
    const navigate = useNavigate();
    const { data: stats, isLoading: statsLoading } = useQuery<NotificationStatistics>({
        queryKey: ['dashboardStats'],
        queryFn: () => DashboardService.getStatistics(),
        refetchInterval: 5000 
    });

    const { data: recent, isLoading: recentLoading } = useQuery<NotificationHistory[]>({
        queryKey: ['recentActivity'],
        queryFn: DashboardService.getRecentActivity,
        refetchInterval: 5000 
    });

    // Derive chart data from notification history for the last 7 days
    const chartData = useMemo(() => {
        if (!recent) return [];
        
        const days = Array.from({ length: 7 }, (_, i) => subDays(new Date(), 6 - i));
        
        return days.map(day => {
            const count = recent.filter(n => isSameDay(new Date(n.sentDate), day)).length;
            return {
                name: format(day, 'EEE'),
                total: count
            };
        });
    }, [recent]);

    const handleDownloadReport = () => {
        if (!recent) return;
        const csv = [
            ['Date', 'To', 'Status', 'Message'],
            ...recent.map(n => [n.sentDate, n.to, n.status, n.message])
        ].map(e => e.join(",")).join("\n");
        
        const blob = new Blob([csv], { type: 'text/csv' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `pns-report-${new Date().toISOString().split('T')[0]}.csv`;
        a.click();
    };

    return (
        <div className="space-y-8 pb-10">
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-black tracking-tight text-foreground uppercase">
                        Dashboard Overview
                    </h2>
                    <p className="text-muted-foreground font-medium">Welcome back to your notification control center.</p>
                </div>
                <div className="flex items-center gap-3">
                    <Button 
                        onClick={handleDownloadReport}
                        variant="outline"
                        className="gap-2 border-2 border-border font-bold shadow-sm"
                    >
                        <Download className="h-4 w-4" /> Export CSV
                    </Button>
                    <Button 
                        onClick={() => navigate('/notifications')}
                        className="gap-2 bg-primary hover:bg-primary/90 text-primary-foreground shadow-xl border-b-4 border-black/20 active:border-b-0 active:translate-y-1 transition-all font-bold"
                    >
                        Send New
                    </Button>
                </div>
            </div>

            <motion.div 
                variants={container}
                initial="hidden"
                animate="show"
                className="grid gap-6 md:grid-cols-2 lg:grid-cols-4"
            >
                {[
                    { title: "Total Notifications", value: stats?.totalRequests ?? 0, icon: Bell, trend: "+12%", color: "text-primary", bg: "bg-primary/10" },
                    { title: "Success Rate", value: `${Math.round(stats?.successRate ?? 0)}%`, icon: Zap, trend: "+0.2%", color: "text-emerald-500", bg: "bg-emerald-500/10" },
                    { title: "Failed Messages", value: stats?.failed ?? 0, icon: ArrowDownRight, trend: "0%", color: "text-rose-500", bg: "bg-rose-500/10" },
                    { title: "Active Projects", value: 4, icon: Users, trend: "+2", color: "text-blue-500", bg: "bg-blue-500/10" }
                ].map((stat, i) => (
                    <motion.div key={i} variants={item}>
                        <Card className="border-2 border-border bg-card shadow-lg hover:shadow-xl transition-all duration-300 relative overflow-hidden group">
                            <div className={`absolute top-0 left-0 w-1 h-full ${stat.color.replace('text-', 'bg-')}`} />
                            <CardHeader className="flex flex-row items-center justify-between pb-2 space-y-0">
                                <CardTitle className="text-[10px] font-black uppercase tracking-widest text-muted-foreground">
                                    {stat.title}
                                </CardTitle>
                                <div className={`p-2 rounded-xl ${stat.bg} ${stat.color} group-hover:scale-110 transition-transform`}>
                                    <stat.icon className="h-4 w-4" />
                                </div>
                            </CardHeader>
                            <CardContent>
                                {statsLoading ? (
                                    <Skeleton className="h-9 w-20" />
                                ) : (
                                    <>
                                        <div className="text-3xl font-black tracking-tighter">{stat.value}</div>
                                        <div className="flex items-center gap-1.5 mt-1">
                                            <span className={`text-[10px] font-black ${stat.trend.startsWith('+') ? 'text-emerald-500' : 'text-rose-500'}`}>
                                                {stat.trend}
                                            </span>
                                            <span className="text-[10px] font-bold text-muted-foreground uppercase opacity-60">Performance</span>
                                        </div>
                                    </>
                                )}
                            </CardContent>
                        </Card>
                    </motion.div>
                ))}
            </motion.div>

            <div className="grid gap-6 lg:grid-cols-3">
                <Card className="lg:col-span-2 border-2 border-border bg-card shadow-xl overflow-hidden">
                    <CardHeader className="border-b border-border bg-muted/5">
                        <div className="flex items-center gap-2">
                            <TrendingUp className="h-5 w-5 text-primary" />
                            <div>
                                <CardTitle className="text-lg font-black uppercase">Message Volume</CardTitle>
                                <CardDescription className="font-medium">Daily real-time traffic for the last 7 days</CardDescription>
                            </div>
                        </div>
                    </CardHeader>
                    <CardContent className="pt-6">
                        <div className="h-[300px] min-h-[300px] w-full relative">
                            <ResponsiveContainer width="100%" height="100%">
                                <AreaChart data={chartData}>
                                    <defs>
                                        <linearGradient id="colorTotal" x1="0" y1="0" x2="0" y2="1">
                                            <stop offset="5%" stopColor="hsl(var(--primary))" stopOpacity={0.3}/>
                                            <stop offset="95%" stopColor="hsl(var(--primary))" stopOpacity={0}/>
                                        </linearGradient>
                                    </defs>
                                    <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="hsl(var(--border))" />
                                    <XAxis 
                                        dataKey="name" 
                                        axisLine={false} 
                                        tickLine={false} 
                                        tick={{ fontSize: 10, fontWeight: 700, fill: 'currentColor' }} 
                                        dy={10}
                                    />
                                    <YAxis 
                                        axisLine={false} 
                                        tickLine={false} 
                                        tick={{ fontSize: 10, fontWeight: 700, fill: 'currentColor' }} 
                                        domain={[0, 'auto']}
                                        allowDecimals={false}
                                    />
                                    <Tooltip 
                                        contentStyle={{ 
                                            backgroundColor: 'hsl(var(--card))', 
                                            borderColor: 'hsl(var(--border))', 
                                            borderRadius: '12px',
                                            fontSize: '12px',
                                            fontWeight: 'bold',
                                            boxShadow: '0 10px 15px -3px rgb(0 0 0 / 0.1)'
                                        }} 
                                    />
                                    <Area 
                                        type="monotone" 
                                        dataKey="total" 
                                        stroke="hsl(var(--primary))" 
                                        strokeWidth={3}
                                        fillOpacity={1} 
                                        fill="url(#colorTotal)" 
                                    />
                                </AreaChart>
                            </ResponsiveContainer>
                        </div>
                    </CardContent>
                </Card>

                <Card className="border-2 border-border bg-card shadow-xl overflow-hidden">
                    <CardHeader className="border-b border-border bg-muted/5">
                        <div className="flex items-center gap-2">
                            <Clock className="h-5 w-5 text-blue-500" />
                            <div>
                                <CardTitle className="text-lg font-black uppercase">Recent Activity</CardTitle>
                                <CardDescription className="font-medium">Real-time updates from your system</CardDescription>
                            </div>
                        </div>
                    </CardHeader>
                    <CardContent className="pt-6">
                        <div className="space-y-4">
                            {recentLoading ? (
                                Array.from({ length: 5 }).map((_, i) => (
                                    <Skeleton key={i} className="h-20 w-full rounded-xl" />
                                ))
                            ) : (
                                recent && recent.length > 0 ? (
                                    recent.slice(0, 6).map((item, i) => (
                                        <motion.div
                                            key={item.id}
                                            initial={{ opacity: 0, x: 20 }}
                                            animate={{ opacity: 1, x: 0 }}
                                            transition={{ delay: i * 0.05 }}
                                            className="p-4 rounded-xl border-2 border-border/50 hover:border-primary/30 bg-muted/20 hover:bg-muted/40 transition-all cursor-pointer group"
                                        >
                                            <div className="flex items-center justify-between mb-2">
                                                <div className="flex items-center gap-3">
                                                    <div className={`h-2.5 w-2.5 rounded-full ${item.status === 'Sent' ? 'bg-emerald-500 shadow-[0_0_8px_rgba(16,185,129,0.6)]' : 'bg-rose-500 animate-pulse'}`} />
                                                    <div className="flex items-center gap-1.5">
                                                        <Mail className="h-3 w-3 text-blue-500" />
                                                        <span className={`text-[11px] font-black uppercase ${item.status === 'Sent' ? 'text-emerald-500' : 'text-rose-500'}`}>{item.status}</span>
                                                    </div>
                                                </div>
                                                <span className="text-[10px] text-muted-foreground font-bold bg-background/50 px-2 py-0.5 rounded-full border border-border/40">
                                                    {formatDate(item.sentDate)}
                                                </span>
                                            </div>
                                            <p className="text-[11px] text-muted-foreground truncate font-medium">
                                                To: <span className="text-foreground font-black">{item.to || 'System'}</span>
                                            </p>
                                        </motion.div>
                                    ))
                                ) : (
                                    <div className="text-center py-10">
                                        <p className="text-sm text-muted-foreground font-bold italic">No recent activity found.</p>
                                    </div>
                                )
                            )}
                        </div>
                        <Button variant="ghost" className="w-full mt-4 text-xs font-black uppercase text-primary hover:bg-primary/5">
                            View Full History
                        </Button>
                    </CardContent>
                </Card>
            </div>
        </div>
    );
}
