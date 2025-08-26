namespace RandomNumbersAPI.Models
{
    public class CustomRandomRequest
    {
        public string Type { get; set; } = string.Empty; // "number", "decimal", "string"
        public int? Min { get; set; }
        public int? Max { get; set; }
        public int? Decimals { get; set; } = 2;
        public int? Length { get; set; } = 8;
    }
}