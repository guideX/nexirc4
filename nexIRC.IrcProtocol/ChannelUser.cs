using System.Collections;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Represents a user in a specific channel
    /// </summary>
    public class ChannelUser {
        /// <summary>
        /// User
        /// </summary>
        public User User { get; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; }
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick => User.Nick;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        public ChannelUser(User user, string status) {
            User = user;
            Status = status;
        }
        /// <summary>
        /// Costructor
        /// </summary>
        /// <param name="user"></param>
        public ChannelUser(User user)
            : this(user, string.Empty) {
        }

        public override string ToString() => Status + User.Nick;
    }

    public class ChannelUserComparer : IComparer<ChannelUser>, IComparer {
        public int Compare(ChannelUser u1, ChannelUser u2) {
            if (!string.IsNullOrWhiteSpace(u1.Status) && !string.IsNullOrWhiteSpace(u2.Status)) {
                var statuses = Channel.UserStatuses;
                var s1 = u1.Status[0];
                var s2 = u2.Status[0];
                if (Array.IndexOf(statuses, s1) < Array.IndexOf(statuses, s2))
                    return -1;
                if (Array.IndexOf(statuses, s1) > Array.IndexOf(statuses, s2))
                    return 1;
                return u1.Nick.CompareTo(u2.Nick);
            }

            if (!string.IsNullOrWhiteSpace(u1.Status))
                return -1;

            if (!string.IsNullOrWhiteSpace(u2.Status))
                return 1;

            return u1.Nick.CompareTo(u2.Nick);
        }

        public int Compare(object x, object y) {
            if (x is not ChannelUser u1 || y is not ChannelUser u2)
                return 0;
            return Compare(u1, u2);
        }
    }
}