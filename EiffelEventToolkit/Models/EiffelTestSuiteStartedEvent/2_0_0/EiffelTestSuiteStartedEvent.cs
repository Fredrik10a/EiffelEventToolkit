//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------


namespace Eiffel.Models.EiffelTestSuiteStartedEvent.V2_0_0
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class EiffelTestSuiteStartedEvent
    {
        [Newtonsoft.Json.JsonProperty("meta", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Meta Meta { get; set; } = new Meta();

        [Newtonsoft.Json.JsonProperty("data", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data Data { get; set; } = new Data();

        [Newtonsoft.Json.JsonProperty("links", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<Links> Links { get; set; } = new System.Collections.ObjectModel.Collection<Links>();


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Meta
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$")]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("type", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MetaType Type { get; set; }

        [Newtonsoft.Json.JsonProperty("version", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MetaVersion Version { get; set; } = Eiffel.Models.EiffelTestSuiteStartedEvent.V2_0_0.MetaVersion._2_0_0;

        [Newtonsoft.Json.JsonProperty("time", Required = Newtonsoft.Json.Required.Always)]
        public long Time { get; set; }

        [Newtonsoft.Json.JsonProperty("tags", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> Tags { get; set; }

        [Newtonsoft.Json.JsonProperty("source", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Source Source { get; set; }

        [Newtonsoft.Json.JsonProperty("security", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Security Security { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Data
    {
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("categories", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> Categories { get; set; }

        [Newtonsoft.Json.JsonProperty("types", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ItemConverterType = typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public System.Collections.Generic.ICollection<Types> Types { get; set; }

        [Newtonsoft.Json.JsonProperty("liveLogs", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<LiveLogs> LiveLogs { get; set; }

        [Newtonsoft.Json.JsonProperty("customData", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<CustomData> CustomData { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Links
    {
        [Newtonsoft.Json.JsonProperty("type", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Type { get; set; }

        [Newtonsoft.Json.JsonProperty("target", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$")]
        public string Target { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public enum MetaType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"EiffelTestSuiteStartedEvent")]
        EiffelTestSuiteStartedEvent = 0,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public enum MetaVersion
    {

        [System.Runtime.Serialization.EnumMember(Value = @"2.0.0")]
        _2_0_0 = 0,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Source
    {
        [Newtonsoft.Json.JsonProperty("domainId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string DomainId { get; set; }

        [Newtonsoft.Json.JsonProperty("host", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Host { get; set; }

        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("serializer", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^pkg:")]
        public string Serializer { get; set; }

        [Newtonsoft.Json.JsonProperty("uri", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Uri { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Security
    {
        [Newtonsoft.Json.JsonProperty("sdm", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Sdm Sdm { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public enum Types
    {

        [System.Runtime.Serialization.EnumMember(Value = @"ACCESSIBILITY")]
        ACCESSIBILITY = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"BACKUP_RECOVERY")]
        BACKUP_RECOVERY = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"COMPATIBILITY")]
        COMPATIBILITY = 2,


        [System.Runtime.Serialization.EnumMember(Value = @"CONVERSION")]
        CONVERSION = 3,


        [System.Runtime.Serialization.EnumMember(Value = @"DISASTER_RECOVERY")]
        DISASTER_RECOVERY = 4,


        [System.Runtime.Serialization.EnumMember(Value = @"FUNCTIONAL")]
        FUNCTIONAL = 5,


        [System.Runtime.Serialization.EnumMember(Value = @"INSTALLABILITY")]
        INSTALLABILITY = 6,


        [System.Runtime.Serialization.EnumMember(Value = @"INTEROPERABILITY")]
        INTEROPERABILITY = 7,


        [System.Runtime.Serialization.EnumMember(Value = @"LOCALIZATION")]
        LOCALIZATION = 8,


        [System.Runtime.Serialization.EnumMember(Value = @"MAINTAINABILITY")]
        MAINTAINABILITY = 9,


        [System.Runtime.Serialization.EnumMember(Value = @"PERFORMANCE")]
        PERFORMANCE = 10,


        [System.Runtime.Serialization.EnumMember(Value = @"PORTABILITY")]
        PORTABILITY = 11,


        [System.Runtime.Serialization.EnumMember(Value = @"PROCEDURE")]
        PROCEDURE = 12,


        [System.Runtime.Serialization.EnumMember(Value = @"RELIABILITY")]
        RELIABILITY = 13,


        [System.Runtime.Serialization.EnumMember(Value = @"SECURITY")]
        SECURITY = 14,


        [System.Runtime.Serialization.EnumMember(Value = @"STABILITY")]
        STABILITY = 15,


        [System.Runtime.Serialization.EnumMember(Value = @"USABILITY")]
        USABILITY = 16,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class LiveLogs
    {
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("uri", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Uri { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class CustomData
    {
        [Newtonsoft.Json.JsonProperty("key", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Key { get; set; }

        [Newtonsoft.Json.JsonProperty("value", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object Value { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Sdm
    {
        [Newtonsoft.Json.JsonProperty("authorIdentity", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string AuthorIdentity { get; set; }

        [Newtonsoft.Json.JsonProperty("encryptedDigest", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string EncryptedDigest { get; set; }


    }
        // Overriden classes for default values
        public partial class Meta
        {
            public Meta()
            {
                Id = System.Guid.NewGuid().ToString();
                Time = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
}