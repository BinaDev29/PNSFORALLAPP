import * as signalR from "@microsoft/signalr";

class SignalRService {
    private connection: signalR.HubConnection | null = null;
    private callbacks: { [key: string]: ((data: any) => void)[] } = {};

    public async startConnection() {
        if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) return;

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/notification", {
                accessTokenFactory: () => localStorage.getItem('pns_auth_token') || ''
            })
            .withAutomaticReconnect()
            .build();

        this.connection.on("ReceiveNotificationUpdate", (message: string) => {
            this.trigger("notificationCreated", message);
        });

        this.connection.on("ReceiveDashboardStats", (stats: any) => {
            this.trigger("statsUpdated", stats);
        });

        try {
            await this.connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.error("SignalR Connection Error: ", err);
            setTimeout(() => this.startConnection(), 5000);
        }
    }

    public on(event: string, callback: (data: any) => void) {
        if (!this.callbacks[event]) {
            this.callbacks[event] = [];
        }
        this.callbacks[event].push(callback);
    }

    private trigger(event: string, data: any) {
        if (this.callbacks[event]) {
            this.callbacks[event].forEach(callback => callback(data));
        }
    }
}

export const signalRService = new SignalRService();
