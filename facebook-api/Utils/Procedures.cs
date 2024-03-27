namespace facebook_api.Utils
{
    public static class Procedures
    {
        // post
        public static readonly string pr_post_delete = "pr_post_delete";
        public static readonly string pr_post_get_feed_posts = "pr_post_get_feed_posts";
        public static readonly string pr_post_get_user_profile_posts = "pr_post_get_user_profile_posts";
        public static readonly string pr_post_upsert = "pr_post_upsert";
        public static readonly string pr_post_get_by_id = "pr_post_get_by_id";
        
        // friendsip
        public static readonly string pr_friendship_send_request = "pr_friendship_send_request";
        public static readonly string pr_friendship_get_by_status = "pr_friendship_get_by_status";
        public static readonly string pr_friendship_update_request = "pr_friendship_update_request";
        public static readonly string pr_friendship_check_status = "pr_friendship_check_status";

        // comment
        public static readonly string pr_comment_create = "pr_comment_create";
        public static readonly string pr_comment_update = "pr_comment_update";
        public static readonly string pr_comment_get_all = "pr_comment_get_all";
        public static readonly string pr_comment_delete = "pr_comment_delete";
        public static readonly string pr_comment_get_by_id = "pr_comment_get_by_id";

        // reaction
        public static readonly string pr_reaction_like = "pr_reaction_like";
        public static readonly string pr_reaction_dislike = "pr_reaction_dislike";

        // user
        public static readonly string pr_user_change_password = "pr_user_change_password";
        public static readonly string pr_user_create = "pr_user_create";
        public static readonly string pr_user_get_by_email = "pr_user_get_by_email";
        public static readonly string pr_user_create_password_token = "pr_user_create_password_token";
        public static readonly string pr_user_edit = "pr_user_edit";
        public static readonly string pr_user_get_by_id = "pr_user_get_by_id";
        public static readonly string pr_user_get_profile = "pr_user_get_profile";
        public static readonly string pr_user_search = "pr_user_search";
        public static readonly string pr_user_verify_email = "pr_user_verify_email";
        public static readonly string pr_user_get_by_token = "pr_user_get_by_token";
    }
}
