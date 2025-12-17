import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { User, Lock, Mail, Save, Loader2, ShieldCheck } from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";
import { motion, AnimatePresence } from "framer-motion";
import { cn } from "@/lib/utils";

export default function ProfilePage() {
    const [isLoading, setIsLoading] = useState(false);
    const [activeTab, setActiveTab] = useState("general");
    const [user, setUser] = useState({
        name: "Admin User",
        email: "admin@pns.com",
        currentPassword: "",
        newPassword: "",
        confirmPassword: ""
    });

    const [originalUser, setOriginalUser] = useState({
        name: "Admin User",
        email: "admin@pns.com"
    });

    const handleUpdateProfile = async (e: React.FormEvent) => {
        e.preventDefault();

        // precise change detection
        if (user.name === originalUser.name && user.email === originalUser.email) {
            toast.info("No changes detected", {
                description: "Update your profile information to save changes."
            });
            return;
        }

        setIsLoading(true);
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 1500));

        // Update the baseline after successful save
        setOriginalUser({ name: user.name, email: user.email });

        setIsLoading(false);
        toast.success("Profile updated successfully");
    };

    const handleChangePassword = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!user.currentPassword || !user.newPassword || !user.confirmPassword) {
            toast.info("No changes detected", {
                description: "Please fill in all password fields to update security settings."
            });
            return;
        }

        if (user.newPassword !== user.confirmPassword) {
            toast.error("Validation Error", {
                description: "New passwords do not match."
            });
            return;
        }

        setIsLoading(true);
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 1550));
        setIsLoading(false);
        toast.success("Password changed successfully");
        setUser({ ...user, currentPassword: "", newPassword: "", confirmPassword: "" });
    };

    return (
        <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
            <div className="flex flex-col gap-2">
                <h2 className="text-3xl font-bold tracking-tight bg-gradient-to-r from-primary to-violet-500 bg-clip-text text-transparent">
                    Profile Settings
                </h2>
                <p className="text-muted-foreground">
                    Manage your account settings and preferences.
                </p>
            </div>

            <div className="w-full">
                <div className="flex space-x-2 bg-muted/30 p-1 rounded-lg w-full max-w-md mb-6">
                    <button
                        onClick={() => setActiveTab("general")}
                        className={cn(
                            "flex-1 flex items-center justify-center gap-2 text-sm font-medium py-2 px-3 rounded-md transition-all duration-300",
                            activeTab === "general" ? "bg-background shadow-sm text-foreground" : "text-muted-foreground hover:bg-muted/50 hover:text-foreground"
                        )}
                    >
                        <User className="w-4 h-4" />
                        General Info
                    </button>
                    <button
                        onClick={() => setActiveTab("security")}
                        className={cn(
                            "flex-1 flex items-center justify-center gap-2 text-sm font-medium py-2 px-3 rounded-md transition-all duration-300",
                            activeTab === "security" ? "bg-background shadow-sm text-foreground" : "text-muted-foreground hover:bg-muted/50 hover:text-foreground"
                        )}
                    >
                        <ShieldCheck className="w-4 h-4" />
                        Security
                    </button>
                </div>

                <AnimatePresence mode="wait">
                    {activeTab === "general" ? (
                        <motion.div
                            key="general"
                            initial={{ opacity: 0, x: -20 }}
                            animate={{ opacity: 1, x: 0 }}
                            exit={{ opacity: 0, x: 20 }}
                            transition={{ duration: 0.3 }}
                        >
                            <Card className="border-border/50 bg-card/50 backdrop-blur-sm">
                                <CardHeader>
                                    <CardTitle>General Information</CardTitle>
                                    <CardDescription>
                                        Update your personal details here.
                                    </CardDescription>
                                </CardHeader>
                                <form onSubmit={handleUpdateProfile}>
                                    <CardContent className="space-y-4">
                                        <div className="grid gap-2">
                                            <Label htmlFor="name">Full Name</Label>
                                            <div className="relative">
                                                <User className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                                <Input
                                                    id="name"
                                                    className="pl-9"
                                                    value={user.name}
                                                    onChange={(e) => setUser({ ...user, name: e.target.value })}
                                                />
                                            </div>
                                        </div>
                                        <div className="grid gap-2">
                                            <Label htmlFor="email">Email Address</Label>
                                            <div className="relative">
                                                <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                                <Input
                                                    id="email"
                                                    type="email"
                                                    className="pl-9"
                                                    value={user.email}
                                                    onChange={(e) => setUser({ ...user, email: e.target.value })}
                                                />
                                            </div>
                                        </div>
                                    </CardContent>
                                    <CardFooter>
                                        <Button type="submit" disabled={isLoading} className="gap-2">
                                            {isLoading && <Loader2 className="h-4 w-4 animate-spin" />}
                                            <Save className="h-4 w-4" />
                                            Save Changes
                                        </Button>
                                    </CardFooter>
                                </form>
                            </Card>
                        </motion.div>
                    ) : (
                        <motion.div
                            key="security"
                            initial={{ opacity: 0, x: 20 }}
                            animate={{ opacity: 1, x: 0 }}
                            exit={{ opacity: 0, x: -20 }}
                            transition={{ duration: 0.3 }}
                        >
                            <Card className="border-border/50 bg-card/50 backdrop-blur-sm">
                                <CardHeader>
                                    <CardTitle>Password & Security</CardTitle>
                                    <CardDescription>
                                        Manage your password and security settings.
                                    </CardDescription>
                                </CardHeader>
                                <form onSubmit={handleChangePassword}>
                                    <CardContent className="space-y-4">
                                        <div className="grid gap-2">
                                            <Label htmlFor="current">Current Password</Label>
                                            <div className="relative">
                                                <Lock className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                                <Input
                                                    id="current"
                                                    type="password"
                                                    className="pl-9"
                                                    value={user.currentPassword}
                                                    onChange={(e) => setUser({ ...user, currentPassword: e.target.value })}
                                                />
                                            </div>
                                        </div>
                                        <div className="grid gap-2">
                                            <Label htmlFor="new">New Password</Label>
                                            <div className="relative">
                                                <Lock className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                                <Input
                                                    id="new"
                                                    type="password"
                                                    className="pl-9"
                                                    value={user.newPassword}
                                                    onChange={(e) => setUser({ ...user, newPassword: e.target.value })}
                                                />
                                            </div>
                                        </div>
                                        <div className="grid gap-2">
                                            <Label htmlFor="confirm">Confirm Password</Label>
                                            <div className="relative">
                                                <Lock className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                                <Input
                                                    id="confirm"
                                                    type="password"
                                                    className="pl-9"
                                                    value={user.confirmPassword}
                                                    onChange={(e) => setUser({ ...user, confirmPassword: e.target.value })}
                                                />
                                            </div>
                                        </div>
                                    </CardContent>
                                    <CardFooter>
                                        <Button type="submit" disabled={isLoading} className="gap-2">
                                            {isLoading && <Loader2 className="h-4 w-4 animate-spin" />}
                                            <Save className="h-4 w-4" />
                                            Update Password
                                        </Button>
                                    </CardFooter>
                                </form>
                            </Card>
                        </motion.div>
                    )}
                </AnimatePresence>
            </div>
        </div>
    );
}
