using LealPasswordV2.Core.Database.Models;

namespace LealPasswordV2.Core.Database.ResourceAccess.Interfaces;

internal interface IRegisterAccess<T> : IAccess<T> where T : class
{
    Task<IEnumerable<Register>> GetAllByUserId(string userId);
}