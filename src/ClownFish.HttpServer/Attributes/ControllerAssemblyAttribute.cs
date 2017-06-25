using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Attributes
{
    /// <summary>
	/// 指示包含HttpHandler/Service/Controller的程序集
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ControllerAssemblyAttribute : Attribute
    {
    }
}
