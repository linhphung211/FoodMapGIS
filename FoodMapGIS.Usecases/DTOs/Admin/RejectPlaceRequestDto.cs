using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Admin
{
    public record RejectPlaceRequestDto
    {
            // Ly do tu choi (bat buoc)
            public string Reason { get; init; } = string.Empty;
    }
}
