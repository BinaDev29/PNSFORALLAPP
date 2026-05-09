import { useQuery } from "@tanstack/react-query";
import { DashboardService, NotificationHistory } from "@/services/api";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { formatDistanceToNow } from "date-fns";
import { Mail, MessageSquare, Bell, CheckCircle2, XCircle, Clock, ExternalLink, Search, Filter } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";

export default function HistoryPage() {
    const [searchTerm, setSearchTerm] = useState("");
    
    const { data: history, isLoading } = useQuery<NotificationHistory[]>({
        queryKey: ['notification-history'],
        queryFn: DashboardService.getNotificationHistory
    });

    const filteredHistory = history?.filter(item => {
        const search = searchTerm.toLowerCase();
        return (
            (item.to || "").toLowerCase().includes(search) ||
            (item.status || "").toLowerCase().includes(search) ||
            (item.notificationId || "").toLowerCase().includes(search) ||
            (item.title || "").toLowerCase().includes(search)
        );
    });

    const getStatusIcon = (status: string) => {
        switch ((status || "").toLowerCase()) {
            case 'sent':
            case 'delivered':
                return <CheckCircle2 className="h-4 w-4 text-emerald-500" />;
            case 'failed':
                return <XCircle className="h-4 w-4 text-destructive" />;
            default:
                return <Clock className="h-4 w-4 text-amber-500" />;
        }
    };

    const getChannelIcon = (type: string) => {
        const t = (type || "").toLowerCase();
        if (t.includes('email')) return <Mail className="h-4 w-4" />;
        if (t.includes('sms')) return <MessageSquare className="h-4 w-4" />;
        return <Bell className="h-4 w-4" />;
    };

    return (
        <div className="space-y-6 pb-10">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-extrabold tracking-tight bg-gradient-to-r from-primary to-indigo-400 bg-clip-text text-transparent">
                        Notification History
                    </h2>
                    <p className="text-muted-foreground">
                        Detailed log of all messages dispatched through the system.
                    </p>
                </div>
            </div>

            <Card className="border-border/40 bg-card/40 backdrop-blur-xl shadow-xl overflow-hidden">
                <CardHeader className="border-b border-border/40 bg-muted/20 pb-4">
                    <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                        <CardTitle className="text-lg font-semibold flex items-center gap-2">
                            <Clock className="h-5 w-5 text-primary" /> Delivery Logs
                        </CardTitle>
                        <div className="flex items-center gap-2 w-full md:w-auto">
                            <div className="relative w-full md:w-72">
                                <Search className="absolute left-3 top-2.5 h-4 w-4 text-muted-foreground" />
                                <Input 
                                    placeholder="Search recipient or status..." 
                                    className="pl-9 bg-background/50 h-9"
                                    value={searchTerm}
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                />
                            </div>
                            <Button variant="outline" size="icon" className="h-9 w-9 shrink-0">
                                <Filter className="h-4 w-4" />
                            </Button>
                        </div>
                    </div>
                </CardHeader>
                <CardContent className="p-0">
                    <div className="relative overflow-x-auto">
                        <Table>
                            <TableHeader className="bg-muted/30">
                                <TableRow className="hover:bg-transparent border-border/40">
                                    <TableHead className="w-[280px]">Recipient & Channel</TableHead>
                                    <TableHead>Notification Reference</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead className="text-right">Sent Date</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {isLoading ? (
                                    Array.from({ length: 5 }).map((_, i) => (
                                        <TableRow key={i}>
                                            <TableCell><Skeleton className="h-10 w-full" /></TableCell>
                                            <TableCell><Skeleton className="h-10 w-full" /></TableCell>
                                            <TableCell><Skeleton className="h-10 w-full" /></TableCell>
                                            <TableCell><Skeleton className="h-10 w-full" /></TableCell>
                                        </TableRow>
                                    ))
                                ) : (
                                    <AnimatePresence mode="popLayout">
                                        {filteredHistory?.map((item) => (
                                            <motion.tr
                                                key={item.id}
                                                layout
                                                initial={{ opacity: 0 }}
                                                animate={{ opacity: 1 }}
                                                exit={{ opacity: 0 }}
                                                className="group border-border/40 hover:bg-muted/30 transition-colors"
                                            >
                                                <TableCell className="font-medium">
                                                    <div className="flex items-center gap-3">
                                                        <div className="flex h-9 w-9 items-center justify-center rounded-lg bg-primary/10 text-primary ring-1 ring-primary/20 group-hover:bg-primary group-hover:text-primary-foreground transition-all">
                                                            {getChannelIcon(item.notificationType || 'Email')}
                                                        </div>
                                                        <div className="flex flex-col">
                                                            <span className="text-sm font-bold truncate max-w-[180px]">
                                                                {item.to || 'Unknown Recipient'}
                                                            </span>
                                                            <span className="text-[10px] text-muted-foreground uppercase font-semibold tracking-wider">
                                                                {item.notificationType || 'Email'}
                                                            </span>
                                                        </div>
                                                    </div>
                                                </TableCell>
                                                <TableCell>
                                                    <div className="flex items-center gap-2 text-xs font-mono text-muted-foreground group-hover:text-foreground transition-colors">
                                                        <span className="bg-muted px-1.5 py-0.5 rounded border border-border/50">
                                                            ID: {(item.notificationId || "N/A").split('-')[0]}...
                                                        </span>
                                                        <ExternalLink className="h-3 w-3 opacity-0 group-hover:opacity-100 transition-opacity" />
                                                    </div>
                                                </TableCell>
                                                <TableCell>
                                                    <Badge 
                                                        variant="outline" 
                                                        className={`flex items-center gap-1.5 w-fit px-2.5 py-0.5 rounded-full border-none shadow-sm ${
                                                            (item.status || "").toLowerCase() === 'sent' ? 'bg-emerald-500/10 text-emerald-500' : 
                                                            (item.status || "").toLowerCase() === 'failed' ? 'bg-destructive/10 text-destructive' : 
                                                            'bg-amber-500/10 text-amber-500'
                                                        }`}
                                                    >
                                                        {getStatusIcon(item.status)}
                                                        <span className="capitalize text-[11px] font-bold">{item.status || 'Pending'}</span>
                                                    </Badge>
                                                </TableCell>
                                                <TableCell className="text-right">
                                                    <div className="flex flex-col items-end">
                                                        <span className="text-sm font-medium">
                                                            {item.sentDate ? formatDistanceToNow(new Date(item.sentDate), { addSuffix: true }) : 'Just now'}
                                                        </span>
                                                        <span className="text-[10px] text-muted-foreground">
                                                            {item.sentDate ? new Date(item.sentDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : ''}
                                                        </span>
                                                    </div>
                                                </TableCell>
                                            </motion.tr>
                                        ))}
                                    </AnimatePresence>
                                )}
                            </TableBody>
                        </Table>
                    </div>
                </CardContent>
                {!isLoading && filteredHistory?.length === 0 && (
                    <div className="flex flex-col items-center justify-center py-20 text-center">
                        <div className="h-16 w-16 rounded-full bg-muted flex items-center justify-center mb-4">
                            <Search className="h-8 w-8 text-muted-foreground" />
                        </div>
                        <h3 className="text-lg font-semibold">No records found</h3>
                        <p className="text-muted-foreground max-w-xs mx-auto">
                            No notification history matches your current search criteria.
                        </p>
                    </div>
                )}
            </Card>
        </div>
    );
}
