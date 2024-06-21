using System.Runtime.Serialization;

namespace MovieApi.Http.Responses;

[DataContract]
public class ApiResponse<T>
{
    [DataMember]
    public string Version { get { return "1.0.0"; } }

    [DataMember]
    public int StatusCode { get; set; }

    [DataMember(EmitDefaultValue = true)]
    public string? Message { get; set; }

    [DataMember(EmitDefaultValue = true)]
    public T? Data { get; set; }

    [DataMember(EmitDefaultValue = true)]
    public object? Errors { get; set; }
}