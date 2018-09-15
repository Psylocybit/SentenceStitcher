using System;

namespace SentenceStitcher
{
    public class StitcherWorker
    {
        #region Fields

        private Stitcher stitcher;
        private int startIndex;

        #endregion

        #region Constructors

        public StitcherWorker(Stitcher caller, int start)
        {
            this.stitcher = caller;
            this.startIndex = start;
        }

        #endregion
    }
}