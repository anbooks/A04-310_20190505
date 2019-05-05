using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class MoPagerOption
    {
        /// <summary>
        /// 当前页  必传
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 总条数  必传
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 分页记录数（每页条数 默认每页15条）
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 路由地址(格式如：/Controller/Action) 默认自动获取
        /// </summary>
        public string RouteUrl { get; set; }
       
        /// <summary>
        /// 样式 默认 bootstrap样式 1
        /// </summary>
        public int StyleNum { get; set; }
    }
    public class Text
    {
        public int po { get; set; }
    }
        /// <summary>
        /// 分页标签
        /// </summary>
        public class TextCollectionTagHelper : TagHelper
    {
        //
        public Text Text { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //if (Text.po == 1)
            //{
                output.TagName = "div";
                output.Attributes.Add("style", "display:none");
           // }

            base.Process(context, output);

        }

        public class PagerTagHelper : TagHelper
        {

            public MoPagerOption PagerOption { get; set; }

            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                //var pagestart = PagerOption.CurrentPage - 2 > 0 ? PagerOption.CurrentPage - 2 : 1;
                //var pageend = PagerOption.CurrentPage + 2 >= PagerOption.Total ? PagerOption.Total : pagestart + 4;

                output.TagName = "div";

                if (PagerOption.PageSize <= 0) { PagerOption.PageSize = 15; }
                if (PagerOption.CurrentPage <= 0) { PagerOption.CurrentPage = 1; }
                if (PagerOption.Total <= 0) { return; }

                //总页数
                var totalPage = PagerOption.Total / PagerOption.PageSize + (PagerOption.Total % PagerOption.PageSize > 0 ? 1 : 0);
                if (totalPage <= 0) { return; }
                //当前路由地址
                if (string.IsNullOrEmpty(PagerOption.RouteUrl))
                {

                    //PagerOption.RouteUrl = helper.ViewContext.HttpContext.Request.RawUrl;
                    if (!string.IsNullOrEmpty(PagerOption.RouteUrl))
                    {

                        var lastIndex = PagerOption.RouteUrl.LastIndexOf("/");
                        PagerOption.RouteUrl = PagerOption.RouteUrl.Substring(0, lastIndex);
                    }
                }
                PagerOption.RouteUrl = PagerOption.RouteUrl.TrimEnd('/');
                var pagestart = PagerOption.CurrentPage - 2 > 0 ? PagerOption.CurrentPage - 2 : 1;
                var pageend = PagerOption.CurrentPage + 2 >= totalPage ? totalPage : pagestart + 4;
                //构造分页样式
                var sbPage = new StringBuilder(string.Empty);
                switch (PagerOption.StyleNum)
                {
                    case 2:
                        {
                            break;
                        }
                    default:
                        {
                            #region 默认样式

                            sbPage.Append("<nav>");
                            sbPage.Append("  <ul class=\"pagination\">");
                            sbPage.AppendFormat("       <li><a href=\"{0}/{1}\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span></a></li>",
                                                    PagerOption.RouteUrl,
                                                    PagerOption.CurrentPage - 1 <= 0 ? 1 : PagerOption.CurrentPage - 1);

                            for (int i = pagestart; i <= pageend; i++)
                            {

                                sbPage.AppendFormat("       <li {1}><a href=\"{2}/{0}\">{0}</a></li>",
                                   i,
                                   i == PagerOption.CurrentPage ? "class=\"active\"" : "",
                                   PagerOption.RouteUrl);

                            }

                            sbPage.Append("       <li>");
                            sbPage.AppendFormat("         <a href=\"{0}/{1}\" aria-label=\"Next\">",
                                                PagerOption.RouteUrl,
                                                PagerOption.CurrentPage + 1 > totalPage ? PagerOption.CurrentPage : PagerOption.CurrentPage + 1);
                            sbPage.Append("               <span aria-hidden=\"true\">&raquo;</span>");
                            sbPage.Append("         </a>");
                            sbPage.Append("       </li>");
                            sbPage.Append("   </ul>");
                            sbPage.Append("</nav>");
                            #endregion
                        }
                        break;
                }

                output.Content.SetHtmlContent(sbPage.ToString());
                //output.TagMode = TagMode.SelfClosing;
                //return base.ProcessAsync(context, output);
            }

        }
    }
}
