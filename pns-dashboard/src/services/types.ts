export interface NotificationDto {
    id: string;
    clientApplicationId: string;
    to: string[];
    title: string;
    message: string;
    notificationTypeId: string;
    priorityId: string;
}

export interface CreateNotificationDto {
    clientApplicationId: string;
    to: string[];
    title: string;
    message: string;
    notificationTypeId: string;
    priorityId: string;
}

export interface UpdateNotificationDto {
    id: string;
    title: string;
    message: string;
    notificationTypeId: string;
    priorityId: string;
}

export interface NotificationStatisticsDto {
    totalSent: number;
    totalSeen: number;
    totalUnseen: number;
    totalFailed: number;
}

export interface BaseCommandResponse {
    id?: string;
    success: boolean;
    message?: string;
    errors?: string[];
}
