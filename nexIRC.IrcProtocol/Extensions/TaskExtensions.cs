using System.Text;
namespace nexIRC.IrcProtocol.Extensions {
    /// <summary>
    /// Task Extensions
    /// </summary>
    internal static class TaskExtensions {
        /// <summary>
        /// Safe Fire And Forget
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <param name="onException"></param>
        public static async void SafeFireAndForget(this Task task, bool continueOnCapturedContext = true, Action<Exception>? onException = null) {
            try {
                await task.ConfigureAwait(continueOnCapturedContext);
            } catch (Exception ex) when (onException != null) {
                onException(ex);
            }
        }
        /// <summary>
        /// Split to Lines
        /// </summary>
        /// <param name="stringToSplit"></param>
        /// <param name="maxLineLength"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitToLines(this string stringToSplit, int maxLineLength) {
            string[] words = stringToSplit.Split(' ');
            StringBuilder line = new StringBuilder();
            foreach (string word in words) {
                if (word.Length + line.Length <= maxLineLength) {
                    line.Append(word + " ");
                } else {
                    if (line.Length > 0) {
                        yield return line.ToString().Trim();
                        line.Clear();
                    }
                    string overflow = word;
                    while (overflow.Length > maxLineLength) {
                        yield return overflow.Substring(0, maxLineLength);
                        overflow = overflow.Substring(maxLineLength);
                    }
                    line.Append(overflow + " ");
                }
            }
            yield return line.ToString().Trim();
        }
    }
}