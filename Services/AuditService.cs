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
            string? personType = await GetPersonTypeByUserName(userName);
            var auditLog = new AuditLog
            {
                PersonID = personID,
                UserName = userName,
                UserType = personType,
                Action = action,
                ControllerName = controllerName,
                Parameters = parameters,
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

        public async Task<string> GetPersonTypeByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Equals("Anonymous", StringComparison.OrdinalIgnoreCase))
            {
                return "";
            }
            try
            {
                var person = await _context.Persons
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync( p => p.FullName == userName );
                return person?.PersonType ?? "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error resolving PersonType for username: {UserName}", userName );
                return "";
            }
        }
    }
}
