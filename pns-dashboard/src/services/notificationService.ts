import api from './api';

export interface NotificationDto {
    id: string;
    notificationTypeId: string;
    clientApplicationId: string;
    priorityId: string;
    message: string;
    recipient: string;
    sentAt: string;
    status: string;
    isSeen: boolean;
    // Add other properties based on DTO
}

export interface NotificationStatisticsDto {
    totalSent: number;
    totalFailed: number;
    totalDelivered: number;
    successRate: number;
    // Add other properties based on DTO
}

export const NotificationService = {
    getAll: async () => {
        const response = await api.get<NotificationDto[]>('/Notification');
        return response.data;
    },

    getById: async (id: string) => {
        const response = await api.get<NotificationDto>(`/Notification/${id}`);
        return response.data;
    },

    getUnseen: async (clientApplicationId: string) => {
        const response = await api.get<NotificationDto[]>(`/Notification/unseen/${clientApplicationId}`);
        return response.data;
    },

    getStatistics: async (startDate?: string, endDate?: string, clientApplicationId?: string) => {
        const response = await api.get<NotificationStatisticsDto>('/Notification/statistics', {
            params: { startDate, endDate, clientApplicationId }
        });
        return response.data;
    },

    create: async (data: any) => {
        const response = await api.post('/Notification', data);
        return response.data;
    },

    markAsSeen: async (id: string) => {
        await api.put(`/Notification/${id}/seen`);
    }
};
