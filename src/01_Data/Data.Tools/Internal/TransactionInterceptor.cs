using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Mkh.Data.Abstractions;
using Mkh.Data.Tools.Annotations;

namespace Mkh.Data.Tools.Internal
{
    /// <summary>
    /// 特性事务拦截器
    /// </summary>
    internal class TransactionInterceptor : IInterceptor
    {
        private readonly IDbContext _context;
        private readonly IRepositoryManager _manager;

        public TransactionInterceptor(IDbContext context, IRepositoryManager manager)
        {
            _context = context;
            _manager = manager;
        }

        public void Intercept(IInvocation invocation)
        {
            var transactionAttribute = invocation.MethodInvocationTarget.GetCustomAttribute<TransactionAttribute>();
            if (transactionAttribute == null)
            {
                //调用业务方法
                invocation.Proceed();
            }
            else
            {
                InterceptAsync(invocation, transactionAttribute);
            }
        }

        private async void InterceptAsync(IInvocation invocation, TransactionAttribute attribute)
        {
            //创建工作单元
            using var uow = _context.NewUnitOfWork(attribute.IsolationLevel);
            try
            {
                //使仓储绑定工作单元
                foreach (var repository in _manager.Repositories)
                {
                    repository.BindingUow(uow);
                }

                //调用业务方法
                invocation.Proceed();

                dynamic result = invocation.ReturnValue;
                if (result is Task)
                {
                    await result;
                }

                //保存
                uow.SaveChanges();
            }
            catch
            {
                uow.Rollback();

                throw;
            }
        }
    }
}
