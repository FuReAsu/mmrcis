using Microsoft.EntityFrameworkCore;
using mmrcis.Data;
using mmrcis.Models;

namespace mmrcis.Services
{
    public class AuditService : IAuditService
    {
        private readonly CisDbContext _context;
        private readonly ILogger<AuditService> _logger;

        public AuditService(CisDbContext context, ILogger<AuditService> logger)
        {
           _context = context; 
           _logger = logger;
        }

        public async Task LogActionAsync(string userName, string action, string controllerName, string parameters, string ipAddress, string userAgent)
        {
            int? personID = await GetPersonIdByUserName(userName);
            var auditLog = new AuditLog
            {
                PersonID = personID,
                UserName = userName,
                Action = action,
                ControllerName = controllerName,
                Timestamp = DateTime.Now,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
        
        public async Task<int?> GetPersonIdByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Equals("Anonymous", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            try
            {
                var person = await _context.Persons
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(p => p.FullName == userName);
                return person?.ID;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Error resolving PersonID for username: {UserName}", userName);
               return null;
            }
        }
    }
}
