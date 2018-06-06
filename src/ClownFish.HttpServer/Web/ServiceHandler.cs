using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.Base.Reflection;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Authentication;
using ClownFish.HttpServer.Common;
using ClownFish.HttpServer.Result;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 实现IHttpHandler接口，用于执行某个服务方法
	/// </summary>
	public sealed class ServiceHandler : ITaskHttpHandler, IHttpHandlerInitializer, IDisposable
    {
		private readonly Type _serviceType;
		private readonly MethodInfo _method;
        private object _instance;

        private readonly bool _hasReturn;
        private readonly bool _isTaskMethod;


        /// <summary>
        /// 获取将要执行的 controller/service 类型
        /// </summary>
        public Type ServiceType => _serviceType;
        /// <summary>
        /// 获取将要执行的Action方法
        /// </summary>
        public MethodInfo ActionMethod => _method;

        /// <summary>
        /// 获取实际要执行的 controller/service 实例
        /// </summary>
        public object Instance => _instance;



		internal ServiceHandler(Type serviceType, MethodInfo method)
		{
			_serviceType = serviceType;
			_method = method;

            _isTaskMethod = _method.IsTaskMethod();

            if( _isTaskMethod )
                _hasReturn = _method.GetTaskMethodResultType() != null;
            else
                _hasReturn = _method.HasReturn();

            // 创建类型实例
            _instance = _serviceType.FastNew();
        }



		/// <summary>
		/// 处理HTTP请求的入口方法
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// 处理HTTP请求的入口方法
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task<IActionResult> ProcessRequestAsync(HttpContext context)
		{
            IRequireHttpContext xx = _instance as IRequireHttpContext;
            if( xx != null )
                xx.HttpContext = context;


            // 构造方法的调用参数
            ParameterResolver pr = ObjectFactory.New<ParameterResolver>();
			object[] parameters = pr.GetParameters(_method, context.Request);

			// 执行方法
			object result = null;

            if( _isTaskMethod ) {
                if( _hasReturn ) {
                    Task task = (Task)_method.FastInvoke(_instance, parameters);
                    await task;

                    // 从 Task<T> 中获取返回值
                    PropertyInfo property = task.GetType().GetProperty("Result", BindingFlags.Instance | BindingFlags.Public);
                    result = property.FastGetValue(task);
                }
                else {
                    await (Task)_method.FastInvoke(_instance, parameters);
                }
            }
            else {
                if( _hasReturn )
                    result = _method.FastInvoke(_instance, parameters);
                else
                    _method.FastInvoke(_instance, parameters);
            }


			// 没有执行结果，直接返回（不产生输出）
			if( result == null )
				return null;


			// 转换结果
			IActionResult actionResult = result as IActionResult;
			if( actionResult == null ) {
				ResultConvert converter = ObjectFactory.New<ResultConvert>();
				actionResult = converter.Convert(result, context);
			}

			return actionResult;
		}


        /// <summary>
        /// 检查授权
        /// </summary>
        internal void CheckAuthorization(HttpContext context)
        {
            AuthorizeAttribute attr = _method.GetMyAttribute<AuthorizeAttribute>();
            if( attr == null )
                attr = _serviceType.GetMyAttribute<AuthorizeAttribute>();
            if( attr == null )
                return;


            if( attr.AuthenticateRequest(context) == false)
                throw new System.Web.HttpException(403,
                            "很抱歉，您没有合适的权限访问该资源：" + context.Request.RawUrl);
        }

        /// <summary>
        /// 从将要执行的方法或者类型查找特定的标记
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindAttribute<T>() where T: Attribute
        {
            T attr = _method.GetMyAttribute<T>();

            if( attr == null )
                attr = _serviceType.GetMyAttribute<T>();

            if( attr == null )
                return null;

            return attr;
        }

        /// <summary>
        /// Dispose()
        /// </summary>
        public void Dispose()
        {
            if( _instance != null ) {
                IDisposable disposable = _instance as IDisposable;
                if(  disposable != null ) {
                    disposable.Dispose();
                }

                _instance = null;
            }
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpContext context)
        {
            IHttpHandlerInitializer handler = _instance as IHttpHandlerInitializer;
            if( handler != null ) {
                handler.Init(context);
            }
        }
    }
}
