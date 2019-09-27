namespace XmlToJsonRigSoftware
{
    public class KeyValueTypeJson
    {
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        public object Value { get; set; }
        public short Type { get; set; } = 0;
    }
}
