/// <summary>
/// 消息状态码 
/// </summary>
public enum MsgEnumStateCode
{
    /// <summary>
    /// 从服务端读取数据失败, 一般情况下为连接断开
    /// </summary>
    ReadFailed ,

    /// <summary>
    /// 从服务端读取数据成功
    /// </summary>
    ReadSuccess,
}


/// <summary>
/// 网络连接状态码
/// </summary>
public enum ConnEnumStateCode
{
    Connecting,
    Connected,
    DisConnect,
    ConnectFailed,
    ConnectError,
}


