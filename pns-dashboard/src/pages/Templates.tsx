import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { 
    Plus, 
    Mail, 
    Edit, 
    Trash2, 
    Eye, 
    Send,
    Layout,
    CheckCircle2,
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
import { Textarea } from "@/components/ui/textarea";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { DashboardService, EmailTemplate, CreateEmailTemplateRequest } from "@/services/api";
import { useState } from "react";
import { toast } from "sonner";
import { motion, AnimatePresence } from "framer-motion";
import { Skeleton } from "@/components/ui/skeleton";

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

export default function TemplatesPage() {
    const queryClient = useQueryClient();
    const [isPreviewOpen, setIsPreviewOpen] = useState(false);
    const [selectedTemplate, setSelectedTemplate] = useState<EmailTemplate | null>(null);
    const [isCreateOpen, setIsCreateOpen] = useState(false);

    // Form state for new template
    const [newTemplate, setNewTemplate] = useState<CreateEmailTemplateRequest>({
        name: "",
        subject: "",
        bodyHtml: "",
        bodyText: ""
    });

    const { data: templates, isLoading } = useQuery<EmailTemplate[]>({
        queryKey: ['emailTemplates'],
        queryFn: DashboardService.getEmailTemplates
    });

    const createMutation = useMutation({
        mutationFn: DashboardService.createEmailTemplate,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['emailTemplates'] });
            toast.success("Template created successfully");
            setIsCreateOpen(false);
            setNewTemplate({ name: "", subject: "", bodyHtml: "", bodyText: "" });
        }
    });

    const deleteMutation = useMutation({
        mutationFn: DashboardService.deleteEmailTemplate,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['emailTemplates'] });
            toast.success("Template deleted successfully");
        }
    });

    const handleCreateSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!newTemplate.name || !newTemplate.subject || !newTemplate.bodyHtml) {
            toast.error("Please fill all required fields");
            return;
        }
        createMutation.mutate(newTemplate);
    };

    return (
        <div className="space-y-8 pb-10">
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
                <div className="space-y-1">
                    <h2 className="text-3xl font-black tracking-tight text-foreground uppercase">
                        Email Templates
                    </h2>
                    <p className="text-muted-foreground font-medium">Design and manage professional email templates for your notifications.</p>
                </div>
                
                <Dialog open={isCreateOpen} onOpenChange={setIsCreateOpen}>
                    <DialogTrigger asChild>
                        <Button className="gap-2 bg-emerald-500 hover:bg-emerald-600 text-white shadow-xl border-b-4 border-emerald-700 active:border-b-0 active:translate-y-1 transition-all font-bold">
                            <Plus className="h-5 w-5" /> Create Template
                        </Button>
                    </DialogTrigger>
                    <DialogContent className="max-w-2xl bg-card border-2 border-border shadow-2xl rounded-3xl overflow-hidden">
                        <DialogHeader>
                            <DialogTitle className="text-2xl font-black uppercase tracking-tight text-foreground">Create New Template</DialogTitle>
                            <DialogDescription className="font-bold text-muted-foreground">Define your email structure and layout.</DialogDescription>
                        </DialogHeader>
                        <form onSubmit={handleCreateSubmit} className="space-y-6 pt-4">
                            <div className="grid gap-6 md:grid-cols-2">
                                <div className="space-y-2">
                                    <Label htmlFor="name" className="text-xs font-black uppercase tracking-widest text-primary">Template Name</Label>
                                    <Input 
                                        id="name" 
                                        placeholder="e.g. Welcome Email" 
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={newTemplate.name}
                                        onChange={(e) => setNewTemplate({...newTemplate, name: e.target.value})}
                                    />
                                </div>
                                <div className="space-y-2">
                                    <Label htmlFor="subject" className="text-xs font-black uppercase tracking-widest text-primary">Email Subject</Label>
                                    <Input 
                                        id="subject" 
                                        placeholder="e.g. Welcome to PNS!" 
                                        className="bg-muted/30 border-2 border-border/50 font-bold"
                                        value={newTemplate.subject}
                                        onChange={(e) => setNewTemplate({...newTemplate, subject: e.target.value})}
                                    />
                                </div>
                            </div>
                            <div className="space-y-2">
                                <Label htmlFor="bodyHtml" className="text-xs font-black uppercase tracking-widest text-primary">HTML Content</Label>
                                <Textarea 
                                    id="bodyHtml" 
                                    placeholder="<h1>Hello {{name}}</h1>" 
                                    className="min-h-[200px] bg-muted/20 font-mono text-xs border-2 border-border/50"
                                    value={newTemplate.bodyHtml}
                                    onChange={(e) => setNewTemplate({...newTemplate, bodyHtml: e.target.value})}
                                />
                            </div>
                            <DialogFooter className="pt-4 border-t border-border">
                                <Button type="button" variant="outline" onClick={() => setIsCreateOpen(false)} className="font-bold border-2">Cancel</Button>
                                <Button type="submit" disabled={createMutation.isPending} className="bg-emerald-500 hover:bg-emerald-600 text-white font-bold shadow-lg shadow-emerald-500/20">
                                    {createMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                    Create Template
                                </Button>
                            </DialogFooter>
                        </form>
                    </DialogContent>
                </Dialog>
            </div>

            <motion.div
                variants={container}
                initial="hidden"
                animate="show"
                className="grid gap-6 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-3"
            >
                {isLoading ? (
                    Array.from({ length: 4 }).map((_, i) => (
                        <Card key={i} className="border-2 border-border bg-card shadow-lg overflow-hidden">
                            <CardHeader><Skeleton className="h-24 w-full" /></CardHeader>
                            <CardContent className="space-y-4">
                                <Skeleton className="h-32 w-full" />
                            </CardContent>
                        </Card>
                    ))
                ) : (
                    <AnimatePresence>
                        {templates?.map((template) => (
                            <motion.div key={template.id} variants={item} layout>
                                <Card className="h-full border-2 border-border bg-card hover:bg-muted/30 transition-all duration-300 shadow-xl relative group overflow-hidden">
                                    <div className="absolute top-0 left-0 w-1.5 h-full bg-emerald-500" />
                                    
                                    <CardHeader className="pb-3">
                                        <div className="flex items-start justify-between">
                                            <div className="flex items-center gap-4">
                                                <div className="h-12 w-12 rounded-xl bg-emerald-500/10 border-2 border-emerald-500/20 flex items-center justify-center text-emerald-500 group-hover:scale-110 transition-transform shadow-inner">
                                                    <Mail className="h-6 w-6" />
                                                </div>
                                                <div>
                                                    <div className="flex items-center gap-2">
                                                        <CardTitle className="text-lg font-black text-foreground">{template.name}</CardTitle>
                                                        <Eye className="w-4 h-4 text-muted-foreground cursor-pointer hover:text-emerald-500" onClick={() => {
                                                            setSelectedTemplate(template);
                                                            setIsPreviewOpen(true);
                                                        }} />
                                                        <Edit className="w-4 h-4 text-muted-foreground cursor-pointer hover:text-blue-500" />
                                                    </div>
                                                    <div className="flex items-center gap-1 mt-1 text-emerald-500/80">
                                                        <CheckCircle2 className="w-3 h-3" />
                                                        <span className="text-[10px] font-black uppercase tracking-widest">Verified Template</span>
                                                    </div>
                                                </div>
                                            </div>
                                            <Button 
                                                variant="ghost" 
                                                size="icon" 
                                                className="h-8 w-8 text-muted-foreground hover:text-destructive hover:bg-destructive/10"
                                                onClick={() => {
                                                    if (confirm("Are you sure you want to delete this template?")) {
                                                        deleteMutation.mutate(template.id);
                                                    }
                                                }}
                                            >
                                                <Trash2 className="h-4 w-4" />
                                            </Button>
                                        </div>
                                    </CardHeader>

                                    <CardContent className="space-y-4 pt-2">
                                        <div className="space-y-2 p-4 rounded-xl bg-muted/40 border-2 border-border/50 group-hover:border-emerald-500/30 transition-colors">
                                            <div className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-muted-foreground mb-1">
                                                Subject
                                            </div>
                                            <p className="text-sm font-black text-foreground truncate">
                                                {template.subject}
                                            </p>
                                        </div>

                                        <div className="space-y-2 p-4 rounded-xl bg-muted/20 border-2 border-border/30 border-dashed">
                                            <div className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-muted-foreground mb-1">
                                                <Layout className="w-3 h-3" /> Structure
                                            </div>
                                            <div className="text-[11px] font-mono text-muted-foreground line-clamp-3 bg-card/50 p-2 rounded-lg border border-border/20">
                                                {template.bodyHtml}
                                            </div>
                                        </div>

                                        <div className="flex items-center justify-between pt-2">
                                            <span className="text-[10px] text-muted-foreground font-black uppercase tracking-widest opacity-40">Dynamic Data Support</span>
                                            <Button variant="ghost" className="h-8 gap-2 text-[10px] font-black uppercase tracking-widest text-emerald-500 hover:text-emerald-600 hover:bg-emerald-500/5 group/btn">
                                                Test Send <Send className="w-3 h-3 transition-transform group-hover/btn:translate-x-0.5 group-hover/btn:-translate-y-0.5" />
                                            </Button>
                                        </div>
                                    </CardContent>
                                </Card>
                            </motion.div>
                        ))}
                    </AnimatePresence>
                )}
            </motion.div>

            {/* Preview Dialog */}
            <Dialog open={isPreviewOpen} onOpenChange={setIsPreviewOpen}>
                <DialogContent className="max-w-3xl max-h-[80vh] overflow-hidden flex flex-col bg-card border-2 border-border shadow-2xl rounded-3xl">
                    <DialogHeader className="border-b border-border pb-4 px-2">
                        <DialogTitle className="text-2xl font-black uppercase tracking-tight text-foreground">{selectedTemplate?.name}</DialogTitle>
                        <DialogDescription className="font-bold text-muted-foreground">Previewing the final compiled email structure.</DialogDescription>
                    </DialogHeader>
                    <div className="flex-1 overflow-y-auto p-6 bg-muted/10">
                        <div className="bg-card p-8 rounded-2xl border-2 border-border shadow-sm min-h-[400px]">
                            <div className="mb-8 border-b border-border pb-4">
                                <p className="text-xs font-black uppercase tracking-widest text-muted-foreground mb-2">Subject</p>
                                <p className="text-lg font-black text-foreground">{selectedTemplate?.subject}</p>
                            </div>
                            <div 
                                className="prose prose-sm dark:prose-invert max-w-none text-foreground"
                                dangerouslySetInnerHTML={{ __html: selectedTemplate?.bodyHtml || "" }}
                            />
                        </div>
                    </div>
                    <DialogFooter className="border-t border-border pt-4 px-2 bg-muted/20">
                        <Button variant="outline" onClick={() => setIsPreviewOpen(false)} className="font-bold border-2 border-border">Close Preview</Button>
                        <Button className="bg-emerald-500 hover:bg-emerald-600 text-white font-bold shadow-lg shadow-emerald-500/20">Edit Template</Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </div>
    );
}
