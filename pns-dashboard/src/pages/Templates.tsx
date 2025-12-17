import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Plus, Loader2, Mail, Trash2, Pencil } from "lucide-react";
import { DashboardService, CreateEmailTemplateRequest, EmailTemplate } from "@/services/api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { useState } from "react";
import { toast } from "sonner";

export default function TemplatesPage() {
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [editingId, setEditingId] = useState<string | null>(null);
    const [templateToDelete, setTemplateToDelete] = useState<string | null>(null);
    const [formData, setFormData] = useState<CreateEmailTemplateRequest>({
        name: '',
        subject: '',
        bodyHtml: '',
        bodyText: ''
    });

    const queryClient = useQueryClient();

    const { data: templates, isLoading } = useQuery({
        queryKey: ['emailTemplates'],
        queryFn: DashboardService.getEmailTemplates
    });

    const createMutation = useMutation({
        mutationFn: DashboardService.createEmailTemplate,
        onSuccess: () => {
            toast.success("Email template created successfully");
            handleCloseDialog();
            queryClient.invalidateQueries({ queryKey: ['emailTemplates'] });
        },
        onError: () => {
            toast.error("Failed to create email template");
        }
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: string, data: CreateEmailTemplateRequest }) => DashboardService.updateEmailTemplate(id, data),
        onSuccess: () => {
            toast.success("Email template updated successfully");
            handleCloseDialog();
            queryClient.invalidateQueries({ queryKey: ['emailTemplates'] });
        },
        onError: () => {
            toast.error("Failed to update email template");
        }
    });

    const deleteMutation = useMutation({
        mutationFn: DashboardService.deleteEmailTemplate,
        onSuccess: () => {
            toast.success("Template deleted successfully");
            setTemplateToDelete(null);
            queryClient.invalidateQueries({ queryKey: ['emailTemplates'] });
        },
        onError: (error: any) => {
            console.error("Delete error:", error);
            const errorMessage = error?.response?.data?.message || error?.message || "Failed to delete template";
            toast.error(errorMessage);
        }
    });

    const displayTemplates = templates || [];

    const handleOpenCreate = () => {
        setEditingId(null);
        setFormData({ name: '', subject: '', bodyHtml: '', bodyText: '' });
        setIsDialogOpen(true);
    };

    const handleEdit = (template: EmailTemplate) => {
        setEditingId(template.id);
        setFormData({
            name: template.name,
            subject: template.subject,
            bodyHtml: template.bodyHtml,
            bodyText: template.bodyText || ''
        });
        setIsDialogOpen(true);
    };

    const handleCloseDialog = () => {
        setIsDialogOpen(false);
        setEditingId(null);
        setFormData({ name: '', subject: '', bodyHtml: '', bodyText: '' });
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (editingId) {
            updateMutation.mutate({ id: editingId, data: formData });
        } else {
            createMutation.mutate(formData);
        }
    };

    if (isLoading) {
        return (
            <div className="flex h-[80vh] items-center justify-center">
                <Loader2 className="h-8 w-8 animate-spin text-primary" />
            </div>
        );
    }

    return (
        <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-bold tracking-tight bg-gradient-to-r from-teal-500 to-emerald-600 bg-clip-text text-transparent">
                        Email Templates
                    </h2>
                    <p className="text-muted-foreground">Manage email templates for your notifications.</p>
                </div>
                <div className="flex items-center gap-2">
                    <Dialog open={isDialogOpen} onOpenChange={(open) => !open && handleCloseDialog()}>
                        <DialogTrigger asChild>
                            <Button
                                onClick={handleOpenCreate}
                                className="gap-2 bg-teal-600 hover:bg-teal-700 shadow-lg shadow-teal-500/25"
                            >
                                <Plus className="h-4 w-4" />
                                Create Template
                            </Button>
                        </DialogTrigger>
                        <DialogContent className="sm:max-w-[700px]">
                            <DialogHeader>
                                <DialogTitle>{editingId ? "Edit Email Template" : "Create Email Template"}</DialogTitle>
                                <DialogDescription>
                                    {editingId ? "Update existing template details." : "Design a new email template. HTML body is supported."}
                                </DialogDescription>
                            </DialogHeader>
                            <form onSubmit={handleSubmit} className="grid gap-4 py-4">
                                <div className="grid gap-2">
                                    <Label htmlFor="name">Template Name</Label>
                                    <Input
                                        id="name"
                                        value={formData.name}
                                        onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                                        placeholder="e.g., Welcome Email"
                                        required
                                    />
                                </div>
                                <div className="grid gap-2">
                                    <Label htmlFor="subject">Subject Line</Label>
                                    <Input
                                        id="subject"
                                        value={formData.subject}
                                        onChange={(e) => setFormData({ ...formData, subject: e.target.value })}
                                        placeholder="Welcome to our platform!"
                                        required
                                    />
                                </div>
                                <div className="grid gap-2">
                                    <Label htmlFor="bodyHtml">HTML Body</Label>
                                    <Textarea
                                        id="bodyHtml"
                                        value={formData.bodyHtml}
                                        onChange={(e) => setFormData({ ...formData, bodyHtml: e.target.value })}
                                        placeholder="<html><body><h1>Hello...</h1></body></html>"
                                        className="font-mono min-h-[150px]"
                                        required
                                    />
                                </div>
                                <div className="grid gap-2">
                                    <Label htmlFor="bodyText">Plain Text Body (Optional)</Label>
                                    <Textarea
                                        id="bodyText"
                                        value={formData.bodyText}
                                        onChange={(e) => setFormData({ ...formData, bodyText: e.target.value })}
                                        placeholder="Plain text version..."
                                        className="font-mono"
                                    />
                                </div>
                                <DialogFooter>
                                    <Button type="submit" disabled={createMutation.isPending || updateMutation.isPending}>
                                        {(createMutation.isPending || updateMutation.isPending) && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                        {editingId ? "Update Template" : "Save Template"}
                                    </Button>
                                </DialogFooter>
                            </form>
                        </DialogContent>
                    </Dialog>
                </div>
            </div>

            <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                {displayTemplates.map((template) => (
                    <Card key={template.id} className="border-border/50 bg-card/50 backdrop-blur-sm transition-all duration-300 hover:shadow-lg hover:shadow-teal-500/10 hover:border-teal-500/20 group">
                        <CardHeader>
                            <div className="flex items-start justify-between">
                                <CardTitle className="text-lg font-semibold flex items-center gap-2 truncate pr-2" title={template.name}>
                                    <Mail className="w-4 h-4 text-teal-500 flex-shrink-0" />
                                    <span className="truncate">{template.name}</span>
                                </CardTitle>
                                <div className="flex items-center gap-1 flex-shrink-0">
                                    <Button
                                        variant="ghost"
                                        size="icon"
                                        className="h-8 w-8 text-muted-foreground hover:text-primary hover:bg-primary/10 transition-colors"
                                        onClick={() => handleEdit(template)}
                                        title="Edit Template"
                                    >
                                        <Pencil className="w-4 h-4" />
                                    </Button>
                                    <Button
                                        variant="ghost"
                                        size="icon"
                                        className="h-8 w-8 text-muted-foreground hover:text-destructive hover:bg-destructive/10 transition-colors"
                                        onClick={() => setTemplateToDelete(template.id)}
                                        title="Delete Template"
                                    >
                                        <Trash2 className="w-4 h-4" />
                                    </Button>
                                </div>
                            </div>
                            <CardDescription className="line-clamp-1" title={template.subject}>
                                Subject: <span className="text-foreground font-medium">{template.subject}</span>
                            </CardDescription>
                        </CardHeader>
                        <CardContent>
                            <div className="bg-muted/50 p-3 rounded-md border border-border/50">
                                <p className="text-xs font-mono text-muted-foreground line-clamp-3">
                                    {template.bodyHtml}
                                </p>
                            </div>
                        </CardContent>
                    </Card>
                ))}

                {/* Delete Confirmation Dialog */}
                <Dialog open={!!templateToDelete} onOpenChange={(open) => !open && setTemplateToDelete(null)}>
                    <DialogContent>
                        <DialogHeader>
                            <DialogTitle>Delete Template?</DialogTitle>
                            <DialogDescription>
                                Are you sure you want to delete this template? This cannot be undone.
                            </DialogDescription>
                        </DialogHeader>
                        <DialogFooter>
                            <Button variant="outline" onClick={() => setTemplateToDelete(null)}>Cancel</Button>
                            <Button variant="destructive" onClick={() => templateToDelete && deleteMutation.mutate(templateToDelete)}>Delete</Button>
                        </DialogFooter>
                    </DialogContent>
                </Dialog>

                {displayTemplates.length === 0 && (
                    <div className="col-span-full text-center py-12 text-muted-foreground">
                        No email templates found.
                    </div>
                )}
            </div>
        </div>
    );
}
