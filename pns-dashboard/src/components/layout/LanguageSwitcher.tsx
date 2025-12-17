import { useTranslation } from '@/i18n';
import { Button } from "@/components/ui/button";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { Globe } from "lucide-react";

export function LanguageSwitcher() {
    const { i18n } = useTranslation();

    const changeLanguage = (lng: string) => {
        i18n.changeLanguage(lng);
    };

    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="icon" className="w-9 h-9 rounded-full hover:bg-muted">
                    <Globe className="h-4 w-4 transition-transform hover:rotate-12" />
                </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end" className="w-[150px]">
                <DropdownMenuItem onClick={() => changeLanguage('en')} className={i18n.language === 'en' ? 'bg-primary/10 font-bold text-primary' : ''}>
                    English
                </DropdownMenuItem>
                <DropdownMenuItem onClick={() => changeLanguage('es')} className={i18n.language === 'es' ? 'bg-primary/10 font-bold text-primary' : ''}>
                    Español
                </DropdownMenuItem>
                <DropdownMenuItem onClick={() => changeLanguage('fr')} className={i18n.language === 'fr' ? 'bg-primary/10 font-bold text-primary' : ''}>
                    Français
                </DropdownMenuItem>
            </DropdownMenuContent>
        </DropdownMenu>
    );
}
