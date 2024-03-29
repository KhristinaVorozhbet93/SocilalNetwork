﻿using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Domain.Interfaces
{
    public interface IAccountRepository : IRepositoryEF<Account>
    {
        Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken);
        Task<Account?> FindAccountById(Guid id, CancellationToken cancellationToken);
    }
}
