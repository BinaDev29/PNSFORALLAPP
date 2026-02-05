import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Bell, Filter, Plus, Search, Loader2, MoreHorizontal, Edit, Trash2 } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import { DashboardService, NotificationHistory } from "@/services/api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { formatDate } from "@/lib/utils";
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
            console.error("Delete error:", error);
            const errorMessage = error?.response?.data?.message || error?.message || "Failed to delete notification";
            toast.error(errorMessage);
        }
    });

    const filteredNotifications = notifications?.filter(n =>
        n.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
        n.status.toLowerCase().includes(searchTerm.toLowerCase())
    );


    return (
        <div className="space-y-8">
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-bold tracking-tight text-primary">
                        Notifications
                    </h2>
                    <p className="text-muted-foreground">Manage and track all sent notifications across applications.</p>
                </div>
                <div className="flex items-center gap-2">
                    <Button variant="outline" className="gap-2">
                        <Filter className="h-4 w-4" />
                        Filters
                    </Button>
                    <CreateNotificationDialog>
                        <Button className="gap-2 bg-primary hover:bg-primary/90 text-primary-foreground shadow-lg shadow-primary/25">
                            <Plus className="h-4 w-4" />
                            Create New
                        </Button>
                    </CreateNotificationDialog>
                </div>
            </div>

            <Card className="border-border/50 bg-card/50 backdrop-blur-sm shadow-xl">
                <CardHeader>
                    <div className="flex items-center justify-between">
                        <CardTitle>Recent Notifications</CardTitle>
                        <div className="relative w-64">
                            <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
                            <Input
                                placeholder="Search notifications..."
                                className="pl-8 bg-background/50"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>
                    </div>
                </CardHeader>
                <CardContent>
                    <Table>
                        <TableHeader>
                            <TableRow className="hover:bg-transparent">
                                <TableHead>ID</TableHead>
                                <TableHead>Notification ID</TableHead>
                                <TableHead>Status</TableHead>
                                <TableHead className="text-right">Time</TableHead>
                                <TableHead className="w-[50px]"></TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {isLoading ? (
                                Array.from({ length: 5 }).map((_, i) => (
                                    <TableRow key={i}>
                                        <TableCell><Skeleton className="h-4 w-24" /></TableCell>
                                        <TableCell>
                                            <div className="flex items-center gap-2">
                                                <Skeleton className="h-8 w-8 rounded-lg" />
                                                <Skeleton className="h-4 w-24" />
                                            </div>
                                        </TableCell>
                                        <TableCell><Skeleton className="h-6 w-16 rounded-full" /></TableCell>
                                        <TableCell className="text-right"><Skeleton className="h-4 w-32 ml-auto" /></TableCell>
                                        <TableCell><Skeleton className="h-8 w-8 rounded-md" /></TableCell>
                                    </TableRow>
                                ))
                            ) : (
                                filteredNotifications?.map((notification) => (
                                    <TableRow key={notification.id} className="group hover:bg-muted/50 transition-colors">
                                        <TableCell className="font-medium group-hover:text-primary transition-colors">{notification.id.substring(0, 8)}...</TableCell>
                                        <TableCell>
                                            <div className="flex items-center gap-2">
                                                <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center text-primary">
                                                    <Bell className="h-4 w-4" />
                                                </div>
                                                <span className="font-medium">{notification.notificationId.substring(0, 8)}...</span>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <Badge
                                                variant="secondary"
                                                className={`
                                                    ${notification.status === 'Sent' && 'bg-blue-500/15 text-blue-600 hover:bg-blue-500/25'}
                                                    ${notification.status === 'Delivered' && 'bg-emerald-500/15 text-emerald-600 hover:bg-emerald-500/25'}
                                                    ${notification.status === 'Failed' && 'bg-red-500/15 text-red-600 hover:bg-red-500/25'}
                                                `}
                                            >
                                                {notification.status}
                                            </Badge>
                                        </TableCell>
                                        <TableCell className="text-right text-muted-foreground">{formatDate(notification.sentDate)}</TableCell>
                                        <TableCell>
                                            <DropdownMenu>
                                                <DropdownMenuTrigger asChild>
                                                    <Button variant="ghost" className="h-8 w-8 p-0 opacity-0 group-hover:opacity-100 transition-opacity">
                                                        <span className="sr-only">Open menu</span>
                                                        <MoreHorizontal className="h-4 w-4" />
                                                    </Button>
                                                </DropdownMenuTrigger>
                                                <DropdownMenuContent align="end">
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
                                    </TableRow>
                                ))
                            )}
                            {!isLoading && filteredNotifications?.length === 0 && (
                                <TableRow>
                                    <TableCell colSpan={7} className="h-24 text-center">
                                        No results.
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </CardContent>
            </Card>

            {/* Edit Dialog */}
            {notificationToEdit && (
                <CreateNotificationDialog
                    open={editDialogOpen}
                    onOpenChange={setEditDialogOpen}
                    notification={notificationToEdit}
                >
                    {/* Trigger is handled by state */}
                    <span className="hidden"></span>
                </CreateNotificationDialog>
            )}

            {/* Delete Confirmation Dialog */}
            <Dialog open={!!notificationToDelete} onOpenChange={(open) => !open && setNotificationToDelete(null)}>
                <DialogContent>
                    <DialogHeader>
                        <DialogTitle>Are you sure?</DialogTitle>
                        <DialogDescription>
                            This action cannot be undone. This will permanently delete the notification history record.
                        </DialogDescription>
                    </DialogHeader>
                    <DialogFooter>
                        <Button variant="outline" onClick={() => setNotificationToDelete(null)}>
                            Cancel
                        </Button>
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
