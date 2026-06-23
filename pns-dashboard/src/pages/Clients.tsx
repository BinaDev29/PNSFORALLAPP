import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { 
    Plus, 
    Search, 
    ExternalLink, 
    Shield, 
    Key, 
    Copy, 
    Check, 
    Edit, 
    Trash2, 
    Smartphone,
    Loader2
} from "lucide-react";
import { 
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Skeleton } from "@/components/ui/skeleton";
import { DashboardService, ClientApplication, CreateClientApplicationRequest } from "@/services/api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { toast } from "sonner";

const container = {
    hidden: { opacity: 0 },
    show: {
        opacity: 1,
        transition: {
            staggerChildren: 0.1
        }
    }
};

const item = {
    hidden: { y: 20, opacity: 0 },
    show: { y: 0, opacity: 1 }
};

export default function ClientsPage() {
    const queryClient = useQueryClient();
    const [searchTerm, setSearchTerm] = useState("");
    const [copiedId, setCopiedId] = useState<string | null>(null);
    const [isRegisterOpen, setIsRegisterOpen] = useState(false);
    const [isEditOpen, setIsEditOpen] = useState(false);
    const [editClient, setEditClient] = useState<ClientApplication | null>(null);

    const [newClient, setNewClient] = useState<CreateClientApplicationRequest>({
        appId: "",
        key: "",
        name: "",
        slogan: "",
        logo: "",
        senderEmail: "",
        appPassword: ""
    });

    const { data: clients, isLoading } = useQuery<ClientApplication[]>({
        queryKey: ['activeClients'],
        queryFn: DashboardService.getClientApplications
    });

    const createMutation = useMutation({
        mutationFn: DashboardService.createClientApplication,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['activeClients'] });
            toast.success("Application registered successfully");
            setIsRegisterOpen(false);
            setNewClient({ appId: "", key: "", name: "", slogan: "", logo: "", senderEmail: "", appPassword: "" });
        }
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: string, data: Partial<CreateClientApplicationRequest> }) => 
            DashboardService.updateClientApplication(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['activeClients'] });
            toast.success("Application updated successfully");
            setIsEditOpen(false);
            setEditClient(null);
        }
    });

    const deleteMutation = useMutation({
        mutationFn: DashboardService.deleteClientApplication,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['activeClients'] });
            toast.success("Application deleted successfully");
        }
    });

    const copyToClipboard = (text: string, id: string) => {
        navigator.clipboard.writeText(text);
        setCopiedId(id);
        toast.success("Copied to clipboard");
        setTimeout(() => setCopiedId(null), 2000);
    };

    const handleRegisterSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!newClient.name || !newClient.appId) {
            toast.error("Name and App ID are required");
            return;
        }
        createMutation.mutate(newClient);
    };

    const handleEditSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!editClient) return;
        
        const updateData: any = {
            name: editClient.name,
            slogan: editClient.slogan,
            logo: editClient.logo,
            senderEmail: editClient.senderEmail,
        };

        // Only send password if it's not empty (user wants to change it)
        if (editClient.appPassword && editClient.appPassword.trim() !== "") {
            updateData.appPassword = editClient.appPassword;
        }

        updateMutation.mutate({
            id: editClient.id,
            data: updateData
        });
    };

    const filteredClients = clients?.filter(client =>
        client.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        client.appId.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="space-y-8 pb-10">
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-black tracking-tight text-foreground uppercase">
                        Client Applications
                    </h2>
                    <p className="text-muted-foreground font-medium">Manage connected applications and their API credentials.</p>
                </div>
                <Dialog open={isRegisterOpen} onOpenChange={setIsRegisterOpen}>
                    <DialogTrigger asChild>
                        <Button className="gap-2 bg-primary hover:bg-primary/90 text-primary-foreground shadow-xl border-b-4 border-black/20 active:border-b-0 active:translate-y-1 transition-all font-bold">
                            <Plus className="h-5 w-5" /> Register App
                        </Button>
                    </DialogTrigger>
                    <DialogContent className="max-w-2xl bg-card border-2 border-border shadow-2xl rounded-3xl">
                        <DialogHeader>
                            <DialogTitle className="text-2xl font-black uppercase tracking-tight">Register New App</DialogTitle>
                            <DialogDescription className="font-medium text-muted-foreground">Register your application to start using the PNS services.</DialogDescription>
                        </DialogHeader>
                        <form onSubmit={handleRegisterSubmit} className="space-y-6 pt-4">
                            <div className="grid gap-6 md:grid-cols-2">
                                <div className="space-y-2">
                                    <Label className="text-xs font-black uppercase tracking-widest text-primary">Application Name</Label>
                                    <Input 
                                        placeholder="e.g. Finance Portal" 
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={newClient.name}
                                        onChange={(e) => setNewClient({...newClient, name: e.target.value})}
                                    />
                                </div>
                                <div className="space-y-2">
                                    <Label className="text-xs font-black uppercase tracking-widest text-primary">App ID</Label>
                                    <Input 
                                        placeholder="e.g. finance-app" 
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={newClient.appId}
                                        onChange={(e) => setNewClient({...newClient, appId: e.target.value})}
                                    />
                                </div>
                            </div>
                            <div className="space-y-2">
                                <Label className="text-xs font-black uppercase tracking-widest text-primary">Slogan / Description</Label>
                                <Input 
                                    placeholder="The ultimate finance solution" 
                                    className="bg-muted/30 border-2 border-border/50 font-bold"
                                    value={newClient.slogan}
                                    onChange={(e) => setNewClient({...newClient, slogan: e.target.value})}
                                />
                            </div>
                            <div className="grid gap-6 md:grid-cols-2">
                                <div className="space-y-2">
                                    <Label className="text-xs font-black uppercase tracking-widest text-primary">Sender Email</Label>
                                    <Input 
                                        type="email"
                                        placeholder="notifications@company.com" 
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={newClient.senderEmail}
                                        onChange={(e) => setNewClient({...newClient, senderEmail: e.target.value})}
                                    />
                                </div>
                                <div className="space-y-2">
                                    <Label className="text-xs font-black uppercase tracking-widest text-primary">App Password</Label>
                                    <Input 
                                        type="password"
                                        placeholder="••••••••" 
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={newClient.appPassword}
                                        onChange={(e) => setNewClient({...newClient, appPassword: e.target.value})}
                                    />
                                </div>
                            </div>
                            <DialogFooter className="pt-4 border-t border-border">
                                <Button type="button" variant="outline" onClick={() => setIsRegisterOpen(false)} className="font-bold border-2">Cancel</Button>
                                <Button type="submit" disabled={createMutation.isPending} className="bg-primary text-primary-foreground font-bold shadow-lg">
                                    {createMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                    Register Application
                                </Button>
                            </DialogFooter>
                        </form>
                    </DialogContent>
                </Dialog>
            </div>

            <div className="relative max-w-md">
                <Search className="absolute left-3 top-3 h-5 w-5 text-muted-foreground" />
                <input
                    type="text"
                    placeholder="Search applications..."
                    className="w-full bg-card border-2 border-border rounded-xl py-2.5 pl-10 pr-4 focus:ring-2 focus:ring-primary/50 focus:border-primary transition-all outline-none text-sm font-bold shadow-sm"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />
            </div>

            <motion.div
                variants={container}
                initial="hidden"
                animate="show"
                className="grid gap-6 md:grid-cols-2 lg:grid-cols-3"
            >
                {isLoading ? (
                    Array.from({ length: 3 }).map((_, i) => (
                        <Card key={i} className="border-2 border-border bg-card shadow-lg overflow-hidden">
                            <CardHeader><Skeleton className="h-20 w-full rounded-xl" /></CardHeader>
                            <CardContent className="space-y-4">
                                <Skeleton className="h-10 w-full" />
                                <Skeleton className="h-20 w-full" />
                            </CardContent>
                        </Card>
                    ))
                ) : (
                    <AnimatePresence mode="popLayout">
                        {filteredClients?.map((client) => (
                            <motion.div key={client.id} variants={item} layout>
                                <Card className="h-full border-2 border-border bg-card hover:bg-muted/50 transition-all duration-300 shadow-xl relative group overflow-hidden">
                                    <div className="absolute top-0 left-0 w-1 h-full bg-primary" />
                                    
                                    <CardHeader className="pb-2">
                                        <div className="flex items-start justify-between">
                                            <div className="flex items-center gap-4">
                                                <div className="h-14 w-14 rounded-2xl bg-muted border-2 border-border/50 flex items-center justify-center overflow-hidden group-hover:border-primary/50 transition-colors shadow-inner">
                                                    {client.logo ? (
                                                        <img src={client.logo} alt={client.name} className="h-full w-full object-cover" />
                                                    ) : (
                                                        <Smartphone className="h-7 w-7 text-primary" />
                                                    )}
                                                </div>
                                                <div>
                                                    <CardTitle className="text-xl font-black text-foreground">{client.name}</CardTitle>
                                                    <div className="flex items-center gap-1.5 mt-1">
                                                        <Badge variant="outline" className="bg-muted/50 text-[10px] font-bold tracking-tighter border-border/50">
                                                            {client.appId}
                                                        </Badge>
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="flex gap-1">
                                                <Button 
                                                    variant="ghost" 
                                                    size="icon" 
                                                    className="h-8 w-8 text-muted-foreground hover:text-foreground hover:bg-muted"
                                                    onClick={() => {
                                                        setEditClient(client);
                                                        setIsEditOpen(true);
                                                    }}
                                                >
                                                    <Edit className="h-4 w-4" />
                                                </Button>
                                                <Button 
                                                    variant="ghost" 
                                                    size="icon" 
                                                    className="h-8 w-8 text-muted-foreground hover:text-destructive hover:bg-destructive/10"
                                                    onClick={() => {
                                                        if (window.confirm("Are you sure you want to delete this application?")) {
                                                            deleteMutation.mutate(client.id);
                                                        }
                                                    }}
                                                >
                                                    <Trash2 className="h-4 w-4" />
                                                </Button>
                                            </div>
                                        </div>
                                    </CardHeader>

                                    <CardContent className="space-y-5 pt-2">
                                        <p className="text-sm text-muted-foreground line-clamp-2 font-medium italic">
                                            {client.slogan || "No description provided for this application."}
                                        </p>

                                        <div className="space-y-3">
                                            <div className="bg-muted/30 rounded-xl p-3 border-2 border-border/50 group-hover:border-primary/30 transition-colors">
                                                <div className="flex items-center justify-between mb-2">
                                                    <div className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-primary">
                                                        <Shield className="h-3 w-3" /> API Credentials
                                                    </div>
                                                </div>
                                                <div className="space-y-2">
                                                    <div className="flex items-center justify-between group/key bg-card p-2 rounded-lg border border-border/30 shadow-sm">
                                                        <div className="flex items-center gap-2">
                                                            <Key className="h-3 w-3 text-muted-foreground" />
                                                            <span className="text-[11px] font-mono text-foreground">App ID</span>
                                                        </div>
                                                        <Button 
                                                            variant="ghost" 
                                                            size="icon" 
                                                            className="h-6 w-6"
                                                            onClick={() => copyToClipboard(client.appId, client.id + 'id')}
                                                        >
                                                            {copiedId === client.id + 'id' ? <Check className="h-3 w-3 text-emerald-500" /> : <Copy className="h-3 w-3" />}
                                                        </Button>
                                                    </div>
                                                    <div className="flex items-center justify-between group/key bg-card p-2 rounded-lg border border-border/30 shadow-sm">
                                                        <div className="flex items-center gap-2">
                                                            <Shield className="h-3 w-3 text-muted-foreground" />
                                                            <span className="text-[11px] font-mono text-foreground">Password</span>
                                                        </div>
                                                        <Button 
                                                            variant="ghost" 
                                                            size="icon" 
                                                            className="h-6 w-6"
                                                            onClick={() => copyToClipboard(client.appPassword || "********", client.id + 'pass')}
                                                        >
                                                            {copiedId === client.id + 'pass' ? <Check className="h-3 w-3 text-emerald-500" /> : <Copy className="h-3 w-3" />}
                                                        </Button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div className="flex items-center justify-between pt-2">
                                            <Badge className="bg-emerald-500/10 text-emerald-600 hover:bg-emerald-500/20 border-none px-3 py-1 font-black text-[10px] uppercase">
                                                Active System
                                            </Badge>
                                            <Button variant="link" className="text-primary p-0 h-auto text-xs font-bold group/link">
                                                View Docs <ExternalLink className="ml-1 h-3 w-3 transition-transform group-hover/link:translate-x-0.5 group-hover/link:-translate-y-0.5" />
                                            </Button>
                                        </div>
                                    </CardContent>
                                </Card>
                            </motion.div>
                        ))}
                    </AnimatePresence>
                )}
            </motion.div>

            {/* Edit Dialog */}
            <Dialog open={isEditOpen} onOpenChange={setIsEditOpen}>
                <DialogContent className="max-w-2xl bg-card border-2 border-border shadow-2xl rounded-3xl">
                    <DialogHeader>
                        <DialogTitle className="text-2xl font-black uppercase tracking-tight">Edit Application</DialogTitle>
                        <DialogDescription className="font-medium text-muted-foreground">Update your application's profile and credentials.</DialogDescription>
                    </DialogHeader>
                    {editClient && (
                        <form onSubmit={handleEditSubmit} className="space-y-6 pt-4">
                            <div className="grid gap-6 md:grid-cols-2">
                                <div className="space-y-2">
                                    <Label className="text-xs font-black uppercase tracking-widest text-primary">Application Name</Label>
                                    <Input 
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={editClient.name}
                                        onChange={(e) => setEditClient({...editClient, name: e.target.value})}
                                    />
                                </div>
                                <div className="space-y-2">
                                    <Label className="text-xs font-black uppercase tracking-widest text-primary">App ID (Read-only)</Label>
                                    <Input 
                                        disabled
                                        className="bg-muted/10 border-2 border-border/30 font-mono text-xs"
                                        value={editClient.appId}
                                    />
                                </div>
                            </div>
                            <div className="space-y-2">
                                <Label className="text-xs font-black uppercase tracking-widest text-primary">Slogan / Description</Label>
                                <Input 
                                    className="bg-muted/30 border-2 border-border/50 font-bold"
                                    value={editClient.slogan || ""}
                                    onChange={(e) => setEditClient({...editClient, slogan: e.target.value})}
                                />
                            </div>
                            <div className="grid gap-6 md:grid-cols-2">
                                <div className="space-y-2">
                                    <Label className="text-xs font-black uppercase tracking-widest text-primary">Sender Email</Label>
                                    <Input 
                                        type="email"
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={editClient.senderEmail || ""}
                                        onChange={(e) => setEditClient({...editClient, senderEmail: e.target.value})}
                                    />
                                </div>
                                <div className="space-y-2">
                                    <Label className="text-xs font-black uppercase tracking-widest text-primary">App Password</Label>
                                    <Input 
                                        type="password"
                                        placeholder="Leave empty to keep current"
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={editClient.appPassword || ""}
                                        onChange={(e) => setEditClient({...editClient, appPassword: e.target.value})}
                                    />
                                </div>
                            </div>
                            <DialogFooter className="pt-4 border-t border-border">
                                <Button type="button" variant="outline" onClick={() => setIsEditOpen(false)} className="font-bold border-2">Cancel</Button>
                                <Button type="submit" disabled={updateMutation.isPending} className="bg-blue-600 hover:bg-blue-700 text-white font-bold shadow-lg">
                                    {updateMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                    Save Changes
                                </Button>
                            </DialogFooter>
                        </form>
                    )}
                </DialogContent>
            </Dialog>
        </div>
    );
}
