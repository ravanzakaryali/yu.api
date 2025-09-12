namespace Yu.Application.DTOs;

public class GetEmployeesFilterRequestDto
{
    public string? SearchTerm { get; set; } // Employee adı, username və ya email üçün
    public DateTime? CreatedDateFrom { get; set; } // Yaradılma başlanğıc tarixi
    public DateTime? CreatedDateTo { get; set; } // Yaradılma bitmə tarixi
    public bool? IsActive { get; set; } // Aktiv status filter
    // Pagination parametrləri
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
