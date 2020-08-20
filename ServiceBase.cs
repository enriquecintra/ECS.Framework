namespace ECS.Framework
{
    /// <summary>
    /// Classe base para os serviços
    /// </summary>
    public class ServiceBase
    {
        public ServiceResult Ok() => new ServiceResult(true, System.Net.HttpStatusCode.OK);

        public ServiceResult Ok(object data) => new ServiceResult(true, System.Net.HttpStatusCode.OK, data);

        public ServiceResult BadRequest(string message) => new ServiceResult(false, System.Net.HttpStatusCode.BadRequest, message);

        public ServiceResult NotFound(string message) => new ServiceResult(false, System.Net.HttpStatusCode.NotFound, message);

        public ServiceResult Conflict(string message) => new ServiceResult(false, System.Net.HttpStatusCode.Conflict, message);

        public ServiceResult Created() => new ServiceResult(true, System.Net.HttpStatusCode.Created);

        public ServiceResult Created(object data) => new ServiceResult(true, System.Net.HttpStatusCode.Created, data);
    }
}