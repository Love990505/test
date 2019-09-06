using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BookManagement.Controllers
{
    public class BookController : Controller
    {
        readonly Models.CodeService codeService = new Models.CodeService();
        readonly Models.BookService bookService = new Models.BookService();


        /// <summary>
        /// 圖書資料查詢
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Index()
        {
            //ViewBag.UserNameData = this.codeService.GetUserName();
            //ViewBag.BookClassNameData = this.codeService.GetBookClassName();
            //ViewBag.CodeNameData = this.codeService.GetCodeName();
            return View();
        }
        /// <summary>
        /// 取得圖書類別
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetDropDownListClass()
        {
            List<SelectListItem> ClassItem =codeService.GetBookClassName();  //區域變數開頭需小寫，按右鍵重新命名
            return Json(ClassItem);
        }
        /// <summary>
        /// 取得使用者名字
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetDropDownListUserName()
        {
            List<SelectListItem> UserNameItem = codeService.GetUserName();
            return Json(UserNameItem);
        }
        /// <summary>
        /// 取得圖書狀態
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetDropDownListCodeName()
        {
            List<SelectListItem> CodeNameItem = codeService.GetCodeName();
            return Json(CodeNameItem);
        }
        
        /// <summary>
        /// 圖書資料查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost()]

        public ActionResult Index(Models.BookSearchArg arg)
        {
            //ViewBag.UserNameData = this.codeService.GetUserName();
            //ViewBag.BookClassNameData = this.codeService.GetBookClassName();
            //ViewBag.CodeNameData = this.codeService.GetCodeName();
            //ViewBag.SearchResult = bookService.GetBookByCondtioin(arg);
            return View("Index");
        }
        /// <summary>
        /// 查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetBookCondtioin(Models.BookSearchArg arg)
        {
            List<Models.Books> tableData = bookService.GetBookByCondtioin(arg);
            return Json(tableData);
        }


        /// <summary>
        /// 新增圖書畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult InsertBook()
        {
            //ViewBag.BookClassNameData = this.codeService.GetBookClassName();
            return View(new Models.Books());
        }

        /// <summary>
        /// 新增圖書
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertBook(Models.Books book)
        {
            ViewBag.BookClassNameData = this.codeService.GetBookClassName();
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dateTime = DateTime.Parse(book.BookBoughtDate);
                    int BookID = bookService.InsertBook(book);
                    return RedirectToAction("BookData", new { BookID = BookID });
                }
                catch
                {
                    Response.Write("<script language=javascript>alert('日期格式錯誤')</script>");
                }
            }
            return View(book);
        }

        /// <summary>
        /// 刪除圖書
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteBook(int BookID)
        {
            try
            {
                bookService.DeleteBook(BookID);
                return this.Json(true);
            }
            catch (Exception ex)
            {
                return this.Json(false);
            }
        }

        /// <summary>
        /// 明細圖書畫面
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult BookData(int BookID)
        {
            Models.Books books = bookService.GetBookDetail(BookID).FirstOrDefault();
            return View(books);
        }

        /// <summary>
        /// 修改圖書畫面
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult UpdateBook(int BookID)
        {
            ViewBag.BookClassNameData = this.codeService.GetBookClassName();
            ViewBag.UserNameData = this.codeService.GetUserName();
            ViewBag.CodeNameData = this.codeService.GetCodeName();
            Models.Books books = bookService.GetBookData(BookID).FirstOrDefault();
            return View(books);
        }

        /// <summary>
        /// 修改圖書存檔
        /// </summary>
        /// <param name="BookID"></param>
        /// <param name="books"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult UpdateBook(Models.Books books)
        {
            ViewBag.BookClassNameData = this.codeService.GetBookClassName();
            ViewBag.UserNameData = this.codeService.GetUserName();
            ViewBag.CodeNameData = this.codeService.GetCodeName();
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dateTime = DateTime.Parse(books.BookBoughtDate);
                    bookService.UpdateBookData(books);
                    return RedirectToAction("BookData", new { BookID = books.BookID });
                }
                catch(Exception ex)
                {
                    Response.Write("<script language=javascript>alert('日期格式錯誤')</script>");
                }
            }
            return View(books);
        }

        /// <summary>
        /// 借閱紀錄畫面
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult BookLendRecord(int BookID)
        {
            ViewBag.LendRecord = bookService.GetBookLendRecord(BookID);
            return View("BookLendRecord");
        }
    }
}