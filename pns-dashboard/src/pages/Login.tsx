import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useAuth } from "@/contexts/AuthContext";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";
import { Loader2, Lock, Mail } from "lucide-react";

export default function LoginPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);

        try {
            // In a real app, we would make an API call here.
            // For now, we'll simulate a login.
            // await DashboardService.login(email, password); 

            // Simple validation simulation
            if (email && password) {
                // Simulate delay
                await new Promise(resolve => setTimeout(resolve, 1000));

                // Assuming successful login returns a token
                const mockToken = "mock-jwt-token-" + Date.now();
                login(mockToken);

                toast.success("Welcome back!", {
                    description: "You have successfully logged in."
                });

                navigate("/");
            } else {
                toast.error("Validation Error", {
                    description: "Please enter both email and password."
                });
            }
        } catch (error) {
            console.error(error);
            toast.error("Login Failed", {
                description: "Invalid email or password. Please try again."
            });
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-background relative overflow-hidden">
            {/* Background Effects */}
            <div className="fixed inset-0 -z-10 h-full w-full bg-background" />
            <div className="fixed inset-0 -z-10 h-full w-full bg-[linear-gradient(to_right,#8080800a_1px,transparent_1px),linear-gradient(to_bottom,#8080800a_1px,transparent_1px)] bg-[size:24px_24px]"></div>
            <div className="fixed left-[-20%] top-[-10%] -z-10 h-[500px] w-[500px] rounded-full bg-primary/20 blur-[100px] opacity-30"></div>
            <div className="fixed right-[-20%] bottom-[-10%] -z-10 h-[500px] w-[500px] rounded-full bg-blue-500/20 blur-[100px] opacity-30"></div>

            <Card className="w-full max-w-md border-border/50 bg-card/50 backdrop-blur-xl shadow-2xl">
                <CardHeader className="space-y-1 text-center">
                    <CardTitle className="text-3xl font-bold tracking-tight bg-gradient-to-br from-primary via-violet-500 to-indigo-500 bg-clip-text text-transparent">
                        PNS Admin
                    </CardTitle>
                    <CardDescription>
                        Enter your credentials to access the dashboard
                    </CardDescription>
                </CardHeader>
                <form onSubmit={handleLogin}>
                    <CardContent className="space-y-4">
                        <div className="space-y-2">
                            <Label htmlFor="email">Email</Label>
                            <div className="relative">
                                <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                <Input
                                    id="email"
                                    type="email"
                                    placeholder="admin@pns.com"
                                    className="pl-9 bg-background/50"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    disabled={isLoading}
                                    required
                                />
                            </div>
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="password">Password</Label>
                            <div className="relative">
                                <Lock className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                <Input
                                    id="password"
                                    type="password"
                                    placeholder="••••••••"
                                    className="pl-9 bg-background/50"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    disabled={isLoading}
                                    required
                                />
                            </div>
                        </div>
                    </CardContent>
                    <CardFooter>
                        <Button className="w-full font-bold shadow-lg shadow-primary/25" type="submit" disabled={isLoading}>
                            {isLoading ? (
                                <>
                                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                    Signing In...
                                </>
                            ) : (
                                "Sign In"
                            )}
                        </Button>
                    </CardFooter>
                </form>
            </Card>
        </div>
    );
}
