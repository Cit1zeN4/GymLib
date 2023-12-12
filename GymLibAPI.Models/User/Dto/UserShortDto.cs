namespace GymLibAPI.Models.User.Dto;

public class UserShortDto
{
    public UserShortDto() { }

    public UserShortDto(UserEntity entity)
    {
        Id = entity.Id;
        UserName = entity.UserName;
        Email = entity.Email;
        FollowersCount = entity.Followers.Count;
        FollowingCount = entity.Following.Count;
    }

    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
}