namespace ClownFish.HttpServer.Authentication
{
    /// <summary>
    /// 用于用户的基本操作的接口
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Name属性
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 判断是否拥有某些权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool IsInRole(string role);
    }
}
