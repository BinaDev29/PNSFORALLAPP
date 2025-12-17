// Simple I18n implementation to avoid dependency issues
import { useState, useEffect } from 'react';

// Translation resources
const resources: Record<string, any> = {
    en: {
        translation: {
            dashboard: {
                title: "Dashboard Overview",
                welcome: "Welcome back to PNS Admin Dashboard.",
                totalNotifications: "Total Notifications",
                successRate: "Success Rate",
                failedMessages: "Failed Messages",
                activeClients: "Active Clients",
                revenueOverview: "Revenue Overview",
                recentActivity: "Recent Activity",
                newReservation: "Notification Sent",
                customer: "App",
                downloadReport: "Download Report",
                createNew: "Create New",
                minsAgo: "mins ago",
                stats: {
                    notificationsChange: "+12% from last month",
                    successChange: "+0.5% from last week",
                    failedChange: "-5 from yesterday",
                    clientsChange: "+3 new apps"
                }
            },
            sidebar: {
                dashboard: "Dashboard",
                notifications: "Notifications",
                history: "History",
                templates: "Templates",
                clients: "Client Apps",
                settings: "Settings",
                profile: "Admin User",
                signOut: "Sign Out"
            }
        }
    },
    es: {
        translation: {
            dashboard: {
                title: "Resumen del Panel",
                welcome: "Bienvenido de nuevo al Panel de Administración de PNS.",
                totalNotifications: "Notificaciones Totales",
                successRate: "Tasa de Éxito",
                failedMessages: "Mensajes Fallidos",
                activeClients: "Clientes Activos",
                revenueOverview: "Resumen de Ingresos",
                recentActivity: "Actividad Reciente",
                newReservation: "Notificación Enviada",
                customer: "Aplicación",
                downloadReport: "Descargar Informe",
                createNew: "Crear Nuevo",
                minsAgo: "hace min",
                stats: {
                    notificationsChange: "+12% desde el mes pasado",
                    successChange: "+0.5% desde la semana pasada",
                    failedChange: "-5 desde ayer",
                    clientsChange: "+3 aplicaciones nuevas"
                }
            },
            sidebar: {
                dashboard: "Panel",
                notifications: "Notificaciones",
                history: "Historial",
                templates: "Plantillas",
                clients: "Apps Clientes",
                settings: "Ajustes",
                profile: "Admin Usuario",
                signOut: "Cerrar Sesión"
            }
        }
    },
    fr: {
        translation: {
            dashboard: {
                title: "Aperçu du Tableau de Bord",
                welcome: "Bon retour sur le tableau de bord d'administration PNS.",
                totalNotifications: "Total des Notifications",
                successRate: "Taux de Réussite",
                failedMessages: "Messages Échoués",
                activeClients: "Clients Actifs",
                revenueOverview: "Aperçu des Revenus",
                recentActivity: "Activité Récente",
                newReservation: "Notification Envoyée",
                customer: "App",
                downloadReport: "Télécharger le Rapport",
                createNew: "Créer Nouveau",
                minsAgo: "il y a min",
                stats: {
                    notificationsChange: "+12% depuis le mois dernier",
                    successChange: "+0.5% depuis la semaine dernière",
                    failedChange: "-5 depuis hier",
                    clientsChange: "+3 nouvelles apps"
                }
            },
            sidebar: {
                dashboard: "Tableau de Bord",
                notifications: "Notifications",
                history: "Historique",
                templates: "Modèles",
                clients: "Apps Clientes",
                settings: "Paramètres",
                profile: "Admin Utilisateur",
                signOut: "Déconnexion"
            }
        }
    }
};

// Simple event emitter for language changes
class I18nService {
    language = 'en';
    listeners: Set<Function> = new Set();

    changeLanguage(lang: string) {
        if (resources[lang]) {
            this.language = lang;
            this.notify();
        }
    }

    notify() {
        this.listeners.forEach(l => l(this.language));
    }

    subscribe(listener: Function) {
        this.listeners.add(listener);
        return () => {
            this.listeners.delete(listener);
        };
    }
}

export const i18nInstance = new I18nService();

export function useTranslation() {
    const [lang, setLang] = useState(i18nInstance.language);

    useEffect(() => {
        return i18nInstance.subscribe(setLang);
    }, []);

    const t = (key: string, defaultValue?: string) => {
        const keys = key.split('.');
        let value = resources[lang]?.translation;

        for (const k of keys) {
            value = value?.[k];
            if (!value) break;
        }

        return value || defaultValue || key;
    };

    return {
        t,
        i18n: {
            changeLanguage: (l: string) => i18nInstance.changeLanguage(l),
            language: lang
        }
    };
}

export default i18nInstance;
