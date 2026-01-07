| Method                     | SystemComplexity | EntityCount | Mean            | Error         | StdDev        | Median          | Ratio | RatioSD |
|--------------------------- |----------------- |------------ |----------------:|--------------:|--------------:|----------------:|------:|--------:|
| DumbFlatArrays             | 0                | 100         |        979.9 ns |      83.02 ns |     223.02 ns |        884.1 ns |  0.54 |    0.12 |
| ManualComponentCollections | 0                | 100         |        821.8 ns |       4.05 ns |       3.59 ns |        821.5 ns |  0.45 |    0.00 |
| ManualArchetypeEntry       | 0                | 100         |      1,144.6 ns |      11.67 ns |      10.34 ns |      1,145.1 ns |  0.63 |    0.01 |
| ManualEntities             | 0                | 100         |      4,923.9 ns |      82.61 ns |      84.84 ns |      4,889.4 ns |  2.70 |    0.05 |
| ParamGroupForEach          | 0                | 100         |      1,826.1 ns |      17.97 ns |      15.93 ns |      1,825.5 ns |  1.00 |    0.01 |
|                            |                  |             |                 |               |               |                 |       |         |
| DumbFlatArrays             | 1                | 100         |        664.9 ns |       4.42 ns |       3.45 ns |        666.0 ns |  0.37 |    0.00 |
| ManualComponentCollections | 1                | 100         |        818.1 ns |       9.33 ns |       8.73 ns |        815.7 ns |  0.45 |    0.01 |
| ManualArchetypeEntry       | 1                | 100         |      1,144.7 ns |      17.88 ns |      14.93 ns |      1,140.8 ns |  0.63 |    0.01 |
| ManualEntities             | 1                | 100         |      4,953.8 ns |      41.05 ns |      36.39 ns |      4,940.1 ns |  2.74 |    0.04 |
| ParamGroupForEach          | 1                | 100         |      1,806.0 ns |      24.44 ns |      21.67 ns |      1,812.2 ns |  1.00 |    0.02 |
|                            |                  |             |                 |               |               |                 |       |         |
| DumbFlatArrays             | 2                | 100         |     28,256.4 ns |     321.47 ns |     300.71 ns |     28,055.9 ns |  0.96 |    0.01 |
| ManualComponentCollections | 2                | 100         |     28,175.1 ns |      58.96 ns |      49.24 ns |     28,186.0 ns |  0.95 |    0.00 |
| ManualArchetypeEntry       | 2                | 100         |     28,558.9 ns |      61.85 ns |      48.29 ns |     28,553.6 ns |  0.97 |    0.00 |
| ManualEntities             | 2                | 100         |     32,370.9 ns |     113.91 ns |     100.98 ns |     32,396.3 ns |  1.10 |    0.01 |
| ParamGroupForEach          | 2                | 100         |     29,534.0 ns |     134.46 ns |     125.77 ns |     29,505.5 ns |  1.00 |    0.01 |
|                            |                  |             |                 |               |               |                 |       |         |
| DumbFlatArrays             | 0                | 100000      |    590,023.7 ns |  11,596.64 ns |  13,354.71 ns |    584,924.5 ns |  0.44 |    0.01 |
| ManualComponentCollections | 0                | 100000      |    843,045.3 ns |  16,656.72 ns |  22,236.25 ns |    841,343.3 ns |  0.63 |    0.02 |
| ManualArchetypeEntry       | 0                | 100000      |  1,273,437.8 ns |  24,909.97 ns |  31,503.13 ns |  1,275,145.7 ns |  0.95 |    0.03 |
| ManualEntities             | 0                | 100000      |  6,268,356.3 ns | 121,841.04 ns | 145,043.11 ns |  6,252,601.1 ns |  4.66 |    0.14 |
| ParamGroupForEach          | 0                | 100000      |  1,346,864.7 ns |  26,801.25 ns |  26,322.41 ns |  1,353,530.9 ns |  1.00 |    0.03 |
|                            |                  |             |                 |               |               |                 |       |         |
| DumbFlatArrays             | 1                | 100000      |    598,562.2 ns |  11,418.90 ns |  14,441.25 ns |    596,595.9 ns |  0.43 |    0.02 |
| ManualComponentCollections | 1                | 100000      |    869,666.9 ns |  17,005.82 ns |  25,453.51 ns |    863,868.4 ns |  0.63 |    0.02 |
| ManualArchetypeEntry       | 1                | 100000      |  1,320,800.0 ns |  26,313.66 ns |  40,967.20 ns |  1,322,636.5 ns |  0.95 |    0.04 |
| ManualEntities             | 1                | 100000      |  6,453,230.0 ns | 122,971.04 ns | 120,774.00 ns |  6,453,220.5 ns |  4.65 |    0.15 |
| ParamGroupForEach          | 1                | 100000      |  1,387,587.4 ns |  26,723.25 ns |  37,462.26 ns |  1,377,528.2 ns |  1.00 |    0.04 |
|                            |                  |             |                 |               |               |                 |       |         |
| DumbFlatArrays             | 2                | 100000      | 28,126,315.5 ns |  49,348.64 ns |  41,208.35 ns | 28,137,214.8 ns |  0.96 |    0.00 |
| ManualComponentCollections | 2                | 100000      | 28,686,066.6 ns |  73,711.40 ns |  68,949.69 ns | 28,672,204.7 ns |  0.97 |    0.00 |
| ManualArchetypeEntry       | 2                | 100000      | 29,194,147.1 ns |  85,129.75 ns |  79,630.42 ns | 29,185,449.7 ns |  0.99 |    0.00 |
| ManualEntities             | 2                | 100000      | 34,631,968.9 ns | 271,038.42 ns | 240,268.50 ns | 34,528,831.4 ns |  1.18 |    0.01 |
| ParamGroupForEach          | 2                | 100000      | 29,437,672.6 ns |  95,513.61 ns |  89,343.49 ns | 29,435,153.7 ns |  1.00 |    0.00 |