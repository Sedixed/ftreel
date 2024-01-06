﻿using ftreel.Dto.user;
using ftreel.Entities;

namespace ftreel.Services;

public interface IUserService
{
    /**
     * Find a user in database by its ID.
     */
    User FindUser(int id);
    
    /**
     * Find all users from database.
     */
    IList<User> FindAllUser();
    
    /**
     * Create a user in database.
     */
    User CreateUser(SaveUserDTO createRequest);

    /**
     * Update a user in database.
     */
    User UpdateUser(int id, SaveUserDTO updateRequest);

    /**
     * Delete a user using its ID.
     */
    void DeleteUser(int id);
}