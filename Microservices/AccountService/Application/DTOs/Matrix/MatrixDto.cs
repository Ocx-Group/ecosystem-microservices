using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs.Matrix;

/// <summary>
/// Nodo del arbol de matriz. Los nombres de serializacion se fijan explicitamente
/// porque Newtonsoft esta configurado sin naming strategy: sin estos atributos el
/// JSON saldria en PascalCase y el frontend, que consume la misma forma que
/// <see cref="UserUniLevelTreeDto"/> (id / userName / imageProfileUrl / children),
/// no encontraria ningun campo.
/// </summary>
public class MatrixDto
{
    [JsonProperty("id")] public int UserId { get; set; }
    [JsonProperty("userName")] public string Username { get; set; } = string.Empty;
    [JsonProperty("father")] public int Father { get; set; }
    [JsonProperty("level")] public int Level { get; set; }
    [JsonProperty("imageProfileUrl")] public string? ImageProfileUrl { get; set; }
    [JsonProperty("qualificationCount")] public int? QualificationCount { get; set; }
    [JsonProperty("children")] public List<MatrixDto> Children { get; set; } = [];
}
