using Architecture_BE.DAL.UnitOfWork;

namespace Architecture_BE.Business.Services
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        public ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
