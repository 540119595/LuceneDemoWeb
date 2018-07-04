using System;
using System.ComponentModel.DataAnnotations;

namespace LuceneDemoWeb.Models
{
    public class EditViewArticle
    {
        [Display(Name = "文章Id")]
        public int Id { get; set; }

        [Display(Name = "栏目Id")]
        public int ChannelId { get; set; }

        [Display(Name = "内容类别Id")]
        public int CategoryId { get; set; }

        [Display(Name = "标题")]
        [Required]
        [StringLength(64)]
        public string Title { get; set; }

        //[System.Web.Mvc.AllowHtml]  // core不需要了
        [Display(Name = "正文")]
        [DataType(DataType.MultilineText)]   //如在视图中使用强类类型的辅助方法@Html.EditorFor(model =>model),则此字段将被渲染成<textarea>文本域标签。
        public string Contents { get; set; }

        [Display(Name = "摘要")]
        [StringLength(2048)] // 页面实际是把文本全部传回
        public string Summary { get; set; }

        [Display(Name = "创建者Id")]
        [StringLength(64)]
        public string CreatorId { get; set; }

        [Display(Name = "创建时间")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; }

        [Display(Name = "更新者Id")]
        [StringLength(64)]
        public string LastUpdateUserId { get; set; }

        [Display(Name = "更新时间")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdateTime { get; set; }

        [Display(Name = "缩略图地址")]
        [StringLength(120)]
        public string Thumbnail { get; set; }

        [Display(Name = "关键词")]
        [StringLength(32)]
        public string Keywords { get; set; }
    }
}
