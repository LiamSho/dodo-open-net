﻿using System;
using System.Globalization;
using System.Threading;
using DoDo.Open.Sdk.Models.Bots;
using DoDo.Open.Sdk.Models.Channels;
using DoDo.Open.Sdk.Models.Events;
using DoDo.Open.Sdk.Models.Islands;
using DoDo.Open.Sdk.Models.Members;
using DoDo.Open.Sdk.Models.Messages;
using DoDo.Open.Sdk.Models.Personals;
using DoDo.Open.Sdk.Models.Resources;
using DoDo.Open.Sdk.Models.Roles;
using DoDo.Open.Sdk.Models.WebSockets;
using Newtonsoft.Json;

namespace DoDo.Open.Sdk.Services
{
    /// <summary>
    /// 示例事件处理服务
    /// </summary>
    public class DemoEventProcessService : EventProcessService
    {
        private readonly OpenApiService _openApiService;

        public DemoEventProcessService(OpenApiService openApiService)
        {
            _openApiService = openApiService;
        }

        public override void Connected(string message)
        {
            Console.WriteLine(message);
        }

        public override void Disconnected(string message)
        {
            Console.WriteLine(message);
        }

        public override void Reconnected(string message)
        {
            Console.WriteLine(message);
        }

        public override void Exception(string message)
        {
            Console.WriteLine(message);
        }

        public override void ChannelMessageEvent(EventSubjectOutput<EventSubjectDataBusiness<EventBodyChannelMessage<MessageText>>> input)
        {
            var eventBody = input.Data.EventBody;
            var messageBody = input.Data.EventBody.MessageBody;

            var content = messageBody.Content;
            var reply = "";

            Console.WriteLine($"\n【{content}】");

            if (content.StartsWith("菜单"))
            {
                reply += $"<@!{eventBody.DodoId}>\n\n";
                reply += "**菜单来咯！**\n\n";
                reply += "【机器人】取机器人信息\n";
                reply += "【机器人】置机器人群退出\n";
                reply += "【群】取群列表\n";
                reply += "【群】取群信息\n";
                reply += "【群】取群成员列表\n";
                reply += "【频道】取频道列表\n";
                reply += "【频道】取频道信息\n";
                reply += "【频道】置频道文本消息发送\n";
                reply += "【频道】置频道图片消息发送\n";
                reply += "【频道】置频道视频消息发送\n";
                reply += "【频道】置频道消息更新\n";
                reply += "【频道】置频道消息撤回\n";
                reply += "【身份组】取身份组列表\n";
                reply += "【身份组】置身份组成员新增\n";
                reply += "【身份组】置身份组成员移除\n";
                reply += "【成员】取成员信息\n";
                reply += "【成员】取成员身份组列表\n";
                reply += "【成员】置成员昵称\n";
                reply += "【成员】置成员禁言\n";
                reply += "【个人】置个人文本消息发送\n";
                reply += "【个人】置个人图片消息发送\n";
                reply += "【个人】置个人视频消息发送\n";
                reply += "【资源】置资源图片上传\n";
                reply += "【事件】取WebSocket连接\n";
            }
            else if (content.Contains("置机器人群退出"))
            {
                var output = _openApiService.SetBotIslandLeave(new SetBotIslandLeaveInput
                {
                    IslandId = eventBody.IslandId
                });

                if (output)
                {
                    reply += "置机器人群成功！";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置成员禁言"))
            {
                var output = _openApiService.SetMemberBan(new SetMemberBanInput
                {
                    IslandId = eventBody.IslandId,
                    DoDoId = eventBody.DodoId,
                    Duration = 30,
                    Reason = "禁言测试"
                });

                if (output)
                {
                    reply += "置成员禁言成功！";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("取群列表"))
            {
                var outputList = _openApiService.GetIslandList(new GetIslandListInput());

                if (outputList != null)
                {
                    foreach (var output in outputList)
                    {
                        reply += $"群号：{output.IslandId}\n";
                        reply += $"群名称：{output.IslandName}\n";
                        reply += $"群头像：{output.CoverUrl}\n";
                        reply += $"系统公告频道号：{output.SystemChannelId}\n";
                        reply += $"进群默认频道号：{output.DefaultChannelId}\n";
                        reply += "\n";
                    }
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("取群信息"))
            {
                var output = _openApiService.GetIslandInfo(new GetIslandInfoInput
                {
                    IslandId = eventBody.IslandId
                });

                if (output != null)
                {
                    reply += $"群号：{output.IslandId}\n";
                    reply += $"群名称：{output.IslandName}\n";
                    reply += $"群头像：{output.CoverUrl}\n";
                    reply += $"群描述：{output.Description}\n";
                    reply += $"系统公告频道号：{output.SystemChannelId}\n";
                    reply += $"进群默认频道号：{output.DefaultChannelId}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("取群成员列表"))
            {
                var outputList = _openApiService.GetIslandMemberList(new GetIslandMemberListInput
                {
                    IslandId = eventBody.IslandId,
                    PageNo = 1,
                    PageSize = 100
                });

                if (outputList != null)
                {
                    foreach (var output in outputList)
                    {
                        reply += $"DoDo号：{output.DodoId}\n";
                        reply += $"在群昵称：{output.NickName}\n";
                        reply += $"头像：{output.AvatarUrl}\n";
                        reply += $"加群时间：{output.JoinTime}\n";
                        reply += $"性别：{output.Sex}\n";
                        reply += $"等级：{output.Level}\n";
                        reply += $"是否机器人：{output.IsBot}\n";
                        reply += "\n";
                    }
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("取频道列表"))
            {
                var outputList = _openApiService.GetChannelList(new GetChannelListInput
                {
                    IslandId = eventBody.IslandId
                });

                if (outputList != null)
                {
                    foreach (var output in outputList)
                    {
                        reply += $"频道号：{output.ChannelId}\n";
                        reply += $"频道名称：{output.ChannelName}\n";
                        reply += $"默认频道标识：{output.DefaultFlag}\n";
                        reply += "\n";
                    }
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("取频道信息"))
            {
                var output = _openApiService.GetChannelInfo(new GetChannelInfoInput
                {
                    ChannelId = eventBody.ChannelId
                });

                if (output != null)
                {
                    reply += $"频道号：{output.ChannelId}\n";
                    reply += $"频道名称：{output.ChannelName}\n";
                    reply += $"默认频道标识：{output.DefaultFlag}\n";
                    reply += "\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置频道文本消息发送"))
            {
                var output = _openApiService.SetChannelMessageSend(new SetChannelMessageSendInput<MessageText>
                {
                    ChannelId = eventBody.ChannelId,
                    MessageBody = new MessageText
                    {
                        Content = "测试文本消息"
                    }
                });

                if (output != null)
                {
                    reply += $"消息ID：{output.MessageId}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置频道图片消息发送"))
            {
                var output = _openApiService.SetChannelMessageSend(new SetChannelMessageSendInput<MessagePicture>
                {
                    ChannelId = eventBody.ChannelId,
                    MessageBody = new MessagePicture
                    {
                        Url = "https://img.imdodo.com/dodo/8c77d48865bf547a69fb3bba6228760c.png",
                        Width = 500,
                        Height = 500,
                        IsOriginal = 1
                    }
                });

                if (output != null)
                {
                    reply += $"消息ID：{output.MessageId}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置频道视频消息发送"))
            {
                var output = _openApiService.SetChannelMessageSend(new SetChannelMessageSendInput<MessageVideo>
                {
                    ChannelId = eventBody.ChannelId,
                    MessageBody = new MessageVideo
                    {
                        Url = "https://video.imdodo.com/dodo/ff85c752daf7d67884cb9ad3921a5d01.mp4",
                        CoverUrl = "https://img.imdodo.com/dodo/8c77d48865bf547a69fb3bba6228760c.png",
                        Duration = 100,
                        Size = 100
                    }
                });

                if (output != null)
                {
                    reply += $"消息ID：{output.MessageId}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置频道消息更新"))
            {
                var output = _openApiService.SetChannelMessageModify(new SetChannelMessageModifyInput<MessageText>
                {
                    MessageId = eventBody.MessageId,
                    ChannelId = eventBody.ChannelId,
                    MessageBody = new MessageText
                    {
                        Content = "修改后的文本"
                    }
                });

                if (output != null)
                {
                    reply += "置频道消息更新成功！";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置频道消息撤回"))
            {
                Thread.Sleep(3000);
                var output = _openApiService.SetChannelMessageWithdraw(new SetChannelMessageWithdrawInput
                {
                    MessageId = eventBody.MessageId,
                    Reason = "撤回测试"
                });

                if (output)
                {
                    reply += "置频道消息撤回成功！";
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("取身份组列表"))
            {
                var outputList = _openApiService.GetRoleList(new GetRoleListInput
                {
                    IslandId = eventBody.IslandId
                });

                if (outputList != null)
                {
                    foreach (var output in outputList)
                    {
                        reply += $"身份组ID：{output.RoleId}\n";
                        reply += $"身份组名称：{output.RoleName}\n";
                        reply += "\n";
                    }
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("置身份组成员新增"))
            {
                var output = _openApiService.SetRoleMemberAdd(new SetRoleMemberAddInput
                {
                    IslandId = eventBody.IslandId,
                    DoDoId = eventBody.DodoId,
                    RoleId = ""
                });

                if (output)
                {
                    reply += "置身份组成员新增成功！";
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("置身份组成员移除"))
            {
                var output = _openApiService.SetRoleMemberRemove(new SetRoleMemberRemoveInput
                {
                    IslandId = eventBody.IslandId,
                    DoDoId = eventBody.DodoId,
                    RoleId = ""
                });

                if (output)
                {
                    reply += "置身份组成员移除成功！";
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("取成员信息"))
            {
                var output = _openApiService.GetMemberInfo(new GetMemberInfoInput
                {
                    IslandId = eventBody.IslandId,
                    DoDoId = eventBody.DodoId
                });

                if (output != null)
                {
                    reply += $"群号：{output.IslandId}\n";
                    reply += $"DoDo号：{output.DodoId}\n";
                    reply += $"在群昵称：{output.NickName}\n";
                    reply += $"头像：{output.AvatarUrl}\n";
                    reply += $"加群时间：{output.JoinTime}\n";
                    reply += $"性别：{output.Sex}\n";
                    reply += $"等级：{output.Level}\n";
                    reply += $"是否机器人：{output.IsBot}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("取成员身份组列表"))
            {
                var outputList = _openApiService.GetMemberRoleList(new GetMemberRoleListInput
                {
                    IslandId = eventBody.IslandId,
                    DodoId = eventBody.DodoId
                });

                if (outputList != null)
                {
                    foreach (var output in outputList)
                    {
                        reply += $"身份组ID：{output.RoleId}\n";
                        reply += $"身份组名称：{output.RoleName}\n";
                        reply += "\n";
                    }
                }
                else
                {
                    reply += "调用接口失败！";
                }

            }
            else if (content.Contains("置成员昵称"))
            {
                var output = _openApiService.SetMemberNick(new SetMemberNickInput
                {
                    IslandId = eventBody.IslandId,
                    DoDoId = eventBody.DodoId,
                    NickName = "昵称修改测试"
                });

                if (output)
                {
                    reply += "置成员禁言成功！";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置成员禁言"))
            {
                var output = _openApiService.SetMemberBan(new SetMemberBanInput
                {
                    IslandId = eventBody.IslandId,
                    DoDoId = eventBody.DodoId,
                    Duration = 30,
                    Reason = "禁言测试"
                });

                if (output)
                {
                    reply += "置成员禁言成功！";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置个人文本消息发送"))
            {
                var output = _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessageText>
                {
                    DoDoId = eventBody.DodoId,
                    MessageBody = new MessageText
                    {
                        Content = "测试文本消息"
                    }
                });

                if (output != null)
                {
                    reply += $"消息ID：{output.MessageId}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置个人图片消息发送"))
            {
                var output = _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessagePicture>
                {
                    DoDoId = eventBody.DodoId,
                    MessageBody = new MessagePicture
                    {
                        Url = "https://img.imdodo.com/dodo/8c77d48865bf547a69fb3bba6228760c.png",
                        Width = 500,
                        Height = 500,
                        IsOriginal = 1
                    }
                });

                if (output != null)
                {
                    reply += $"消息ID：{output.MessageId}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置个人视频消息发送"))
            {
                var output = _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessageVideo>
                {
                    DoDoId = eventBody.DodoId,
                    MessageBody = new MessageVideo
                    {
                        Url = "https://video.imdodo.com/dodo/ff85c752daf7d67884cb9ad3921a5d01.mp4",
                        CoverUrl = "https://img.imdodo.com/dodo/8c77d48865bf547a69fb3bba6228760c.png",
                        Duration = 100,
                        Size = 100
                    }
                });

                if (output != null)
                {
                    reply += $"消息ID：{output.MessageId}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("置资源图片上传"))
            {
                var output = _openApiService.UploadResourcePicture(new SetResourceUploadInput
                {
                    FilePath = "https://img.imdodo.com/dodo/8c77d48865bf547a69fb3bba6228760c.png"
                });

                if (output != null)
                {
                    reply += $"链接：{output.Url}\n";
                    reply += $"高度：{output.Height}\n";
                    reply += $"宽度：{output.Width }\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }
            else if (content.Contains("取WebSocket连接"))
            {
                var output = _openApiService.GetWebSocketConnection(new GetWebSocketConnectionInput());

                if (output != null)
                {
                    reply += $"节点：{output.Endpoint}\n";
                }
                else
                {
                    reply += "调用接口失败！";
                }
            }

            if (reply != "")
            {
                _openApiService.SetChannelMessageSend(new SetChannelMessageSendInput<MessageText>
                {
                    ChannelId = eventBody.ChannelId,
                    MessageBody = new MessageText
                    {
                        Content = reply
                    }
                });
            }

        }

        public override void PersonalMessageEvent(EventSubjectOutput<EventSubjectDataBusiness<EventBodyPersonalMessage<MessageText>>> input)
        {
            var eventBody = input.Data.EventBody;
            var messageBody = input.Data.EventBody.MessageBody;

            _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessageText>
            {
                DoDoId = eventBody.DodoId,
                MessageBody = new MessageText
                {
                    Content = "触发个人消息事件："
                }
            });

            _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessageText>
            {
                DoDoId = eventBody.DodoId,
                MessageBody = messageBody
            });
        }

        public override void PersonalMessageEvent(EventSubjectOutput<EventSubjectDataBusiness<EventBodyPersonalMessage<MessagePicture>>> input)
        {
            var eventBody = input.Data.EventBody;
            var messageBody = input.Data.EventBody.MessageBody;

            _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessageText>
            {
                DoDoId = eventBody.DodoId,
                MessageBody = new MessageText
                {
                    Content = "触发个人消息事件："
                }
            });

            _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessagePicture>
            {
                DoDoId = eventBody.DodoId,
                MessageBody = messageBody
            });
        }

        public override void PersonalMessageEvent(EventSubjectOutput<EventSubjectDataBusiness<EventBodyPersonalMessage<MessageVideo>>> input)
        {
            var eventBody = input.Data.EventBody;
            var messageBody = input.Data.EventBody.MessageBody;

            _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessageText>
            {
                DoDoId = eventBody.DodoId,
                MessageBody = new MessageText
                {
                    Content = "触发个人消息事件："
                }
            });

            _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessageVideo>
            {
                DoDoId = eventBody.DodoId,
                MessageBody = messageBody
            });
        }

        public override void ChannelMessageEvent(EventSubjectOutput<EventSubjectDataBusiness<EventBodyChannelMessage<MessagePicture>>> input)
        {
            var eventBody = input.Data.EventBody;
            var messageBody = input.Data.EventBody.MessageBody;

            _openApiService.SetChannelMessageSend(new SetChannelMessageSendInput<MessageText>
            {
                ChannelId = eventBody.ChannelId,
                MessageBody = new MessageText
                {
                    Content = "触发频道消息事件："
                }
            });

            _openApiService.SetChannelMessageSend(new SetChannelMessageSendInput<MessagePicture>
            {
                ChannelId = eventBody.ChannelId,
                MessageBody = messageBody
            });
        }

        public override void ChannelMessageEvent(EventSubjectOutput<EventSubjectDataBusiness<EventBodyChannelMessage<MessageVideo>>> input)
        {
            var eventBody = input.Data.EventBody;
            var messageBody = input.Data.EventBody.MessageBody;

            _openApiService.SetChannelMessageSend(new SetChannelMessageSendInput<MessageText>
            {
                ChannelId = eventBody.ChannelId,
                MessageBody = new MessageText
                {
                    Content = "触发频道消息事件："
                }
            });

            _openApiService.SetChannelMessageSend(new SetChannelMessageSendInput<MessageVideo>
            {
                ChannelId = eventBody.ChannelId,
                MessageBody = messageBody
            });
        }

        public override void MessageReactionEvent(EventSubjectOutput<EventSubjectDataBusiness<EventBodyMessageReaction>> input)
        {
            var eventBody = input.Data.EventBody;
            var messageBody = input.Data.EventBody;

            _openApiService.SetPersonalMessageSend(new SetPersonalMessageSendInput<MessageText>
            {
                DoDoId = eventBody.DodoId,
                MessageBody = new MessageText
                {
                    Content = $"触发消息反应事件：{JsonConvert.SerializeObject(messageBody)}"
                }
            });
        }
    }
}
