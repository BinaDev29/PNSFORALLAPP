import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useAuth } from "@/contexts/AuthContext";
import { useNavigate, Link } from "react-router-dom";
import { toast } from "sonner";
import { Loader2, User, Mail, Lock, Eye, EyeOff } from "lucide-react";
import { AuthService } from "@/services/api";

export default function RegisterPage() {
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        userName: "",
        email: "",
        password: "",
        confirmPassword: ""
    });
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({ ...formData, [e.target.id]: e.target.value });
    };

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();

        if (formData.password !== formData.confirmPassword) {
            toast.error("Password Mismatch", { description: "Passwords do not match." });
            return;
        }

        setIsLoading(true);

        try {
            const response = await AuthService.register({
                firstName: formData.firstName,
                lastName: formData.lastName,
                userName: formData.userName,
                email: formData.email,
                password: formData.password
            });

            if (response.token) {
                login(response.token, response);
                toast.success("Welcome!", { description: "Registration successful." });
                navigate("/");
            }
        } catch (error: any) {
            console.error(error);
            toast.error("Registration Failed", {
                description: error.response?.data?.message || "Something went wrong."
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

            <Card className="w-full max-w-lg border-border/50 bg-card/50 backdrop-blur-xl shadow-2xl">
                <CardHeader className="space-y-1 text-center">
                    <CardTitle className="text-3xl font-bold tracking-tight bg-gradient-to-br from-primary via-violet-500 to-indigo-500 bg-clip-text text-transparent">
                        Create Account
                    </CardTitle>
                    <CardDescription>
                        Enter your details to create a new account
                    </CardDescription>
                </CardHeader>
                <form onSubmit={handleRegister}>
                    <CardContent className="space-y-4">
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <Label htmlFor="firstName">First Name</Label>
                                <Input id="firstName" required value={formData.firstName} onChange={handleChange} className="bg-background/50" />
                            </div>
                            <div className="space-y-2">
                                <Label htmlFor="lastName">Last Name</Label>
                                <Input id="lastName" required value={formData.lastName} onChange={handleChange} className="bg-background/50" />
                            </div>
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="userName">Username</Label>
                            <div className="relative">
                                <User className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                <Input id="userName" required value={formData.userName} onChange={handleChange} className="pl-9 bg-background/50" />
                            </div>
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="email">Email</Label>
                            <div className="relative">
                                <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                <Input id="email" type="email" required value={formData.email} onChange={handleChange} className="pl-9 bg-background/50" />
                            </div>
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="password">Password</Label>
                            <div className="relative">
                                <Lock className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                <Input
                                    id="password"
                                    type={showPassword ? "text" : "password"}
                                    required
                                    value={formData.password}
                                    onChange={handleChange}
                                    className="pl-9 pr-10 bg-background/50"
                                />
                                <Button
                                    type="button"
                                    variant="ghost"
                                    size="sm"
                                    className="absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent text-muted-foreground hover:text-foreground"
                                    onClick={() => setShowPassword(!showPassword)}
                                >
                                    {showPassword ? (
                                        <EyeOff className="h-4 w-4" />
                                    ) : (
                                        <Eye className="h-4 w-4" />
                                    )}
                                </Button>
                            </div>
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="confirmPassword">Confirm Password</Label>
                            <div className="relative">
                                <Lock className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                                <Input
                                    id="confirmPassword"
                                    type={showConfirmPassword ? "text" : "password"}
                                    required
                                    value={formData.confirmPassword}
                                    onChange={handleChange}
                                    className="pl-9 pr-10 bg-background/50"
                                />
                                <Button
                                    type="button"
                                    variant="ghost"
                                    size="sm"
                                    className="absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent text-muted-foreground hover:text-foreground"
                                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                                >
                                    {showConfirmPassword ? (
                                        <EyeOff className="h-4 w-4" />
                                    ) : (
                                        <Eye className="h-4 w-4" />
                                    )}
                                </Button>
                            </div>
                        </div>
                    </CardContent>
                    <CardFooter className="flex flex-col space-y-4">
                        <Button className="w-full font-bold shadow-lg shadow-primary/25" type="submit" disabled={isLoading}>
                            {isLoading ? (
                                <>
                                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                    Creating Account...
                                </>
                            ) : (
                                "Register"
                            )}
                        </Button>
                        <div className="text-center text-sm text-muted-foreground">
                            Already have an account?{" "}
                            <Link to="/login" className="text-primary hover:underline font-medium">
                                Sign In
                            </Link>
                        </div>
                    </CardFooter>
                </form>
            </Card>
        </div>
    );
}
