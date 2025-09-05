namespace Yu.Application.DTOs;

public class GetClientsFilterRequestDto
{
    public string? SearchTerm { get; set; } // Client adı, telefon nömrəsi və ya email üçün
    public DateTime? RegisterDateFrom { get; set; } // Qeydiyyat başlanğıc tarixi
    public DateTime? RegisterDateTo { get; set; } // Qeydiyyat bitmə tarixi
    // Pagination parametrləri
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
