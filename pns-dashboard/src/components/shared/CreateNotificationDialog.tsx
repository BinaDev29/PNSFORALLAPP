import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { DashboardService } from "@/services/api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import React, { useState } from "react"
import { toast } from "sonner"
import { Loader2 } from "lucide-react"

interface CreateNotificationDialogProps {
    children?: React.ReactNode;
    notification?: any; // Using any for flexibility with history objects
    open?: boolean;
    onOpenChange?: (open: boolean) => void;
}

export function CreateNotificationDialog({ children, notification, open: controlledOpen, onOpenChange: setControlledOpen }: CreateNotificationDialogProps) {
    const [internalOpen, setInternalOpen] = useState(false);
    const isOpen = controlledOpen !== undefined ? controlledOpen : internalOpen;
    const setIsOpen = setControlledOpen || setInternalOpen;

    const [formData, setFormData] = useState({
        title: '',
        message: '',
        to: '',
        clientApplicationId: '',
        notificationTypeId: '',
        priorityId: ''
    });

    // Reset or populate form when dialog opens or notification changes
    React.useEffect(() => {
        const fetchDetails = async () => {
            if (isOpen) {
                if (notification) {
                    // Pre-fill with available data first to show something quickly
                    setFormData(prev => ({
                        ...prev,
                        title: notification.subject || notification.title || '',
                        message: notification.message || '',
                        to: notification.recipient || notification.to || '',
                        clientApplicationId: notification.clientApplicationId || '',
                        notificationTypeId: notification.notificationTypeId || '',
                        priorityId: notification.priorityId || ''
                    }));

                    // Fetch full details to get IDs if missing
                    try {
                        const targetId = notification.notificationId || notification.id;
                        const details: any = await DashboardService.getNotificationById(targetId);
                        if (details) {
                            setFormData({
                                title: details.subject || details.title || notification.subject || notification.title || '',
                                message: details.message || notification.message || '',
                                to: details.recipient || details.to || notification.recipient || notification.to || '',
                                clientApplicationId: details.clientApplicationId || notification.clientApplicationId || '',
                                notificationTypeId: details.notificationTypeId || notification.notificationTypeId || '',
                                priorityId: details.priorityId || notification.priorityId || ''
                            });
                        }
                    } catch (e) {
                        console.error("Failed to fetch notification details for edit", e);
                    }
                } else {
                    setFormData({ title: '', message: '', to: '', clientApplicationId: '', notificationTypeId: '', priorityId: '' });
                }
            }
        };

        fetchDetails();
    }, [isOpen, notification]);

    const { data: clients } = useQuery({ queryKey: ['clients'], queryFn: DashboardService.getClientApplications });
    const { data: types } = useQuery({ queryKey: ['notificationTypes'], queryFn: DashboardService.getNotificationTypes });
    const { data: priorities } = useQuery({ queryKey: ['priorities'], queryFn: DashboardService.getPriorities });
    const queryClient = useQueryClient();

    const createMutation = useMutation({
        mutationFn: DashboardService.createNotification,
        onSuccess: () => {
            toast.success("Notification sent successfully");
            setIsOpen(false);
            setFormData({ title: '', message: '', to: '', clientApplicationId: '', notificationTypeId: '', priorityId: '' });
            queryClient.invalidateQueries({ queryKey: ['recentActivity'] });
        },
        onError: () => {
            toast.error("Failed to send notification");
        }
    });

    const updateMutation = useMutation({
        mutationFn: (data: any) => DashboardService.updateNotification(notification.id, data),
        onSuccess: () => {
            toast.success("Notification updated successfully");
            setIsOpen(false);
            queryClient.invalidateQueries({ queryKey: ['recentActivity'] });
        },
        onError: () => {
            toast.error("Failed to update notification");
        }
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        // Validation for CREATE mode (in edit mode we might be lenient or reuse)
        if (!notification) {
            if (!formData.clientApplicationId || !formData.notificationTypeId || !formData.priorityId) {
                toast.error("Please fill in all required fields");
                return;
            }
        }

        const payload = {
            ...formData,
            to: formData.to.split(',').map(email => email.trim())
        };

        if (notification) {
            updateMutation.mutate(payload);
        } else {
            createMutation.mutate(payload);
        }
    };

    const isPending = createMutation.isPending || updateMutation.isPending;

    return (
        <Dialog open={isOpen} onOpenChange={setIsOpen}>
            <DialogTrigger asChild={!!children}>
                {children}
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>{notification ? "Edit Notification" : "Create Notification"}</DialogTitle>
                    <DialogDescription>
                        {notification ? "Modify the details of this notification." : "Send a new notification to your users."}
                    </DialogDescription>
                </DialogHeader>
                <form onSubmit={handleSubmit} className="grid gap-4 py-4">
                    <div className="grid gap-2">
                        <Label htmlFor="app">Application</Label>
                        <Select value={formData.clientApplicationId} onValueChange={(val) => setFormData({ ...formData, clientApplicationId: val })}>
                            <SelectTrigger>
                                <SelectValue placeholder="Select application" />
                            </SelectTrigger>
                            <SelectContent>
                                {clients?.map(client => (
                                    <SelectItem key={client.id} value={client.id}>{client.name}</SelectItem>
                                ))}
                            </SelectContent>
                        </Select>
                    </div>
                    <div className="grid gap-2">
                        <Label htmlFor="title">Title</Label>
                        <Input
                            id="title"
                            value={formData.title}
                            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setFormData({ ...formData, title: e.target.value })}
                            required
                        />
                    </div>
                    <div className="grid gap-2">
                        <Label htmlFor="message">Message</Label>
                        <Textarea
                            id="message"
                            value={formData.message}
                            onChange={(e: React.ChangeEvent<HTMLTextAreaElement>) => setFormData({ ...formData, message: e.target.value })}
                            required
                        />
                    </div>
                    <div className="grid gap-2">
                        <Label htmlFor="to">
                            {types?.find(t => t.id === formData.notificationTypeId)?.name === 'SMS'
                                ? "Recipient Phone Numbers (comma separated)"
                                : "Recipient Emails (comma separated)"}
                        </Label>
                        <Input
                            id="to"
                            value={formData.to}
                            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setFormData({ ...formData, to: e.target.value })}
                            placeholder={types?.find(t => t.id === formData.notificationTypeId)?.name === 'SMS'
                                ? "+251911223344, +251922334455"
                                : "user@example.com, another@example.com"}
                            required
                        />
                    </div>
                    <div className="grid grid-cols-2 gap-4">
                        <div className="grid gap-2">
                            <Label htmlFor="type">Type</Label>
                            <Select value={formData.notificationTypeId} onValueChange={(val) => setFormData({ ...formData, notificationTypeId: val })}>
                                <SelectTrigger>
                                    <SelectValue placeholder="Select type" />
                                </SelectTrigger>
                                <SelectContent>
                                    {types?.map(type => (
                                        <SelectItem key={type.id} value={type.id}>{type.name}</SelectItem>
                                    ))}
                                </SelectContent>
                            </Select>
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="priority">Priority</Label>
                            <Select value={formData.priorityId} onValueChange={(val) => setFormData({ ...formData, priorityId: val })}>
                                <SelectTrigger>
                                    <SelectValue placeholder="Select priority" />
                                </SelectTrigger>
                                <SelectContent>
                                    {priorities?.map(priority => (
                                        <SelectItem key={priority.id} value={priority.id}>{priority.description}</SelectItem>
                                    ))}
                                </SelectContent>
                            </Select>
                        </div>
                    </div>
                    <DialogFooter>
                        <Button type="submit" disabled={isPending}>
                            {isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                            {notification ? "Update Notification" : "Send Notification"}
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    )
}
