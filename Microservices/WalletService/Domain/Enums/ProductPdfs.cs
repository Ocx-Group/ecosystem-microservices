namespace Ecosystem.WalletService.Domain.Enums;

public class ProductPdfMapping
{
    public static readonly Dictionary<string, string> ProductToPdfMap = new Dictionary<string, string>
    {
        { "00 Pool_5", "eco_pool_hormiga" },
        { "00 Pool_10", "eco_pool_hormiga" },
        { "00 Pool_25", "eco_pool_mariposa" },
        { "01 Pool_50", "eco_pool_colibri" },
        { "02 Pool_100", "eco_pool_yiguirro" },
        { "03 Pool_250", "eco_pool_caballo" },
        { "04 Pool_500", "eco_pool_delfin" },
        { "05 Pool_1000", "eco_pool_lobo" },
        { "06 Pool_2500", "eco_pool_leon" },
        { "07 Pool_5000", "eco_pool_aguila" },
        { "08 Pool_7500", "eco_pool_cocodrilo" },
        { "09 Pool_10000", "eco_pool_tiburon" },
    };
}