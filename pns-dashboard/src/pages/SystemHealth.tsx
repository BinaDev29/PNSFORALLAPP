import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Activity, Server, Database, Cpu, Wifi, CheckCircle2, AlertTriangle, XCircle } from "lucide-react";
import { motion } from "framer-motion";
import { useQuery } from "@tanstack/react-query";
import { Skeleton } from "@/components/ui/skeleton";

// Mock service for health check
const getSystemHealth = async () => {
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1000));
    return {
        status: "healthy",
        uptime: "99.98%",
        latency: "24ms",
        services: [
            { name: "API Gateway", status: "operational", latency: "12ms" },
            { name: "Notification Engine", status: "operational", latency: "18ms" },
            { name: "Email Service", status: "operational", latency: "45ms" },
            { name: "Database (Primary)", status: "operational", latency: "5ms" },
            { name: "Cache Layer", status: "operational", latency: "2ms" },
            { name: "Storage Service", status: "degraded", latency: "120ms" }
        ],
        resources: {
            cpu: 45,
            memory: 62,
            storage: 28
        }
    };
};

export default function SystemHealthPage() {
    const { data: health, isLoading } = useQuery({
        queryKey: ['systemHealth'],
        queryFn: getSystemHealth,
        refetchInterval: 5000 // Real-time feel
    });


    const getStatusIcon = (status: string) => {
        switch (status) {
            case "operational": return <CheckCircle2 className="w-5 h-5 text-emerald-500" />;
            case "degraded": return <AlertTriangle className="w-5 h-5 text-amber-500" />;
            case "outage": return <XCircle className="w-5 h-5 text-rose-500" />;
            default: return <Activity className="w-5 h-5 text-muted-foreground" />;
        }
    };

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

    return (
        <div className="space-y-8">
            <div className="flex flex-col gap-2">
                <h2 className="text-3xl font-bold tracking-tight text-emerald-600">
                    System Health
                </h2>
                <p className="text-muted-foreground">
                    Real-time monitoring of system services and infrastructure.
                </p>
            </div>

            <motion.div
                variants={container}
                initial="hidden"
                animate="show"
                className="grid gap-6 md:grid-cols-2 lg:grid-cols-4"
            >
                <motion.div variants={item}>
                    <Card className="bg-card/50 backdrop-blur-sm border-border/50">
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">Overall Status</CardTitle>
                            <Activity className="h-4 w-4 text-emerald-500" />
                        </CardHeader>
                        <CardContent>
                            <div className="text-2xl font-bold text-emerald-500 uppercase tracking-wide">
                                {isLoading ? <Skeleton className="h-8 w-24" /> : health?.status}
                            </div>
                            <p className="text-xs text-muted-foreground">All systems operational</p>
                        </CardContent>
                    </Card>
                </motion.div>
                <motion.div variants={item}>
                    <Card className="bg-card/50 backdrop-blur-sm border-border/50">
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">Uptime (30d)</CardTitle>
                            <Wifi className="h-4 w-4 text-blue-500" />
                        </CardHeader>
                        <CardContent>
                            <div className="text-2xl font-bold">{isLoading ? <Skeleton className="h-8 w-24" /> : health?.uptime}</div>
                            <p className="text-xs text-muted-foreground">last incident: 42d ago</p>
                        </CardContent>
                    </Card>
                </motion.div>
                <motion.div variants={item}>
                    <Card className="bg-card/50 backdrop-blur-sm border-border/50">
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">Avg Latency</CardTitle>
                            <Activity className="h-4 w-4 text-purple-500" />
                        </CardHeader>
                        <CardContent>
                            <div className="text-2xl font-bold">{isLoading ? <Skeleton className="h-8 w-24" /> : health?.latency}</div>
                            <p className="text-xs text-muted-foreground">global average</p>
                        </CardContent>
                    </Card>
                </motion.div>
                <motion.div variants={item}>
                    <Card className="bg-card/50 backdrop-blur-sm border-border/50">
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">Active Nodes</CardTitle>
                            <Server className="h-4 w-4 text-orange-500" />
                        </CardHeader>
                        <CardContent>
                            <div className="text-2xl font-bold">{isLoading ? <Skeleton className="h-8 w-16" /> : "8/8"}</div>
                            <p className="text-xs text-muted-foreground">regions healthy</p>
                        </CardContent>
                    </Card>
                </motion.div>
            </motion.div>

            <div className="grid gap-6 md:grid-cols-2">
                <motion.div
                    initial={{ opacity: 0, x: -20 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ delay: 0.3 }}
                >
                    <Card className="h-full bg-card/50 backdrop-blur-sm border-border/50">
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <Server className="w-5 h-5" />
                                Services Status
                            </CardTitle>
                            <CardDescription>Real-time service availability checks.</CardDescription>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-4">
                                {isLoading ? (
                                    Array.from({ length: 6 }).map((_, i) => (
                                        <div key={i} className="flex items-center justify-between p-3 rounded-lg bg-muted/30 border border-border/50">
                                            <div className="flex items-center gap-3">
                                                <Skeleton className="w-5 h-5 rounded-full" />
                                                <div className="space-y-2">
                                                    <Skeleton className="h-4 w-24" />
                                                    <Skeleton className="h-3 w-16" />
                                                </div>
                                            </div>
                                            <Skeleton className="h-6 w-12 rounded-full" />
                                        </div>
                                    ))
                                ) : (
                                    health?.services.map((service, i) => (
                                        <div key={i} className="flex items-center justify-between p-3 rounded-lg bg-muted/30 border border-border/50 hover:bg-muted/50 transition-colors">
                                            <div className="flex items-center gap-3">
                                                {getStatusIcon(service.status)}
                                                <div>
                                                    <p className="text-sm font-medium leading-none">{service.name}</p>
                                                    <p className="text-xs text-muted-foreground mt-1 capitalize">{service.status}</p>
                                                </div>
                                            </div>
                                            <div className="text-right">
                                                <span className={`text-xs font-mono px-2 py-1 rounded-full ${service.status === 'operational' ? 'bg-emerald-500/10 text-emerald-500' : 'bg-amber-500/10 text-amber-500'}`}>
                                                    {service.latency}
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
                    <Card className="h-full bg-card/50 backdrop-blur-sm border-border/50">
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <Cpu className="w-5 h-5" />
                                System Resources
                            </CardTitle>
                            <CardDescription>Live server resource usage.</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-8">
                            <div className="space-y-2">
                                <div className="flex items-center justify-between text-sm">
                                    <span className="flex items-center gap-2"><Cpu className="w-4 h-4 text-muted-foreground" /> CPU Usage</span>
                                    <span className="font-mono">{isLoading ? "..." : `${health?.resources.cpu}%`}</span>
                                </div>
                                <div className="h-2 w-full bg-secondary rounded-full overflow-hidden">
                                    <motion.div
                                        className="h-full bg-blue-500"
                                        initial={{ width: 0 }}
                                        animate={{ width: isLoading ? 0 : `${health?.resources.cpu}%` }}
                                        transition={{ duration: 1 }}
                                    />
                                </div>
                            </div>

                            <div className="space-y-2">
                                <div className="flex items-center justify-between text-sm">
                                    <span className="flex items-center gap-2"><Database className="w-4 h-4 text-muted-foreground" /> Memory Usage</span>
                                    <span className="font-mono">{isLoading ? "..." : `${health?.resources.memory}%`}</span>
                                </div>
                                <div className="h-2 w-full bg-secondary rounded-full overflow-hidden">
                                    <motion.div
                                        className="h-full bg-purple-500"
                                        initial={{ width: 0 }}
                                        animate={{ width: isLoading ? 0 : `${health?.resources.memory}%` }}
                                        transition={{ duration: 1 }}
                                    />
                                </div>
                            </div>

                            <div className="space-y-2">
                                <div className="flex items-center justify-between text-sm">
                                    <span className="flex items-center gap-2"><Server className="w-4 h-4 text-muted-foreground" /> Storage</span>
                                    <span className="font-mono">{isLoading ? "..." : `${health?.resources.storage}%`}</span>
                                </div>
                                <div className="h-2 w-full bg-secondary rounded-full overflow-hidden">
                                    <motion.div
                                        className="h-full bg-orange-500"
                                        initial={{ width: 0 }}
                                        animate={{ width: isLoading ? 0 : `${health?.resources.storage}%` }}
                                        transition={{ duration: 1 }}
                                    />
                                </div>
                            </div>
                        </CardContent>
                    </Card>
                </motion.div>
            </div>
        </div>
    );
}
