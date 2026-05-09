using Application.Contracts.IRepository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DeviceTokenRepository : GenericRepository<DeviceToken>, IDeviceTokenRepository
    {
        private readonly PnsDbContext _dbContext;

        public DeviceTokenRepository(PnsDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
