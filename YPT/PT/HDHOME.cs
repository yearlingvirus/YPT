﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Utils;

namespace YPT.PT
{
    public class HDHOME : AbstractPT
    {
        public HDHOME(PTUser user) : base(user)
        {
        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.HDHome;
            }
        }

        public override string Sign()
        {
            if (_cookie != null && _cookie.Count > 0)
            {
                string htmlResult = HttpUtils.GetDataGetHtml(Site.SignUrl, _cookie);

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载

                //这个是每次第一次签到的
                HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"outer\"]/table/tr/td/table/tr/td/p");//跟Xpath一样，轻松的定位到相应节点下
                if (node == null)
                    //这个是已经签到过的
                    node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"outer\"]/table/tr/td/table/tr/td");
                if (node != null)
                    return node.InnerText;
                else
                    return htmlResult;
            }
            else
            {
                return "无法获取Cookie信息，签到失败，请重新登录系统。";
            }
        }
    }
}
