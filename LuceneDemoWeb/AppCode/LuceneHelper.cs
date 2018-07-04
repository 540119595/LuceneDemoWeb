using LuceneDemoWeb.Models;
using System;
using System.IO;
using System.Linq;

namespace LuceneDemoWeb.AppCode
{
    /// <summary>
    /// 全文检索
    /// <para>
    /// 第三方应用Lucene.NET帮助类
    /// </para>
    /// </summary>
    public class LuceneHelper
    {
        /// <summary>
        /// 索引存放文件夹名称
        /// </summary>
        private const string DIR_ARTICLES_INDEX = "ArticlesIndex";
        // 存放索引文件的目录
        private static string _articlesIndexDirPath = "";
        // 索引存储位置
        private static Lucene.Net.Store.Directory _directory = null;

        public LuceneHelper(string path)
        {
            // 初始化 索引存放目录
            _articlesIndexDirPath = System.IO.Directory.CreateDirectory(path + "//" + DIR_ARTICLES_INDEX).FullName;
            _directory = InitDirectory();
        }

        /// <summary>
        /// 初始化索引存储位置（文件系统FSDirectory）
        /// </summary>
        private static Lucene.Net.Store.Directory InitDirectory()
        {
            if (_directory == null) _directory = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(_articlesIndexDirPath));
            if (Lucene.Net.Index.IndexWriter.IsLocked(_directory)) Lucene.Net.Index.IndexWriter.Unlock(_directory);
            var lockFilePath = Path.Combine(_articlesIndexDirPath, "write.lock");
            if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
            return _directory;
        }

        private Lucene.Net.Analysis.PanGu.PanGuAnalyzer CreateAnalyzer()
        {
            return new Lucene.Net.Analysis.PanGu.PanGuAnalyzer(false, new PanGu.Match.MatchOptions()
            {
                ChineseNameIdentify = true,
                FilterStopWords = true,
                FrequencyFirst = true,
                EnglishMultiDimensionality = true,
                EnglishSegment = true
            }, new PanGu.Match.MatchParameter());
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="article">文章信息</param>
        public void AddIndex(EditViewArticle article)
        {
            //DeleteIndexById(article.Id);
            CreateIndex(article);
        }

        private void CreateIndex(EditViewArticle article)
        {
            //using (var analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48))
            //{
            var options = new Lucene.Net.Index.IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, null)
            {
                OpenMode = Lucene.Net.Index.OpenMode.CREATE
            };
            //using (var indexWriter = new Lucene.Net.Index.IndexWriter(_directory, analyzer, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED))
            using (var indexWriter = new Lucene.Net.Index.IndexWriter(_directory, options))
            {
                var document = new Lucene.Net.Documents.Document
                {
                    new Lucene.Net.Documents.TextField("Id", article.Id.ToString(), Lucene.Net.Documents.Field.Store.YES),
                    new Lucene.Net.Documents.TextField("Title", article.Title, Lucene.Net.Documents.Field.Store.YES),
                    // HTML文本
                    // old版本//document.Add(new Lucene.Net.Documents.Field("Contents", article.Contents, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));
                    //document.Add(new Lucene.Net.Documents.TextField("Contents", article.Contents, Lucene.Net.Documents.Field.Store.YES));
                    // 纯文本
                    new Lucene.Net.Documents.TextField("TContents", article.Summary, Lucene.Net.Documents.Field.Store.YES),
                    new Lucene.Net.Documents.TextField("CreateTime", article.CreateTime.ToString(), Lucene.Net.Documents.Field.Store.YES)
                };

                indexWriter.AddDocument(document, this.CreateAnalyzer());
                indexWriter.Commit();
            }
            //}
        }

        public void Search(string keyword)
        {
            using (var indexer = Lucene.Net.Index.DirectoryReader.Open(_directory))
            {
                var searcher = new Lucene.Net.Search.IndexSearcher(indexer);
                var queryParsers = new Lucene.Net.QueryParsers.Classic.QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "Title", new Lucene.Net.Analysis.PanGu.PanGuAnalyzer(true));
                var query = queryParsers.Parse(keyword);
                var hits = searcher.Search(query, null, 16).ScoreDocs;
                var results = hits.Select(hit =>
                {
                    var doc = searcher.Doc(hit.Doc);
                    return new EditViewArticle
                    {
                        Id = Convert.ToInt32(doc.Get("Id")),
                        Title = doc.Get("Title"),
                        Summary = doc.Get("TContents"),
                        //Title = HightLight(searchInfo, doc.Get("Title")),                                                                         
                        //Summary = HightLight(searchInfo, doc.Get("TContents")),
                        CreateTime = DateTime.Parse(doc.Get("CreateTime"))
                    };
                }).OrderByDescending(a => a.CreateTime).ToList();
            }
        }
    }
}
