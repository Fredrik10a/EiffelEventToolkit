using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

public class CustomTypeResolver : CSharpTypeResolver
{
    public CustomTypeResolver(CSharpGeneratorSettings settings) : base(settings) { }

    public override string Resolve(JsonSchema schema, bool isNullable, string typeNameHint)
    {
        // Check if schema is of type integer and map it to long
        if (schema.Type == JsonObjectType.Integer && !schema.IsEnumeration)
        {
            return "long" + (isNullable ? "?" : string.Empty);
        }

        // Default behavior for other types
        return base.Resolve(schema, isNullable, typeNameHint);
    }
}
