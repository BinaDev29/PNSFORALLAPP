// File Path: Persistence/Repositories/NotificationRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    // NotificationRepository ከ`GenericRepository` ይወርሳል
    public class NotificationRepository(PnsDbContext dbContext) : GenericRepository<Notification>(dbContext), INotificationRepository
    {
        // የ`INotificationRepository`ን ተግባራት ከ`GenericRepository` ስለሚወርስ
        // በዚህ class ውስጥ ምንም ተጨማሪ ኮድ መጻፍ አያስፈልግም
        // ነገር ግን ለ`Notification` ብቻ የሚያገለግሉ ልዩ ዘዴዎች ካስፈለጉ እዚህ ይጨመራሉ
    }
}