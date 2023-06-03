using Models;

namespace Common
{
    public class ConfigVersion : BaseModel
    {
        public int Version { get; set; }
        public int FilesCount { get; set; }
    }
}
