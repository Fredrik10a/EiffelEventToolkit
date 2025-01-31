//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------


namespace Eiffel.Models.EiffelTestExecutionRecipeCollectionCreatedEvent.V4_2_0
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class EiffelTestExecutionRecipeCollectionCreatedEvent
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
        public MetaVersion Version { get; set; } = Eiffel.Models.EiffelTestExecutionRecipeCollectionCreatedEvent.V4_2_0.MetaVersion._4_2_0;

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
        [Newtonsoft.Json.JsonProperty("selectionStrategy", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public SelectionStrategy SelectionStrategy { get; set; } = new SelectionStrategy();

        [Newtonsoft.Json.JsonProperty("batchesUri", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BatchesUri { get; set; }

        [Newtonsoft.Json.JsonProperty("batches", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Batches> Batches { get; set; }

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

        [Newtonsoft.Json.JsonProperty("domainId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string DomainId { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public enum MetaType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"EiffelTestExecutionRecipeCollectionCreatedEvent")]
        EiffelTestExecutionRecipeCollectionCreatedEvent = 0,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public enum MetaVersion
    {

        [System.Runtime.Serialization.EnumMember(Value = @"4.2.0")]
        _4_2_0 = 0,


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
        [Newtonsoft.Json.JsonProperty("authorIdentity", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string AuthorIdentity { get; set; }

        [Newtonsoft.Json.JsonProperty("integrityProtection", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IntegrityProtection IntegrityProtection { get; set; }

        [Newtonsoft.Json.JsonProperty("sequenceProtection", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<SequenceProtection> SequenceProtection { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class SelectionStrategy
    {
        [Newtonsoft.Json.JsonProperty("tracker", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Tracker { get; set; }

        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("uri", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Uri { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Batches
    {
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("priority", Required = Newtonsoft.Json.Required.Always)]
        public long Priority { get; set; }

        [Newtonsoft.Json.JsonProperty("recipes", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<Recipes> Recipes { get; set; } = new System.Collections.ObjectModel.Collection<Recipes>();

        [Newtonsoft.Json.JsonProperty("dependencies", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Dependencies> Dependencies { get; set; }


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
    public partial class IntegrityProtection
    {
        [Newtonsoft.Json.JsonProperty("signature", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Signature { get; set; }

        [Newtonsoft.Json.JsonProperty("alg", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public IntegrityProtectionAlg Alg { get; set; }

        [Newtonsoft.Json.JsonProperty("publicKey", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PublicKey { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class SequenceProtection
    {
        [Newtonsoft.Json.JsonProperty("sequenceName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string SequenceName { get; set; }

        [Newtonsoft.Json.JsonProperty("position", Required = Newtonsoft.Json.Required.Always)]
        public long Position { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Recipes
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$")]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("testCase", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public TestCase TestCase { get; set; } = new TestCase();

        [Newtonsoft.Json.JsonProperty("constraints", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Constraints> Constraints { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Dependencies
    {
        [Newtonsoft.Json.JsonProperty("dependent", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Dependent { get; set; }

        [Newtonsoft.Json.JsonProperty("dependency", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Dependency { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public enum IntegrityProtectionAlg
    {

        [System.Runtime.Serialization.EnumMember(Value = @"HS256")]
        HS256 = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"HS384")]
        HS384 = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"HS512")]
        HS512 = 2,


        [System.Runtime.Serialization.EnumMember(Value = @"RS256")]
        RS256 = 3,


        [System.Runtime.Serialization.EnumMember(Value = @"RS384")]
        RS384 = 4,


        [System.Runtime.Serialization.EnumMember(Value = @"RS512")]
        RS512 = 5,


        [System.Runtime.Serialization.EnumMember(Value = @"ES256")]
        ES256 = 6,


        [System.Runtime.Serialization.EnumMember(Value = @"ES384")]
        ES384 = 7,


        [System.Runtime.Serialization.EnumMember(Value = @"ES512")]
        ES512 = 8,


        [System.Runtime.Serialization.EnumMember(Value = @"PS256")]
        PS256 = 9,


        [System.Runtime.Serialization.EnumMember(Value = @"PS384")]
        PS384 = 10,


        [System.Runtime.Serialization.EnumMember(Value = @"PS512")]
        PS512 = 11,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class TestCase
    {
        [Newtonsoft.Json.JsonProperty("tracker", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Tracker { get; set; }

        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("version", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Version { get; set; }

        [Newtonsoft.Json.JsonProperty("uri", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Uri { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class Constraints
    {
        [Newtonsoft.Json.JsonProperty("key", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Key { get; set; }

        [Newtonsoft.Json.JsonProperty("value", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object Value { get; set; }


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