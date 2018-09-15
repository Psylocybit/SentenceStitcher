using System;
using System.Collections.Generic;

namespace SentenceStitcher
{
    public class Stitcher
    {
        #region Constructors

        public Stitcher(StitcherSegment[] segments)
        {
            this.Segments = new List<StitcherSegment>(segments);
            this.IsRunning = false;
            this.Workers = new List<StitcherWorker>();
        }

        public Stitcher(string[] inputs)
        {
            this.Segments = new List<StitcherSegment>(inputs.Length);

            for (var i = 0; i < inputs.Length; i++)
                this.Segments.Add(new StitcherSegment(inputs[i]));

            this.IsRunning = false;
            this.Workers = new List<StitcherWorker>();
        }

        #endregion

        #region Properties

        public List<StitcherSegment> Segments { get; private set; }

        internal bool IsRunning { get; private set; }

        private List<StitcherWorker> Workers { get; set; }

        #endregion

        #region Calculated Properties

        public IEnumerable<StitcherSegment> StartingSegments
        {
            get
            {
                foreach (var s in this.Segments)
                    if (s.IsStarting)
                        yield return s;
            }
        }

        public IEnumerable<StitcherSegment> EndingSegments
        {
            get
            {
                foreach (var s in this.Segments)
                    if (s.IsEnding)
                        yield return s;
            }
        }

        #endregion

        #region Methods

        public string Process()
        {
            string result = string.Empty;

            this.IsRunning = true;

            var resultRaw = new List<string>();
            var links = new Dictionary<int, List<SegmentLink>>();

            for (int i = 0; i < Segments.Count; i++)
            {
                links[i] = new List<SegmentLink>();

                var segmentA = Segments[i];
                var intersectMax = 0;
                var maxIndex = -1;
                bool yes = false;

                for (int j = 0; j < Segments.Count; j++)
                {
                    if (i == j)
                        continue;

                    var segmentB = Segments[j];

                    var link = new SegmentLink(segmentA, segmentB);
                    var intersections = (List<string>)link.GetIntersection();

                    if (intersections.Count > intersectMax)
                    {
                        maxIndex = j;
                        intersectMax = intersections.Count;
                        links[i].Add(link);
                        yes = true;
                    }
                    else intersectMax = 0;
                }

                // join some o deez
                if (yes)
                    resultRaw.Add(segmentA.ToString() + Segments[maxIndex].ToString());

                yes = false;
            }

            this.IsRunning = false;

            Console.WriteLine(resultRaw.Count);

            foreach (var r in resultRaw)
                Console.WriteLine(r);

            return result;
        }

        public override string ToString()
        {
            return this.Segments.ToFormattedString('\"');
        }

        #endregion
    }
}
