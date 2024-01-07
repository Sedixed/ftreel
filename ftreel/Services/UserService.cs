using System.ComponentModel.DataAnnotations;
using ftreel.DATA;
using ftreel.Dto.user;
using ftreel.Entities;
using ftreel.Exceptions;
using ftreel.Utils;

namespace ftreel.Services;

public class UserService : IUserService
{
    private readonly AppDBContext _dbContext;

    public UserService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /**
     * Find a user in database by its ID.
     */
    public User FindUser(int id)
    {
        var user = _dbContext.Users.Find(id);
        if (user == null)
        {
            throw new ObjectNotFoundException();
        }

        return user;
    }

    /**
     * Find all users from database.
     */
    public IList<User> FindAllUser()
    {
        var users = _dbContext.Users.ToList();
        return users;
    }

    /**
     * Create a user in database.
     */
    public User CreateUser(SaveUserDTO createRequest)
    {
        // Check if fields are not null.
        if (createRequest.Mail == null || createRequest.Password == null)
        {
            throw new Exception("One or more fields are null.");
        }
        
        // Check if the user has a well formed email.
        if (!IsMailValid(createRequest.Mail))
        {
            throw new Exception(createRequest.Mail + " is not a valid email.");
        }
        
        // Check if user exists.
        var userDb = _dbContext.Users.FirstOrDefault(u => u.Mail == createRequest.Mail);
        if (userDb != null) throw new Exception(createRequest.Mail +" already exists.");
        
        // Create the user.
        var user = new User
        {
            Mail = createRequest.Mail,
            Password = PasswordManager.HashPassword(createRequest.Password),
            Roles = createRequest.Roles
        };

        _dbContext.Add(user);
        _dbContext.SaveChanges();
        
        return user;
    }

    /**
     * Update a user in database.
     */
    public User UpdateUser(int id, SaveUserDTO updateRequest)
    {
        throw new NotImplementedException();
    }

    /**
     * Delete a user using its ID.
     */
    public void DeleteUser(int id)
    {
        var user = _dbContext.Users.Find(id);
        if (user == null)
        {
            throw new ObjectNotFoundException();
        }

        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }

    /**
     * Private method to check if an email address is valid.
     */
    private static bool IsMailValid(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}