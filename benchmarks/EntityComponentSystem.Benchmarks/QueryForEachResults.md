Ran In-Process, the allocations might be a false positive

| Method                     | SystemComplexity | EntityCount | Mean            | Error         | Ratio | Allocated | Alloc Ratio |
|--------------------------- |----------------- |------------ |----------------:|--------------:|------:|----------:|------------:|
| DumbFlatArrays             | 0                | 100         |        536.5 ns |       7.64 ns |  0.35 |         - |          NA |
| ManualComponentCollections | 0                | 100         |        650.1 ns |       4.35 ns |  0.42 |         - |          NA |
| ManualArchetypeEntry       | 0                | 100         |        899.5 ns |       6.69 ns |  0.58 |         - |          NA |
| ParamGroupForEach          | 0                | 100         |      1,540.3 ns |       4.31 ns |  1.00 |         - |          NA |
| ManualEntities             | 0                | 100         |      4,456.8 ns |      21.58 ns |  2.89 |         - |          NA |
|                            |                  |             |                 |               |       |           |             |
| DumbFlatArrays             | 1                | 100         |     27,734.3 ns |      61.80 ns |  0.95 |         - |          NA |
| ManualComponentCollections | 1                | 100         |     27,979.7 ns |     120.18 ns |  0.96 |         - |          NA |
| ManualArchetypeEntry       | 1                | 100         |     28,179.0 ns |      57.17 ns |  0.97 |         - |          NA |
| ParamGroupForEach          | 1                | 100         |     29,043.4 ns |      73.79 ns |  1.00 |         - |          NA |
| ManualEntities             | 1                | 100         |     31,900.6 ns |      92.76 ns |  1.10 |         - |          NA |
|                            |                  |             |                 |               |       |           |             |
| DumbFlatArrays             | 0                | 100000      |    598,174.4 ns |   7,580.24 ns |  0.55 |       6 B |        0.55 |
| ManualComponentCollections | 0                | 100000      |    671,933.9 ns |  13,139.76 ns |  0.62 |       6 B |        0.55 |
| ManualArchetypeEntry       | 0                | 100000      |  1,006,836.2 ns |  10,146.54 ns |  0.92 |      11 B |        1.00 |
| ParamGroupForEach          | 0                | 100000      |  1,092,450.2 ns |  13,835.08 ns |  1.00 |      11 B |        1.00 |
| ManualEntities             | 0                | 100000      |  5,731,453.5 ns |  83,044.33 ns |  5.25 |      44 B |        4.00 |
|                            |                  |             |                 |               |       |           |             |
| DumbFlatArrays             | 1                | 100000      | 28,097,326.1 ns |  51,650.05 ns |  0.97 |     177 B |        1.00 |
| ManualComponentCollections | 1                | 100000      | 28,267,062.2 ns |  75,828.74 ns |  0.97 |     177 B |        1.00 |
| ManualArchetypeEntry       | 1                | 100000      | 28,717,181.5 ns | 101,713.29 ns |  0.99 |     177 B |        1.00 |
| ParamGroupForEach          | 1                | 100000      | 29,023,692.1 ns |  85,321.44 ns |  1.00 |     177 B |        1.00 |
| ManualEntities             | 1                | 100000      | 33,968,888.9 ns | 190,987.19 ns |  1.17 |     378 B |        2.14 |

