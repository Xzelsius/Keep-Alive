// Copyright (c) Raphael Strotz. All rights reserved.

namespace RS.KeepAlive
{
    public class KeepAliveOptions
    {
        public string[] Targets { get; set; } = new string[0];
        public int Interval { get; set; } = 0;
    }
}
