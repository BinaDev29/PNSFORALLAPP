import { Construction } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";

interface ComingSoonProps {
    title: string;
    description?: string;
}

export function ComingSoon({ title, description }: ComingSoonProps) {
    const navigate = useNavigate();

    return (
        <div className="h-[60vh] flex flex-col items-center justify-center text-center p-8 border-2 border-dashed border-border/50 rounded-3xl bg-card/20 backdrop-blur-sm animate-in fade-in zoom-in-95 duration-500">
            <div className="bg-primary/10 p-6 rounded-full mb-6 ring-8 ring-primary/5">
                <Construction className="w-12 h-12 text-primary" />
            </div>
            <h2 className="text-3xl font-bold tracking-tight mb-3">{title}</h2>
            <p className="text-muted-foreground max-w-md mb-8 text-lg">
                {description || "We're working hard to bring you this feature. Stay tuned for updates!"}
            </p>
            <div className="flex gap-4">
                <Button variant="outline" onClick={() => navigate(-1)}>
                    Go Back
                </Button>
                <Button onClick={() => navigate('/')}>
                    Return to Dashboard
                </Button>
            </div>
        </div>
    );
}
