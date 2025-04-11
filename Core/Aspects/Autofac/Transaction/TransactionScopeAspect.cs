using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.EntityFrameworkCore;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
   
        public override void Intercept(IInvocation invocation)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };
            
            var db = ServiceTool.ServiceProvider.GetService(default) as DbContext;

            var transactionScope = db.Database.BeginTransaction();//new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Commit();
                    transactionScope.Dispose();
                }
                catch (System.Exception e)
                {
                    transactionScope.Rollback();
                    transactionScope.Dispose();
                    throw;
                }

            }
        }
    }
}
