// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Design",
    "CA1028:Enum Storage should be Int32",
    Justification = "Enums are often mapped to a region of bits, having a specific data type helps make it more explicit",
    Scope = "module")]
[assembly: SuppressMessage(
    "Performance",
    "CA1815:Override equals and operator equals on value types",
    Justification = "Structs rarely need to be compared this way.",
    Scope = "module")]
[assembly: SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not match keywords",
    Justification = "Needed for terse queries.",
    Scope = "module")]
