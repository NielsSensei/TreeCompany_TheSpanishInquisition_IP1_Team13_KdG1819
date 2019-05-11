namespace Domain.Projects
{
    public enum LikeVisibility
    {
        OnlyLikeCount = 0,
        FbLikeCount = 1,
        TwitterLikeCount = 2,
        LikeCountAndFbLikeCount = 3,
        LikeCountAndTwitterLikeCount = 4,
        FbLikeCountAndTwitterLikeCount = 5,
        LikeFbAndTwitterCount = 6
    }
}