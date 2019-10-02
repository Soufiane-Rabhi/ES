namespace ES
{
    /// <summary>
    /// User model.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets user id.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets user name.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets whether the user is authenticated or not.
        /// </summary>        
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel" /> instance.
        /// </summary>
        /// 
        /// <param name="id">User identifier.</param>
        /// <param name="userName">User name.</param>
        /// <param name="isAuthenticated">Whether the user is authenticated or not.</param>
        public UserViewModel(string id, string userName, bool isAuthenticated = true)
        {
            Id = id;
            UserName = userName;
            IsAuthenticated = isAuthenticated;
        }
    }
}
