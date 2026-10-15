using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Menu
{
    public record MenuGroupDto
    {
        public string GroupName { get; init; } = string.Empty;
        public List<MenuItemDto> Items { get; init; } = new();
    }
}
