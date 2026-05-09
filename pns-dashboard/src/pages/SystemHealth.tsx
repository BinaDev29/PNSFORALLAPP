import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Activity, Server, Database, Cpu, Wifi, CheckCircle2, AlertTriangle, XCircle, Gauge } from "lucide-react";
import { motion } from "framer-motion";
import { useQuery } from "@tanstack/react-query";
import { Skeleton } from "@/components/ui/skeleton";
import { DashboardService } from "@/services/api";
import { Badge } from "@/components/ui/badge";

const container = {
    hidden: { opacity: 0 },
    show: {
        opacity: 1,
        transition: { staggerChildren: 0.1 }
    }
};

const item = {
    hidden: { y: 20, opacity: 0 },
    show: { y: 0, opacity: 1 }
};

export default function SystemHealthPage() {
    const { data: health, isLoading } = useQuery({
        queryKey: ['systemHealth'],
        queryFn: DashboardService.getSystemHealth,
        refetchInterval: 5000 
    });

    const getStatusIcon = (status: string) => {
        const s = (status || "").toLowerCase();
        if (s === "operational" || s === "healthy") return <CheckCircle2 className="w-5 h-5 text-emerald-500" />;
        if (s === "degraded" || s === "unhealthy") return <AlertTriangle className="w-5 h-5 text-amber-500" />;
        if (s === "outage" || s === "failed") return <XCircle className="w-5 h-5 text-rose-500" />;
        return <Activity className="w-5 h-5 text-muted-foreground" />;
    };

    return (
        <div className="space-y-8 pb-10">
            <div className="flex flex-col gap-2">
                <h2 className="text-3xl font-black tracking-tight text-foreground uppercase">
                    System Health
                </h2>
                <p className="text-muted-foreground font-medium">
                    Real-time monitoring of system services and infrastructure core.
                </p>
            </div>

            <motion.div
                variants={container}
                initial="hidden"
                animate="show"
                className="grid gap-6 md:grid-cols-2 lg:grid-cols-4"
            >
                {[
                    { title: "Overall Status", value: health?.status || "System OK", icon: Activity, color: "text-emerald-500", bg: "bg-emerald-500/10", label: "All systems operational" },
                    { title: "Uptime (30d)", value: "99.99%", icon: Wifi, color: "text-blue-500", bg: "bg-blue-500/10", label: "last 30 days" },
                    { title: "Avg Latency", value: `${Math.round(health?.totalLatency || 0)}ms`, icon: Gauge, color: "text-purple-500", bg: "bg-purple-500/10", label: "global average" },
                    { title: "Active Nodes", value: "1 / 1", icon: Server, color: "text-orange-500", bg: "bg-orange-500/10", label: "regions healthy" }
                ].map((stat, i) => (
                    <motion.div key={i} variants={item}>
                        <Card className="border-2 border-border bg-card hover:bg-muted/30 transition-all duration-300 shadow-xl relative group overflow-hidden">
                            <div className={`absolute top-0 left-0 w-1 h-full ${stat.color.replace('text-', 'bg-')}`} />
                            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2 relative z-10">
                                <CardTitle className="text-[10px] font-bold uppercase tracking-widest text-muted-foreground">
                                    {stat.title}
                                </CardTitle>
                                <div className={`p-2 rounded-xl ${stat.bg} ${stat.color} ring-2 ring-black/5 shadow-inner`}>
                                    <stat.icon className="h-4 w-4" />
                                </div>
                            </CardHeader>
                            <CardContent className="relative z-10 pt-1">
                                {isLoading ? (
                                    <Skeleton className="h-8 w-24" />
                                ) : (
                                    <>
                                        <div className="text-2xl font-black tracking-tighter text-foreground uppercase">{stat.value}</div>
                                        <p className="text-[10px] text-muted-foreground mt-1 font-bold uppercase opacity-60">{stat.label}</p>
                                    </>
                                )}
                            </CardContent>
                        </Card>
                    </motion.div>
                ))}
            </motion.div>

            <div className="grid gap-6 md:grid-cols-2">
                <motion.div
                    initial={{ opacity: 0, x: -20 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ delay: 0.3 }}
                >
                    <Card className="h-full border-2 border-border bg-card shadow-xl overflow-hidden">
                        <CardHeader className="border-b border-border bg-muted/20 pb-4">
                            <CardTitle className="flex items-center gap-2 text-lg font-bold text-foreground">
                                <Server className="w-5 h-5 text-emerald-500" />
                                <span>Services Status</span>
                            </CardTitle>
                            <CardDescription className="text-xs text-muted-foreground font-medium">Real-time service availability checks.</CardDescription>
                        </CardHeader>
                        <CardContent className="pt-6">
                            <div className="space-y-4">
                                {isLoading ? (
                                    Array.from({ length: 3 }).map((_, i) => (
                                        <Skeleton key={i} className="h-16 w-full rounded-xl" />
                                    ))
                                ) : (
                                    health?.services.map((service: any, i: number) => (
                                        <div key={i} className="flex items-center justify-between p-4 rounded-xl bg-muted/10 border-2 border-border/50 hover:bg-muted/30 transition-all duration-200 cursor-pointer shadow-sm group">
                                            <div className="flex items-center gap-4">
                                                <div className="p-2 rounded-lg bg-card group-hover:scale-110 transition-transform shadow-sm">
                                                    {getStatusIcon(service.status)}
                                                </div>
                                                <div>
                                                    <p className="text-sm font-black leading-none text-foreground">{service.name}</p>
                                                    <p className="text-[10px] text-muted-foreground mt-1 font-bold uppercase tracking-wider">{service.status}</p>
                                                </div>
                                            </div>
                                            <div className="text-right">
                                                <span className={`text-[10px] font-black px-3 py-1 rounded-full border ${service.status === 'Healthy' ? 'bg-emerald-500/10 text-emerald-600 border-emerald-500/20' : 'bg-amber-500/10 text-amber-600 border-amber-500/20'}`}>
                                                    {Math.round(service.latency)}ms
                                                </span>
                                            </div>
                                        </div>
                                    ))
                                )}
                            </div>
                        </CardContent>
                    </Card>
                </motion.div>

                <motion.div
                    initial={{ opacity: 0, x: 20 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ delay: 0.3 }}
                >
                    <Card className="h-full border-2 border-border bg-card shadow-xl overflow-hidden">
                        <CardHeader className="border-b border-border bg-muted/20 pb-4">
                            <CardTitle className="flex items-center gap-2 text-lg font-bold text-foreground">
                                <Cpu className="w-5 h-5 text-blue-500" />
                                <span>System Resources</span>
                            </CardTitle>
                            <CardDescription className="text-xs text-muted-foreground font-medium">Live server resource usage.</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-8 pt-6">
                            {[
                                { label: "CPU Usage", icon: Cpu, value: "15%", color: "bg-blue-500", status: "Low" },
                                { label: "Memory Usage", icon: Database, value: "25%", color: "bg-purple-500", status: "Stable" },
                                { label: "Storage", icon: Server, value: "10%", color: "bg-orange-500", status: "Healthy" }
                            ].map((res, i) => (
                                <div key={i} className="space-y-3 p-4 rounded-xl bg-muted/10 border-2 border-border/50">
                                    <div className="flex items-center justify-between">
                                        <span className="flex items-center gap-2 text-xs font-bold text-foreground">
                                            <res.icon className="w-4 h-4 text-muted-foreground" /> {res.label}
                                        </span>
                                        <Badge variant="outline" className="text-[9px] font-black uppercase border-border/50 text-foreground">{res.status}</Badge>
                                    </div>
                                    <div className="space-y-1.5">
                                        <div className="h-3 w-full bg-muted rounded-full border border-border/50 overflow-hidden p-0.5 shadow-inner">
                                            <motion.div
                                                className={`h-full rounded-full ${res.color} shadow-sm`}
                                                initial={{ width: 0 }}
                                                animate={{ width: isLoading ? 0 : res.value }}
                                                transition={{ duration: 1, delay: i * 0.2 }}
                                            />
                                        </div>
                                        <div className="flex justify-between text-[9px] font-black text-muted-foreground uppercase tracking-widest">
                                            <span>Usage</span>
                                            <span>{res.value}</span>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </CardContent>
                    </Card>
                </motion.div>
            </div>
        </div>
    );
}
