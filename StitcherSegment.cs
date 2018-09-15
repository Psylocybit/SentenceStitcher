using System;
using System.Collections.Generic;
using System.Linq;

namespace SentenceStitcher
{
    public struct StitcherSegment
    {
        #region Globals

        internal static readonly char[] DelimiterChars = { ' ', ',', '.', '?', '!', ';', ':', '\t' };
        internal static readonly char[] PunctuationChars = { ',', '.', '?', '!', ';', ':' };
        internal static readonly char[] EndPunctuationChars = { '.', '?', '!' };

        #endregion

        #region Constructors

        public StitcherSegment(string input)
        {
            this.Input = input;
            this.Fragments = new List<string>(ProcessSegmentInput(this.Input));
        }

        #endregion

        #region Properties

        public string Input { get; }

        public List<string> Fragments { get; private set; }

        #endregion

        #region Calculated Properties

        public List<string> Words => (from s in this.Input.Split(DelimiterChars)
                                      where s != string.Empty
                                      select s).ToList();

        public List<char> Punctuation =>
            this.Input.ToCharArray().Intersect(PunctuationChars).Distinct().ToList();

        public bool HasPunctuation => this.Punctuation.Any();

        public bool IsStarting => char.IsUpper(this.Fragments[0][0]);

        public bool IsEnding =>
            this.Fragments.GetLast().Length == 1 && EndPunctuationChars.Contains(this.Fragments.GetLast()[0]);

        #endregion

        #region Methods

        internal static IEnumerable<string> ProcessSegmentInput(string input, bool trim = true)
        {
            var splitInput = input.SplitAndKeep(DelimiterChars);

            return trim
                ? (from s in splitInput
                   where s.Trim() != string.Empty
                   select s)
                : splitInput;
        }

        public override string ToString() => this.Fragments.ToFormattedString('\"');

        #endregion
    }
}
