namespace mmrcis.Services
{
    public interface IAuditService
    {
       Task LogActionAsync(string userName, string action, string controllerName, string parameters, string ipAddress, string userAgent);
    }
}


