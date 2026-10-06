using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.DTOs
{
    public record RegisterRequestDto(
    string Login,
    string Password,
    string Name);
}
