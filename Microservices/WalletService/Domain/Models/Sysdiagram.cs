
namespace Ecosystem.WalletService.Domain.Models;

public partial class Sysdiagram
{
    public string Name { get; set; } = null!;

    public int PrincipalId { get; set; }

    public long DiagramId { get; set; }

    public int? Version { get; set; }

    public byte[]? Definition { get; set; }
}
