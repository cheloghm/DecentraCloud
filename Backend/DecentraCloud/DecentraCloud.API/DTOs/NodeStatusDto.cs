﻿namespace DecentraCloud.API.DTOs
{
    public class NodeStatusDto
    {
        public string NodeId { get; set; }
        public long Uptime { get; set; }
        public long Downtime { get; set; }
        public StorageStatsDto StorageStats { get; set; }
        public string OnlineStatus { get; set; }
        public string CauseOfDowntime { get; set; }
    }
}
