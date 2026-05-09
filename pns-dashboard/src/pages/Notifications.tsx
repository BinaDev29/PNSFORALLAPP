import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Bell, Filter, Plus, Search, Loader2, MoreHorizontal, Edit, Trash2, Mail, MessageSquare, ExternalLink, Clock } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import { DashboardService, NotificationHistory } from "@/services/api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { formatDistanceToNow } from "date-fns";
import { CreateNotificationDialog } from "@/components/shared/CreateNotificationDialog";
import { useState } from "react";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog";
import { toast } from "sonner";
import { motion, AnimatePresence } from "framer-motion";

export default function NotificationsPage() {
    const [searchTerm, setSearchTerm] = useState("");
    const [notificationToDelete, setNotificationToDelete] = useState<string | null>(null);
    const [notificationToEdit, setNotificationToEdit] = useState<NotificationHistory | null>(null);
    const [editDialogOpen, setEditDialogOpen] = useState(false);

    const queryClient = useQueryClient();

    const { data: notifications, isLoading } = useQuery({
        queryKey: ['recentActivity'],
        queryFn: DashboardService.getRecentActivity
    });

    const deleteMutation = useMutation({
        mutationFn: DashboardService.deleteNotification,
        onSuccess: () => {
            toast.success("Notification deleted successfully");
            setNotificationToDelete(null);
            queryClient.invalidateQueries({ queryKey: ['recentActivity'] });
        },
        onError: (error: any) => {
            const errorMessage = error?.response?.data?.message || "Failed to delete notification";
            toast.error(errorMessage);
        }
    });

    const filteredNotifications = notifications?.filter(n =>
        (n.to || "").toLowerCase().includes(searchTerm.toLowerCase()) ||
        (n.status || "").toLowerCase().includes(searchTerm.toLowerCase()) ||
        (n.notificationId || "").toLowerCase().includes(searchTerm.toLowerCase())
    );

    const getChannelIcon = (type?: string) => {
        const t = (type || "").toLowerCase();
        if (t.includes('email')) return <Mail className="h-4 w-4" />;
        if (t.includes('sms')) return <MessageSquare className="h-4 w-4" />;
        return <Bell className="h-4 w-4" />;
    };

    return (
        <div className="space-y-6 pb-10">
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-extrabold tracking-tight bg-gradient-to-r from-primary to-indigo-400 bg-clip-text text-transparent">
                        Notifications
                    </h2>
                    <p className="text-muted-foreground text-sm">Manage and track all sent notifications across applications.</p>
                </div>
                <div className="flex items-center gap-2">
                    <Button variant="outline" size="sm" className="gap-2 border-border/60">
                        <Filter className="h-4 w-4" />
                        Filters
                    </Button>
                    <CreateNotificationDialog>
                        <Button size="sm" className="gap-2 bg-primary hover:bg-primary/90 text-primary-foreground shadow-lg shadow-primary/25">
                            <Plus className="h-4 w-4" />
                            Create New
                        </Button>
                    </CreateNotificationDialog>
                </div>
            </div>

            <Card className="border-border/40 bg-card/40 backdrop-blur-xl shadow-xl overflow-hidden">
                <CardHeader className="border-b border-border/40 bg-muted/20 pb-4">
                    <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                        <CardTitle className="text-lg font-semibold flex items-center gap-2">
                            <Bell className="h-5 w-5 text-primary" /> Recent Activity
                        </CardTitle>
                        <div className="relative w-full md:w-64">
                            <Search className="absolute left-3 top-2.5 h-4 w-4 text-muted-foreground" />
                            <Input
                                placeholder="Search notifications..."
                                className="pl-9 bg-background/50 h-9"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>
                    </div>
                </CardHeader>
                <CardContent className="p-0">
                    <div className="relative overflow-x-auto">
                        <Table>
                            <TableHeader className="bg-muted/30">
                                <TableRow className="hover:bg-transparent border-border/40">
                                    <TableHead className="w-[300px]">Recipient & Channel</TableHead>
                                    <TableHead>Notification Reference</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead className="text-right">Time</TableHead>
                                    <TableHead className="w-[50px]"></TableHead>
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
                                            <TableCell><Skeleton className="h-10 w-10" /></TableCell>
                                        </TableRow>
                                    ))
                                ) : (
                                    <AnimatePresence mode="popLayout">
                                        {filteredNotifications?.map((notification) => (
                                            <motion.tr 
                                                key={notification.id} 
                                                layout
                                                initial={{ opacity: 0 }}
                                                animate={{ opacity: 1 }}
                                                exit={{ opacity: 0 }}
                                                className="group border-border/40 hover:bg-muted/30 transition-colors"
                                            >
                                                <TableCell className="font-medium">
                                                    <div className="flex items-center gap-3">
                                                        <div className="flex h-9 w-9 items-center justify-center rounded-lg bg-primary/10 text-primary ring-1 ring-primary/20 group-hover:bg-primary group-hover:text-primary-foreground transition-all">
                                                            {getChannelIcon(notification.notificationType)}
                                                        </div>
                                                        <div className="flex flex-col overflow-hidden">
                                                            <span className="text-sm font-bold truncate max-w-[200px]">
                                                                {notification.to || 'System'}
                                                            </span>
                                                            <span className="text-[10px] text-muted-foreground uppercase font-semibold tracking-wider">
                                                                {notification.notificationType || 'Email'}
                                                            </span>
                                                        </div>
                                                    </div>
                                                </TableCell>
                                                <TableCell>
                                                    <div className="flex items-center gap-2 text-xs font-mono text-muted-foreground group-hover:text-foreground transition-colors">
                                                        <span className="bg-muted/50 px-1.5 py-0.5 rounded border border-border/50">
                                                            ID: {(notification.notificationId || "N/A").split('-')[0]}...
                                                        </span>
                                                        <ExternalLink className="h-3 w-3 opacity-0 group-hover:opacity-100 transition-opacity" />
                                                    </div>
                                                </TableCell>
                                                <TableCell>
                                                    <Badge
                                                        variant="outline"
                                                        className={`flex items-center gap-1.5 w-fit px-2.5 py-0.5 rounded-full border-none shadow-sm ${
                                                            notification.status === 'Sent' || notification.status === 'Delivered' ? 'bg-emerald-500/10 text-emerald-500' : 
                                                            notification.status === 'Failed' ? 'bg-destructive/10 text-destructive' : 
                                                            'bg-amber-500/10 text-amber-500'
                                                        }`}
                                                    >
                                                        <span className="capitalize text-[11px] font-bold">{notification.status}</span>
                                                    </Badge>
                                                </TableCell>
                                                <TableCell className="text-right">
                                                    <div className="flex flex-col items-end">
                                                        <span className="text-sm font-medium">
                                                            {notification.sentDate ? formatDistanceToNow(new Date(notification.sentDate), { addSuffix: true }) : 'Just now'}
                                                        </span>
                                                        <span className="text-[10px] text-muted-foreground flex items-center gap-1">
                                                            <Clock className="h-3 w-3" />
                                                            {notification.sentDate ? new Date(notification.sentDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : ''}
                                                        </span>
                                                    </div>
                                                </TableCell>
                                                <TableCell>
                                                    <DropdownMenu>
                                                        <DropdownMenuTrigger asChild>
                                                            <Button variant="ghost" className="h-8 w-8 p-0 opacity-0 group-hover:opacity-100 transition-opacity">
                                                                <MoreHorizontal className="h-4 w-4" />
                                                            </Button>
                                                        </DropdownMenuTrigger>
                                                        <DropdownMenuContent align="end" className="bg-card/95 backdrop-blur-xl border-border/50">
                                                            <DropdownMenuLabel>Actions</DropdownMenuLabel>
                                                            <DropdownMenuItem onClick={() => {
                                                                setNotificationToEdit(notification);
                                                                setEditDialogOpen(true);
                                                            }}>
                                                                <Edit className="mr-2 h-4 w-4" />
                                                                Edit
                                                            </DropdownMenuItem>
                                                            <DropdownMenuItem
                                                                className="text-destructive focus:text-destructive"
                                                                onClick={() => setNotificationToDelete(notification.notificationId)}
                                                            >
                                                                <Trash2 className="mr-2 h-4 w-4" />
                                                                Delete
                                                            </DropdownMenuItem>
                                                        </DropdownMenuContent>
                                                    </DropdownMenu>
                                                </TableCell>
                                            </motion.tr>
                                        ))}
                                    </AnimatePresence>
                                )}
                            </TableBody>
                        </Table>
                    </div>
                </CardContent>
            </Card>

            {/* Confirmation Dialogs ... same as before */}
            <Dialog open={!!notificationToDelete} onOpenChange={(open) => !open && setNotificationToDelete(null)}>
                <DialogContent className="bg-card/95 backdrop-blur-xl border-border/50">
                    <DialogHeader>
                        <DialogTitle>Confirm Deletion</DialogTitle>
                        <DialogDescription>
                            This action will permanently remove this notification record from the history.
                        </DialogDescription>
                    </DialogHeader>
                    <DialogFooter>
                        <Button variant="outline" onClick={() => setNotificationToDelete(null)}>Cancel</Button>
                        <Button
                            variant="destructive"
                            onClick={() => notificationToDelete && deleteMutation.mutate(notificationToDelete)}
                            disabled={deleteMutation.isPending}
                        >
                            {deleteMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                            Delete
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </div>
    );
}
