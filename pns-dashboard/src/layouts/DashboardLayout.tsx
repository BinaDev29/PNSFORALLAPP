import { Outlet } from "react-router-dom";
import { Sidebar } from "@/components/layout/Sidebar";

export default function DashboardLayout() {
    return (
        <div className="min-h-screen bg-background font-sans text-foreground antialiased selection:bg-primary/20 selection:text-primary relative overflow-hidden">
            {/* Background Gradients/Patterns */}
            <div className="fixed inset-0 -z-10 h-full w-full bg-background" />
            <div className="fixed inset-0 -z-10 h-full w-full bg-[linear-gradient(to_right,#8080800a_1px,transparent_1px),linear-gradient(to_bottom,#8080800a_1px,transparent_1px)] bg-[size:24px_24px]"></div>
            <div className="fixed left-[-20%] top-[-10%] -z-10 h-[500px] w-[500px] rounded-full bg-primary/20 blur-[100px] opacity-30"></div>
            <div className="fixed right-[-20%] bottom-[-10%] -z-10 h-[500px] w-[500px] rounded-full bg-blue-500/20 blur-[100px] opacity-30"></div>

            <Sidebar />
            <main className="md:ml-64 min-h-screen p-6 md:p-8 transition-all duration-300 ease-in-out">
                <div className="max-w-7xl mx-auto space-y-8">
                    <Outlet />
                </div>
            </main>
        </div>
    );
}
