using System;
using System.Collections.Generic;

namespace SentenceStitcher
{
    public struct SegmentLink
    {
        #region Fields

        public StitcherSegment First;

        public StitcherSegment Second;

        #endregion

        #region Constructors

        public SegmentLink(StitcherSegment first, StitcherSegment second)
        {
            this.First = first;
            this.Second = second;
        }

        #endregion

        #region Methods

        public IEnumerable<string> GetIntersection(bool combine = false)
        {
            var indices = (A0: -1, A1: -1, B0: -1, B1: -1);

            bool IsIntersecting() =>
                indices.A0 >= 0 && indices.A1 >= 0 &&
                indices.B0 >= 0 && indices.B1 >= 0;

            bool IsWithinBounds(int boundA, int boundB) =>
                indices.A1 < boundA && indices.B1 < boundB;

            bool IsIntersectionCountValid() =>
                indices.A1 - indices.A0 == indices.B1 - indices.B0;

            int GetIntersectionCount()
            {
                if (!IsIntersectionCountValid())
                    throw new Exception("Mismatched intersections detected.");
                
                return indices.A1 - indices.A0;
            }

            for (int i = 0; i < First.Fragments.Count - 1; i++)
            {
                var fai = First.Fragments[i];

                for (int j = 0; j < Second.Fragments.Count - 1; j++)
                {
                    var fbj = Second.Fragments[j];

                    if (fai == fbj)
                    {
                        bool loopSwitch = true;

                        indices = (i, i, j, j);

                        while (loopSwitch)
                        {
                            if (IsWithinBounds(First.Fragments.Count, Second.Fragments.Count))
                            {
                                var fa = First.Fragments[indices.A1];
                                var fb = Second.Fragments[indices.B1];

                                if (fa == fb)
                                {
                                    indices.A1++;
                                    indices.B1++;
                                }
                                else
                                {
                                    indices = (-1, -1, -1, -1);
                                    loopSwitch = false;
                                }
                            }
                            else
                                loopSwitch = false;
                        }
                    }

                    if (IsIntersecting() && IsIntersectionCountValid())
                        if (GetIntersectionCount() == 1)
                            if (!(indices.B0 == 0 && indices.A1 == First.Fragments.Count - 1) &&
                                !(indices.A0 == 0 && indices.B1 == Second.Fragments.Count - 1))
                                indices = (-1, -1, -1, -1);
                }

                if (IsIntersecting())
                    break;
            }

            var intersections = new List<string>();

            if (IsIntersecting())
            {
                if (combine)
                {
                    if (indices.A0 == 0) // at beginning of first sentence
                    {
                        if (indices.B1 == Second.Fragments.Count) // at end of second sentence
                        {
                            var count = First.Fragments.Count - (indices.A1 - indices.A0);
                            var firstPart = First.Fragments.GetRange(indices.A1, count);
                            var secondPart = Second.Fragments;
                            foreach (var s in firstPart)
                            {
                                secondPart.Add(s);
                            }

                            intersections.AddRange(secondPart);
                        }
                    }
                    else if (indices.A1 == First.Fragments.Count) // end of first sentence
                    {
                        if (indices.B0 == 0) // at beginning of second sentence
                        {
                            var count = Second.Fragments.Count - (indices.B1 - indices.B0);
                            var secondPart = Second.Fragments.GetRange(indices.B1, count);
                            var firstPart = First.Fragments;
                            foreach (var s in secondPart)
                            {
                                firstPart.Add(s);
                            }

                            intersections.AddRange(firstPart);
                        }
                    }
                }
                else
                {
                    intersections.AddRange(First.Fragments.GetRange(indices.A0, indices.A1 - indices.A0));
                }
            }

            return intersections;
        }

        #endregion
    }
}