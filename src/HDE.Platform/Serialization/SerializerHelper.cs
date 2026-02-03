using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HDE.Platform.Serialization
{
    public static class SerializerHelper
    {
        [Obsolete]
        public static TData Load<TData>(string file)
    where TData : new()
        {
            using var stream = File.OpenRead(file);
            return (TData)new XmlSerializer(typeof(TData)).Deserialize(stream);
        }

        [Obsolete]
        public static void Save<TData>(TData data, string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using var stream = File.OpenWrite(file);
            new XmlSerializer(typeof(TData)).Serialize(stream, data);
        }

        public static async Task SerializeToStreamAsync<T>(T obj, Stream stream, JsonSerializerOptions options = null)
        {
            ArgumentNullException.ThrowIfNull(stream);

            await JsonSerializer.SerializeAsync(stream, obj, options ?? new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        // Deserialize object from stream
        public static async Task<T> DeserializeFromStreamAsync<T>(Stream stream, JsonSerializerOptions options = null)
        {
            ArgumentNullException.ThrowIfNull(stream);

            return await JsonSerializer.DeserializeAsync<T>(stream, options ?? new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        // Synchronous versions
        public static void SerializeToStream<T>(T obj, Stream stream, JsonSerializerOptions options = null)
        {
            ArgumentNullException.ThrowIfNull(stream);

            using var writer = new Utf8JsonWriter(stream);
            JsonSerializer.Serialize(writer, obj, options ?? new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public static T DeserializeFromStream<T>(Stream stream, JsonSerializerOptions options = null)
        {
            ArgumentNullException.ThrowIfNull(stream);

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(json, options ?? new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
