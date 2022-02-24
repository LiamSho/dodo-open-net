﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DoDo.Open.Sdk.Models.Channels
{
    public class WithdrawChannelMessageInput
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        [JsonProperty("messageId")]
        public string MessageId { get; set; }

        /// <summary>
        /// 撤回原因
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
