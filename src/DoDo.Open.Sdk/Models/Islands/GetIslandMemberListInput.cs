﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DoDo.Open.Sdk.Models.Islands
{
    public class GetIslandMemberListInput
    {
        /// <summary>
        /// 群号
        /// </summary>
        [JsonProperty("islandId")]
        public string IslandId { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [JsonProperty("pageNo")]
        public int PageNo { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// 上一页最大ID值，为提升分页查询性能，需要传入上一页查询记录中的最大ID值，第一页时请传0
        /// </summary>
        [JsonProperty("maxId")]
        public int MaxId { get; set; }
    }
}
