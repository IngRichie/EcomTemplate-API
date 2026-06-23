public class StoreSettingsDTO
{
    public string StoreName { get; set; } = string.Empty;

    public string StoreEmail { get; set; } = string.Empty;

    public string StorePhone { get; set; } = string.Empty;

    public string StoreAddress { get; set; } = string.Empty;

    public string Currency { get; set; } = "GHS";

    public decimal DeliveryFee { get; set; }

    public decimal FreeDeliveryThreshold { get; set; }

    public int EstimatedDeliveryTime { get; set; }

    public decimal TaxPercentage { get; set; }

    public string SupportWhatsApp { get; set; } = string.Empty;

    public string SupportEmail { get; set; } = string.Empty;
}