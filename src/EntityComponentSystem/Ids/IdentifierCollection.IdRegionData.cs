namespace Tourmi.EntityComponentSystem;

internal partial class IdentifierCollection
{
    private struct IdRegionData
    {
        /// <summary>
        /// Offset for all the identifiers created via this region
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// Amount of alive identifiers in the region
        /// </summary>
        public uint AliveCount { get; set; }

        /// <summary>
        /// Amount of identifiers that were already initialized in the region
        /// </summary>
        public uint InitializedCount { get; set; }

        /// <summary>
        /// Capacity of the region
        /// </summary>
        public uint Capacity { get; set; }

        /// <summary>
        /// The next available identifier that can be made alive
        /// </summary>
        public readonly uint NextAliveIndex => Offset + AliveCount;

        /// <summary>
        /// The next available identifier to be initialized
        /// </summary>
        public readonly uint NextInitializedIndex => Offset + InitializedCount;

        /// <summary>
        /// The last possible id of this region
        /// </summary>
        public readonly uint EndIdInclusive => Offset + (Capacity - 1);

        /// <summary>
        /// Returns true if the region data is full.
        /// </summary>
        public readonly bool IsFull => AliveCount >= Capacity;

        /// <summary>
        /// Returns true if the index fits in the given region.
        /// </summary>
        public readonly bool Contains(uint index) => index >= Offset && index < Offset + Capacity;

        /// <summary>
        /// Splits and shorten this region based on the <paramref name="other"/> region.
        /// </summary>
        public readonly IdRegionData[] SplitWith(IdRegionData other)
        {
            if (EndIdInclusive < other.Offset || Offset > other.EndIdInclusive)
            {
                // No overlap, return this.
                return [this];
            }

            if (other.Offset > Offset && other.EndIdInclusive < EndIdInclusive)
            {
                // This region is split in two by the other.
                return [
                    new() { Offset = Offset, Capacity = other.Offset - Offset },
                    new() { Offset = other.EndIdInclusive + 1, Capacity = EndIdInclusive - other.EndIdInclusive }
                ];
            }

            if (Offset < other.Offset && EndIdInclusive >= other.Offset && EndIdInclusive < other.EndIdInclusive)
            {
                // This region starts before the other, and ends before.
                return [new() { Offset = Offset, Capacity = other.Offset - Offset }];
            }

            if (Offset >= other.Offset && EndIdInclusive > other.EndIdInclusive && Offset <= other.EndIdInclusive)
            {
                // This region starts inside the other, and ends after.
                return [new() { Offset = other.EndIdInclusive + 1, Capacity = EndIdInclusive - other.EndIdInclusive }];
            }

            // This region is fully contained or equal to the other so we cannot split it.
            return [];
        }
    }
}
