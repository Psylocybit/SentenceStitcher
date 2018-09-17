using System;
using System.Collections.Generic;
using System.Linq;

namespace SentenceStitcher
{
    public class Stitcher
    {
        #region Constructors

        public Stitcher(IEnumerable<string> stringInputs)
        {
            this.IsRunning = false;
            this.Segments = stringInputs.Select(s => new StitcherSegment(s)).ToList();
        }

        public Stitcher(IEnumerable<StitcherSegment> segments)
        {
            this.IsRunning = false;
            this.Segments = segments.ToList();
        }

        #endregion

        #region Properties

        public List<StitcherSegment> Segments { get; private set; }

        internal bool IsRunning { get; private set; }
        
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
            bool done = false;
            var links = new Dictionary<int, List<SegmentLink>>();

            this.IsRunning = true;

            while (!done)
            {
                for (int i = 0; i < Segments.Count; i++)
                {
                    links[i] = new List<SegmentLink>();

                    var segmentA = Segments[i];

                    for (int j = 0; j < Segments.Count; j++)
                    {
                        if (i == j)
                            continue;

                        var segmentB = Segments[j];

                        var link = new SegmentLink(segmentA, segmentB);
                        var intersections = (List<string>)link.GetIntersection(true);

                        if (intersections.Count > 0)
                        {
                            var newSegment = new StitcherSegment(intersections);
                            this.Segments.Remove(segmentA);
                            this.Segments.Remove(segmentB);
                            this.Segments.Add(newSegment);
                        }
                    }
                }

                if (this.Segments.Count == 1)
                    done = true;
            }

            var resultList = new List<string>();

            foreach (var s in this.Segments)
                foreach (var f in s.Fragments)
                    resultList.Add(f);

            while (resultList.Count > 1)
            {
                string a = resultList[0];
                string b = resultList[1];
                string ab;

                if (a[a.Length - 1] == ',' || a[a.Length - 1] == '.')
                    ab = string.Join(' ', a, b);
                else if (b[b.Length - 1] == ',' || b[b.Length - 1] == '.')
                    ab = string.Join(new char(), a, b);
                else
                    ab = string.Join(' ', a, b);

                resultList.RemoveAt(1);
                resultList[0] = ab;
            }

            this.IsRunning = false;

            return resultList[0];
        }

        public override string ToString()
        {
            return this.Segments.ToFormattedString('\"');
        }

        #endregion
    }
}
