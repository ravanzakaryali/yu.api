namespace Yu.Application.DTOs;

public class GetOrdersFilterRequestDto
{
    public string? SearchTerm { get; set; } // Client adı və ya telefon nömrəsi üçün
    public OrderStatus? Status { get; set; } // Status filter üçün
    public DateTime? CreatedDateFrom { get; set; } // Başlanğıc tarixi
    public DateTime? CreatedDateTo { get; set; } // Bitmə tarixi
    
    // Pagination parametrləri
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 