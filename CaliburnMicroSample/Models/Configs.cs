namespace CaliburnMicroSample.Models
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Resources;
    using YamlDotNet.Serialization;

    /// <summary>
    /// Configs
    /// 添加需要保存的配置属性
    /// 通过序列化和反序列化自动保存到配置文件
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Configs
    {
        #region properties
        /// <summary>
        /// 语言设置
        /// </summary>
        [JsonProperty(PropertyName = "Language")]
        [YamlMember(Alias = "Language", ApplyNamingConventions = false)]
        public string Language { get; set; }

        // [JsonProperty(PropertyName = "IsConfigsChanged")]
        // [YamlMember(Alias = "IsConfigsChanged", ApplyNamingConventions = false)]
        // [JsonIgnore]
        // [YamlIgnore]
        // public bool IsChanged { get; set; } = false;

        #endregion

        #region methods
        
        public static Configs LoadConfigs()
        {
            Configs configs;
            // string configfile = Environment.CurrentDirectory + "\\Configs\\Configs.json";
            string configfile = Environment.CurrentDirectory + "\\Configs\\Configs.yml";
            if (!File.Exists(configfile))
            {
                return LoadDefaultConfigs();
            }
            // 读方式打开配置文件
            FileStream fs = new FileStream(configfile, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            // configs = JsonConvert.DeserializeObject<Configs>(reader.ReadToEnd());

            IDeserializer deserializer = new DeserializerBuilder().Build();
            try
            {
                configs = deserializer.Deserialize<Configs>(reader.ReadToEnd());
            }
            catch (Exception)
            {
                configs = null;
            }

            reader.Close();

            // 加载配置失败，使用默认配置
            if (configs == null)
            {
                return LoadDefaultConfigs();
            }

            return configs;
        }

        /// <summary>
        /// 默认设置
        /// </summary>
        /// <returns>configs</returns>
        public static Configs LoadDefaultConfigs()
        {
            Configs configs;
            // string configfile = "pack://application:,,,/Configs/Configs.json";
            string configfile = "pack://application:,,,/Configs/Configs.yml";
            StreamResourceInfo stream = Application.GetResourceStream(new Uri(configfile));
            StreamReader reader = new StreamReader(stream.Stream);
            // configs = JsonConvert.DeserializeObject<Configs>(reader.ReadToEnd());

            IDeserializer deserializer = new DeserializerBuilder().Build();
            configs = deserializer.Deserialize<Configs>(reader.ReadToEnd());

            // 获取系统语言
            string info = System.Globalization.CultureInfo.CurrentCulture.Name;
            configs.Language = info;

            reader.Close();

            return configs;
        }

        public void SaveConfigs()
        {
            if (!IsConfigsChanged())
            {
                return;
            }

            string configfile = Environment.CurrentDirectory + "\\Configs";
            // 如果目录不存在则创建
            if (!Directory.Exists(configfile))
            {
                Directory.CreateDirectory(configfile);
            }
            // configfile += "\\Configs.json";
            configfile += "\\Configs.yml";
            // 写入配置文件，新建文件覆盖
            FileStream fs = new FileStream(configfile, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs);
            // string json = JsonConvert.SerializeObject(App.configs, Formatting.Indented);
            // writer.Write(json);

            ISerializer serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(this);
            writer.Write(yaml);

            writer.Flush();
            writer.Close();
        }

        private bool IsConfigsChanged()
        {
            Configs OldConfigs = LoadConfigs();

            if (!OldConfigs.Language.Equals(Language)) return true;
            // Other Properties

            return false;
        }

        #endregion
    }
}
