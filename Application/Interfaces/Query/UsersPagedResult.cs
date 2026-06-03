using Application.DTOs.RequestDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Query
{
    public interface IUserQueryService
    {
        Task<UsersPagedResult> GetPagedAsync(PaginationRequest request);

    }
}
