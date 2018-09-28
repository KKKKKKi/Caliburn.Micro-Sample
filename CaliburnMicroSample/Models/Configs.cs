namespace CaliburnMicroSample.Models
{
    /// <summary>
    /// Configs Model
    /// 添加需要保存的配置属性
    /// 通过序列化和反序列化自动保存到配置文件
    /// </summary>
    public class Configs
    {
        /// <summary>
        /// 语言设置
        /// </summary>
        public string Language { get; set; }
    }
}
