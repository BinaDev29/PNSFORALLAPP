import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Plus, Loader2, Smartphone, Key, Trash2, Pencil, Eye, EyeOff } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import { DashboardService, CreateClientApplicationRequest, ClientApplication } from "@/services/api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Label } from "@/components/ui/label";
import { useState } from "react";
import { toast } from "sonner";
// ... imports removed ...

export default function ClientsPage() {
    const [clientToDelete, setClientToDelete] = useState<string | null>(null);
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [editingId, setEditingId] = useState<string | null>(null);
    const [showAppPassword, setShowAppPassword] = useState(false);
    const [formData, setFormData] = useState<CreateClientApplicationRequest>({
        appId: '',
        key: '',
        name: '',
        slogan: '',
        logo: '',
        senderEmail: '',
        appPassword: '',
        smsSenderName: '',
        smsSenderNumber: ''
    });

    const queryClient = useQueryClient();

    const { data: clients, isLoading } = useQuery({
        queryKey: ['activeClients'],
        queryFn: DashboardService.getClientApplications
    });

    const createMutation = useMutation({
        mutationFn: DashboardService.createClientApplication,
        onSuccess: () => {
            toast.success("Client application created successfully");
            handleCloseDialog();
            queryClient.invalidateQueries({ queryKey: ['activeClients'] });
        },
        onError: () => {
            toast.error("Failed to create client application");
        }
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: string, data: CreateClientApplicationRequest }) => DashboardService.updateClientApplication(id, data),
        onSuccess: () => {
            toast.success("Client application updated successfully");
            handleCloseDialog();
            queryClient.invalidateQueries({ queryKey: ['activeClients'] });
        },
        onError: () => {
            toast.error("Failed to update client application");
        }
    });

    const deleteMutation = useMutation({
        mutationFn: DashboardService.deleteClientApplication,
        onSuccess: () => {
            toast.success("Application deleted successfully");
            setClientToDelete(null);
            queryClient.invalidateQueries({ queryKey: ['activeClients'] });
        },
        onError: (error: any) => {
            console.error("Delete error:", error);
            let errorMessage = error?.response?.data?.message || error?.response?.data || error?.message || "Failed to delete application";

            if (typeof errorMessage === 'string' && (errorMessage.includes("entity changes") || errorMessage.includes("inner exception") || errorMessage.includes("REFERENCE constraint"))) {
                errorMessage = "Cannot delete application: It is currently being used by existing notifications or templates. Please delete associated data first.";
            }

            toast.error(errorMessage);
        }
    });

    const displayClients = clients || [];

    const handleOpenCreate = () => {
        setEditingId(null);
        setFormData({ appId: '', key: '', name: '', slogan: '', logo: '', senderEmail: '', appPassword: '', smsSenderName: '', smsSenderNumber: '' });
        setIsDialogOpen(true);
    };

    const handleOpenEdit = (client: ClientApplication) => {
        setEditingId(client.id);
        // We need to map ClientApplication to CreateClientApplicationRequest format.
        // Assuming client contains senderEmail and appPassword if fetched, otherwise we might need to handle empty/masked values.
        // Note: The ClientApplication interface in api.ts doesn't show senderEmail and appPassword.
        // If they are not returned by the API, we can't edit them easily without clearing them or asking user to re-enter.
        // For now, let's assume they might be there or we handle them as optional/empty.
        setFormData({
            appId: client.appId,
            key: client.key,
            name: client.name,
            slogan: client.slogan || '',
            logo: client.logo || '',
            senderEmail: '', // Cannot pre-fill sensitive/missing data if API doesn't return it
            appPassword: '',  // password should definitely not be filled back
            smsSenderName: client.smsSenderName || '',
            smsSenderNumber: client.smsSenderNumber || ''
        });
        setIsDialogOpen(true);
    };

    const handleCloseDialog = () => {
        setIsDialogOpen(false);
        setEditingId(null);
        setFormData({ appId: '', key: '', name: '', slogan: '', logo: '', senderEmail: '', appPassword: '', smsSenderName: '', smsSenderNumber: '' });
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (editingId) {
            updateMutation.mutate({ id: editingId, data: formData });
        } else {
            createMutation.mutate(formData);
        }
    };


    return (
        <div className="space-y-8">
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-bold tracking-tight text-blue-600">
                        Client Applications
                    </h2>
                    <p className="text-muted-foreground">Manage connected applications and their API credentials.</p>
                </div>
                <div className="flex items-center gap-2">
                    <Dialog open={isDialogOpen} onOpenChange={(open) => !open && handleCloseDialog()}>
                        <DialogTrigger asChild>
                            <Button
                                onClick={handleOpenCreate}
                                className="gap-2 bg-blue-600 hover:bg-blue-700 shadow-lg shadow-blue-500/25"
                            >
                                <Plus className="h-4 w-4" />
                                Register App
                            </Button>
                        </DialogTrigger>
                        <DialogContent className="sm:max-w-[500px]">
                            <DialogHeader>
                                <DialogTitle>{editingId ? "Edit Application" : "Register New Application"}</DialogTitle>
                                <DialogDescription>
                                    {editingId ? "Update existing client application details." : "Add a new client application to the system. All fields are required."}
                                </DialogDescription>
                            </DialogHeader>
                            <form onSubmit={handleSubmit} className="grid gap-4 py-4">
                                <div className="grid grid-cols-2 gap-4">
                                    <div className="grid gap-2">
                                        <Label htmlFor="name">App Name</Label>
                                        <Input
                                            id="name"
                                            value={formData.name}
                                            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                                            required
                                        />
                                    </div>
                                    <div className="grid gap-2">
                                        <Label htmlFor="appId">App ID</Label>
                                        <Input
                                            id="appId"
                                            value={formData.appId}
                                            onChange={(e) => setFormData({ ...formData, appId: e.target.value })}
                                            required
                                        />
                                    </div>
                                </div>
                                <div className="grid gap-2">
                                    <Label htmlFor="key">API Key</Label>
                                    <Input
                                        id="key"
                                        value={formData.key}
                                        onChange={(e) => setFormData({ ...formData, key: e.target.value })}
                                        required
                                    />
                                </div>
                                <div className="grid gap-2">
                                    <Label htmlFor="slogan">Slogan</Label>
                                    <Input
                                        id="slogan"
                                        value={formData.slogan}
                                        onChange={(e) => setFormData({ ...formData, slogan: e.target.value })}
                                        required
                                    />
                                </div>
                                <div className="grid gap-2">
                                    <Label htmlFor="logo">Logo URL</Label>
                                    <Input
                                        id="logo"
                                        value={formData.logo}
                                        onChange={(e) => setFormData({ ...formData, logo: e.target.value })}
                                        required
                                    />
                                </div>
                                <div className="grid grid-cols-2 gap-4">
                                    <div className="grid gap-2">
                                        <Label htmlFor="senderEmail">Sender Email</Label>
                                        <Input
                                            id="senderEmail"
                                            type="email"
                                            value={formData.senderEmail}
                                            onChange={(e) => setFormData({ ...formData, senderEmail: e.target.value })}
                                            required={!editingId} // Only required for new creation if we assume edit doesn't require re-entry unless changing
                                            placeholder={editingId ? "(Leave empty to keep unchanged)" : ""}
                                        />
                                    </div>
                                    <div className="grid gap-2">
                                        <Label htmlFor="appPassword">App Password</Label>
                                        <div className="relative">
                                            <Input
                                                id="appPassword"
                                                type={showAppPassword ? "text" : "password"}
                                                value={formData.appPassword}
                                                onChange={(e) => setFormData({ ...formData, appPassword: e.target.value })}
                                                required={!editingId}
                                                className="pr-10"
                                                placeholder={editingId ? "(Leave empty to keep unchanged)" : ""}
                                            />
                                            <Button
                                                type="button"
                                                variant="ghost"
                                                size="sm"
                                                className="absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent text-muted-foreground hover:text-foreground"
                                                onClick={() => setShowAppPassword(!showAppPassword)}
                                            >
                                                {showAppPassword ? (
                                                    <EyeOff className="h-4 w-4" />
                                                ) : (
                                                    <Eye className="h-4 w-4" />
                                                )}
                                            </Button>
                                        </div>
                                    </div>
                                </div>
                                <div className="grid grid-cols-2 gap-4">
                                    <div className="grid gap-2">
                                        <Label htmlFor="smsSenderName">SMS Sender Name</Label>
                                        <Input
                                            id="smsSenderName"
                                            value={formData.smsSenderName}
                                            onChange={(e) => setFormData({ ...formData, smsSenderName: e.target.value })}
                                            placeholder="Optional"
                                        />
                                    </div>
                                    <div className="grid gap-2">
                                        <Label htmlFor="smsSenderNumber">SMS Sender Number</Label>
                                        <Input
                                            id="smsSenderNumber"
                                            value={formData.smsSenderNumber}
                                            onChange={(e) => setFormData({ ...formData, smsSenderNumber: e.target.value })}
                                            placeholder="e.g. +1234567890"
                                        />
                                    </div>
                                </div>
                                <DialogFooter>
                                    <Button type="submit" disabled={createMutation.isPending || updateMutation.isPending}>
                                        {(createMutation.isPending || updateMutation.isPending) && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                        {editingId ? "Update Application" : "Register Application"}
                                    </Button>
                                </DialogFooter>
                            </form>
                        </DialogContent>
                    </Dialog>
                </div>
            </div>

            <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                {isLoading ? (
                    Array.from({ length: 3 }).map((_, i) => (
                        <Card key={i} className="relative overflow-hidden border-border/50 bg-card/50 backdrop-blur-sm">
                            <CardHeader className="flex flex-row items-start justify-between space-y-0 pb-2 gap-2">
                                <div className="flex-1 space-y-2">
                                    <Skeleton className="h-6 w-3/4" />
                                    <Skeleton className="h-4 w-1/2" />
                                </div>
                                <Skeleton className="h-12 w-12 rounded-lg" />
                            </CardHeader>
                            <CardContent>
                                <div className="space-y-3 mt-1">
                                    <Skeleton className="h-10 w-full" />
                                    <Skeleton className="h-4 w-1/3" />
                                </div>
                            </CardContent>
                        </Card>
                    ))
                ) : (
                    displayClients.map((client) => (
                        <Card key={client.id} className="relative overflow-hidden border-border/50 bg-card/50 backdrop-blur-sm transition-all duration-300 hover:shadow-lg hover:shadow-blue-500/10 hover:border-blue-500/20 group">
                            <CardHeader className="flex flex-row items-start justify-between space-y-0 pb-2 gap-2">
                                <div className="flex-1 min-w-0 space-y-1">
                                    <CardTitle className="text-xl font-bold truncate pr-1" title={client.name}>
                                        {client.name}
                                    </CardTitle>
                                    <CardDescription className="line-clamp-2 text-xs">
                                        {client.slogan || "No slogan provided"}
                                    </CardDescription>
                                </div>
                                <div className="flex flex-col items-end gap-2 flex-shrink-0">
                                    <div className="flex gap-1">
                                        <Button
                                            variant="ghost"
                                            size="icon"
                                            className="h-8 w-8 text-primary hover:bg-primary/10 transition-opacity"
                                            onClick={() => handleOpenEdit(client)}
                                            title="Edit Application"
                                        >
                                            <Pencil className="w-4 h-4" />
                                        </Button>
                                        <Button
                                            variant="ghost"
                                            size="icon"
                                            className="h-8 w-8 text-destructive hover:bg-destructive/10 transition-opacity"
                                            onClick={() => setClientToDelete(client.id)}
                                            title="Delete Application"
                                        >
                                            <Trash2 className="w-4 h-4" />
                                        </Button>
                                    </div>
                                    <div className="h-12 w-12 flex items-center justify-center rounded-lg bg-white/5 p-1 border border-border/10">
                                        {client.logo ? (
                                            <img src={client.logo} alt="logo" className="w-full h-full object-contain rounded-md" />
                                        ) : (
                                            <Smartphone className="w-8 h-8 text-muted-foreground/20" />
                                        )}
                                    </div>
                                </div>
                            </CardHeader>
                            <CardContent>
                                <div className="space-y-3 mt-1">
                                    <div className="flex items-center gap-2 text-sm text-muted-foreground p-2 rounded-md bg-secondary/50">
                                        <Key className="w-4 h-4 flex-shrink-0" />
                                        <code className="text-xs truncate font-mono">{client.key}</code>
                                    </div>
                                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                        <div className="font-semibold text-xs uppercase tracking-wider text-muted-foreground/70">App ID:</div>
                                        <div className="font-mono text-xs">{client.appId}</div>
                                    </div>
                                </div>
                            </CardContent>
                        </Card>
                    ))
                )}
                {!isLoading && displayClients.length === 0 && (
                    <div className="col-span-full text-center py-12 text-muted-foreground">
                        No client applications found.
                    </div>
                )}
            </div>

            <Dialog open={!!clientToDelete} onOpenChange={(open) => !open && setClientToDelete(null)}>
                <DialogContent>
                    <DialogHeader>
                        <DialogTitle>Delete Application?</DialogTitle>
                        <DialogDescription>
                            Are you sure you want to delete this application? This cannot be undone.
                        </DialogDescription>
                    </DialogHeader>
                    <DialogFooter>
                        <Button variant="outline" onClick={() => setClientToDelete(null)}>Cancel</Button>
                        <Button variant="destructive" onClick={() => clientToDelete && deleteMutation.mutate(clientToDelete)} disabled={deleteMutation.isPending}>
                            {deleteMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                            Delete
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </div >
    );
}
