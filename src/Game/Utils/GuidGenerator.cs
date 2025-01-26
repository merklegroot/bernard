using System;

namespace Bernard.Utils;

public static class GuidGenerator
{
    public static string NewGuid()
    {
        return Guid.NewGuid().ToString();
    }
}