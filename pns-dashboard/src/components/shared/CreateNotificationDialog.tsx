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
import { Badge } from "@/components/ui/badge"
import { DashboardService } from "@/services/api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import React, { useState } from "react"
import { toast } from "sonner"
import { Loader2, Plus, X, Layout } from "lucide-react"

interface CreateNotificationDialogProps {
    children?: React.ReactNode;
    notification?: any;
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

    // Template-related state
    const [selectedTemplateId, setSelectedTemplateId] = useState<string>('none');
    const [templateVars, setTemplateVars] = useState<{ key: string; value: string }[]>([{ key: '', value: '' }]);

    React.useEffect(() => {
        const fetchDetails = async () => {
            if (isOpen) {
                if (notification) {
                    setFormData(prev => ({
                        ...prev,
                        title: notification.subject || notification.title || '',
                        message: notification.message || '',
                        to: notification.recipient || notification.to || '',
                        clientApplicationId: notification.clientApplicationId || '',
                        notificationTypeId: notification.notificationTypeId || '',
                        priorityId: notification.priorityId || ''
                    }));
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
                    setSelectedTemplateId('none');
                    setTemplateVars([{ key: '', value: '' }]);
                }
            }
        };
        fetchDetails();
    }, [isOpen, notification]);

    const { data: clients } = useQuery({ queryKey: ['clients'], queryFn: DashboardService.getClientApplications });
    const { data: types } = useQuery({ queryKey: ['notificationTypes'], queryFn: DashboardService.getNotificationTypes });
    const { data: priorities } = useQuery({ queryKey: ['priorities'], queryFn: DashboardService.getPriorities });
    const { data: templates } = useQuery({ queryKey: ['emailTemplates'], queryFn: DashboardService.getEmailTemplates });
    const queryClient = useQueryClient();

    const selectedTemplate = selectedTemplateId !== 'none' ? templates?.find(t => t.id === selectedTemplateId) : undefined;
    const selectedClient = clients?.find(c => c.id === formData.clientApplicationId);

    const createMutation = useMutation({
        mutationFn: DashboardService.createNotification,
        onSuccess: () => {
            toast.success("Notification sent successfully");
            setIsOpen(false);
            setFormData({ title: '', message: '', to: '', clientApplicationId: '', notificationTypeId: '', priorityId: '' });
            setSelectedTemplateId('none');
            setTemplateVars([{ key: '', value: '' }]);
            queryClient.invalidateQueries({ queryKey: ['recentActivity'] });
        },
        onError: (err: any) => {
            toast.error(err?.response?.data?.message || "Failed to send notification");
        }
    });

    const templateMutation = useMutation({
        mutationFn: DashboardService.sendTemplatedNotification,
        onSuccess: () => {
            toast.success("Templated notification sent successfully!");
            setIsOpen(false);
            setFormData({ title: '', message: '', to: '', clientApplicationId: '', notificationTypeId: '', priorityId: '' });
            setSelectedTemplateId('none');
            setTemplateVars([{ key: '', value: '' }]);
            queryClient.invalidateQueries({ queryKey: ['recentActivity'] });
        },
        onError: (err: any) => {
            toast.error(err?.response?.data?.message || "Failed to send templated notification");
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

    const handleAddVar = () => setTemplateVars(prev => [...prev, { key: '', value: '' }]);
    const handleRemoveVar = (i: number) => setTemplateVars(prev => prev.filter((_, idx) => idx !== i));
    const handleVarChange = (i: number, field: 'key' | 'value', val: string) => {
        setTemplateVars(prev => prev.map((v, idx) => idx === i ? { ...v, [field]: val } : v));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!notification) {
            if (!formData.clientApplicationId || !formData.notificationTypeId || !formData.priorityId) {
                toast.error("Please fill in all required fields");
                return;
            }
        }

        const recipients = formData.to.split(',').map(email => email.trim()).filter(Boolean);

        // If a template is selected, compile on client side and send via standard notification creation
        if (selectedTemplateId !== 'none' && selectedTemplate && !notification) {
            let processedBody = selectedTemplate.bodyHtml;
            templateVars.forEach(v => {
                if (v.key.trim()) {
                    const regex = new RegExp(`{{\\s*${v.key.trim().replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&')}\\s*}}`, 'gi');
                    processedBody = processedBody.replace(regex, v.value);
                }
            });

            const payload = {
                title: selectedTemplate.subject,
                message: processedBody,
                clientApplicationId: formData.clientApplicationId,
                notificationTypeId: formData.notificationTypeId,
                priorityId: formData.priorityId,
                to: recipients
            };

            createMutation.mutate(payload);
            return;
        }

        const payload = { ...formData, to: recipients };
        if (notification) {
            updateMutation.mutate(payload);
        } else {
            createMutation.mutate(payload);
        }
    };

    const isPending = createMutation.isPending || updateMutation.isPending || templateMutation.isPending;

    return (
        <Dialog open={isOpen} onOpenChange={setIsOpen}>
            <DialogTrigger asChild={!!children}>
                {children}
            </DialogTrigger>
            <DialogContent className="sm:max-w-[500px] my-8 max-h-[90vh] overflow-y-auto">
                <DialogHeader className="mt-4">
                    <DialogTitle>{notification ? "Edit Notification" : "Create Notification"}</DialogTitle>
                    <DialogDescription>
                        {notification ? "Modify the details of this notification." : "Send a new notification to your users."}
                    </DialogDescription>
                </DialogHeader>
                <form onSubmit={handleSubmit} className="grid gap-4 py-4 mb-4">

                    {/* Application selector */}
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

                    {/* Template selector (only in create mode) */}
                    {!notification && (
                        <div className="grid gap-2">
                            <Label htmlFor="template" className="flex items-center gap-2">
                                <Layout className="w-3 h-3" /> Email Template
                                <Badge variant="outline" className="text-[10px]">Optional</Badge>
                            </Label>
                            <Select value={selectedTemplateId} onValueChange={setSelectedTemplateId}>
                                <SelectTrigger>
                                    <SelectValue placeholder="No template (plain message)" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value="none">No template (plain message)</SelectItem>
                                    {templates?.map(tmpl => (
                                        <SelectItem key={tmpl.id} value={tmpl.id}>{tmpl.name}</SelectItem>
                                    ))}
                                </SelectContent>
                            </Select>
                            {selectedTemplate && (
                                <p className="text-xs text-muted-foreground bg-muted/40 rounded-lg px-3 py-2 border border-border/30">
                                    📧 Subject: <strong>{selectedTemplate.subject}</strong>
                                </p>
                            )}
                        </div>
                    )}

                    {/* Template variables (shown only when template is selected) */}
                    {selectedTemplateId !== 'none' && !notification && (
                        <div className="grid gap-2">
                            <Label className="text-xs font-semibold uppercase tracking-wider text-primary">
                                Template Variables <span className="normal-case text-muted-foreground font-normal">(e.g. UserName, Code)</span>
                            </Label>
                            <div className="space-y-2 p-3 bg-muted/30 rounded-xl border border-border/50">
                                {templateVars.map((v, i) => (
                                    <div key={i} className="flex gap-2 items-center">
                                        <Input
                                            placeholder="Variable (e.g. UserName)"
                                            value={v.key}
                                            onChange={e => handleVarChange(i, 'key', e.target.value)}
                                            className="flex-1 h-8 text-xs font-mono"
                                        />
                                        <span className="text-muted-foreground text-xs">=</span>
                                        <Input
                                            placeholder="Value"
                                            value={v.value}
                                            onChange={e => handleVarChange(i, 'value', e.target.value)}
                                            className="flex-1 h-8 text-xs"
                                        />
                                        <Button type="button" variant="ghost" size="icon" className="h-8 w-8 text-destructive hover:bg-destructive/10" onClick={() => handleRemoveVar(i)}>
                                            <X className="h-3 w-3" />
                                        </Button>
                                    </div>
                                ))}
                                <Button type="button" variant="outline" size="sm" className="w-full h-7 text-xs gap-1 mt-1" onClick={handleAddVar}>
                                    <Plus className="h-3 w-3" /> Add Variable
                                </Button>
                            </div>
                        </div>
                    )}

                    {/* Title & Message (only when no template selected) */}
                    {selectedTemplateId === 'none' && (
                        <>
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
                        </>
                    )}

                    {/* Recipients */}
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

                    {/* Type & Priority */}
                    <div className="grid grid-cols-2 gap-4">
                        <div className="grid gap-2">
                            <Label htmlFor="type">Type</Label>
                            <Select value={formData.notificationTypeId} onValueChange={(val) => setFormData({ ...formData, notificationTypeId: val })}>
                                <SelectTrigger>
                                    <SelectValue placeholder="Select type" />
                                </SelectTrigger>
                                <SelectContent>
                                    {types?.filter(type => ['email', 'sms'].includes(type.name.toLowerCase())).map(type => (
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
                                    {priorities?.reduce((acc: any[], current) => {
                                        const normalizedName = current.description.replace(/ priority/i, '').trim().toLowerCase();
                                        const exists = acc.find(p => p.description.replace(/ priority/i, '').trim().toLowerCase() === normalizedName);
                                        if (!exists) {
                                            acc.push(current);
                                        } else if (current.description.length < exists.description.length) {
                                            const index = acc.indexOf(exists);
                                            acc[index] = current;
                                        }
                                        return acc;
                                    }, []).map(priority => (
                                        <SelectItem key={priority.id} value={priority.id}>{priority.description}</SelectItem>
                                    ))}
                                </SelectContent>
                            </Select>
                        </div>
                    </div>

                    <DialogFooter>
                        <Button
                            type="submit"
                            disabled={isPending}
                            className={selectedTemplateId !== 'none' ? "bg-emerald-600 hover:bg-emerald-700 text-white" : ""}
                        >
                            {isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                            {notification ? "Update Notification" : selectedTemplateId !== 'none' ? "📧 Send via Template" : "Send Notification"}
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    )
}
