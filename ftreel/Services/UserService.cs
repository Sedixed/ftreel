using ftreel.DATA;
using ftreel.Dto.user;
using ftreel.Entities;

namespace ftreel.Services;

public class UserService : IUserService
{
    private readonly AppDBContext _dbContext;

    public UserService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User FindUser(int id)
    {
        throw new NotImplementedException();
    }

    public IList<User> FindAllUser()
    {
        throw new NotImplementedException();
    }

    public User CreateUser(SaveUserDTO createRequest)
    {
        throw new NotImplementedException();
    }

    public User UpdateUser(int id, SaveUserDTO updateRequest)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(int id)
    {
        throw new NotImplementedException();
    }
}