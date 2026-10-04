Ran In-Process, the allocations might be a false positive due to that

| Method                     | SystemComplexity | EntityCount | Mean            | Error         | Ratio | Allocated | Alloc Ratio |
|--------------------------- |----------------- |------------ |----------------:|--------------:|------:|----------:|------------:|
| DumbFlatArrays             | 0                | 100         |        140.7 ns |       0.40 ns |  0.16 |         - |          NA |
| ManualComponentCollections | 0                | 100         |        424.8 ns |       1.76 ns |  0.48 |         - |          NA |
| ForEach                    | 0                | 100         |        884.5 ns |       8.34 ns |  1.00 |         - |          NA |
| ManualArchetypeEntry       | 0                | 100         |        459.8 ns |       3.76 ns |  0.52 |         - |          NA |
| ManualEntities             | 0                | 100         |      3,018.8 ns |       5.41 ns |  3.41 |         - |          NA |
|                            |                  |             |                 |               |       |           |             |
| DumbFlatArrays             | 1                | 100         |     25,458.8 ns |      44.69 ns |  0.96 |         - |          NA |
| ManualComponentCollections | 1                | 100         |     25,781.1 ns |       6.81 ns |  0.97 |         - |          NA |
| ForEach                    | 1                | 100         |     26,607.9 ns |     133.26 ns |  1.00 |         - |          NA |
| ManualArchetypeEntry       | 1                | 100         |     25,819.8 ns |      26.99 ns |  0.97 |         - |          NA |
| ManualEntities             | 1                | 100         |     28,340.5 ns |      40.62 ns |  1.07 |         - |          NA |
|                            |                  |             |                 |               |       |           |             |
| DumbFlatArrays             | 0                | 100000      |    113,388.1 ns |     441.76 ns |  0.36 |       1 B |        0.20 |
| ManualComponentCollections | 0                | 100000      |    162,756.5 ns |     540.13 ns |  0.52 |       3 B |        0.60 |
| ForEach                    | 0                | 100000      |    313,599.4 ns |   1,506.00 ns |  1.00 |       5 B |        1.00 |
| ManualArchetypeEntry       | 0                | 100000      |    453,494.0 ns |   3,952.16 ns |  1.45 |       2 B |        0.40 |
| ManualEntities             | 0                | 100000      |  2,984,879.9 ns |  27,286.28 ns |  9.52 |      20 B |        4.00 |
|                            |                  |             |                 |               |       |           |             |
| DumbFlatArrays             | 1                | 100000      | 22,717,839.0 ns |  61,976.92 ns |  0.98 |     325 B |        1.04 |
| ManualComponentCollections | 1                | 100000      | 23,376,731.0 ns | 301,448.69 ns |  1.00 |     147 B |        0.47 |
| ForEach                    | 1                | 100000      | 23,262,610.9 ns |  53,141.57 ns |  1.00 |     314 B |        1.00 |
| ManualArchetypeEntry       | 1                | 100000      | 23,418,230.3 ns |  43,098.22 ns |  1.01 |     325 B |        1.04 |
| ManualEntities             | 1                | 100000      | 26,873,851.3 ns | 296,198.81 ns |  1.16 |     336 B |        1.07 |

