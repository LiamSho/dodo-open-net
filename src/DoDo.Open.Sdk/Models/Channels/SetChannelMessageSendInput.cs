﻿using DoDo.Open.Sdk.Models.Messages;
using System.Text.Json.Serialization;

namespace DoDo.Open.Sdk.Models.Channels
{
    public class SetChannelMessageSendInput<T>
    where T : MessageBodyBase
    {
        /// <summary>
        /// 频道ID
        /// </summary>
        [JsonPropertyName("channelId")]
        public string ChannelId { get; set; }

        /// <summary>
        /// 消息类型，1：文字消息，2：图片消息，3：视频消息，5：文件消息
        /// </summary>
        [JsonPropertyName("messageType")]
        public int MessageType
        {
            get
            {
                if (MessageBody is MessageBodyPicture)
                {
                    return MessageTypeConst.Picture;
                }
                else if (MessageBody is MessageBodyVideo)
                {
                    return MessageTypeConst.Video;
                }
                else if (MessageBody is MessageBodyFile)
                {
                    return MessageTypeConst.File;
                }

                return MessageTypeConst.Text;
            }
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonPropertyName("messageBody")]
        public T MessageBody { get; set; }

        /// <summary>
        /// 引用消息ID
        /// </summary>
        [JsonPropertyName("referencedMessageId")]
        public string ReferencedMessageId { get; set; }
    }
}
