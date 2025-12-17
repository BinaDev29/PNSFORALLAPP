import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Plus, Loader2, Smartphone, Key, Trash2 } from "lucide-react";
import { DashboardService, CreateClientApplicationRequest } from "@/services/api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Label } from "@/components/ui/label";
import { useState } from "react";
import { toast } from "sonner";
// ... imports removed ...

export default function ClientsPage() {
    const [clientToDelete, setClientToDelete] = useState<string | null>(null);
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [formData, setFormData] = useState<CreateClientApplicationRequest>({
        appId: '',
        key: '',
        name: '',
        slogan: '',
        logo: '',
        senderEmail: '',
        appPassword: ''
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
            setIsDialogOpen(false);
            setFormData({ appId: '', key: '', name: '', slogan: '', logo: '', senderEmail: '', appPassword: '' });
            queryClient.invalidateQueries({ queryKey: ['activeClients'] });
        },
        onError: () => {
            toast.error("Failed to create client application");
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

    // Remove the unused filteredClients login since searchTerm was removed
    // Just use clients directly or bring back search if needed. For now simpler is better.
    const displayClients = clients || [];



    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        createMutation.mutate(formData);
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
                    <h2 className="text-3xl font-bold tracking-tight bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent">
                        Client Applications
                    </h2>
                    <p className="text-muted-foreground">Manage connected applications and their API credentials.</p>
                </div>
                <div className="flex items-center gap-2">
                    <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
                        <DialogTrigger asChild>
                            <Button className="gap-2 bg-blue-600 hover:bg-blue-700 shadow-lg shadow-blue-500/25">
                                <Plus className="h-4 w-4" />
                                Register App
                            </Button>
                        </DialogTrigger>
                        <DialogContent className="sm:max-w-[500px]">
                            <DialogHeader>
                                <DialogTitle>Register New Application</DialogTitle>
                                <DialogDescription>
                                    Add a new client application to the system. All fields are required.
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
                                            required
                                        />
                                    </div>
                                    <div className="grid gap-2">
                                        <Label htmlFor="appPassword">App Password</Label>
                                        <Input
                                            id="appPassword"
                                            type="password"
                                            value={formData.appPassword}
                                            onChange={(e) => setFormData({ ...formData, appPassword: e.target.value })}
                                            required
                                        />
                                    </div>
                                </div>
                                <DialogFooter>
                                    <Button type="submit" disabled={createMutation.isPending}>
                                        {createMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                        Register Application
                                    </Button>
                                </DialogFooter>
                            </form>
                        </DialogContent>
                    </Dialog>
                </div>
            </div>

            <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                {displayClients.map((client) => (
                    <Card key={client.id} className="relative overflow-hidden border-border/50 bg-card/50 backdrop-blur-sm transition-all duration-300 hover:shadow-lg hover:shadow-blue-500/10 hover:border-blue-500/20 group">
                        <div className="absolute top-0 right-0 p-4 opacity-50 group-hover:opacity-100 transition-opacity flex gap-2">
                            <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8 text-destructive hover:bg-destructive/10"
                                onClick={() => setClientToDelete(client.id)}
                            >
                                <Trash2 className="w-4 h-4" />
                            </Button>
                            {client.logo ? <img src={client.logo} alt="logo" className="w-12 h-12 object-contain rounded-lg bg-white/5 p-1" /> : <Smartphone className="w-12 h-12 text-muted-foreground/20" />}
                        </div>
                        <CardHeader className="pb-2">
                            <CardTitle className="text-xl font-bold">{client.name}</CardTitle>
                            <CardDescription className="line-clamp-1">{client.slogan || "No slogan provided"}</CardDescription>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-3 mt-2">
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
                ))}
                {displayClients.length === 0 && (
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
        </div>
    );
}
