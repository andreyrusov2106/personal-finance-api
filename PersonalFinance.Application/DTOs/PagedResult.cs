using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Items { get; init; } = new List<T>();
        public int TotalCount { get; set; }

    }
}
