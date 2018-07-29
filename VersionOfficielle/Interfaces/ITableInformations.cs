using System.Collections.Generic;
using System.Linq;
using static VersionOfficielle.CAllInPokerGame;

namespace VersionOfficielle.Interfaces
{
    /// <summary>
    /// Represents a All in or Fold poker table.
    /// </summary>
    interface ITableInformations
    {
        /// <summary>
        /// Gets the stack of the callers. If the stack of the caller is the highest, this method
        /// will return the 2nd highest stack after him (of the players that plays after him).
        /// Otherwise, it will return his stack. 
        /// We do that for each callers.
        /// 
        /// This method must be called ONLY when there's one or more callers from a open shove.
        /// </summary>
        /// <returns>Returns the list of players that call with their stack in BB that repreents the BIg Blind Equivalent Shove.</returns>
        Dictionary<Position, byte> getBBEquivalentOfCallers();
        /// <summary>
        /// Gets the stack of the open-shover. If the stack of the open-shover is the highest, then this
        /// method will return the 2nd highest stack after him (of the players that plays after him).
        /// Otherwise, it will return his stack.
        /// 
        /// This method must be called ONLY when there's a open shove.
        /// </summary>
        /// <exception cref="Exception">If there was no open shove, this method will throw a exception.</exception>
        /// <returns>Returns a stack in BB that represents the Big Blind Equivalent Shove.</returns>
        byte getBBEquivalentShove();
        /// <summary>
        /// Get the position of ourself.
        /// </summary>
        /// <exception cref="Exception">Throws an exception if we couldn't find ourself.</exception>
        /// <returns>Returns the current position of ourself.</returns>
        Position getMyPosition();
        /// <summary>
        /// Get the position of the player that open-shoved.
        /// </summary>
        /// <exception cref="Exception">Throws an exception if there were no player that open-shoved.</exception>
        /// <returns>Returns the position of the player that open-shoved.</returns>
        Position getPositionOfOpenShover();
        /// <summary>
        /// Get the number of callers after the open-shover.
        /// </summary>
        /// <returns>Returns the number of callers (1 or 2) after the open-shover.</returns>
        byte getNumberOfCallers();
        /// <summary>
        /// Gets our equivalent stack if we shove. If our stack is the highest, then this
        /// method will return the 2nd highest stack after us (of the players that plays after us).
        /// Otherwise, it will return our stack.
        /// 
        /// This method must be called ONLY when there's NO open shove.
        /// </summary>
        /// <exception cref="Exception">If there was a open shove, this method will throw a exception.</exception>
        /// <returns>Returns a stack in BB that represents Big Blind Equivalent Shove.</returns>
        byte getOurBBEquivalentShove();
        /// <summary>
        /// Returns true if our hand is in the received range.
        /// </summary>
        /// <param name="_range">Range must be in this format (22+ AJo+ AJs+ QJs).</param>
        /// <returns>Returns true if our hand is in the received range. Otherwise, it returns false.</returns>
        bool isOurHandInThisRange(string _range);
        /// <summary>
        /// Indicates if someone called a open shove before us.
        /// </summary>
        /// <returns>Returns true if someone called an open shove before us. Otherwise, returns false.</returns>
        bool someoneCalledAnOpenShoved();
        /// <summary>
        /// Indicates if someone open shoved before us.
        /// </summary>
        /// <returns>Returns true if someone open shoved before us. Otherwise, returns false.</returns>
        bool someoneOpenShoved();

        /// <summary>
        /// Returns the logs of the table information. Useful because before a bot has to make a decision,
        /// the table has to know when to call the bot to make a decision. Because of this, it will
        /// happen often that we will need to debug detection methods and therefore, we need the
        /// logs from the table.
        /// </summary>
        /// <returns>Returns the logs of what happened during a hand.</returns>
        List<string> GetDetectionLogsForCurrentHand();
    }
}
