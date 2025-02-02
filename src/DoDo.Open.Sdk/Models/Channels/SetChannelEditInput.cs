﻿namespace DoDo.Open.Sdk.Models.Channels
{
    public class SetChannelEditInput
    {
        /// <summary>
        /// 群号
        /// </summary>
        public string IslandId { get; set; }

        /// <summary>
        /// 频道ID
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// 频道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 分组ID
        /// </summary>
        public string GroupId { get; set; }
    }
}
