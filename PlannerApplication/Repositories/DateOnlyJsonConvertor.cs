using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlannerApplication.Repositories
{
    internal class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        /// <summary>
        /// Reads the string and prse it as DateOnly.
        /// </summary>
        /// <param name="reader">reader</param>
        /// <param name="typeToConvert">type to convert</param>
        /// <param name="options">options</param>
        /// <returns>DateOnly fields.</returns>
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.Parse(reader.GetString()!);
        }

        /// <summary>
        /// Writes the DateOonly type to the file as string.
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value">value</param>
        /// <param name="options">options</param>
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
