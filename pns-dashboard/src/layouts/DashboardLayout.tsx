import { useState } from "react";
import { Outlet } from "react-router-dom";
import { Sidebar } from "@/components/layout/Sidebar";
import { Button } from "@/components/ui/button";
import { Menu } from "lucide-react";

export default function DashboardLayout() {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

    return (
        <div className="min-h-screen bg-background font-sans text-foreground antialiased selection:bg-primary/20 selection:text-primary relative overflow-hidden">
            {/* Background Gradients/Patterns */}
            <div className="fixed inset-0 -z-10 h-full w-full bg-background" />
            <div className="fixed inset-0 -z-10 h-full w-full bg-[linear-gradient(to_right,#8080800a_1px,transparent_1px),linear-gradient(to_bottom,#8080800a_1px,transparent_1px)] bg-[size:24px_24px]"></div>
            <div className="fixed left-[-20%] top-[-10%] -z-10 h-[500px] w-[500px] rounded-full bg-primary/20 blur-[100px] opacity-30"></div>
            <div className="fixed right-[-20%] bottom-[-10%] -z-10 h-[500px] w-[500px] rounded-full bg-blue-500/20 blur-[100px] opacity-30"></div>

            {/* Mobile Header */}
            <header className="md:hidden sticky top-0 z-30 flex h-16 items-center gap-4 border-b border-border/40 bg-background/60 backdrop-blur-xl px-6">
                <Button
                    variant="ghost"
                    size="icon"
                    onClick={() => setIsSidebarOpen(true)}
                    className="hover:bg-primary/10"
                >
                    <Menu className="h-6 w-6 text-primary" />
                    <span className="sr-only">Open menu</span>
                </Button>
                <div className="flex-1">
                    <h2 className="text-xl font-bold bg-gradient-to-r from-primary to-indigo-500 bg-clip-text text-transparent">PNS</h2>
                </div>
            </header>

            <Sidebar isOpen={isSidebarOpen} onClose={() => setIsSidebarOpen(false)} />

            <main className="md:ml-64 min-h-screen p-6 md:p-8 transition-all duration-300 ease-in-out">
                <div className="max-w-7xl mx-auto space-y-8">
                    <Outlet />
                </div>
            </main>
        </div>
    );
}
