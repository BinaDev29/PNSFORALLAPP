import axios from 'axios';
// I might need to create this or use date-fns if available, or just Intl

const api = axios.create({
    baseURL: '/api',
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem('pns_auth_token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export interface AuthResponse {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    token: string;
    roles: string[];
}

export interface AuthRequest {
    email: string;
    password: string;
}

export interface RegistrationRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    userName?: string;
}

export interface UserProfileUpdateDto {
    firstName: string;
    lastName: string;
    email: string;
}

export interface ChangePasswordDto {
    currentPassword: string;
    newPassword: string;
}

export const AuthService = {
    login: async (data: AuthRequest) => {
        const response = await api.post<AuthResponse>('/Auth/login', data);
        if (response.data.token) {
            localStorage.setItem('pns_auth_token', response.data.token);
            localStorage.setItem('user', JSON.stringify(response.data));
        }
        return response.data;
    },

    register: async (data: RegistrationRequest) => {
        const response = await api.post<AuthResponse>('/Auth/register', data);
        if (response.data.token) {
            localStorage.setItem('pns_auth_token', response.data.token);
            localStorage.setItem('user', JSON.stringify(response.data));
        }
        return response.data;
    },

    updateProfile: async (data: UserProfileUpdateDto) => {
        const response = await api.put('/Auth/profile', data);
        // Refresh local storage user data if needed
        const currentUser = AuthService.getCurrentUser();
        if (currentUser) {
            const newUser = { ...currentUser, ...data };
            localStorage.setItem('user', JSON.stringify(newUser));
        }
        return response.data;
    },

    changePassword: async (data: ChangePasswordDto) => {
        const response = await api.post('/Auth/change-password', data);
        return response.data;
    },

    logout: () => {
        localStorage.removeItem('pns_auth_token');
        localStorage.removeItem('user');
        window.location.href = '/login';
    },

    getCurrentUser: () => {
        const userStr = localStorage.getItem('user');
        return userStr ? JSON.parse(userStr) : null;
    },

    isAuthenticated: () => {
        return !!localStorage.getItem('pns_auth_token');
    }
};

// Response wrapper if backend uses a standard wrapper (doesn't look like it for GETs, but maybe for some)
// Based on controllers, GETs return the data directly or wrapped in ActionResult.

export interface NotificationStatistics {
    totalRequests: number;
    pending: number;
    sent: number;
    failed: number;
    seen: number;
    scheduled: number;
    successRate: number;
}

export interface NotificationHistory {
    id: string;
    status: string;
    notificationId: string;
    sentDate: string; // ISO string
    to?: string;      // Comma separated list of recipients
    title?: string;
    message?: string;
    notificationType?: string;
    errorMessage?: string;
}

export interface ClientApplication {
    id: string;
    appId: string;
    key: string;
    name: string;
    slogan?: string;
    logo?: string;
}

export interface Notification {
    id: string;
    title: string;
    message: string;
    to: string[];
    clientApplicationId: string;
    notificationTypeId: string;
    priorityId: string;
    status?: string;
    sentDate?: string;
}

export const DashboardService = {
    getStatistics: async (startDate?: Date, endDate?: Date) => {
        const params = new URLSearchParams();
        if (startDate) params.append('startDate', startDate.toISOString());
        if (endDate) params.append('endDate', endDate.toISOString());

        const response = await api.get<NotificationStatistics>('/Notification/statistics', { params });
        return response.data;
    },

    getClientApplications: async () => {
        const response = await api.get<ClientApplication[]>('/ClientApplication');
        return response.data;
    },

    getRecentActivity: async () => {
        // NotificationHistory seems to be the best source for "activity" with dates
        const response = await api.get<NotificationHistory[]>('/NotificationHistory');
        // Sort by sentDate desc on client side if not already sorted
        return response.data.sort((a, b) => new Date(b.sentDate).getTime() - new Date(a.sentDate).getTime()).slice(0, 10);
    },

    getNotificationHistory: async () => {
        const response = await api.get<NotificationHistory[]>('/NotificationHistory');
        return response.data.sort((a, b) => new Date(b.sentDate).getTime() - new Date(a.sentDate).getTime());
    },

    getRecentNotifications: async () => {
        // Fallback if History is not enough or empty
        const response = await api.get<Notification[]>('/Notification');
        return response.data;
    },

    getPriorities: async () => {
        const response = await api.get<Priority[]>('/Priority');
        return response.data;
    },

    getNotificationTypes: async () => {
        const response = await api.get<NotificationType[]>('/NotificationType');
        return response.data;
    },

    createNotification: async (data: CreateNotificationRequest) => {
        const response = await api.post('/Notification', data);
        return response.data;
    },

    createClientApplication: async (data: CreateClientApplicationRequest) => {
        const response = await api.post('/ClientApplication', data);
        return response.data;
    },

    updateClientApplication: async (id: string, data: Partial<CreateClientApplicationRequest>) => {
        const response = await api.put(`/ClientApplication/${id}`, data);
        return response.data;
    },

    deleteClientApplication: async (id: string) => {
        await api.delete(`/ClientApplication/${id}`);
    },

    getEmailTemplates: async () => {
        const response = await api.get<EmailTemplate[]>('/EmailTemplate');
        return response.data;
    },

    createEmailTemplate: async (data: CreateEmailTemplateRequest) => {
        const response = await api.post('/EmailTemplate', data);
        return response.data;
    },

    updateEmailTemplate: async (id: string, data: Partial<CreateEmailTemplateRequest>) => {
        const response = await api.put(`/EmailTemplate/${id}`, data);
        return response.data;
    },

    deleteEmailTemplate: async (id: string) => {
        await api.delete(`/EmailTemplate/${id}`);
    },

    getNotificationById: async (id: string) => {
        const response = await api.get<Notification>(`/Notification/${id}`);
        return response.data;
    },

    deleteNotification: async (id: string) => {
        await api.delete(`/Notification/${id}`);
    },

    updateNotification: async (id: string, data: any) => {
        // Assuming endpoint for update exists on Notification resource
        const response = await api.put(`/Notification/${id}`, data);
        return response.data;
    }
};

export interface Priority {
    id: string;
    description: string;
    level: number;
}

export interface NotificationType {
    id: string;
    name: string;
    description?: string;
}

export interface CreateNotificationRequest {
    title: string;
    message: string;
    notificationTypeId: string;
    clientApplicationId: string;
    to: string[];
    priorityId: string;
}

export interface CreateClientApplicationRequest {
    appId: string;
    key: string;
    name: string;
    slogan: string;
    logo: string;
    senderEmail: string;
    appPassword: string;
}

export interface EmailTemplate {
    id: string;
    name: string;
    subject: string;
    bodyHtml: string;
    bodyText?: string;
}

export interface CreateEmailTemplateRequest {
    name: string;
    subject: string;
    bodyHtml: string;
    bodyText?: string;
}


export default api;
