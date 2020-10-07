namespace CalibrationInstructionsManager.Core.Models.Types
{
    public class ChannelSettingType : IChannelSettingType
    {
        private int _id;
        private string _name;

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
